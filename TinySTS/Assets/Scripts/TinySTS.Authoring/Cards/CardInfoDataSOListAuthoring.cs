using PFrame.Entities;
using PFrame.Entities.Authoring;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace TinySTS.Authoring
{
    [GameDataAuthoring(Name = "CardInfoData")]
    public class CardInfoDataSOListConverter : AGameDataSOListConverter<CardInfoData, CardInfoDataSO>
    {
        public override byte TypeId => (byte)EGameDataType.CardInfo;

        public override CardInfoData ConvertGameData(GameObject gameObject, Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem, CardInfoDataSO dataSO)
        {
            var data = new CardInfoData();
            data.Id = dataSO.Id;
            data.Name = dataSO.Name;

            data.Type = dataSO.Type;
            data.GroupType = dataSO.GroupType;
            //data.MaxLevel = dataSO.MaxLevel;

            ushort[] dataIds = dataSO.Datas.Select((d) => d == null ? (ushort)0 : d.DataId).ToArray();
            data.DataIdsAssetRef = EntityUtil.CreateArrayAssetRef(dataIds);
            //data.DataIdsAssetRef = EntityAuthoringUtil.CreateEntitiesAsset(dataSO.Datas);

            if(dataSO.SkillInfoData != null)
                data.SkillInfoDataId = dataSO.SkillInfoData.DataId;

            return data;
        }

        public override void DeclareReferencedPrefab(List<GameObject> referencedPrefabs, CardInfoDataSO dataSO)
        {
        }
    }

    public class CardInfoDataSOListAuthoring : AGameDataSOListAuthoring<CardInfoData, CardInfoDataSO, CardInfoDataSOListConverter>
    {
    }
}
