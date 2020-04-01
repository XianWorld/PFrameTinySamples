using PFrame.Entities;
using PFrame.Tiny;
using PFrame.Tiny.UI;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Tiny.Input;
using Unity.Transforms;

namespace TinySTS
{
    [UpdateInGroup(typeof(ServiceUpdateSystemGroup))]
    [UpdateAfter(typeof(ServiceStageUpdateSystem))]
    [UpdateBefore(typeof(ServiceUpdateSystem))]
    public class PrototypeStageUpdateSystem : ComponentSystem
    {
        GameDataManagerSystem gameDataManagerSystem;
        InputSystem inputSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<PrototypeStage>();
        }

        protected override void OnStartRunning()
        {
            base.OnStartRunning();

            gameDataManagerSystem = World.GetExistingSystem<GameDataManagerSystem>();
            inputSystem = World.GetExistingSystem<InputSystem>();
        }



        protected override void OnUpdate()
        {
            #region stage state
            //load event
            Entities.ForEach((Entity entity, ref ServiceStage serviceStage, ref PrototypeStage prototypeStage, ref EnterLoadStateEvent loadEvent) =>
            {
                EntityUtil.SetActive(EntityManager, prototypeStage.BattleWindowEntity, false);

                ////create player
                //var playerEntity = GameUtil.CreateCreature(EntityManager, gameDataManagerSystem, prototypeStage.PlayerData);

                //var translation = new Translation { Value = new float3(0f, 0f, -2f) };
                //EntityManager.SetComponentData(playerEntity, translation);

                //prototypeStage.PlayerEntity = playerEntity;

                //CommonUtil.SetState<LoadedState, LoadedEvent, LoadState>(World, entity);
                EntityUtil.SetState<LoadedState, EnterLoadedStateEvent, LoadState, ExitLoadStateEvent>(EntityManager, entity);
            });

            //unload event
            Entities.ForEach((Entity entity, ref ServiceStage serviceStage, ref PrototypeStage prototypeStage, ref EnterUnloadStateEvent unloadEvent) =>
            {
                //CommonUtil.SetState<NoneState, UnloadedEvent, UnloadState>(World, entity);
                EntityUtil.SetState<UnloadedState, EnterUnloadedStateEvent, UnloadState, ExitUnloadStateEvent>(EntityManager, entity);
            });

            //loaded event
            Entities.ForEach((Entity entity, ref PrototypeStage prototypeStage, ref EnterLoadedStateEvent loadedEvent) =>
            {
                var windowEntity = prototypeStage.PrototypeWindowEntity;
                //var prototypeWindow = EntityManager.GetComponentData<PrototypeWindow>(windowEntity);
                EntityUtil.SetActive(EntityManager, windowEntity, true);
            });

            //loaded state
            Entities.ForEach((Entity entity, ref PrototypeStage prototypeStage, ref LoadedState loadedState) =>
            {
                var windowEntity = prototypeStage.PrototypeWindowEntity;
                var battleWindowEntity = prototypeStage.BattleWindowEntity;
                var prototypeWindow = EntityManager.GetComponentData<PrototypeWindow>(windowEntity);
                var startButtonEntity = prototypeWindow.StartButtonEntity;

                if (EntityManager.HasComponent<UIButtonClickEvent>(startButtonEntity))
                {
                    //disable the prototype window
                    EntityUtil.SetActive(EntityManager, windowEntity, false);

                    //disable the prototype window
                    EntityUtil.SetActive(EntityManager, battleWindowEntity, true);

                    //create player
                    var playerEntity = GameUtil.CreateCreature(EntityManager, gameDataManagerSystem, prototypeStage.PlayerData);

                    var translation = new Translation { Value = new float3(-4f, 0f, 0f) };
                    EntityManager.SetComponentData(playerEntity, translation);

                    //create play card deck
                    var cardDeckBuffer = EntityManager.GetBuffer<CardDeckElement>(playerEntity);

                    var baseDeckEntity = cardDeckBuffer[(byte)ECardDeckType.BaseDeck].DeckEntity;
                    var playDeckEntity = cardDeckBuffer[(byte)ECardDeckType.PlayDeck].DeckEntity;
                    var drawDeckEntity = cardDeckBuffer[(byte)ECardDeckType.DrawDeck].DeckEntity;

                    var baseCardElementBuffer = EntityManager.GetBuffer<CardElement>(baseDeckEntity);

                    var cardElementArray = baseCardElementBuffer.ToNativeArray(Allocator.Temp);
                    for (int i = 0; i < cardElementArray.Length; i++)
                    {
                        var cardEntity = EntityManager.Instantiate(cardElementArray[i].CardEntity);
                        cardElementArray[i] = new CardElement { CardEntity = cardEntity };

                        //create view from template
                        var cardViewEntity = GameUtil.CreateCardView(EntityManager, gameDataManagerSystem, cardEntity);
                        EntityUtil.SetActive(EntityManager, cardViewEntity, false);
                    }

                    var playCardElementBuffer = EntityManager.GetBuffer<CardElement>(playDeckEntity);
                    playCardElementBuffer.AddRange(cardElementArray);

                    var drawCardElementBuffer = EntityManager.GetBuffer<CardElement>(drawDeckEntity);
                    drawCardElementBuffer.AddRange(cardElementArray);

                    cardElementArray.Dispose();

                    //create battle entity
                    var battleEntity = EntityManager.CreateEntity();

                    var battle = new Battle();
                    battle.EnemyDatasAssetRef = prototypeStage.EnemyDatasAssetRef;
                    battle.PlayerEntity = playerEntity;
                    battle.BattleWindowEntity = battleWindowEntity;

                    EntityManager.AddComponentData<Battle>(battleEntity, battle);

                    EntityManager.AddComponent<FSMState>(battleEntity);

                    CommonUtil.SetFSMStateCmd<BattleStartState, EnterBattleStartStateEvent, ExitBattleStartStateEvent>(EntityManager, battleEntity, (byte)EBattleState.BattleStart);
                    //EntityUtil.AddState<BattleStartState, EnterBattleStartStateEvent>(EntityManager, entity);
                    prototypeStage.BattleEntity = battleEntity;
                }

                //if (inputSystem.GetMouseButtonDown(0))
                //{
                //    Entities
                //    .ForEach((Entity textEntity, ref TextMesh textMesh) =>
                //    {
                //        if (textMesh.Text.Equals("Hello"))
                //            textMesh.Text = "World";
                //        else
                //            textMesh.Text = "Hello";

                //    });

                //}
            });
            #endregion

            //#region battle state
            ////battle start event
            //Entities.ForEach((Entity entity, ref PrototypeStage prototypeStage, ref EnterBattleStartStateEvent battleStartEvent) =>
            //{
            //    var windowEntity = prototypeStage.GamePlayWindowEntity;
            //    var gamePlayWindow = EntityManager.GetComponentData<GamePlayWindow>(windowEntity);

            //    EntityUtil.SetActive(EntityManager, gamePlayWindow.StartButtonEntity, false);

            //    //create monster
            //    var monsterEntity = GameUtil.CreateCreature(EntityManager, gameDataManagerSystem, prototypeStage.MonsterData);

            //    var translation = new Translation { Value = new float3(0f, 0f, 0f) };
            //    EntityManager.SetComponentData(monsterEntity, translation);

            //    prototypeStage.MonsterEntity = monsterEntity;

            //    //create play card deck
            //    var playerEntity = prototypeStage.PlayerEntity;
            //    var cardDeckBuffer = EntityManager.GetBuffer<CardDeckElement>(playerEntity);

            //    var baseDeckEntity = cardDeckBuffer[(byte)ECardDeckType.BaseDeck].DeckEntity;
            //    var playDeckEntity = cardDeckBuffer[(byte)ECardDeckType.PlayDeck].DeckEntity;
            //    var drawDeckEntity = cardDeckBuffer[(byte)ECardDeckType.DrawDeck].DeckEntity;

            //    var baseCardElementBuffer = EntityManager.GetBuffer<CardElement>(baseDeckEntity);

            //    var cardElementArray = baseCardElementBuffer.ToNativeArray(Allocator.Temp);
            //    for (int i = 0; i < cardElementArray.Length; i++)
            //    {
            //        var cardEntity = EntityManager.Instantiate(cardElementArray[i].CardEntity);
            //        cardElementArray[i] = new CardElement { CardEntity = cardEntity };

            //        //create view from template
            //        var cardViewEntity = GameUtil.CreateCardView(EntityManager, gameDataManagerSystem, cardEntity);
            //        EntityUtil.SetActive(EntityManager, cardViewEntity, false);
            //    }

            //    var playCardElementBuffer = EntityManager.GetBuffer<CardElement>(playDeckEntity);
            //    playCardElementBuffer.AddRange(cardElementArray);

            //    var drawCardElementBuffer = EntityManager.GetBuffer<CardElement>(drawDeckEntity);
            //    drawCardElementBuffer.AddRange(cardElementArray);

            //    cardElementArray.Dispose();

            //    EntityUtil.SetState<BattleState, EnterBattleStateEvent, BattleStartState, ExitBattleStartStateEvent>(EntityManager, entity);
            //});

            ////battle event
            //Entities.ForEach((Entity entity, ref PrototypeStage prototypeStage, ref EnterBattleStateEvent enterEvent) =>
            //{
            //    //enter player round
            //    EntityUtil.AddState<PlayerTurnStartState, EnterPlayerTurnStartStateEvent>(EntityManager, entity);
            //});
            //#endregion

            //#region player round state
            ////player round start event
            //Entities.ForEach((Entity entity, ref PrototypeStage prototypeStage, ref EnterPlayerTurnStartStateEvent enterEvent) =>
            //{
            //    var windowEntity = prototypeStage.GamePlayWindowEntity;
            //    var gamePlayWindow = EntityManager.GetComponentData<GamePlayWindow>(windowEntity);

            //    //draw cards to hand
            //    var playerEntity = prototypeStage.PlayerEntity;
            //    var cardDeckBuffer = EntityManager.GetBuffer<CardDeckElement>(playerEntity);

            //    var drawDeckEntity = cardDeckBuffer[(byte)ECardDeckType.DrawDeck].DeckEntity;
            //    var handDeckEntity = cardDeckBuffer[(byte)ECardDeckType.HandDeck].DeckEntity;

            //    var drawCardElementBuffer = EntityManager.GetBuffer<CardElement>(drawDeckEntity);
            //    int drawNum = 1;
            //    var cardElementArray = new NativeArray<CardElement>(drawNum, Allocator.Temp);

            //    for (int i = 0; i < drawNum; i++)
            //    {
            //        if (drawCardElementBuffer.Length > 0)
            //        {
            //            var cardElement = drawCardElementBuffer[0];
            //            cardElementArray[i] = cardElement;
            //            drawCardElementBuffer.RemoveAt(0);
            //        }
            //    }

            //    //add card view to hand card panel
            //    for (int i = 0; i < drawNum; i++)
            //    {
            //        var cardEntity = cardElementArray[i].CardEntity;
            //        if(cardEntity != Entity.Null)
            //        {
            //            var card = EntityManager.GetComponentData<Card>(cardEntity);
            //            var cardViewEntity = card.ViewEntity;

            //            EntityUtil.SetActive(EntityManager, cardViewEntity, true);
            //            EntityUtil.SetParent(EntityManager, cardViewEntity, gamePlayWindow.HandCardPanelEntity);
            //        }
            //    }

            //    var handCardElementBuffer = EntityManager.GetBuffer<CardElement>(handDeckEntity);
            //    handCardElementBuffer.AddRange(cardElementArray);

            //    cardElementArray.Dispose();
            //});

            //#endregion
        }
    }
}