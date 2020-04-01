using PFrame.Entities;
using Unity.Collections;
using Unity.Entities;

namespace TinySTS
{
    public struct Test1 : IComponentData
    {
        public BlobAssetReference<ShortArrayAsset> ShortArrayAssetRef;
    }
}