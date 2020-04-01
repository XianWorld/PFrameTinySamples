using PFrame.Entities.Authoring;
using UnityEngine;

namespace TinySTS.Authoring
{
    [CreateAssetMenu(fileName = "CardLibraryData_", menuName = "TinySTS/CardLibraryData")]
    public class CardLibraryDataSO : AGameDataSO
    {
        public override byte DataType => (byte)EGameDataType.CardLibrary;

        public CardInfoDataSO[] Datas;
    }
}
