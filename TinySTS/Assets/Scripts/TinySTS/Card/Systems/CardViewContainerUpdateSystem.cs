using PFrame.Entities;
using PFrame.Tiny.UI;
using Unity.Entities;
using Unity.Transforms;

namespace TinySTS
{
    //[UpdateInGroup(typeof(InitializationSystemGroup))]
    public class CardViewContainerUpdateSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            var gameDataManagerSystem = World.GetExistingSystem<GameDataManagerSystem>();

            ////add card view cmd
            //Entities.ForEach((Entity entity, DynamicBuffer<AddCardViewCmdElement> cmdElementBuffer, ref CardViewContainer container) =>
            //{
            //    var count = cmdElementBuffer.Length;
            //    if (count == 0)
            //        return;

            //});

            Entities
                .WithNone<UpdatedState>()
                .ForEach((Entity entity, DynamicBuffer<CardViewElement> buffer, ref UIElement uiElement, ref CardViewContainer container) =>
            {
                var count = buffer.Length;
                if (count == 0)
                    return;

                var rect = uiElement.Rect;
                var width = rect.width;

                GameUtil.UpdateCardViewContainerLayout(container, rect, count, (i, pos, rot) =>
                {
                    var cardViewEntity = buffer[i].CardViewEntity;
                    EntityManager.SetComponentData(cardViewEntity, new Translation { Value = pos });
                    EntityManager.SetComponentData(cardViewEntity, new Rotation { Value = rot });
                });

                EntityManager.AddComponent<UpdatedState>(entity);
            });

        }
    }
}