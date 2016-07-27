/**********************************************************
*Author: CWHISME with PC DESKTOP-TCL3N2O
*Date: 7/27/2016 5:18:05 PM
*Func:
**********************************************************/

namespace CryStory.Runtime
{
    [Category("System/Values")]
    [Help("判断某个变量是否符合条件")]
    public class CheckMissionVar : Condition
    {
        [ValueNameSelect(ValueScope.Mission)]
        public string ValueName;
        [ValueKeyReference("ValueName", ValueScope.Mission)]
        public ValueCompare Compare;
        [ValueKeyReference("ValueName", ValueScope.Mission)]
        public string Value;

        public override bool OnCheck()
        {
            Value v = GetMission.GetValue(ValueName);
            if (v != null)
                return v.Compare(Value, Compare);
            return false;
        }
    }
}