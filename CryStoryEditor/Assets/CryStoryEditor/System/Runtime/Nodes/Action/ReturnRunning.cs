/**********************************************************
*Author: CWHISME with PC DESKTOP-TCL3N2O
*Date: 2017.1.14
*Func:
**********************************************************/

namespace CryStory.Runtime
{
    [Help("一直返回Running")]
    [Category("System")]
    public class ReturnRunning : Action
    {

        protected override EnumResult OnUpdate()
        {
            return EnumResult.Running;
        }

        public override string ToDescription()
        {
            return "阻塞";
        }
    }
}