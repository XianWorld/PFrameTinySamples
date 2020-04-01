using PFrame.Entities;
using PFrame.Entities.Authoring;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace TinySTS.Authoring
{
    [GameDataAuthoring(Name = "SkillInfoData")]
    public class SkillInfoDataSOListConverter : AGameDataSOListConverter<SkillInfoData, SkillInfoDataSO>
    {
        public override byte TypeId => (byte)EGameDataType.CardInfo;

        public override SkillInfoData ConvertGameData(GameObject gameObject, Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem, SkillInfoDataSO dataSO)
        {
            var data = new SkillInfoData();
            data.Id = dataSO.Id;
            data.Name = dataSO.Name;

            ushort[] dataIds = dataSO.Datas.Select((d) => d == null ? (ushort)0 : d.DataId).ToArray();
            data.DataIdsAssetRef = EntityUtil.CreateArrayAssetRef(dataIds);
            //data.DataIdsAssetRef = EntityAuthoringUtil.CreateEntitiesAsset(dataSO.Datas);

            return data;
        }

        public override void DeclareReferencedPrefab(List<GameObject> referencedPrefabs, SkillInfoDataSO dataSO)
        {
        }
    }

    public class SkillInfoDataSOListAuthoring : AGameDataSOListAuthoring<SkillInfoData, SkillInfoDataSO, SkillInfoDataSOListConverter>
    {
    }
}
