using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace TinySTS
{
    public enum ECardType : byte
    {
        Attack,
        Skill,
        Power,
        Curse,
        Status
    }

    public enum ECardGroupType : byte
    {
        Red,
        Green,
        Colorless,
        Curses,
        Statuses,
    }

    public enum ECardDeckType : byte
    {
        BaseDeck,
        PlayDeck,
        HandDeck,
        DrawDeck,
        DiscardDeck,
        ConsumeDeck,
        Max
    }

    /**
     * CardEntity
     * [C]Card
     * [C]CardTag
     * [C]Card Deck Type
     * [B]StatElement
     * [C]Skill
     * [C]TargetType
     * [B]SkillEffectElement
     * [C]Buff
     * [B]BuffEffectElement
     **/
    public struct Card : IComponentData

    {
        public ushort Id;
        public ushort DataId;
        public ushort InfoDataId;

        public NativeString32 Name;
        public byte Level;

        public Entity OwnerEntity;

        public Entity ViewEntity;
    }

    #region Card tag
    public struct AttackCardTag : IComponentData
    {

    }

    public struct SkillCardTag : IComponentData
    {

    }

    public struct AblilityCardTag : IComponentData
    {

    }

    public struct StateCardTag : IComponentData
    {

    }
    #endregion

    #region Card Deck
    public struct HandDeck : IComponentData
    {

    }

    public struct DrawDeck : IComponentData
    {

    }

    public struct DiscardDeck : IComponentData
    {

    }

    public struct ConsumeDeck : IComponentData
    {

    }
    #endregion

    public struct CardDeck : IComponentData
    {
        public ECardDeckType Type;
        public Entity OwnerEntity;
    }

    public struct CardDeckElement : IBufferElementData
    {
        public Entity DeckEntity;
    }
}
