using PFrame.Entities;
using Unity.Collections;
using Unity.Entities;

namespace TinySTS
{
    public struct SkillInfoData : IBufferElementData, IGameData
    {
        public ushort Id;
        public NativeString32 Name;
        public ushort DataId => Id;
        public byte DataType => (byte)EGameDataType.SkillInfo;
        public string DataName => Name.ToString();

        //public byte MaxLevel;

        public BlobAssetReference<UshortArrayAsset> DataIdsAssetRef;
    }
}
