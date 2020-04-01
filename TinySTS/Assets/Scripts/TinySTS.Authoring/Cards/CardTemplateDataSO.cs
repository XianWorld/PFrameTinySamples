using PFrame.Entities.Authoring;
using UnityEngine;

namespace TinySTS.Authoring
{
    [CreateAssetMenu(fileName = "CardTemplateData_", menuName = "TinySTS/CardTemplateData")]
    public class CardTemplateDataSO : AGameDataSO
    {
        public override byte DataType => (byte)EGameDataType.CardTemplate;

        public ECardType Type;
        public ECardGroupType GroupType;

        public GameObject Prefab;
    }
}
