/**********************************************************
*Author: wangjiaying
*Date: 2016.7.11
*Func:
**********************************************************/
using UnityEngine;

namespace CryStory.Runtime
{
    [Help("设置作用域为当前Mission的变量的值")]
    [Category("System/Values")]
    public class SetMissionVar : Action
    {
        [ValueNameSelect(ValueScope.Mission)]
        public string ValueName;
        [ValueKeyReference("ValueName", ValueScope.Mission)]
        public ValueFunctor Functor;
        [ValueKeyReference("ValueName", ValueScope.Mission)]
        public string Value;

        protected override EnumResult OnStart()
        {
            GetMission._valueContainer[ValueName].OperationValue(Functor, Value);
            return EnumResult.Success;
        }

        public override string ToDescription()
        {
            return "设置变量 [" + ValueName + "] " + Functor + " [" + Value + "]";
        }
    }
}