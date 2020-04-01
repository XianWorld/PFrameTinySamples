using PFrame.Entities;
using PFrame.Entities.Authoring;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace TinySTS.Authoring
{
    [RequiresEntityConversion]
    public class MainServiceAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
	{
        public byte Id;
	    public EStageType StartStage = EStageType.Splash;

        //public ServiceStageDataSO[] Datas = new ServiceStageDataSO[(byte)EStageType.Max];
        public ushort[] DataIds = new ushort[(byte)EStageType.Max];

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
	    {
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	    {
	        var serviceComp = new Service()
	        {
	            Id = Id
            };
	        dstManager.AddComponentData(entity, serviceComp);

            var assetRef = EntityUtil.CreateArrayAssetRef(DataIds);
            //var assetRef = EntityAuthoringUtil.CreateEntitiesAsset(DataIds);

            var mainService = new MainService()
            {
                StartStage = (byte)StartStage,
                DataIdsAssetRef = assetRef
            };
            dstManager.AddComponentData(entity, mainService);
        }
    }
}
