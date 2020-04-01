using PFrame.Entities;
using Unity.Collections;
using Unity.Entities;

namespace TinySTS
{
    public struct SkillData : IBufferElementData, IGameData
    {
        public ushort Id;
        public NativeString32 Name;
        public ushort DataId => Id;
        public byte DataType => (byte)EGameDataType.Skill;
        public string DataName => Name.ToString();

        public byte ManaCost;

        public ETargetType TargetType;
        public bool IsSpecificRelation;
        public ERelationType TargetRelationType;
        public bool IsWithoutSelf;
        public byte TargetNum;

        //public Entity Prefab;

        //effect ids
        public BlobAssetReference<UshortArrayAsset> DataIdsAssetRef;
    }
}
