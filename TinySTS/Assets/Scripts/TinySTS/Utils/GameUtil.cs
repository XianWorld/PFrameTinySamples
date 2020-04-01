//using UnityEngine;
using Unity.Entities;
using PFrame.Entities;
using Unity.Collections;
using Unity.Tiny;
using System;
using Unity.Mathematics;
using Unity.Transforms;

namespace TinySTS
{
    public class GameUtil
    {
        public static Entity CreateCreature(EntityManager entityManager, GameDataManagerSystem gameDataManagerSystem, LevelData data)
        {
            var dataId = data.Id;
            if (!gameDataManagerSystem.GetGameData<CreatureData>(dataId, out var creatureData))
                return Entity.Null;

            var entity = entityManager.Instantiate(creatureData.Prefab);

            //creature
            var creature = entityManager.GetComponentData<Creature>(entity);
            creature.DataId = dataId;
            creature.Level = data.Level;
            entityManager.SetComponentData(entity, creature);

            #region init stats
            //stat element
            var minLevel = creatureData.MinLevel;
            var maxLevel = creatureData.MaxLevel;

            ref var statValues = ref creatureData.StatValuesAssetRef.Value.Array;
            ref var statAddValues = ref creatureData.StatAddValuesAssetRef.Value.Array;

            var levelNum = data.Level - minLevel;
            var statElementBuffer = entityManager.GetBuffer<StatElement>(entity);
            for (int i = 0; i < (byte)ECreatureStatType.Max; i++)
            {
                statElementBuffer[i] = new StatElement
                {
                    BaseValue = (short)(statValues[i] + statAddValues[i] * levelNum)
                };
            }
            #endregion

            #region build base deck
            var cardElementList = new NativeList<CardElement>(Allocator.Temp);
            //card deck??? card deck data -> card deck instance
            var startDeckDataId = creatureData.StartDeckDataId;
            if (gameDataManagerSystem.GetGameData<CardDeckData>(startDeckDataId, out var cardDeckData))
            {
                ushort cardId = 0;

                int count = cardDeckData.ItemDatasAssetRef.Value.Count;
                ref var itemDatas = ref cardDeckData.ItemDatasAssetRef.Value.ItemDatas;
                for (int i = 0; i < count; i++)
                {
                    var itemData = itemDatas[i];
                    var num = itemData.Num;

                    for (int j = 0; j < num; j++)
                    {
                        var cardEntity = CreateCard(entityManager, gameDataManagerSystem, itemData.Id, itemData.Level);

                        var card = entityManager.GetComponentData<Card>(cardEntity);
                        card.Id = cardId;
                        card.OwnerEntity = entity;
                        entityManager.SetComponentData(cardEntity, card);

                        cardId++;

                        var cardElement = new CardElement();
                        cardElement.CardEntity = cardEntity;
                        //cardBuffer.Add(cardElement);
                        cardElementList.Add(cardElement);
                    }
                }
            }

            //card deck element buffer
            byte cardDeckNum = (byte)ECardDeckType.Max;
            var cardDeckArray = new NativeArray<CardDeckElement>(cardDeckNum, Allocator.Temp);
            for (int i = 0; i < cardDeckNum; i++)
            {
                var deckEntity = entityManager.CreateEntity();
                entityManager.AddComponentData(deckEntity, new CardDeck
                {
                    Type = (ECardDeckType)i,
                    OwnerEntity = entity
                });
                entityManager.AddBuffer<CardElement>(deckEntity);
                //cardDeckBuffer.Add(new CardDeckElement { DeckEntity = deckEntity });
                cardDeckArray[i] = new CardDeckElement { DeckEntity = deckEntity };
            }
            var cardDeckBuffer = entityManager.AddBuffer<CardDeckElement>(entity);
            cardDeckBuffer.AddRange(cardDeckArray);
            cardDeckArray.Dispose();

            //card element buffer
            var baseDeckEntity = cardDeckBuffer[(byte)ECardDeckType.BaseDeck].DeckEntity;
            var cardBuffer = entityManager.GetBuffer<CardElement>(baseDeckEntity);
            cardBuffer.AddRange(cardElementList.AsArray());
            cardElementList.Dispose();

            //var cardBuffer = entityManager.AddBuffer<CardElement>(entity);
            //cardBuffer.AddRange(cardElementList.AsArray());
            //cardElementList.Dispose();
            #endregion

            return entity;
        }

