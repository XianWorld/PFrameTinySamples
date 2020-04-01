using PFrame.Entities.Authoring;
using UnityEngine;

namespace TinySTS.Authoring
{
    [CreateAssetMenu(fileName = "SkillData_", menuName = "TinySTS/SkillData")]
    public class SkillDataSO : AGameDataSO
    {
        public override byte DataType => (byte)EGameDataType.Skill;

        public byte ManaCost;

        public ETargetType TargetType;
        public bool IsSpecificRelation;
        public ERelationType TargetRelationType;
        public bool IsWithoutSelf;
        public byte TargetNum;

        //public GameObject Prefab;
        public EffectDataSO[] EffectDatas = new EffectDataSO[1];
    }
}
