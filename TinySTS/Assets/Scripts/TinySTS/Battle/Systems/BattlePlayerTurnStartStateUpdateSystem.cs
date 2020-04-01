using PFrame.Entities;
using Unity.Collections;
using Unity.Entities;

namespace TinySTS
{
    [UpdateInGroup(typeof(ServiceUpdateSystemGroup))]
    public class BattlePlayerTurnStartStateUpdateSystem : ComponentSystem
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<Battle>();
        }

        protected override void OnUpdate()
        {
            //player turn start event
            Entities.ForEach((Entity entity, ref Battle battle, ref EnterPlayerTurnStartStateEvent enterEvent) =>
            {
                var windowEntity = battle.BattleWindowEntity;
                var battleWindow = EntityManager.GetComponentData<BattleWindow>(windowEntity);

                //draw cards to hand
                var playerEntity = battle.PlayerEntity;
                var cardDeckBuffer = EntityManager.GetBuffer<CardDeckElement>(playerEntity);

                var drawDeckEntity = cardDeckBuffer[(byte)ECardDeckType.DrawDeck].DeckEntity;
                var handDeckEntity = cardDeckBuffer[(byte)ECardDeckType.HandDeck].DeckEntity;

                var drawCardElementBuffer = EntityManager.GetBuffer<CardElement>(drawDeckEntity);
                int drawNum = 5;
                var cardElementArray = new NativeArray<CardElement>(drawNum, Allocator.Temp);

                for (int i = 0; i < drawNum; i++)
                {
                    if (drawCardElementBuffer.Length > 0)
                    {
                        var cardElement = drawCardElementBuffer[0];
                        cardElementArray[i] = cardElement;
                        drawCardElementBuffer.RemoveAt(0);
                    }
                }

                //add card view to hand card panel
                for (int i = 0; i < drawNum; i++)
                {
                    var cardEntity = cardElementArray[i].CardEntity;
                    if (cardEntity != Entity.Null)
                    {
                        var card = EntityManager.GetComponentData<Card>(cardEntity);
                        var cardViewEntity = card.ViewEntity;

                        EntityUtil.SetActive(EntityManager, cardViewEntity, true);

                        GameUtil.AddCardViewElement(EntityManager, battleWindow.HandCardPanelEntity, cardViewEntity);

                        //EntityUtil.SetParent(EntityManager, cardViewEntity, battleWindow.HandCardPanelEntity);
                    }
                }

                var handCardElementBuffer = EntityManager.GetBuffer<CardElement>(handDeckEntity);
                handCardElementBuffer.AddRange(cardElementArray);

                cardElementArray.Dispose();

                battle.IsPlayerTurn = true;
                battle.CurrentExecutorIndex = 0;
                battle.CurrentExecutorEntity = playerEntity;

                CommonUtil.SetFSMStateCmd<PlayerSelectState, EnterPlayerSelectStateEvent, ExitPlayerSelectStateEvent>(EntityManager, entity, (byte)EBattleState.PlayerSelectCard);
            });
        }
    }
}