using PFrame.Entities;
using Unity.Collections;
using Unity.Entities;

namespace TinySTS
{
    [GenerateAuthoringComponent]
    public struct DamageEffect : IComponentData
    {
        public int Damage;
    }

    //public struct DamageEffectData : IBufferElementData, IGameData
    //{
    //    public ushort Id;
    //    public NativeString32 Name;
    //    public ushort DataId => Id;
    //    public byte DataType => (byte)EGameDataType.Effect;
    //    public string DataName => Name.ToString();

    //    public int Damage;
    //}
}
