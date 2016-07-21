/**********************************************************
*Author: wangjiaying
*Date: 2016.7.11
*Func:
**********************************************************/
using UnityEngine;

namespace CryStory.Runtime
{
    [Help("设置Mission变量的值")]
    [Category("Values")]
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
            return "Mission变量 " + ValueName + " " + Functor + " " + Value;
        }
    }
}