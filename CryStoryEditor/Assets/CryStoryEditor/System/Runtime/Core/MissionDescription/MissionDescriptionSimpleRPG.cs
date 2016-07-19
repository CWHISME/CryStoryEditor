/**********************************************************
*Author: wangjiaying
*Date: 2016.7.19
*Func:
**********************************************************/

using System.IO;

namespace CryStory.Runtime
{
    /// <summary>
    /// 简单的RPG类型的任务描述信息
    /// </summary>
    public class MissionDescriptionSimpleRPG : MissionDescription
    {
        public string Name = "No Name Mission";
        public int _missionTargetIndex = 0;
        public string[] MissionTargetList = new string[] { "First description" };

        public void AddTargetIndex()
        {
            if (_missionTargetIndex < MissionTargetList.Length - 1)
                _missionTargetIndex++;
        }

        public override void Serialize(BinaryWriter w)
        {
            w.Write(Name);

            w.Write(MissionTargetList.Length);
            for (int i = 0; i < MissionTargetList.Length; i++)
            {
                w.Write(MissionTargetList[i] == null ? "" : MissionTargetList[i]);
            }
        }

        public override void Deserialize(BinaryReader r)
        {
            Name = r.ReadString();

            int count = r.ReadInt32();
            MissionTargetList = new string[count];
            for (int i = 0; i < count; i++)
            {
                MissionTargetList[i] = r.ReadString();
            }
        }
    }
}