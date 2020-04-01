using PFrame.Entities.Authoring;
using Unity.Entities;
using UnityEngine;

namespace TinySTS.Authoring
{
    [CreateAssetMenu(fileName = "EffectData_", menuName = "TinySTS/EffectData")]
    public class EffectDataSO : AGameDataSO
    {
        public override byte DataType => (byte)EGameDataType.Effect;

        public EEffectType Type;
        public EEffectCastType CastType;

        //damage
        [Header("Damage")]
        public int Damage;

        //creature stat
        public ECreatureStatType CreatureStatType;
        public EBonusType BonusType;
        public float BonusValue;

        //creature stat
        public ESkillStatType SkillStatType;
    }
}
