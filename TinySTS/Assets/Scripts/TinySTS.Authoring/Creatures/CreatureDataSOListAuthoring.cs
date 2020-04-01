using PFrame.Entities;
using PFrame.Entities.Authoring;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace TinySTS.Authoring
{
    [GameDataAuthoring(Name = "CreatureData")]
    public class CreatureDataSOListConverter : AGameDataSOListConverter<CreatureData, CreatureDataSO>
    {
        public override byte TypeId => (byte)EGameDataType.Creature;

        public override CreatureData ConvertGameData(GameObject gameObject, Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem, CreatureDataSO dataSO)
        {
            var data = new CreatureData();
            data.Id = dataSO.Id;
            data.Name = dataSO.Name;

            if(dataSO.CardLibraryData != null)
                data.CardLibraryDataId = dataSO.CardLibraryData.DataId;

            if(dataSO.StartDeckData != null)
                data.StartDeckDataId = dataSO.StartDeckData.DataId;

            data.MinLevel = dataSO.MinLevel;
            data.MaxLevel = dataSO.MaxLevel;

            data.StatValuesAssetRef = EntityUtil.CreateArrayAssetRef(dataSO.StatValues);
            data.StatAddValuesAssetRef = EntityUtil.CreateArrayAssetRef(dataSO.StatAddValues);

            ////stat level asset
            //var builder = new BlobBuilder(Allocator.Temp);
            //ref var root = ref builder.ConstructRoot<StatLevelAsset>();

            //root.MinLevel = dataSO.MinLevel;
            //root.MaxLevel = dataSO.MaxLevel;

            //var len = dataSO.MaxLevel - dataSO.MinLevel;
            //var array = builder.Allocate(ref root.StatValues, len);

            //for (int i = 0; i < len; i++)
            //{
            //    array[i] = dataSO.StatValues[i];
            //}

            //var assetRef = builder.CreateBlobAssetReference<GameDataIdsAsset>(Allocator.Persistent);
            //builder.Dispose();


            data.Prefab = conversionSystem.GetPrimaryEntity(dataSO.Prefab);

            return data;
        }

        public override void DeclareReferencedPrefab(List<GameObject> referencedPrefabs, CreatureDataSO dataSO)
        {
            referencedPrefabs.Add(dataSO.Prefab);
        }
    }

    public class CreatureDataSOListAuthoring : AGameDataSOListAuthoring<CreatureData, CreatureDataSO, CreatureDataSOListConverter>
    {
    }
}
