using PFrame.Entities;
using Unity.Collections;
using Unity.Entities;

namespace TinySTS
{
    public enum ETargetType : byte
    {
        Self,
        All,
        Random,
        Assigned,
        //None,
        //Self,
        //All,
        //AllFriendly,
        //AllHostile,
        //Random,
        //RandomFriendly,
        //RandomHostile,
        //Friendly,
        //Hostile
    }

    //public enum ETargetIndicatorType : byte
    //{
    //    Target,
    //    Random,
    //    All,
    //}

    public struct Skill : IComponentData
    {
        public ushort DataId;
        public ushort InfoDataId;
        //public NativeString32 Name;
        public byte Level;

        public ETargetType TargetType;
        public bool IsSpecificRelation;
        public ERelationType TargetRelationType;
        public bool IsWithoutSelf;
        public byte TargetNum;

        public Entity OwnerEntity;
    }

    //public struct SkillElement : IBufferElementData
    //{
    //    public Entity SkillEntity;
    //}

    public struct SkillEffectElement : IBufferElementData
    {
        public EEffectType EffectType;
        public Entity EffectEntity;
    }
}
