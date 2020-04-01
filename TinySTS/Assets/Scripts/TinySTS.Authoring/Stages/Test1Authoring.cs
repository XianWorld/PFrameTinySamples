using PFrame.Entities;
using PFrame.Entities.Authoring;
using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TinySTS.Authoring
{
    public class Test1Authoring : MonoBehaviour, IConvertGameObjectToEntity
    {

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var test1 = new Test1();

            var values = new short[]
            {
                13,
                15
            };
            test1.ShortArrayAssetRef = EntityUtil.CreateArrayAssetRef(values);

            dstManager.AddComponentData(entity, test1);
        }
    }
}
