//using PFrame.Entities;
//using PFrame.Tiny;
//using PFrame.Tiny.Tweens;
//using PFrame.Tiny.UI;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Mathematics;
//using Unity.Tiny.Input;
//using Unity.Transforms;

//namespace TinySTS
//{
//    [UpdateInGroup(typeof(ServiceUpdateSystemGroup))]
//    //[UpdateAfter(typeof(PrototypeStageUpdateSystem))]
//    public class BattleUpdateSystem : ComponentSystem
//    {
//        GameDataManagerSystem gameDataManagerSystem;
//        InputSystem inputSystem;
//        private TweenSystem tweenSystem;

//        protected override void OnCreate()
//        {
//            base.OnCreate();
//            RequireSingletonForUpdate<Battle>();
//        }

//        protected override void OnStartRunning()
//        {
//            base.OnStartRunning();

//            gameDataManagerSystem = World.GetExistingSystem<GameDataManagerSystem>();
//            inputSystem = World.GetExistingSystem<InputSystem>();
//            tweenSystem = World.GetExistingSystem<TweenSystem>();
//        }

//        protected override void OnUpdate()
//        {
//            //battle start event
//            Entities.ForEach((Entity entity, ref Battle battle, ref EnterBattleStartStateEvent battleStartEvent) =>
//            {
//                //create monsters
//                var assetRef = battle.EnemyDatasAssetRef;
//                var count = assetRef.Value.Count;
//                ref var array = ref assetRef.Value.Array;
//                var enemies = new NativeArray<EnemyTeamMember>(count, Allocator.Temp);
//                for (int i = 0; i < count; i++)
//                {
//                    var enemyData = array[i];
//                    var monsterEntity = GameUtil.CreateCreature(EntityManager, gameDataManagerSystem, enemyData);

//                    var translation = new Translation { Value = new float3(4f, 0f, 0f) };
//                    EntityManager.SetComponentData(monsterEntity, translation);

//                    enemies[i] = new EnemyTeamMember { CreatureEntity = monsterEntity };
//                }

//                var buffer = EntityManager.AddBuffer<EnemyTeamMember>(entity);
//                buffer.AddRange(enemies);

//                enemies.Dispose();
//                //battle.MonsterEntity = monsterEntity;

//                CommonUtil.SetFSMStateCmd<PlayerTurnStartState, EnterPlayerTurnStartStateEvent, ExitPlayerTurnStartStateEvent>(EntityManager, entity, (byte)EBattleState.PlayerTurnStart);
//            });

//            //player turn start event
//            Entities.ForEach((Entity entity, ref Battle battle, ref EnterPlayerTurnStartStateEvent enterEvent) =>
//            {
//                var windowEntity = battle.BattleWindowEntity;
//                var battleWindow = EntityManager.GetComponentData<BattleWindow>(windowEntity);

//                //draw cards to hand
//                var playerEntity = battle.PlayerEntity;
//                var cardDeckBuffer = EntityManager.GetBuffer<CardDeckElement>(playerEntity);

//                var drawDeckEntity = cardDeckBuffer[(byte)ECardDeckType.DrawDeck].DeckEntity;
//                var handDeckEntity = cardDeckBuffer[(byte)ECardDeckType.HandDeck].DeckEntity;

//                var drawCardElementBuffer = EntityManager.GetBuffer<CardElement>(drawDeckEntity);
//                int drawNum = 5;
//                var cardElementArray = new NativeArray<CardElement>(drawNum, Allocator.Temp);

//                for (int i = 0; i < drawNum; i++)
//                {
//                    if (drawCardElementBuffer.Length > 0)
//                    {
//                        var cardElement = drawCardElementBuffer[0];
//                        cardElementArray[i] = cardElement;
//                        drawCardElementBuffer.RemoveAt(0);
//                    }
//                }

//                //add card view to hand card panel
//                for (int i = 0; i < drawNum; i++)
//                {
//                    var cardEntity = cardElementArray[i].CardEntity;
//                    if (cardEntity != Entity.Null)
//                    {
//                        var card = EntityManager.GetComponentData<Card>(cardEntity);
//                        var cardViewEntity = card.ViewEntity;

//                        EntityUtil.SetActive(EntityManager, cardViewEntity, true);

//                        GameUtil.AddCardViewElement(EntityManager, battleWindow.HandCardPanelEntity, cardViewEntity);

//                        //EntityUtil.SetParent(EntityManager, cardViewEntity, battleWindow.HandCardPanelEntity);
//                    }
//                }

//                var handCardElementBuffer = EntityManager.GetBuffer<CardElement>(handDeckEntity);
//                handCardElementBuffer.AddRange(cardElementArray);

//                cardElementArray.Dispose();

//                CommonUtil.SetFSMStateCmd<PlayerSelectCardState, EnterPlayerSelectCardStateEvent, ExitPlayerSelectCardStateEvent>(EntityManager, entity, (byte)EBattleState.PlayerSelectCard);
//            });

//            //player select card
//            Entities.ForEach((Entity entity, ref Battle battle, ref PlayerSelectCardState state) =>
//            {
//                var windowEntity = battle.BattleWindowEntity;
//                var battleWindow = EntityManager.GetComponentData<BattleWindow>(windowEntity);
//                var endTurnButtonEntity = battleWindow.EndTurnButtonEntity;

