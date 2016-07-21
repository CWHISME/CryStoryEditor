/**********************************************************
*Author: wangjiaying
*Date: 2016.7.11
*Func:
**********************************************************/
using UnityEngine;

namespace CryStory.Runtime
{
    [Help("设置全局Story变量的值")]
    [Category("Values")]
    public class SetStoryVar : Action
    {
        [ValueNameSelect(ValueScope.Story)]
        public string ValueName;
        [ValueKeyReference("ValueName", ValueScope.Story)]
        public ValueFunctor Functor;
        [ValueKeyReference("ValueName", ValueScope.Story)]
        public string Value;

        protected override EnumResult OnStart()
        {
            GetMission._valueContainer[ValueName].OperationValue(Functor, Value);
            return EnumResult.Success;
        }

        public override string ToDescription()
        {
            return "Story变量 " + ValueName + " " + Functor + " " + Value;
        }
    }
}