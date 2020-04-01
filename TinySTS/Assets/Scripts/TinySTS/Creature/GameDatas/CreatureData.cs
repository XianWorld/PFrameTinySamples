using PFrame.Entities;
using Unity.Collections;
using Unity.Entities;

namespace TinySTS
{
    public struct CreatureData : IBufferElementData, IGameData
    {
        public ushort Id;
        public NativeString32 Name;
        public ushort DataId => Id;
        public byte DataType => (byte)EGameDataType.Creature;
        public string DataName => Name.ToString();

        public ushort CardLibraryDataId;

        public ushort StartDeckDataId;

        //public BlobAssetReference<StatLevelAsset> StatLevelAssetRef;

        public ushort MinLevel;
        public ushort MaxLevel;

        public BlobAssetReference<ShortArrayAsset> StatValuesAssetRef;
        public BlobAssetReference<ShortArrayAsset> StatAddValuesAssetRef;

        public Entity Prefab;
    }

    //public struct StatLevelAsset
    //{
    //    public ushort MinLevel;
    //    public ushort MaxLevel;
    //    public BlobArray<short> StatValues;
    //    public BlobArray<short> StatAddValues;
    //}

    //public struct CreatureDefData
    //{
    //    public ushort DataId;
    //    public byte Level;
    //}
}
