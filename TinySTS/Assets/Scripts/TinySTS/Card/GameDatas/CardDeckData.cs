using PFrame.Entities;
using System;
using Unity.Collections;
using Unity.Entities;

namespace TinySTS
{
    public struct CardDeckData : IBufferElementData, IGameData
    {
        public ushort Id;
        public NativeString32 Name;
        public ushort DataId => Id;
        public byte DataType => (byte)EGameDataType.CardDeck;
        public string DataName => Name.ToString();

        public BlobAssetReference<CardDeckItemDatasAsset> ItemDatasAssetRef;
    }

    [Serializable]
    public struct CardDeckItemData
    {
        public ushort Id;
        public byte Level;

        public byte Num;
    }

    public struct CardDeckItemDatasAsset
    {
        public ushort Count;
        public BlobArray<CardDeckItemData> ItemDatas;
    }
}
