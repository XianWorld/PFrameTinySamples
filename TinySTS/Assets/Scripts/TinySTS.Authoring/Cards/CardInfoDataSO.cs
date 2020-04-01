using PFrame.Entities.Authoring;
using UnityEngine;

namespace TinySTS.Authoring
{
    [CreateAssetMenu(fileName = "CardInfoData_", menuName = "TinySTS/CardInfoData")]
    public class CardInfoDataSO : AGameDataSO
    {
        public override byte DataType => (byte)EGameDataType.CardInfo;

        public ECardType Type;

        public ECardGroupType GroupType;

        //public byte MaxLevel;

        public CardDataSO[] Datas;

        public SkillInfoDataSO SkillInfoData;
    }
}
