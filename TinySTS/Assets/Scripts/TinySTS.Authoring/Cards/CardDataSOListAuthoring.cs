using PFrame.Entities.Authoring;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace TinySTS.Authoring
{
    [GameDataAuthoring(Name = "CardData")]
    public class CardDataSOListConverter : AGameDataSOListConverter<CardData, CardDataSO>
    {
        public override byte TypeId => (byte)EGameDataType.Card;

        public override CardData ConvertGameData(GameObject gameObject, Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem, CardDataSO dataSO)
        {
            var data = new CardData();
            data.Id = dataSO.Id;
            data.Name = dataSO.Name;

            data.Type = dataSO.Type;
            data.Prefab = conversionSystem.GetPrimaryEntity(dataSO.Prefab);

            return data;
        }

        public override void DeclareReferencedPrefab(List<GameObject> referencedPrefabs, CardDataSO dataSO)
        {
            referencedPrefabs.Add(dataSO.Prefab);
        }
    }

    public class CardDataSOListAuthoring : AGameDataSOListAuthoring<CardData, CardDataSO, CardDataSOListConverter>
    {
    }
}
