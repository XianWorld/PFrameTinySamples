using Unity.Entities;

namespace TinySTS
{
    [GenerateAuthoringComponent]
    public struct BattleWindow : IComponentData
    {
        public Entity HandCardPanelEntity;
        public Entity EndTurnButtonEntity;
        public Entity ManaTextEntity;

        public Entity DrawDeckButtonEntity;
        public Entity DrawDeckNumTextEntity;

        public Entity DiscardDeckButtonEntity;
        public Entity DiscardDeckNumTextEntity;

    }
}