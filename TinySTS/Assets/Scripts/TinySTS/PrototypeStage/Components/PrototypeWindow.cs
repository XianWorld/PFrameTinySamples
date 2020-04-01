using Unity.Entities;

namespace TinySTS
{
    [GenerateAuthoringComponent]
    public struct PrototypeWindow : IComponentData
    {
        public Entity StartButtonEntity;
    }
}