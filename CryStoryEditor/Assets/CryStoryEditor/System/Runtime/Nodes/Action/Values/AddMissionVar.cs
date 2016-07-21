/**********************************************************
*Author: wangjiaying
*Date: 2016.7.11
*Func:
**********************************************************/
using UnityEngine;

namespace CryStory.Runtime
{
    [Help("动态添加指定作用域的变量(不建议使用)")]
    [Category("System/Values")]
    public class AddMissionVar : Action
    {
        public string ValueName;
        public ValueScope Scope;
        public VarType ValueType = VarType.STRING;
        public string Value;

        protected override EnumResult OnStart()
        {
            if (Scope == ValueScope.Mission)
                return GetMission.AddValue(ValueName, ValueType, Value).ToEnumResult();
            else if (Scope == ValueScope.Story)
                return GetStory.AddValue(ValueName, ValueType, Value).ToEnumResult();
            return EnumResult.Failed;
        }

        public override string ToDescription()
        {
            return "添加变量 [<color=yellow>" + ValueName + "</color>](" + ValueType + ")  作用域：" + Scope + " 值为：[<color=#00FF00>" + Value + "</color>] ";
        }
    }
}