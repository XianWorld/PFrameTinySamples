using PFrame.Entities.Authoring;
using UnityEngine;

namespace TinySTS.Authoring
{
    [CreateAssetMenu(fileName = "CreatureData_", menuName = "TinySTS/CreatureData")]
    public class CreatureDataSO : AGameDataSO
    {
        public override byte DataType => (byte)EGameDataType.Creature;

        public CardLibraryDataSO CardLibraryData;

        public CardDeckDataSO StartDeckData;

        public ushort MinLevel;
        public ushort MaxLevel;
        public short[] StatValues = new short[(int)ECreatureStatType.Max];
        public short[] StatAddValues = new short[(int)ECreatureStatType.Max];

        public GameObject Prefab;

    }
}
