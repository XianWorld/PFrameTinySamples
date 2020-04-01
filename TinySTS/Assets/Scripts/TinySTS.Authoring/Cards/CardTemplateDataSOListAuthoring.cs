using PFrame.Entities.Authoring;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace TinySTS.Authoring
{
    [GameDataAuthoring(Name = "CardTemplateData")]
    public class CardTemplateDataSOListConverter : AGameDataSOListConverter<CardTemplateData, CardTemplateDataSO>
    {
        public override byte TypeId => (byte)EGameDataType.CardTemplate;

        public override CardTemplateData ConvertGameData(GameObject gameObject, Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem, CardTemplateDataSO dataSO)
        {
            var data = new CardTemplateData();
            data.Id = dataSO.Id;
            data.Name = dataSO.Name;

            data.Type = dataSO.Type;
            data.GroupType = dataSO.GroupType;
            data.Prefab = conversionSystem.GetPrimaryEntity(dataSO.Prefab);

            return data;
        }

        public override void DeclareReferencedPrefab(List<GameObject> referencedPrefabs, CardTemplateDataSO dataSO)
        {
            referencedPrefabs.Add(dataSO.Prefab);
        }
    }

    public class CardTemplateDataSOListAuthoring : AGameDataSOListAuthoring<CardTemplateData, CardTemplateDataSO, CardTemplateDataSOListConverter>
    {
    }
}
