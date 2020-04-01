using PFrame.Entities;
using Unity.Collections;
using Unity.Entities;

namespace TinySTS
{
    public enum EStageType : byte
    {
        None,
        Prototype,
        Splash,
        MainMenu,
        GamePlay,
        Max
    }

    public struct MainService : IComponentData
    {
        public byte StartStage;
        //public Entity SplashStagePrefab;
        //public Entity MainMenuStagePrefab;
        //public Entity GamePlayStagePrefab;
        //public Entity PrototypeStagePrefab;
        public BlobAssetReference<UshortArrayAsset> DataIdsAssetRef;
    }


    //public struct EntitiesAsset
    //{
    //    public ushort Count;
    //    public BlobArray<Entity> Entities;
    //}
}