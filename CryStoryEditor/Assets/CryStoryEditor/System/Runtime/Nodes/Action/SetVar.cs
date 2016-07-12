/**********************************************************
*Author: wangjiaying
*Date: 2016.7.11
*Func:
**********************************************************/
using UnityEngine;

namespace CryStory.Runtime
{
    public class SetVar : Action
    {
        protected override EnumResult OnStart()
        {
            //Debug.Log("Set Var Start");
            return EnumResult.Success;
        }
    }
}