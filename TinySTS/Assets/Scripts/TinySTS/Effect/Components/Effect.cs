using Unity.Collections;
using Unity.Entities;

namespace TinySTS
{
    public enum EEffectType : byte
    {
        Damage,
        CreatureStat,
        SkillStat,
    }

    public enum EEffectCastType : byte
    {
        Cast,
        Buff,
        Trigger
    }

    /**
     * Effect Entity
     * EffectType:
     * Trigger
     * XXX Effect
     **/
    public struct Effect : IComponentData
    {
        public ushort DataId;

        public EEffectType Type;
        public EEffectCastType CastType;

        public Entity OwnerEntity;
    }
}
