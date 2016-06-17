/**********************************************************
*Author: wangjiaying
*Date: 2016.6.16
*Func:
**********************************************************/

using System.Collections.Generic;

namespace CryStory.Runtime
{
    public class StoryManager
    {
        private static StoryManager _instance;
        public static StoryManager GetInstance
        {
            get
            {
                if (_instance == null)
                    _instance = new StoryManager();
                return _instance;
            }
        }

        public List<Mission> _missionList = new List<Mission>();

        public void Tick()
        {
            EnumResult result;
            for (int i = 0; i < _missionList.Count; i++)
            {
                result = _missionList[i].Tick();
                if (result != EnumResult.Running)
                {
                    Mission mission = _missionList[i]._next as Mission;
                    _missionList.RemoveAt(i);
                    RunMission(mission);
                }
            }
        }

        public void RunMission(Mission mission)
        {
            if (mission == null) return;
            //不允许重复添加同一个任务
            if (_missionList.Find((m) => m._prototypeId == mission._prototypeId) == null)
                _missionList.Add(mission);
        }

        public void EndMission(Mission mission)
        {
            if (_missionList.Contains(mission))
            {
                _missionList.Remove(mission);
                mission.OnAbort();
            }
        }
    }
}