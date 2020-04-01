using PFrame.Entities;
using Unity.Entities;

namespace TinySTS
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class BootstrapSystem : ComponentSystem
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<Bootstrap>();
        }

        protected override void OnUpdate()
        {
            var gameDataManagerSystem = World.GetExistingSystem<GameDataManagerSystem>();

            Entities.ForEach((Entity entity, ref Bootstrap bootstrap) =>
            {
                Entities.ForEach((Entity serviceEntity, ref Service service, ref MainService mainService) =>
                {
                    //if (service.StageId == 0 && mainService.StartStage != 0)
                    {
                        var loadStageCmd = new LoadStageCmd
                        {
                            StageId = mainService.StartStage
                        };
                        EntityManager.AddComponentData(serviceEntity, loadStageCmd);
                    }
                });

                //if(gameDataManagerSystem.GetGameData<CardData>(1, out var cardData))
                //{
                //    var e1 = cardData.LevelAssetRef.Value.CardPrefabs[0];

                //    LogUtil.LogFormat("BootstrapSystem: {0}, {1}", cardData.DataName, e1);
                //}

                EntityManager.DestroyEntity(entity);
            });
        }
    }
}