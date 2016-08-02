/**********************************************************
*Author: CWHISME with PC DESKTOP-TCL3N2O
*Date: 8/2/2016 1:48:22 PM
*Func:
**********************************************************/

namespace CryStory.Runtime
{
    [Help("提升任务目标阶段")]
    [Category("System/SimpleRPG")]
    public class AddDescriptionIndex : Action
    {
        public int AddCount = 1;

        protected override EnumResult OnStart()
        {
            MissionDescriptionSimpleRPG des = GetMission._missionDescription as MissionDescriptionSimpleRPG;
            if (des != null)
            {
                des.AddTargetIndex(AddCount);
                return EnumResult.Success;
            }
            return EnumResult.Failed;
        }
    }
}