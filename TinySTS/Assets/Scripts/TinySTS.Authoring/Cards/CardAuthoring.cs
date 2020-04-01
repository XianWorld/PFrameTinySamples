using Unity.Entities;
using UnityEngine;

namespace TinySTS.Authoring
{
    public class CardAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        //public int Id;
        //public string Name;
        //public int Level;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var card = new Card
            {
            };

            dstManager.AddComponentData<Card>(entity, card);

            //var childCount = transform.childCount;
            //if (childCount > 0)
            //{
            //    var buffer = dstManager.AddBuffer<SkillEffectElement>(entity);
            //    for (int i = 0; i < childCount; i++)
            //    {
            //        var child = transform.GetChild(i).gameObject;
            //        var effectAuthoring = child.GetComponent<EffectAuthoring>();

            //        var effectEntity = conversionSystem.GetPrimaryEntity(child);

            //        var element = new SkillEffectElement
            //        {
            //            EffectType = effectAuthoring.Type,
            //            EffectEntity = effectEntity
            //        };

            //        buffer.Add(element);
            //    }
            //}
        }
    }
}
