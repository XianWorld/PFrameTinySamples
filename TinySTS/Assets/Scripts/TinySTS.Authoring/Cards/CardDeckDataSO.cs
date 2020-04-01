using PFrame.Entities.Authoring;
using System;
using UnityEngine;

namespace TinySTS.Authoring
{
    [CreateAssetMenu(fileName = "CardDeckData_", menuName = "TinySTS/CardDeckData")]
    public class CardDeckDataSO : AGameDataSO
    {
        public override byte DataType => (byte)EGameDataType.CardDeck;

        public CardDeckItemDataSO[] ItemDatas;
    }

    [Serializable]
    public class CardDeckItemDataSO
    {
        public CardInfoDataSO DataSO;
        public byte Level = 1;

        public byte Num = 1;
    }
}
