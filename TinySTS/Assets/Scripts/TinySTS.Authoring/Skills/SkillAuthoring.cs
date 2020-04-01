using Unity.Entities;
using UnityEngine;

namespace TinySTS.Authoring
{
    public class SkillAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        //public int Id;
        //public string Name;
        //public int Level;

        //public EffectAuthoring[] EffectElements;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var skill = new Skill
            {
                //Id = Id,
                //Name = Name,
                //Level = Level
            };

            dstManager.AddComponentData<Skill>(entity, skill);

            var statBuffer = dstManager.AddBuffer<StatElement>(entity);
            var statNum = (byte)ESkillStatType.Max;
            for(int i = 0;i<statNum; i++)
            {
                statBuffer.Add(new StatElement { StatId = (byte)i });
            }

            //if(EffectElements != null)
            //{
            //    var buffer = dstManager.AddBuffer<SkillEffectElement>(entity);
            //    for(int i = 0; i< EffectElements.Length; i++)
            //    {
            //        var effectEntity = conversionSystem.GetPrimaryEntity(EffectElements[i].gameObject);
            //        buffer.Add(new SkillEffectElement { EffectEntity = effectEntity });
            //    }
            //}

            var childCount = transform.childCount;
            if (childCount > 0)
            {
                var buffer = dstManager.AddBuffer<SkillEffectElement>(entity);
                for (int i = 0; i < childCount; i++)
                {
                    var child = transform.GetChild(i).gameObject;
                    var effectAuthoring = child.GetComponent<EffectAuthoring>();

                    var effectEntity = conversionSystem.GetPrimaryEntity(child);

                    var element = new SkillEffectElement
                    {
                        EffectType = effectAuthoring.Type,
                        EffectEntity = effectEntity
                    };

                    buffer.Add(element);
                }
            }
        }
    }
}
