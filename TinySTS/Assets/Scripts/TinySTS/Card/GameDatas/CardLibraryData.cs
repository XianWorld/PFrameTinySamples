using PFrame.Entities;
using Unity.Collections;
using Unity.Entities;

namespace TinySTS
{
    public struct CardLibraryData : IBufferElementData, IGameData
    {
        public ushort Id;
        public NativeString32 Name;
        public ushort DataId => Id;
        public byte DataType => (byte)EGameDataType.Card;
        public string DataName => Name.ToString();

        //public ushort CardNum;

        public BlobAssetReference<UshortArrayAsset> DataIdsAssetRef;
    }
}
