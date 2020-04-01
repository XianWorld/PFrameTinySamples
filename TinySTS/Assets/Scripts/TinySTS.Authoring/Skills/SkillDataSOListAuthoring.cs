using PFrame.Entities;
using PFrame.Entities.Authoring;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace TinySTS.Authoring
{
    [GameDataAuthoring(Name = "SkillData")]
    public class SkillDataSOListConverter : AGameDataSOListConverter<SkillData, SkillDataSO>
    {
        public override byte TypeId => (byte)EGameDataType.Skill;

        public override SkillData ConvertGameData(GameObject gameObject, Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem, SkillDataSO dataSO)
        {
            var data = new SkillData();
            data.Id = dataSO.Id;
            data.Name = dataSO.Name;

            data.ManaCost = dataSO.ManaCost;
            data.TargetType = dataSO.TargetType;
            data.IsSpecificRelation = dataSO.IsSpecificRelation;
            data.TargetRelationType = dataSO.TargetRelationType;
            data.IsWithoutSelf = dataSO.IsWithoutSelf;
            data.TargetNum = dataSO.TargetNum;

            //data.Prefab = conversionSystem.GetPrimaryEntity(dataSO.Prefab);

            data.DataIdsAssetRef = EntityAuthoringUtil.CreateArrayAssetRef(dataSO.EffectDatas);

            return data;
        }

        public override void DeclareReferencedPrefab(List<GameObject> referencedPrefabs, SkillDataSO dataSO)
        {
            //referencedPrefabs.Add(dataSO.Prefab);
        }
    }

    public class SkillDataSOListAuthoring : AGameDataSOListAuthoring<SkillData, SkillDataSO, SkillDataSOListConverter>
    {
    }
}
