using PFrame.Tiny.UI.Authoring;
using Unity.Entities;
using UnityEngine;

namespace TinySTS.Authoring
{
    public class CardViewContainerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float HSpacing = 1f;
        public float ZSpacing = 0.001f;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var cardViewContainer = new CardViewContainer
            {
                HSpacing = HSpacing,
                ZSpacing = ZSpacing
            };

            dstManager.AddComponentData<CardViewContainer>(entity, cardViewContainer);

            dstManager.AddBuffer<CardViewElement>(entity);

            dstManager.AddBuffer<AddCardViewCmdElement>(entity);
        }

        private void OnValidate()
        {
            var uiElementAuthoring = gameObject.GetComponent<UIElementAuthoring>();
            if (uiElementAuthoring == null)
                return;

            var rect = uiElementAuthoring.Rect;
            var width = rect.width;

            var childCount = transform.childCount;
            if (childCount == 0)
                return;

            var container = new CardViewContainer
            {
                HSpacing = HSpacing,
                ZSpacing = ZSpacing
            };
            var rect1 = new Unity.Tiny.Rect(rect.xMin, rect.yMin, rect.width, rect.height);
            GameUtil.UpdateCardViewContainerLayout(container, rect1, childCount, (i, pos, rot) =>
            {
                var child = transform.GetChild(i);
                child.localPosition = pos;
                child.localRotation = rot;
            });

            //var hSpacing = HSpacing;
            //var zSpacing = ZSpacing;
            //float minX = 0f;
            //float minZ = 0f;

            //if (childCount > 1)
            //{
            //    var temp = width / (childCount - 1);
            //    if (temp < hSpacing)
            //        hSpacing = temp;

            //    minX = -((childCount - 1) * hSpacing / 2f);
            //    minZ = -((childCount - 1) * zSpacing / 2f);
            //}

            //float offsetX = minX;
            //float offsetZ = minZ;
            //for (int i = 0; i < childCount; i++)
            //{
            //    var child = transform.GetChild(i);
            //    var pos = child.localPosition;
            //    pos.x = offsetX;
            //    pos.z = offsetZ;
            //    child.localPosition = pos;

            //    offsetX += hSpacing;
            //    offsetZ += zSpacing;
            //}
        }
    }
}
