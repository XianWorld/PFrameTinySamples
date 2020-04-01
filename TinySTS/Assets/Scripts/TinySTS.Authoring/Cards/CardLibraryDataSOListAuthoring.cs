using PFrame.Entities;
using PFrame.Entities.Authoring;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using System.Linq;

namespace TinySTS.Authoring
{
    [GameDataAuthoring(Name = "CardLibraryData")]
    public class CardLibraryDataSOListConverter : AGameDataSOListConverter<CardLibraryData, CardLibraryDataSO>
    {
        public override byte TypeId => (byte)EGameDataType.CardLibrary;

        public override CardLibraryData ConvertGameData(GameObject gameObject, Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem, CardLibraryDataSO dataSO)
        {
            var data = new CardLibraryData();
            data.Id = dataSO.Id;
            data.Name = dataSO.Name;

            ushort[] dataIds = dataSO.Datas.Select((d) => d == null ? (ushort)0: d.DataId).ToArray();
            data.DataIdsAssetRef = EntityUtil.CreateArrayAssetRef(dataIds);

            //var len = dataSO.CardDatas.Length;
            //data.CardNum = (ushort)len;

            //var builder = new BlobBuilder(Allocator.Temp);
            //ref var root = ref builder.ConstructRoot<CardDataAsset>();

            //var array = builder.Allocate(ref root.CardDataIds, len);

            //for (int i = 0; i < len; i++)
            //{
            //    var cardData = dataSO.CardDatas[i];
            //    if (cardData != null)
            //    {
            //        array[i] = cardData.Id;
            //    }
            //}

            //data.DataAssetRef = builder.CreateBlobAssetReference<CardDataAsset>(Allocator.Persistent);
            //builder.Dispose();

            return data;
        }

        public override void DeclareReferencedPrefab(List<GameObject> referencedPrefabs, CardLibraryDataSO dataSO)
        {
        }
    }

    public class CardLibraryDataSOListAuthoring : AGameDataSOListAuthoring<CardLibraryData, CardLibraryDataSO, CardLibraryDataSOListConverter>
    {
    }
}
