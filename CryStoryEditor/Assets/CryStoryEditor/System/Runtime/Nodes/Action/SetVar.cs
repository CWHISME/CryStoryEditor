/**********************************************************
*Author: wangjiaying
*Date: 2016.7.11
*Func:
**********************************************************/
using UnityEngine;

namespace CryStory.Runtime
{
    [Category("Values")]
    public class SetVar : Action
    {
        [ValueNameSelect(ValueScope.Mission)]
        public string ValueName;
        public ValueFunctor Functor;
        public string Value;

        protected override EnumResult OnStart()
        {
            GetMission._valueContainer[ValueName].OperationValue(Functor, Value);
            return EnumResult.Success;
        }
    }
}