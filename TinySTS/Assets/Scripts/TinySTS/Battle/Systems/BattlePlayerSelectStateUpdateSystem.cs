using System;
using PFrame.Entities;
using PFrame.Tiny;
using PFrame.Tiny.Tweens;
using PFrame.Tiny.UI;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace TinySTS
{
    [UpdateInGroup(typeof(ServiceUpdateSystemGroup))]
    public class BattlePlayerSelectStateUpdateSystem : ComponentSystem
    {
        private TweenSystem tweenSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<Battle>();
        }

        protected override void OnStartRunning()
        {
            base.OnStartRunning();

            tweenSystem = World.GetExistingSystem<TweenSystem>();
        }

        protected override void OnUpdate()
        {
            //player select state
            Entities.ForEach((Entity entity, ref Battle battle, ref PlayerSelectState state) =>
            {
                var windowEntity = battle.BattleWindowEntity;
                var battleWindow = EntityManager.GetComponentData<BattleWindow>(windowEntity);
                var endTurnButtonEntity = battleWindow.EndTurnButtonEntity;

                if (EntityManager.HasComponent<UIButtonClickEvent>(endTurnButtonEntity))
                {
                    CommonUtil.SetFSMStateCmd<PlayerTurnEndState, EnterPlayerTurnEndStateEvent, ExitPlayerTurnEndStateEvent>(EntityManager, entity, (byte)EBattleState.PlayerTurnEnd);
                }

                Entities
                    .WithNone<SelectedState>()
                    .ForEach((Entity cardViewEntity, ref CardView cardView, ref PointerInteractable interactable, ref PointerEnterEvent enterEvent) =>
                {
                    //if (cardView.IsSelected)
                    //    return;

                    var typeInfo = TypeManager.GetTypeInfo<Translation>();
                    var info = TweenSystem.GetFieldArgs(typeInfo.TypeIndex, (int)PrimitiveFieldTypes.Float, 4);
                    tweenSystem.AddTween<float>(
                        cardViewEntity,
                        info,
                        0f,
                        0.2f,
                        0.1f
                        );
                });

                Entities
                    .WithNone<SelectedState>()
                    .ForEach((Entity cardViewEntity, ref CardView cardView, ref PointerInteractable interactable, ref PointerExitEvent exitEvent) =>
                {
                    //if (cardView.IsSelected)
                    //    return;

                    var typeInfo = TypeManager.GetTypeInfo<Translation>();
                    var info = TweenSystem.GetFieldArgs(typeInfo.TypeIndex, (int)PrimitiveFieldTypes.Float, 4);
                    tweenSystem.AddTween<float>(
                        cardViewEntity,
                        info,
                        0.2f,
                        0f,
                        0.1f
                        );
                });

                var cardViewQuery = GetEntityQuery(typeof(CardView), typeof(PointerClickEvent));
                var num = cardViewQuery.CalculateEntityCount();
                if (num > 0)
                {
                    var entities = cardViewQuery.ToEntityArray(Allocator.TempJob);
                    var cardViewEntity = entities[0];

                    var selectedCardViewEntity = battle.SelectedCardViewEntity;
                    if (selectedCardViewEntity != Entity.Null)
                    {
                        EntityUtil.RemoveState<SelectedState, ExitSelectedStateEvent>(EntityManager, selectedCardViewEntity, true);

                        battle.SelectedCardEntity = Entity.Null;
                        battle.SelectedCardViewEntity = Entity.Null;

                        //remove all indicated state
                        Entities.ForEach((Entity creatureEntity, ref Creature creature, ref TargetIndicatedState indicatedState) =>
                        {
                            EntityUtil.RemoveState<TargetIndicatedState, ExitTargetIndicatedStateEvent>(EntityManager, creatureEntity, true);
                            //EntityManager.RemoveComponent<TargetIndicator>(creatureEntity);
                        });
                    }

                    if (selectedCardViewEntity != cardViewEntity)
                    {
                        EntityUtil.AddState<SelectedState, EnterSelectedStateEvent>(EntityManager, cardViewEntity, true);

                        var cardView = EntityManager.GetComponentData<CardView>(cardViewEntity);
                        var cardEntity = cardView.CardEntity;

                        battle.SelectedCardEntity = cardEntity;
                        battle.SelectedCardViewEntity = cardViewEntity;

                        var skill = EntityManager.GetComponentData<Skill>(cardEntity);
                        var targetType = skill.TargetType;
                        var targetNum = skill.TargetNum;
                        bool isWithoutSelf = skill.IsWithoutSelf;

                        var playerEntity = battle.PlayerEntity;
                        var enemyMembers = EntityManager.GetBuffer<EnemyTeamMember>(entity).ToNativeArray(Allocator.Temp);

                        var indicatedState = new TargetIndicatedState { IndicatorType = targetType };
                        Entities.ForEach((Entity creatureEntity, ref Creature creature, ref TargetIndicatable indicatable) =>
                        {
                            if(IsSkillTarget(EntityManager, playerEntity, skill, creatureEntity))
                            {
                                EntityUtil.AddStateData<TargetIndicatedState, EnterTargetIndicatedStateEvent>(EntityManager, creatureEntity, indicatedState, true);
                                //EntityManager.SetOrAddComponentData<TargetIndicator>(creatureEntity, indicator);
                            }
                        });

                        //if (targetType == ETargetType.Self)
                        //{
                        //    indicatorType = ETargetIndicatorType.Target;
                        //}
                        //else if (targetType == ETargetType.All)
                        //{
                        //    var indicator = new TargetIndicator { IndicatorType = ETargetIndicatorType.All };
                        //    EntityManager.SetOrAddComponentData<TargetIndicator>(playerEntity, indicator);
                        //    for(int i = 0;i< enemyMembers.Length;i++)
                        //        EntityManager.SetOrAddComponentData<TargetIndicator>(enemyMembers[i].CreatureEntity, indicator);
                        //}
                        //else if (targetType == ETargetType.AllFriendly)
                        //{
                        //    var indicator = new TargetIndicator { IndicatorType = ETargetIndicatorType.All };
                        //    EntityManager.SetOrAddComponentData<TargetIndicator>(playerEntity, indicator);
                        //}
                        //else if (targetType == ETargetType.AllHostile)
                        //{
                        //    var indicator = new TargetIndicator { IndicatorType = ETargetIndicatorType.All };
                        //    for (int i = 0; i < enemyMembers.Length; i++)
                        //        EntityManager.SetOrAddComponentData<TargetIndicator>(enemyMembers[i].CreatureEntity, indicator);
                        //}

                        enemyMembers.Dispose();
                    }

                    entities.Dispose();


                    //CommonUtil.SetFSMStateCmd<PlayerSelectTargetState, EnterPlayerSelectTargetStateEvent, ExitPlayerSelectTargetStateEvent>(EntityManager, entity, (byte)EBattleState.PlayerSelectTarget);
                }

            });

            Entities.ForEach((Entity cardViewEntity, ref CardView cardView, ref Translation translation, ref EnterSelectedStateEvent enterEvent) =>
            {
                //cardView.IsSelected = true;

                var pos = translation.Value;
                pos.y = 0.3f;
                translation.Value = pos;
            });

            Entities.ForEach((Entity cardViewEntity, ref CardView cardView, ref Translation translation, ref ExitSelectedStateEvent enterEvent) =>
            {
                //cardView.IsSelected = false;

                var pos = translation.Value;
                pos.y = 0f;
                translation.Value = pos;
            });

            Entities.ForEach((Entity creatureEntity, ref Creature creature, ref TargetIndicatable indicatable, ref ExitTargetIndicatedStateEvent exitEvent) =>
            {
                var indicatorEntity = indicatable.IndicatorEntity;
                var indicator = EntityManager.GetComponentData<TargetIndicator>(indicatorEntity);

                EntityUtil.SetActive(EntityManager, indicator.SelfIndicatorEntity, false);
                EntityUtil.SetActive(EntityManager, indicator.AllIndicatorEntity, false);
                EntityUtil.SetActive(EntityManager, indicator.RandomIndicatorEntity, false);
                EntityUtil.SetActive(EntityManager, indicator.AssignedIndicatorEntity, false);
            });

            Entities.ForEach((Entity creatureEntity, ref Creature creature, ref TargetIndicatable indicatable, ref TargetIndicatedState indicatedState, ref EnterTargetIndicatedStateEvent enterEvent) =>
            {
                var indicatorEntity = indicatable.IndicatorEntity;
                var indicator = EntityManager.GetComponentData<TargetIndicator>(indicatorEntity);

                var type = indicatedState.IndicatorType;
                if (type == ETargetType.Self)
                    EntityUtil.SetActive(EntityManager, indicator.SelfIndicatorEntity, true);
                else if (type == ETargetType.All)
                    EntityUtil.SetActive(EntityManager, indicator.AllIndicatorEntity, true);
                else if (type == ETargetType.Random)
                    EntityUtil.SetActive(EntityManager, indicator.RandomIndicatorEntity, true);
                else if (type == ETargetType.Assigned)
                    EntityUtil.SetActive(EntityManager, indicator.AssignedIndicatorEntity, true);
            });
        }

        private bool IsSkillTarget(EntityManager entityManager, Entity playerEntity, Skill skill, Entity targetEntity)
        {
            var targetType = skill.TargetType;
            var isSpecificRelation = skill.IsSpecificRelation;
            var targetReleationType = skill.TargetRelationType;
            var targetNum = skill.TargetNum;
            bool isWithoutSelf = skill.IsWithoutSelf;

            if (targetType == ETargetType.Self && playerEntity != targetEntity)
                return false;

            var playerCamp = entityManager.GetComponentData<Camp>(playerEntity);
            var targetCamp = entityManager.GetComponentData<Camp>(targetEntity);

            var relationType = GetRelation(playerCamp.CampType, targetCamp.CampType);

            if (isSpecificRelation && relationType != targetReleationType)
                return false;

            if (isWithoutSelf && playerEntity == targetEntity)
                return false;

            return true;
        }

        public ERelationType GetRelation(ECampType playerCampType, ECampType targetCampType)
        {
            if (playerCampType == targetCampType)
                return ERelationType.Friendly;
            else if (playerCampType == ECampType.Neutral || targetCampType == ECampType.Neutral)
                return ERelationType.Neutral;
            else
                return ERelationType.Hostile;
        }
    }
}