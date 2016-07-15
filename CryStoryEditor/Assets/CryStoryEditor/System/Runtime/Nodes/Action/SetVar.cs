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
        public string ThisIsTestVarForValueLength = "Test";
        public float Var = 555;

        protected override EnumResult OnStart()
        {
            Var = Random.Range(99.9f, 111.6f);
            //Debug.Log("Set Var Start");
            return EnumResult.Failed;
        }
    }
}