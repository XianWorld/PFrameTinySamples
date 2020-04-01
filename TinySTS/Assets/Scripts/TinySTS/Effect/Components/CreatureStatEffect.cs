using Unity.Entities;

namespace TinySTS
{
    public enum EBonusType
    {
        AddValue,
        AddRate
    }

    [GenerateAuthoringComponent]
    public struct CreatureStatEffect : IComponentData
    {
        public ECreatureStatType StatType;

        public EBonusType BonusType;

        public float Value;
    }
}
