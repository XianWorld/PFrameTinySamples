using PFrame.Entities;
using System;
using Unity.Collections;
using Unity.Entities;

namespace TinySTS
{
    public struct CardData : IBufferElementData, IGameData
    {
        public ushort Id;
        public NativeString32 Name;
        public ushort DataId => Id;
        public byte DataType => (byte)EGameDataType.Card;
        public string DataName => Name.ToString();

        public ECardType Type;

        public Entity Prefab;
    }
}
