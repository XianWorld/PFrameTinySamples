using PFrame.Entities;
using Unity.Collections;
using Unity.Entities;

namespace TinySTS
{
    /**
     * Creature Entity
     * [C]Creature
     * [B]StatElement
     * [B]SkillElement
     * [B]BuffElement
     * [B]DeckElement
     **/
    public struct Creature : IComponentData
    {
        public ushort DataId;
        public NativeString32 Name;
        public ushort Level;
    }

    public struct CardElement : IBufferElementData
    {
        public Entity CardEntity;
    }
}
