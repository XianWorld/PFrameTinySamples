using PFrame.Entities;

namespace TinySTS
{
    public enum EGameDataType : byte
    {
        Effect = ECommonGameDataType.Max,
        Skill,
        Buff,
        Card,
        CardInfo,
        CardLibrary,
        CardDeck,
        CardTemplate,
        Creature,
        SkillInfo,
    }
}
