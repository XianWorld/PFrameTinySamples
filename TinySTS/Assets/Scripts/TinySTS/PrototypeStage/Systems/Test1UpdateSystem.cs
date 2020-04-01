using PFrame.Entities;
using PFrame.Tiny;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Tiny.Input;
using Unity.Transforms;

namespace TinySTS
{
    [UpdateInGroup(typeof(ServiceUpdateSystemGroup))]
    [UpdateAfter(typeof(ServiceStageUpdateSystem))]
    [UpdateBefore(typeof(ServiceUpdateSystem))]
    public class Test1UpdateSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, ref Test1 test1) =>
            {
                LogUtil.LogFormat("IsCreated: {0}\r\n", test1.ShortArrayAssetRef.IsCreated);
                ref var shortArray = ref test1.ShortArrayAssetRef.Value.Array;
                LogUtil.LogFormat("short value: {0}, {1}\r\n", shortArray[1], test1.ShortArrayAssetRef.Value.Count);
            });
        }
    }
}