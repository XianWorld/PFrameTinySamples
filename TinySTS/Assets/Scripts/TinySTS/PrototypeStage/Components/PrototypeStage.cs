using PFrame.Entities;
using Unity.Collections;
using Unity.Entities;

namespace TinySTS
{
    public struct PrototypeStage : IComponentData
    {
        public LevelData PlayerData;
        //public LevelData MonsterData;
        public BlobAssetReference<LevelDataArrayAsset> EnemyDatasAssetRef;

        public Entity PrototypeWindowEntity;
        public Entity BattleWindowEntity;

        //public Entity PlayerEntity;
        //public Entity MonsterEntity;
        public Entity BattleEntity;
    }

}