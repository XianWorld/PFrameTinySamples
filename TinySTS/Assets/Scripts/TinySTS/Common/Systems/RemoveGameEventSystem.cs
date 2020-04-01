//using Unity.Entities;

//namespace TinySTS
//{
//    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
//    public class RemoveGameEventSystem : ComponentSystem
//    {
//        protected override void OnUpdate()
//        {
//            Entities.ForEach((Entity entity, ref BattleStartedEvent eventComp) =>
//            {
//                PostUpdateCommands.RemoveComponent<BattleStartedEvent>(entity);
//            });
//        }
//    }
//}