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

        //public List<SaveData> _data;

        //Name:IsSingleNode
        public Dictionary<string, bool> _nextMissionDataNameList = new Dictionary<string, bool>();
        //public string _missionData;

        public byte[] _SaveData;

        public void Save()
        {
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    using (BinaryWriter w = new BinaryWriter(ms))
            //    {
            //        w.Write(JsonUtility.ToJson(_mission));
            //        _mission.Serialize(w);

            //        NodeModifier[] nodes = _mission.Nodes;
            //        w.Write(nodes.Length);
            //        for (int i = 0; i < nodes.Length; i++)
            //        {
            //            NodeModifier node = nodes[i];
            //            System.Type type = node.GetType();

            //            w.Write(type.FullName);
            //            node.Serialize(w);
            //        }

            //    }

            //    //Save
            //    _SaveData = ms.GetBuffer();
            //}
            _SaveData = _mission.Save();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public void Load()
        {
            _mission = new Mission();
            _mission.Load(_SaveData);
            //using (MemoryStream ms = new MemoryStream(_SaveData))
            //{
            //    using (BinaryReader r = new BinaryReader(ms))
            //    {
            //        _mission = JsonUtility.FromJson<Mission>(r.ReadString());
            //        if (_mission == null) _mission = new Mission();
            //        _mission.Deserialize(r);

            //        int count = r.ReadInt32();
            //        for (int i = 0; i < count; i++)
            //        {
            //            string fullName = r.ReadString();
            //            NodeModifier node = ReflectionHelper.CreateInstance<NodeModifier>(fullName);
            //            if (node == null)
            //            {
            //                Debug.LogError("Error: The Mission Node [" + fullName + "] Was Lost!");
            //                return;
            //            }
            //            node.Deserialize(r);
            //            NodeModifier.SetContent(node, _mission);
            //        }
            //    }
            //}
        }

        public void AddNextMissionName(string name, bool singleNode)
        {
            _nextMissionDataNameList[name] = singleNode;
        }
    }

}