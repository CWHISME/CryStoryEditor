/**********************************************************
*Author: CWHISME with PC DESKTOP-TCL3N2O
*Date: 7/28/2016 3:57:39 PM
*Func:
**********************************************************/

using UnityEngine;

namespace CryStory.Runtime
{
    [Help("如果按下某个按钮")]
    public class KeyDown : Condition
    {

        public KeyCode Key;

        public override bool OnCheck()
        {
            return Input.GetKeyDown(Key);
        }

        public override string ToDescription()
        {
            return "如果按下按钮 <color=#EE82EE>" + Key + "</color>";
        }
    }
}