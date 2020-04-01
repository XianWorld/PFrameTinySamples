using Unity.Entities;

namespace TinySTS
{
    [GenerateAuthoringComponent]
    public struct TargetIndicator : IComponentData
    {
        public Entity SelfIndicatorEntity;
        public Entity AllIndicatorEntity;
        public Entity RandomIndicatorEntity;
        public Entity AssignedIndicatorEntity;
    }

    public struct TargetIndicatable : IComponentData
    {
        public Entity IndicatorEntity;
    }

    public struct TargetIndicatedState : IComponentData
    {
        public ETargetType IndicatorType;
    }
    public struct EnterTargetIndicatedStateEvent : IComponentData { }
    public struct ExitTargetIndicatedStateEvent : IComponentData { }
}
