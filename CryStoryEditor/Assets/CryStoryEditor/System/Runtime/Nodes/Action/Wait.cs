/**********************************************************
*Author: CWHISME with PC DESKTOP-TCL3N2O
*Date: 8/15/2016 2:55:35 PM
*Func:
**********************************************************/

using UnityEngine;

namespace CryStory.Runtime
{
    [Help("等待指定时间")]
    [Category("System")]
    public class Wait : Action
    {
        public float WaitTime = 5f;

        private float _remainTime;

        protected override EnumResult OnStart()
        {
            _remainTime = WaitTime;
            return base.OnStart();
        }

        protected override EnumResult OnUpdate()
        {
            if ((_remainTime -= Time.deltaTime) < 0)
                return EnumResult.Success;
            return EnumResult.Running;
        }

        public override string ToDescription()
        {
            return "等待 <color=#00FF00>" + WaitTime + "</color> 秒";
        }
    }
}