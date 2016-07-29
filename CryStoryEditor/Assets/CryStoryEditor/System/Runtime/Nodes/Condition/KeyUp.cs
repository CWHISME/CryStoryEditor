/**********************************************************
*Author: CWHISME with PC DESKTOP-TCL3N2O
*Date: 7/28/2016 4:00:04 PM
*Func:
**********************************************************/

using UnityEngine;

namespace CryStory.Runtime
{
    [Category("System")]
    [Help("如果松开某个按钮")]
    public class KeyUp : Condition
    {
        public KeyCode Key;

        public override bool OnCheck()
        {
            return Input.GetKeyUp(Key);
        }

        public override string ToDescription()
        {
            return "如果松开按钮 <color=#EE82EE>" + Key + "</color>";
        }
    }
}