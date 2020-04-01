using PFrame.Entities.Authoring;
using UnityEngine;

namespace TinySTS.Authoring
{
    [CreateAssetMenu(fileName = "CardData_", menuName = "TinySTS/CardData")]
    public class CardDataSO : AGameDataSO
    {
        public override byte DataType => (byte)EGameDataType.Card;

        public ECardType Type;

        public GameObject Prefab;
    }
}
