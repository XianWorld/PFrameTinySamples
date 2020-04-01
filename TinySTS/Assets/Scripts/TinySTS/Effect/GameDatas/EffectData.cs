using PFrame.Entities;
using Unity.Collections;
using Unity.Entities;

namespace TinySTS
{
    public struct EffectData : IBufferElementData, IGameData
    {
        public ushort Id;
        public NativeString32 Name;
        public ushort DataId => Id;
        public byte DataType => (byte)EGameDataType.Creature;
        public string DataName => Name.ToString();

        public EEffectType Type;
        public EEffectCastType CastType;

        //damage
        public int Damage;

        //creature stat
        public ECreatureStatType CreatureStatType;
        public EBonusType BonusType;
        public float BonusValue;

        //creature stat
        public ESkillStatType SkillStatType;

    }
}
