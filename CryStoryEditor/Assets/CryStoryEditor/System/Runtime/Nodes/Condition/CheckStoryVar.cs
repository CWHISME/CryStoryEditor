/**********************************************************
*Author: CWHISME with PC DESKTOP-TCL3N2O
*Date: 7/27/2016 6:31:44 PM
*Func:
**********************************************************/

namespace CryStory.Runtime
{
    [Category("System/Values")]
    [Help("判断某个变量是否符合条件")]
    public class CheckStoryVar : Condition
    {
        [ValueNameSelect(ValueScope.Story)]
        public string ValueName;
        [ValueKeyReference("ValueName", ValueScope.Story)]
        public ValueCompare Compare;
        [ValueKeyReference("ValueName", ValueScope.Story)]
        public string Value;

        public override bool OnCheck()
        {
            Value v = GetStory.GetValue(ValueName);
            if (v != null)
                return v.Compare(Value, Compare);
            return false;
        }
    }
}