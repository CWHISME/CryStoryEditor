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
        public List<string> _nextMissionDataNameList = new List<string>();
        //public string _missionData;

        public byte[] _SaveData;

        public void Save()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter w = new BinaryWriter(ms))
                {
                    w.Write(JsonUtility.ToJson(_mission));

                    NodeModifier[] nodes = _mission.Nodes;
                    w.Write(nodes.Length);
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        NodeModifier node = nodes[i];
                        System.Type type = node.GetType();

                        w.Write(type.FullName);
                        //w.Write(JsonUtility.ToJson(node));
                        node.Serialize(w);
                    }

                }

                //Save
                _SaveData = ms.GetBuffer();
            }

            UnityEditor.EditorUtility.SetDirty(this);
            //save current mission


            //_data = new List<SaveData>();
            //for (int i = 0; i < nodes.Length; i++)
            //{


            //    _data.Add(data);
            //}

        }

        public void Load()
        {
            using (MemoryStream ms = new MemoryStream(_SaveData))
            {
                using (BinaryReader r = new BinaryReader(ms))
                {
                    _mission = JsonUtility.FromJson<Mission>(r.ReadString());
                    if (_mission == null) _mission = new Mission();

                    int count = r.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        string fullName = r.ReadString();
                        NodeModifier node = ReflectionHelper.CreateInstance<NodeModifier>(fullName);
                        if (node == null)
                        {
                            Debug.LogError("Error: The Mission Node [" + fullName + "] Was Lost!");
                            return;
                        }
                        node.Deserialize(r);
                        NodeModifier.SetContent(node, _mission);
                    }
                }
            }



            //if (_data != null)
            //{
            //    System.Reflection.Assembly asm = System.Reflection.Assembly.GetAssembly(typeof(Story));
            //    for (int i = 0; i < _data.Count; i++)
            //    {
            //        object o = asm.CreateInstance(_data[i]._type);
            //        JsonUtility.FromJsonOverwrite(_data[i]._json, o);
            //        NodeModifier node = o as NodeModifier;
            //        NodeModifier.SetContent(node, _mission);
            //        //_mission.AddContentNode(r);
            //    }
            //}
        }
    }

    [System.Serializable]
    public class SaveData
    {
        public string _type;
        public string _json;
    }

}