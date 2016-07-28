/**********************************************************
*Author: CWHISME with PC DESKTOP-TCL3N2O
*Date: 7/28/2016 9:40:18 AM
*Func:
**********************************************************/

namespace CryStory.Runtime
{
    [Help("代表一个节点已经丢失了！")]
    public class MissingNode : Action
    {
        public override string ToDescription()
        {
            return "<color=red>[" + _name + "] node code was lost! Please resume code if you want to recover node.\nCaution: If you saving now will be lost this node  all the data! </color>";
        }
    }
}