/**********************************************************
*Author: wangjiaying
*Date: 2016.7.11
*Func:
**********************************************************/
using UnityEngine;

namespace CryStory.Runtime
{
    [Help("设置作用域为当前Story的变量的值")]
    [Category("System/Values")]
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
            GetStory._valueContainer[ValueName].OperationValue(Functor, Value);
            return EnumResult.Success;
        }

        public override string ToDescription()
        {
            return "设置变量 [" + ValueName + "] " + Functor + " [" + Value + "]";
        }
    }
}