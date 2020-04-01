using Unity.Entities;
using UnityEngine;

namespace TinySTS.Authoring
{
    public class CardTemplateAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public GameObject Background;
        public GameObject NameText;
        public GameObject TypeText;
        public GameObject ManaText;
        public GameObject DescriptionText;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var cardTemplate = new CardTemplate
            {
                Background = conversionSystem.GetPrimaryEntity(Background),
                NameText = conversionSystem.GetPrimaryEntity(NameText),
                TypeText = conversionSystem.GetPrimaryEntity(TypeText),
                ManaText = conversionSystem.GetPrimaryEntity(ManaText),
                DescriptionText = conversionSystem.GetPrimaryEntity(DescriptionText),
            };

            dstManager.AddComponentData<CardTemplate>(entity, cardTemplate);
        }
    }
}
