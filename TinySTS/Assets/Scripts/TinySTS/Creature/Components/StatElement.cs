using System;
using Unity.Collections;
using Unity.Entities;

namespace TinySTS
{
    public enum ECreatureStatType : byte
    {
        Str,
        Dex,
        MaxLife,
        Life,
        MaxMana,
        Mana,
        Block,
        Max
    }

    public enum ESkillStatType : byte
    {
        ManaCost,
        Max,
    }

    [Serializable]
    public struct StatElement : IBufferElementData
    {
        public byte StatId;

        public short BaseValue;
        public short AddedAvlue;
        public short AddedRate;
        public short Value;

        //public FixedList32<StatModifier> Modifiers;
    }
}
