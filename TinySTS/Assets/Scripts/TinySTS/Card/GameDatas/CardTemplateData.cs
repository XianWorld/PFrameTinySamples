using PFrame.Entities;
using Unity.Collections;
using Unity.Entities;

namespace TinySTS
{
    public struct CardTemplateData : IBufferElementData, IGameData
    {
        public ushort Id;
        public NativeString32 Name;
        public ushort DataId => Id;
        public byte DataType => (byte)EGameDataType.CardTemplate;
        public string DataName => Name.ToString();

        public ECardType Type;
        public ECardGroupType GroupType;

        public Entity Prefab;
    }
}
