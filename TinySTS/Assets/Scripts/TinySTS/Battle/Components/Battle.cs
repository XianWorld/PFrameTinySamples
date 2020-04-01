using PFrame.Entities;
using Unity.Collections;
using Unity.Entities;

namespace TinySTS
{
    public struct Battle : IComponentData
    {
        //public LevelData PlayerData;
        //public LevelData MonsterData;

        //public BlobAssetReference<LevelDataArrayAsset> PlayerDatasAssetRef;
        public BlobAssetReference<LevelDataArrayAsset> EnemyDatasAssetRef;

        public Entity PlayerEntity;
        //public Entity MonsterEntity;

        public Entity BattleWindowEntity;

        public ushort TurnNum;

        public Entity SelectedCardEntity;
        public Entity SelectedCardViewEntity;

        public bool IsPlayerTurn;
        public Entity CurrentExecutorEntity;
        public byte CurrentExecutorIndex;
    }

    public struct PlayerTag : IComponentData
    {

    }

    public enum ECampType : byte
    {
        Neutral,
        Friend,
        Enemy
    }

    public enum ERelationType : byte
    {
        None,
        Neutral,
        Friendly,
        Hostile
    }

    public struct Camp : IComponentData
    {
        public ECampType CampType;
    }

    //public struct BattleTeam : IComponentData
    //{

    //}

    //public struct PlayerTeamMember: IBufferElementData
    //{
    //    public Entity CreatureEntity;
    //}

    public struct EnemyTeamMember : IBufferElementData
    {
        public Entity CreatureEntity;
    }

    public enum EBattleState : byte
    {
        None,
        BattleStart,
        BattleEnd,
        PlayerTurnStart,
        //PlayerTurn,
        PlayerSelectCard,
        PlayerSelectTarget,
        PlayerPlayCard,
        PlayerTurnEnd,
        EnemyTurnStart,
        EnemyTurn,
        EnemyTurnEnd,
    }

    public struct BattleStartState : IComponentData { }
    public struct EnterBattleStartStateEvent : IComponentData { }
    public struct ExitBattleStartStateEvent : IComponentData { }

    //public struct BattleState : IComponentData { }
    //public struct EnterBattleStateEvent : IComponentData { }
    //public struct ExitBattleStateEvent : IComponentData { }

    public struct PlayerTurnStartState : IComponentData { }
    public struct EnterPlayerTurnStartStateEvent : IComponentData { }
    public struct ExitPlayerTurnStartStateEvent : IComponentData { }

    public struct PlayerSelectState : IComponentData { }
    public struct EnterPlayerSelectStateEvent : IComponentData { }
    public struct ExitPlayerSelectStateEvent : IComponentData { }

    public struct PlayerPlayCardState : IComponentData { }
    public struct EnterPlayerPlayCardStateEvent : IComponentData { }
    public struct ExitPlayerPlayCardStateEvent : IComponentData { }

    public struct PlayerTurnEndState : IComponentData { }
    public struct EnterPlayerTurnEndStateEvent : IComponentData { }
    public struct ExitPlayerTurnEndStateEvent : IComponentData { }

    public struct EnemyTurnStartState : IComponentData { }
    public struct EnterEnemyTurnStartStateEvent : IComponentData { }
    public struct ExitEnemyTurnStartStateEvent : IComponentData { }

    public struct ChoseEnemyState : IComponentData { }
    public struct EnterChoseEnemyStateEvent : IComponentData { }
    public struct ExitChoseEnemyStateEvent : IComponentData { }

    public struct EnemyExecuteState : IComponentData { }
    public struct EnterEnemyExecuteStateEvent : IComponentData { }
    public struct ExitEnemyExecuteStateEvent : IComponentData { }

    public struct EnemyTurnEndState : IComponentData { }
    public struct EnterEnemyTurnEndStateEvent : IComponentData { }
    public struct ExitEnemyTurnEndStateEvent : IComponentData { }
}
