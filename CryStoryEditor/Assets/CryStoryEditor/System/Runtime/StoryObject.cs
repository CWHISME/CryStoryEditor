/**********************************************************
*Author: wangjiaying
*Date: 2016.6.20
*Func:
**********************************************************/
using UnityEngine;

namespace CryStory.Runtime
{
    public class StoryObject : ScriptableObject
    {
        public Story _story = new Story();

        public Vector2 StoryCenter { get { return _story.graphCenter; } set { _story.graphCenter = value; } }

        /// <summary>
        /// 直接添加一个新的任务
        /// </summary>
        /// <returns></returns>
        public Mission AddNewMission()
        {
            Mission mission = new Mission();
            _story._missionList.Add(mission);
            return mission;
        }
    }
}