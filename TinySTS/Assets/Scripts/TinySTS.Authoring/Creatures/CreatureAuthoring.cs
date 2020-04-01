using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace TinySTS.Authoring
{
    public class CreatureAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        //public ushort Id;
        //public string Name;
        //public ushort Level;

        //public StatElement[] Stats = new StatElement[(int)ECreatureStatType.Max];

        //public short[] StatBaseValues = new short[(int)ECreatureStatType.Max];
        //public GameObject[] CardPrefabs;


        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var creature = new Creature
            {
                //DataId = Id,
                //Name = Name,
                //Level = Level,
            };
            dstManager.AddComponentData<Creature>(entity, creature);

            var statBuffer = dstManager.AddBuffer<StatElement>(entity);
            var statNum = (byte)ECreatureStatType.Max;
            for (int i = 0; i < statNum; i++)
            {
                var stat = new StatElement
                {
                    StatId = (byte)i,
                    //BaseValue = StatBaseValues[i]
                };
                statBuffer.Add(stat);
            }

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

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            //if(CardPrefabs != null)
            //{
            //    referencedPrefabs.AddRange(CardPrefabs);
            //}
        }
    }
}
