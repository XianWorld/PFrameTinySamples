using Unity.Entities;
using UnityEngine;

namespace TinySTS.Authoring
{
    public class EffectAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public ushort Id;
        public string Name;
        public EEffectType Type;
        public EEffectCastType CastType;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var effect = new Effect
            {
                Type = Type,
                CastType = CastType
            };

            dstManager.AddComponentData<Effect>(entity, effect);
        }
    }
}
