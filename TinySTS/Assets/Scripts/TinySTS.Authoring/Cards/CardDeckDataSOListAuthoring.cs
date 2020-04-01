using PFrame.Entities.Authoring;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace TinySTS.Authoring
{
    [GameDataAuthoring(Name = "CardDeckData")]
    public class CardDeckDataSOListConverter : AGameDataSOListConverter<CardDeckData, CardDeckDataSO>
    {
        public override byte TypeId => (byte)EGameDataType.CardLibrary;

        public override CardDeckData ConvertGameData(GameObject gameObject, Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem, CardDeckDataSO dataSO)
        {
            var data = new CardDeckData();
            data.Id = dataSO.Id;
            data.Name = dataSO.Name;

            data.ItemDatasAssetRef = CreateDatasAssetRef(dataSO.ItemDatas);

            return data;
        }

        public override void DeclareReferencedPrefab(List<GameObject> referencedPrefabs, CardDeckDataSO dataSO)
        {
        }

        private BlobAssetReference<CardDeckItemDatasAsset> CreateDatasAssetRef(CardDeckItemDataSO[] itemDataSOs)
        {
            var builder = new BlobBuilder(Allocator.Temp);
            ref var root = ref builder.ConstructRoot<CardDeckItemDatasAsset>();

            var len = itemDataSOs.Length;
            var array = builder.Allocate(ref root.ItemDatas, len);

            root.Count = (ushort)len;
            for (int i = 0; i < len; i++)
            {
                var levelData = new CardDeckItemData();
                var dataSO = itemDataSOs[i].DataSO;
                if (dataSO != null)
                {
                    levelData.Id = dataSO.Id;
                    levelData.Level = itemDataSOs[i].Level;
                    levelData.Num = itemDataSOs[i].Num;
                }
                array[i] = levelData;
            }

            var assetRef = builder.CreateBlobAssetReference<CardDeckItemDatasAsset>(Allocator.Persistent);
            builder.Dispose();

            return assetRef;
        }

    }

    public class CardDeckDataSOListAuthoring : AGameDataSOListAuthoring<CardDeckData, CardDeckDataSO, CardDeckDataSOListConverter>
    {
    }
}