        public static Entity CreateCard(EntityManager entityManager, GameDataManagerSystem gameDataManagerSystem, ushort cardInfoDataId, byte level)
        {
            if (!gameDataManagerSystem.GetGameData<CardInfoData>(cardInfoDataId, out var cardInfoData))
                return Entity.Null;

            var skillInfoDataId = cardInfoData.SkillInfoDataId;

            var entity = CreateSkillEntity(entityManager, gameDataManagerSystem, skillInfoDataId, level);

            //int count = cardInfoData.DataIdsAssetRef.Value.Count;
            //ref var dataIds = ref cardInfoData.DataIdsAssetRef.Value.Array;

            //var cardDataId = dataIds[level - 1];
            //if (!gameDataManagerSystem.GetGameData<CardData>(cardDataId, out var cardData))
            //    return Entity.Null;

            //var entity = entityManager.Instantiate(cardData.Prefab);

            //card
            var card = new Card();
            card.DataId = cardInfoDataId;
            card.InfoDataId = cardInfoDataId;
            card.Level = level;
            card.Name = cardInfoData.Name;
            entityManager.SetOrAddComponentData(entity, card);

            return entity;
        }

        public static Entity CreateSkillEntity(EntityManager entityManager, GameDataManagerSystem gameDataManagerSystem, ushort skillInfoDataId, byte level)
        {
            if (!gameDataManagerSystem.GetGameData<CardInfoData>(skillInfoDataId, out var infoData))
                return Entity.Null;

            int count = infoData.DataIdsAssetRef.Value.Count;
            ref var dataIds = ref infoData.DataIdsAssetRef.Value.Array;

            var skillDataId = dataIds[level - 1];
            if (!gameDataManagerSystem.GetGameData<SkillData>(skillDataId, out var skillData))
                return Entity.Null;

            var entity = entityManager.CreateEntity();

            //skill
            var skill = new Skill();
            skill.DataId = skillDataId;
            skill.InfoDataId = skillInfoDataId;
            skill.Level = level;
            skill.TargetType = skillData.TargetType;
            skill.IsSpecificRelation = skillData.IsSpecificRelation;
            skill.TargetRelationType = skillData.TargetRelationType;
            skill.IsWithoutSelf = skillData.IsWithoutSelf;
            skill.TargetNum = skillData.TargetNum;

            entityManager.SetOrAddComponentData(entity, skill);

            //add skill effects
            int effectCount = skillData.DataIdsAssetRef.Value.Count;
            ref var effectDataIds = ref skillData.DataIdsAssetRef.Value.Array;

            var effects = new NativeArray<SkillEffectElement>(effectCount, Allocator.Temp);
            for (int i = 0; i < effectCount; i++)
            {
                var effectEntity = CreateSkillEffectEntity(entityManager, gameDataManagerSystem, effectDataIds[i]);

                var effect = entityManager.GetComponentData<Effect>(effectEntity);
                effect.OwnerEntity = entity;
                entityManager.SetComponentData(effectEntity, effect);

                var effectElement = new SkillEffectElement();
                effectElement.EffectEntity = effectEntity;
                effectElement.EffectType = effect.Type;
                effects[i] = effectElement;
            }

            var buffer = entityManager.AddBuffer<SkillEffectElement>(entity);
            buffer.AddRange(effects);

            effects.Dispose();
            return entity;
        }

        public static Entity CreateSkillEffectEntity(EntityManager entityManager, GameDataManagerSystem gameDataManagerSystem, ushort dataId)
        {
            if (!gameDataManagerSystem.GetGameData<EffectData>(dataId, out var effectData))
                return Entity.Null;

            var entity = entityManager.CreateEntity();

            var effectType = effectData.Type;
            var effect = new Effect
            {
                Type = effectType,
                CastType = effectData.CastType

            };

            entityManager.AddComponentData(entity, effect);

            if (effectType == EEffectType.Damage)
            {
                entityManager.AddComponentData(entity, new DamageEffect
                {
                    Damage = effectData.Damage
                });
            }
            else if(effectType == EEffectType.CreatureStat)
            {
                entityManager.AddComponentData(entity, new CreatureStatEffect
                {
                    StatType = effectData.CreatureStatType,
                    BonusType = effectData.BonusType,
                    Value = effectData.BonusValue
                });
            }

            return entity;
        }

