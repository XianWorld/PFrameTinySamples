using PFrame.Entities;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TinySTS
{
    [UpdateInGroup(typeof(ServiceUpdateSystemGroup))]
    public class BattleStartStateUpdateSystem : ComponentSystem
    {
        GameDataManagerSystem gameDataManagerSystem;
        GameConfig gameConfig;

        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<Battle>();
        }

        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            gameDataManagerSystem = World.GetExistingSystem<GameDataManagerSystem>();
            gameConfig = GetSingleton<GameConfig>();
        }

        protected override void OnUpdate()
        {
            var indicatorPrefab = gameConfig.TargetIndicatorPrefab;

            //battle start event
            Entities.ForEach((Entity entity, ref Battle battle, ref EnterBattleStartStateEvent battleStartEvent) =>
            {
                //init player
                var playerEntity = battle.PlayerEntity;
                EntityManager.SetOrAddComponentData(playerEntity, new Camp { CampType = ECampType.Friend });
                //create target indicator
                AddTargetIndicator(playerEntity, indicatorPrefab);

                //create monsters
                var assetRef = battle.EnemyDatasAssetRef;
                var count = assetRef.Value.Count;
                ref var array = ref assetRef.Value.Array;
                var enemies = new NativeArray<EnemyTeamMember>(count, Allocator.Temp);
                for (int i = 0; i < count; i++)
                {
                    var enemyData = array[i];
                    var monsterEntity = GameUtil.CreateCreature(EntityManager, gameDataManagerSystem, enemyData);
                    EntityManager.SetOrAddComponentData(monsterEntity, new Camp { CampType = ECampType.Enemy });

                    var translation = new Translation { Value = new float3(4f, 0f, 0f) };
                    EntityManager.SetComponentData(monsterEntity, translation);

                    AddTargetIndicator(monsterEntity, indicatorPrefab);

                    enemies[i] = new EnemyTeamMember { CreatureEntity = monsterEntity };
                }

                var buffer = EntityManager.AddBuffer<EnemyTeamMember>(entity);
                buffer.AddRange(enemies);

                enemies.Dispose();
                //battle.MonsterEntity = monsterEntity;

                CommonUtil.SetFSMStateCmd<PlayerTurnStartState, EnterPlayerTurnStartStateEvent, ExitPlayerTurnStartStateEvent>(EntityManager, entity, (byte)EBattleState.PlayerTurnStart);
            });
        }

        private void AddTargetIndicator(Entity entity, Entity indicatorPrefab)
        {
            var indicatorEntity = EntityManager.Instantiate(indicatorPrefab);

            var translation = EntityManager.GetComponentData<Translation>(entity);
            var pos = translation.Value;
            pos.y += 3f;
            translation.Value = pos;
            EntityManager.SetComponentData(indicatorEntity, translation);

            EntityManager.AddComponentData(entity, new TargetIndicatable { IndicatorEntity = indicatorEntity });
        }
    }
}