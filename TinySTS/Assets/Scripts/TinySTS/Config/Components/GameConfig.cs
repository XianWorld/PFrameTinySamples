using Unity.Entities;

namespace TinySTS
{
    [GenerateAuthoringComponent]
    public struct GameConfig : IComponentData
    {
        public Entity TargetIndicatorPrefab;
    }
}