//                if (EntityManager.HasComponent<UIButtonClickEvent>(endTurnButtonEntity))
//                {
//                    CommonUtil.SetFSMStateCmd<PlayerTurnEndState, EnterPlayerTurnEndStateEvent, ExitPlayerTurnEndStateEvent>(EntityManager, entity, (byte)EBattleState.PlayerTurnEnd);
//                }

//                Entities
//                    .WithNone<SelectedState>()
//                    .ForEach((Entity cardEntity, ref CardView cardView, ref PointerInteractable interactable, ref PointerEnterEvent enterEvent) =>
//                {
//                    //if (cardView.IsSelected)
//                    //    return;

//                    var typeInfo = TypeManager.GetTypeInfo<Translation>();
//                    var info = TypeManager.GetFieldArgs(typeInfo.TypeIndex, (int)PrimitiveFieldTypes.Float, 4);
//                    tweenSystem.AddTween<float>(
//                        cardEntity,
//                        info,
//                        0f,
//                        0.2f,
//                        0.1f
//                        );
//                });

//                Entities
//                    .WithNone<SelectedState>()
//                    .ForEach((Entity cardEntity, ref CardView cardView, ref PointerInteractable interactable, ref PointerExitEvent exitEvent) =>
//                {
//                    //if (cardView.IsSelected)
//                    //    return;

//                    var typeInfo = TypeManager.GetTypeInfo<Translation>();
//                    var info = TypeManager.GetFieldArgs(typeInfo.TypeIndex, (int)PrimitiveFieldTypes.Float, 4);
//                    tweenSystem.AddTween<float>(
//                        cardEntity,
//                        info,
//                        0.2f,
//                        0f,
//                        0.1f
//                        );
//                });

//                var cardViewQuery = GetEntityQuery(typeof(CardView), typeof(PointerClickEvent));
//                var num = cardViewQuery.CalculateEntityCount();
//                if (num > 0)
//                {
//                    var entities = cardViewQuery.ToEntityArray(Allocator.TempJob);
//                    var cardViewEntity = entities[0];

//                    var selectedCardViewEntity = battle.SelectedCardViewEntity;
//                    if (selectedCardViewEntity != Entity.Null)
//                    {
//                        EntityUtil.RemoveState<SelectedState, ExitSelectedStateEvent>(EntityManager, selectedCardViewEntity, true);
//                        //var cardView = EntityManager.GetComponentData<CardView>(cardViewEntity);
//                        //cardView.IsSelected = true;
//                        //var cardEntity = cardView.CardEntity;
//                        //EntityManager.SetComponentData(cardViewEntity, cardView);

//                        //var translation = EntityManager.GetComponentData<Translation>(cardViewEntity);
//                        //var pos = translation.Value;
//                        //pos.y = 0.3f;
//                        //translation.Value = pos;
//                        //EntityManager.SetComponentData(cardViewEntity, translation);

//                        battle.SelectedCardEntity = Entity.Null;
//                        battle.SelectedCardViewEntity = Entity.Null;

//                        //remove all indicators
//                        Entities.ForEach((Entity creatureEntity, ref Creature creature, ref TargetIndicator targetIndicator) =>
//                        {
//                            EntityManager.RemoveComponent<TargetIndicator>(creatureEntity);
//                        });
//                    }

//                    if (selectedCardViewEntity != cardViewEntity)
//                    {
//                        EntityUtil.AddState<SelectedState, EnterSelectedStateEvent>(EntityManager, cardViewEntity, true);

//                        var cardView = EntityManager.GetComponentData<CardView>(cardViewEntity);
//                        var cardEntity = cardView.CardEntity;

//                        battle.SelectedCardEntity = cardEntity;
//                        battle.SelectedCardViewEntity = cardViewEntity;

//                        var skill = EntityManager.GetComponentData<Skill>(cardEntity);
//                        var targetType = skill.TargetType;
//                        var targetNum = skill.TargetNum;

//                        //if (targetType == ETargetType.All)
//                        //{
//                        //    //add target indicators
//                        //    Entities.ForEach((Entity creatureEntity, ref Creature creature) =>
//                        //    {
//                        //        EntityManager.RemoveComponent<TargetIndicator>(creatureEntity);
//                        //    });
//                        //}
//                    }

//                    entities.Dispose();


//                    //CommonUtil.SetFSMStateCmd<PlayerSelectTargetState, EnterPlayerSelectTargetStateEvent, ExitPlayerSelectTargetStateEvent>(EntityManager, entity, (byte)EBattleState.PlayerSelectTarget);
//                }

//                //Entities.ForEach((Entity cardEntity, ref CardView cardView, ref Translation translation, ref PointerInteractable interactable, ref PointerClickEvent clickEvent) =>
//                //{
//                //    cardView.IsSelected = !cardView.IsSelected;

//                //    var pos = translation.Value;
//                //    if (cardView.IsSelected)
//                //        pos.y = 0.3f;
//                //    else
//                //        pos.y = 0f;

//                //    translation.Value = pos;
//                //});
//            });

//            ////player select target state enter event
//            //Entities.ForEach((Entity entity, ref Battle battle, ref EnterPlayerSelectTargetStateEvent enterEvent) =>
//            //{
//            //    //select target
//            //    var cardEntity = battle.SelectedCardEntity;
//            //    EntityManager.GetComponentData<Skill>(cardEntity);
//            //});

//            ////player select target state
//            //Entities.ForEach((Entity entity, ref Battle battle, ref PlayerSelectTargetState state) =>
//            //{

//            //});

//        }

//        private void SelectCard()
//        {

//        }
//    }
//}