        public static Entity CreateCardView(EntityManager entityManager, GameDataManagerSystem gameDataManagerSystem, Entity cardEntity)
        {
            var card = entityManager.GetComponentData<Card>(cardEntity);
            if (!gameDataManagerSystem.GetGameData<CardInfoData>(card.InfoDataId, out var cardInfoData))
                return Entity.Null;

            if (!gameDataManagerSystem.GetGameData<CardTemplateData>((data) =>
             {
                 if (data.Type == cardInfoData.Type && data.GroupType == cardInfoData.GroupType)
                     return true;
                 return false;
             }, out var templateData))
                return Entity.Null;

            var viewEntity = entityManager.Instantiate(templateData.Prefab);

            card.ViewEntity = viewEntity;
            entityManager.SetComponentData(cardEntity, card);

            entityManager.SetOrAddComponentData<CardView>(viewEntity, new CardView { CardEntity = cardEntity });

            UpdateCardView(entityManager, cardEntity);

            return viewEntity;
        }

        public static bool UpdateCardView(EntityManager entityManager, Entity cardEntity)
        {
            var card = entityManager.GetComponentData<Card>(cardEntity);
            var viewEntity = card.ViewEntity;

            if (viewEntity == Entity.Null)
                return false;

            var cardTemplate = entityManager.GetComponentData<CardTemplate>(viewEntity);

            //Font3DUtil.SetText(entityManager, cardTemplate.NameText, card.Name.ToString());

            TryGetStatElement(entityManager, cardEntity, (byte)ESkillStatType.ManaCost, out var statElement);
            //Font3DUtil.SetText(entityManager, cardTemplate.ManaText, statElement.Value.ToString());

            return true;
        }

        public static bool TryGetStatElement(EntityManager entityManager, Entity entity, byte statId, out StatElement statElement)
        {
            statElement = new StatElement();
            if (!entityManager.HasComponent<StatElement>(entity))
                return false;

            var buffer = entityManager.GetBuffer<StatElement>(entity);

            if (statId >= buffer.Length)
                return false;
            statElement = buffer[statId];
            return true;
        }

        #region card view container
        public static void UpdateCardViewContainerLayout(CardViewContainer container, Rect rect, int count, Action<int, float3, quaternion> action)
        {
            var hSpacing = container.HSpacing;
            var zSpacing = container.ZSpacing;
            var width = rect.width;
            float minX = 0f;
            float minZ = 0f;

            if (count == 0)
                return;

            if (count > 1)
            {
                var temp = width / (count - 1);
                if (temp < hSpacing)
                    hSpacing = temp;

                minX = -((count - 1) * hSpacing / 2f);
                minZ = -((count - 1) * zSpacing / 2f);
            }

            float offsetX = minX;
            float offsetZ = minZ;
            for (int i = 0; i < count; i++)
            {
                //var child = transform.GetChild(i);
                //var pos = child.localPosition;
                //pos.x = offsetX;
                //pos.z = offsetZ;
                //child.localPosition = pos;
                var pos = new float3(offsetX, 0f, offsetZ);
                var rot = quaternion.identity;
                action(i, pos, rot);

                offsetX += hSpacing;
                offsetZ += zSpacing;
            }
        }

        public static void AddCardViewElement(EntityManager entityManager, Entity containerEntity, Entity cardViewEntity)
        {
            var buffer = entityManager.GetBuffer<CardViewElement>(containerEntity);

            var cardViewElement = new CardViewElement
            {
                CardViewEntity = cardViewEntity
            };
            if (EntityUtil.Contains(buffer, cardViewElement))
                return;

            buffer.Add(cardViewElement);

            EntityUtil.SetParent(entityManager, cardViewEntity, containerEntity);

            entityManager.RemoveComponentSafe<UpdatedState>(containerEntity);
        }

        public static void RemoveCardViewElement(EntityManager entityManager, Entity containerEntity, Entity cardViewEntity)
        {
            var buffer = entityManager.GetBuffer<CardViewElement>(containerEntity);

            var cardViewElement = new CardViewElement
            {
                CardViewEntity = cardViewEntity
            };
            var index = EntityUtil.IndexOf(buffer, cardViewElement);
            if (index < 0)
                return;

            buffer.RemoveAt(index);

            EntityUtil.SetParent(entityManager, cardViewEntity, Entity.Null);

            entityManager.RemoveComponentSafe<UpdatedState>(containerEntity);
        }
        #endregion
    }
}

