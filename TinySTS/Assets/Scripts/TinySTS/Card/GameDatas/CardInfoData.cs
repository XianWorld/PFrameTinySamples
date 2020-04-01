using PFrame.Entities;
using System;
using Unity.Collections;
using Unity.Entities;

namespace TinySTS
{
    public struct CardInfoData : IBufferElementData, IGameData
    {
        public ushort Id;
        public NativeString32 Name;
        public ushort DataId => Id;
        public byte DataType => (byte)EGameDataType.Card;
        public string DataName => Name.ToString();

        public ECardType Type;

        public ECardGroupType GroupType;
        //public byte MaxLevel;

        public BlobAssetReference<UshortArrayAsset> DataIdsAssetRef;

        public ushort SkillInfoDataId;
    }
}
