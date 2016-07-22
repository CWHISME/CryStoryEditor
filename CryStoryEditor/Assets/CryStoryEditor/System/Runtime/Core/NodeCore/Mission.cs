/**********************************************************
*Author: wangjiaying
*Date: 2016.6.16
*Func:
**********************************************************/
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CryStory.Runtime
{

    public class Mission : DragModifier
    {

        /// <summary>
        /// 存储任务详情
        /// </summary>
        public MissionDescription _missionDescription;

        /// <summary>
        /// 唯一ID，当然，只针对每一个Mission而言
        /// </summary>
        private List<int> _idList = new List<int>();

        /// <summary>
        /// 为节点计算一个唯一ID
        /// </summary>
        /// <returns></returns>
        public int GenerateID()
        {
            int id;
            do { id = UnityEngine.Random.Range(1, int.MaxValue); }
            while (_idList.Contains(id));
            _idList.Add(id);
            return id;
        }

        /// <summary>
        /// 保存数据，将会把返回保存后的二进制数据
        /// </summary>
        /// <returns></returns>
        public byte[] Save()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter w = new BinaryWriter(ms))
                {
                    w.Write(JsonUtility.ToJson(this));
                    Serialize(w);

                    NodeModifier[] nodes = Nodes;
                    w.Write(nodes.Length);
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        NodeModifier node = nodes[i];
                        System.Type type = node.GetType();

                        w.Write(type.FullName);
                        node.Serialize(w);
                    }
                }
                //Save
                return ms.GetBuffer();
            }
        }

        /// <summary>
        /// 从已保存的二进制数据中加载
        /// </summary>
        /// <param name="data"></param>
        public void Load(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader r = new BinaryReader(ms))
                {
                    JsonUtility.FromJsonOverwrite(r.ReadString(), this);
                    //this = JsonUtility.FromJson<Mission>(r.ReadString());
                    //if (_mission == null) _mission = new Mission();
                    Deserialize(r);

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
                        NodeModifier.SetContent(node, this);
                    }
                }
            }
        }

        /// <summary>
        /// 校验及保存ID
        /// </summary>
        public override void Serialize(System.IO.BinaryWriter w)
        {
            List<int> notUsedID = new List<int>();
            for (int i = 0; i < _idList.Count; i++)
            {
                if (GetNodeByID(_idList[i]) == null)
                    notUsedID.Add(_idList[i]);
            }

            for (int i = 0; i < notUsedID.Count; i++)
            {
                _idList.Remove(notUsedID[i]);
            }

            w.Write(_idList.Count);
            for (int i = 0; i < _idList.Count; i++)
            {
                w.Write(_idList[i]);
            }

            w.Write(_missionDescription.GetType().FullName);
            _missionDescription.Serialize(w);

            OnSaved(w);
        }

        public override void Deserialize(BinaryReader r)
        {
            int count = r.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                _idList.Add(r.ReadInt32());
            }

            string name = r.ReadString();
            _missionDescription = ReflectionHelper.CreateInstance<MissionDescription>(name);
            _missionDescription.Deserialize(r);

            OnLoaded(r);
        }

        public void OnAbort()
        {

        }

    }
}