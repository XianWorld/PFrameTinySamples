using PFrame.Entities.Authoring;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace TinySTS.Authoring
{
    [GameDataAuthoring(Name = "EffectData")]
    public class EffectDataSOListConverter : AGameDataSOListConverter<EffectData, EffectDataSO>
    {
        public override byte TypeId => (byte)EGameDataType.Skill;

        public override EffectData ConvertGameData(GameObject gameObject, Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem, EffectDataSO dataSO)
        {
            var data = new EffectData();
            data.Id = dataSO.Id;
            data.Name = dataSO.Name;

            data.Damage = dataSO.Damage;
            data.CreatureStatType = dataSO.CreatureStatType;
            data.BonusType = dataSO.BonusType;
            data.BonusValue = dataSO.BonusValue;

            return data;
        }

        public override void DeclareReferencedPrefab(List<GameObject> referencedPrefabs, EffectDataSO dataSO)
        {
        }
    }

    public class EffectDataSOListAuthoring : AGameDataSOListAuthoring<EffectData, EffectDataSO, EffectDataSOListConverter>
    {
    }
}
