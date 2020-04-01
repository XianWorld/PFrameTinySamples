using PFrame.Entities.Authoring;
using UnityEngine;

namespace TinySTS.Authoring
{
    [CreateAssetMenu(fileName = "SkillInfoData_", menuName = "TinySTS/SkillInfoData")]
    public class SkillInfoDataSO : AGameDataSO
    {
        public override byte DataType => (byte)EGameDataType.SkillInfo;

        public SkillDataSO[] Datas = new SkillDataSO[1];
    }
}
