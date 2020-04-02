using PFrame.Entities;
using Unity.Entities;

namespace TinySTS
{
    [UpdateInGroup(typeof(ServiceUpdateSystemGroup))]
    public class BattlePlayerPlayCardStateUpdateSystem : ComponentSystem
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<Battle>();
        }

        protected override void OnUpdate()
        {
            //player turn start event
            Entities.ForEach((Entity entity, ref Battle battle, ref EnterPlayerPlayCardStateEvent enterEvent) =>
            {
                var windowEntity = battle.BattleWindowEntity;
                var battleWindow = EntityManager.GetComponentData<BattleWindow>(windowEntity);

                var playerEntity = battle.PlayerEntity;
                var selectedCardEntity = battle.SelectedCardEntity;
                var selectedCardViewEntity = battle.SelectedCardViewEntity;
                var selectedTargetEntity = battle.SelectedTargetEntity;

                //play card effects
                GameUtil.CastSkill(EntityManager, playerEntity, selectedTargetEntity, selectedCardEntity);

                //remove card view from hand deck panel

                GameUtil.RemoveCardViewElement(EntityManager, battleWindow.HandCardPanelEntity, selectedCardViewEntity);
                EntityUtil.SetActive(EntityManager, selectedCardViewEntity, false);
                //remove card from hand deck
                //var cardDeckBuffer = EntityManager.GetBuffer<CardDeckElement>(playerEntity);
                //var drawDeckEntity = cardDeckBuffer[(byte)ECardDeckType.DrawDeck].DeckEntity;
                //var handDeckEntity = cardDeckBuffer[(byte)ECardDeckType.HandDeck].DeckEntity;
                //var discardDeckEntity = cardDeckBuffer[(byte)ECardDeckType.DiscardDeck].DeckEntity;

                GameUtil.RemoveCardFromDeck(EntityManager, playerEntity, ECardDeckType.HandDeck, selectedCardEntity);

                //add card to discard deck
                GameUtil.AddCardToDeck(EntityManager, playerEntity, ECardDeckType.DiscardDeck, selectedCardEntity);

                battle.SelectedCardViewEntity = Entity.Null;
                battle.SelectedCardEntity = Entity.Null;
                battle.SelectedTargetEntity = Entity.Null;

                //clear target indicators

                CommonUtil.SetFSMStateCmd<PlayerSelectState, EnterPlayerSelectStateEvent, ExitPlayerSelectStateEvent>(EntityManager, entity, (byte)EBattleState.PlayerSelectCard);
            });
        }
    }
}