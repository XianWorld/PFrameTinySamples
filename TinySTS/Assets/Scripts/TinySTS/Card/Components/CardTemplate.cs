using Unity.Entities;

namespace TinySTS
{
    public struct CardTemplate : IComponentData
    {
        public Entity Background;
        public Entity NameText;
        public Entity TypeText;
        public Entity ManaText;
        public Entity DescriptionText;
    }

    public struct CardView : IComponentData
    {
        public Entity CardEntity;
        //public bool IsSelected;
    }
}
