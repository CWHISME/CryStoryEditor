/**********************************************************
*Author: wangjiaying
*Date: 2016.6.23
*Func:
**********************************************************/
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CryStory.Runtime
{
    public class MissionObject : ScriptableObject
    {
        public Mission _mission;

        //public Dictionary<string, bool> _nextMissionDataNameList = new Dictionary<string, bool>();
        public List<NextMissionData> _nextMissionNodeList = new List<NextMissionData>();

        public byte[] _SaveData;

        public void Save()
        {
            _SaveData = _mission.SaveThisNode();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public void Load()
        {
            _mission = new Mission();
            _mission.LoadThisNode(_SaveData);
        }

        public void AddNextMissionName(string name, bool singleNode)
        {
            NextMissionData data = _nextMissionNodeList.Find((m) => m.Name == name);
            if (data != null)
            {
                data.IsSingleNode = singleNode;
                return;
            }

            _nextMissionNodeList.Add(new NextMissionData() { Name = name, IsSingleNode = singleNode });
            //_nextMissionDataNameList[name] = singleNode;
        }
    }

    [System.Serializable]
    public class NextMissionData
    {
        public string Name;
        public bool IsSingleNode;
    }

}