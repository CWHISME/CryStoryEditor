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
        private Story _story;
        public Story CurrentStory { get { return _story; } }

        public StoryManager(Story story)
        {
            _story = story;
        }

        private bool _storyEnd = false;
        public void Tick()
        {
            if (_storyEnd) return;
            if (_story.Tick() != EnumResult.Running)
                _storyEnd = true;
        }

        public void LoadStory(byte[] saveData)
        {
            _story = new Story();
            _story.Load(saveData);
            _storyEnd = false;
        }

        private List<Mission> _accomplishMissions = new List<Mission>();
        private List<Mission> _runningMissions = new List<Mission>();

        /// <summary>
        /// 获取已完成任务列表
        /// </summary>
        public List<Mission> AccomplishMissions
        {
            get { return _accomplishMissions; }
        }

        /// <summary>
        /// 获取正在运行任务列表
        /// </summary>
        public List<Mission> RunningMissions
        {
            get { return _runningMissions; }
        }

        /// <summary>
        /// 获取已完成任务列表（实时，将会自动刷新列表，更加消耗）
        /// </summary>
        /// <returns></returns>
        public List<Mission> GetAccomplishMissions()
        {
            RefreshMissionList();
            return _accomplishMissions;
        }

        /// <summary>
        /// 获取正在运行任务列表（实时，将会自动刷新列表，更加消耗）
        /// </summary>
        /// <returns></returns>
        public List<Mission> GetRunningMissions()
        {
            RefreshMissionList();
            return _runningMissions;
        }

        /// <summary>
        /// 刷新任务列表信息
        /// </summary>
        public void RefreshMissionList()
        {
            _accomplishMissions.Clear();
            _runningMissions.Clear();
            NodeModifier[] nodes = _story.Nodes;
            for (int i = 0; i < nodes.Length; i++)
            {
                Mission mi = nodes[i] as Mission;
                if (mi == null) continue;
                AddMIssionParentToList(mi, _accomplishMissions);
                _runningMissions.Add(mi);
            }
        }

        private void AddMIssionParentToList(Mission mission, List<Mission> list)
        {
            if (mission.Parent == null) return;
            Mission p = mission.Parent as Mission;
            if (p == null) return;
            list.Add(p);
        }
    }
}