/**********************************************************
*Author: wangjiaying
*Date: 2016.6.23
*Func:
**********************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace CryStory.Runtime
{
    public class MissionObject : ScriptableObject
    {
        public Mission _mission;

        public List<SaveData> _data;
        public string _missionData;

        public void Save()
        {
            //save current mission
            _missionData = JsonUtility.ToJson(_mission);

            NodeModifier[] nodes = _mission.Nodes;
            _data = new List<SaveData>();
            for (int i = 0; i < nodes.Length; i++)
            {
                NodeModifier node = nodes[i];
                System.Type type = node.GetType();
                SaveData data = new SaveData();
                data._type = type.FullName;
                data._json = JsonUtility.ToJson(node);

                _data.Add(data);
            }

        }

        public void Load()
        {
            if (string.IsNullOrEmpty(_missionData))
            {
                _mission = new Mission();
                return;
            }

            _mission = JsonUtility.FromJson<Mission>(_missionData);
            if (_data != null)
            {
                System.Reflection.Assembly asm = System.Reflection.Assembly.GetAssembly(typeof(Story));
                for (int i = 0; i < _data.Count; i++)
                {
                    object o = asm.CreateInstance(_data[i]._type);
                    JsonUtility.FromJsonOverwrite(_data[i]._json, o);
                    _mission.AddContentNode(o as NodeModifier);
                }
            }
        }
    }

    [System.Serializable]
    public class SaveData
    {
        public string _type;
        public string _json;
    }
}