using PFrame.Entities;
using PFrame.Entities.Authoring;
using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TinySTS.Authoring
{
    public class PrototypeStageAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public LevelData PlayerData;
        public LevelData[] EnemyDatas = new LevelData[1];

        public GameObject PrototypeWindow;
        public GameObject BattleWindow;

        //[SerializeField]
        //public DataSOLevelPair<CreatureDataSO> PlayerData;
        //public DataSOLevelPair<CreatureDataSO> MonsterData;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var serviceState = new ServiceStage()
            {
                Id = (byte)EStageType.Prototype,
            };
            dstManager.AddComponentData(entity, serviceState);

            var prototypeStage = new PrototypeStage()
            {
                PlayerData = PlayerData,
                EnemyDatasAssetRef = EntityUtil.CreateArrayAssetRef(EnemyDatas),
                PrototypeWindowEntity = conversionSystem.GetPrimaryEntity(PrototypeWindow),
                BattleWindowEntity = conversionSystem.GetPrimaryEntity(BattleWindow)
            };
            dstManager.AddComponentData(entity, prototypeStage);
        }
    }
}
