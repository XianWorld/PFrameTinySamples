using PFrame.Entities;
using Unity.Entities;

namespace TinySTS
{
    [UpdateInGroup(typeof(ServiceUpdateSystemGroup))]
    [UpdateAfter(typeof(ServiceUpdateSystem))]
    public class MainServiceUpdateSystem : ComponentSystem
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<MainService>();
        }

        protected override void OnUpdate()
        {
            var gameDataManagerSystem = World.GetExistingSystem<GameDataManagerSystem>();

            Entities.ForEach((Entity entity, ref ServiceStage serviceStageComp, ref EnterUnloadedStateEvent unloadedEventComp) =>
            {
                var stageId = serviceStageComp.Id;
                EntityManager.DestroyEntity(entity);
            });

            Entities.ForEach((Entity entity, ref Service service, ref MainService mainService, ref EnterLoadStateEvent loadEvent) =>
            {
                var stageId = service.StageId;
                var stageEntity = service.StageEntity;

                var stageDataId = mainService.DataIdsAssetRef.Value.Array[stageId];

                Entity prefab = Entity.Null;
                if (stageId >= 0 && stageId < (byte)EStageType.Max)
                {
                    if (gameDataManagerSystem.GetGameData<ServiceStageData>(stageDataId, out var data))
                    {
                        prefab = data.Prefab;

                        //LogUtil.LogFormat("BootstrapSystem: {0}, {1}, {2}\r\n", data.DataName, prefab, TypeManager.GetTypeIndex<LoadEvent>());
                    }
                    //prefab = mainService.StagePrefabsAssetRef.Value.Entities[stageId];
                }
                else
                //if (stageId == (byte)EStageType.Prototype)
                //{
                //    prefab = mainService.PrototypeStagePrefab;
                //}
                //else
                {
                    LogUtil.Log("No StageId: " + stageId);
                    return;
                }

                if(prefab == Entity.Null)
                {
                    LogUtil.Log("No Stage Prefab: " + stageId);
                    return;
                }

                stageEntity = EntityManager.Instantiate(prefab);
                service.StageEntity = stageEntity;

                var serviceStageComp = new ServiceStage
                {
                    Id = stageId,
                    ServiceEntity = entity
                };
                if (EntityManager.HasComponent<ServiceStage>(stageEntity))
                    EntityManager.SetComponentData(stageEntity, serviceStageComp);
                else
                    EntityManager.AddComponentData(stageEntity, serviceStageComp);

                EntityManager.AddComponent<LoadCmd>(stageEntity);
            });
        }
    }
}