//using PFrame.Entities.Authoring;
//using System.Collections.Generic;
//using UnityEngine;

//namespace TinySTS.Authoring
//{
//    [GameDataAuthoring(Name = "StageData")]
//    public class StageDataSOListConverter : AGameDataSOListConverter<StageData, StageDataSO>
//    {
//        public override byte TypeId => (byte)EGameDataType.Stage;

//        public override StageData GetGameData(GameObjectConversionSystem conversionSystem, StageDataSO dataSO)
//        {
//            var data = new StageData();
//            data.Id = dataSO.Id;
//            data.Name = dataSO.Name;

//            data.StageType = dataSO.StageType;
//            data.Prefab = conversionSystem.GetPrimaryEntity(dataSO.Prefab);

//            return data;
//        }

//        public override void DeclareReferencedPrefab(List<GameObject> referencedPrefabs, StageDataSO dataSO)
//        {
//            referencedPrefabs.Add(dataSO.Prefab);
//        }
//    }

//    public class StageDataSOListAuthoring : AGameDataSOListAuthoring<StageData, StageDataSO, StageDataSOListConverter>
//    {
//    }
//}
