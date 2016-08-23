/**********************************************************
*Author: wangjiaying
*Date: 2016.8.2
*Func:
**********************************************************/
using UnityEngine;
using System.Collections;
using CryStory.Runtime;
using UnityEngine.UI;
using System.Text;

namespace CryStory.Example
{
    public class UIShowMissionTarget : MonoBehaviour
    {

        public StoryRunner _storyRunner;
        public Text _text;
        void Start()
        {
            _storyRunner.StoryManager.RefreshMissionList();
            if (_storyRunner.StoryManager.RunningMissions.Count > 0)
            {
                MissionDescriptionSimpleRPG des = (_storyRunner.StoryManager.RunningMissions[0].MissionDescription as MissionDescriptionSimpleRPG);
                des.OnDescriptionChange += RefreshDescription;
                RefreshDescription(des);
            }
        }

        void RefreshDescription(MissionDescriptionSimpleRPG des)
        {
            StringBuilder builder = new StringBuilder("<color=yellow>" + des.Name + "</color>\n\n");
            for (int i = 0; i < des._missionTargetIndex; i++)
            {
                builder.AppendLine(des.MissionTargetList[i]);
            }

            builder.AppendLine("<color=#00FF7F>" + des.MissionTargetList[des._missionTargetIndex] + "</color>");

            _text.text = builder.ToString();
        }
    }
}