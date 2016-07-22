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
    public class Story : DragModifier
    {
        /// <summary>
        /// 描述模板类，存储任务详细描述信息
        /// </summary>
        public string _missionDescriptionType = "CryStory.Runtime.MissionDescriptionSimpleRPG";

        /// <summary>
        /// 当添加至Story中时，将作为MIssion节点的任务描述类，修改为上述设置类型
        /// 若新类型与旧类型不一致，将导致数据丢失
        /// </summary>
        /// <param name="node"></param>
        protected override void OnAddedContentNode(NodeModifier node)
        {
            base.OnAddedContentNode(node);
            Mission mission = node as Mission;
            if (mission != null)
            {
                if (mission._missionDescription == null || mission._missionDescription.GetType().FullName != _missionDescriptionType)
                    mission._missionDescription = ReflectionHelper.CreateInstance<MissionDescription>(_missionDescriptionType);
            }
        }

        public byte[] Save()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter w = new BinaryWriter(ms))
                {
                    w.Write(JsonUtility.ToJson(this));

                    NodeModifier[] nodes = Nodes;
                    w.Write(nodes.Length);
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        Mission node = nodes[i] as Mission;

                        byte[] data = node.Save();
                        w.Write(data.Length);
                        w.Write(data);

                        //Save Next Node Name For Reconnction
                        NodeModifier[] nexts = NextNodes;
                        w.Write(nexts.Length);
                        for (int j = 0; j < nexts.Length; j++)
                        {
                            w.Write(nexts[j]._name);
                        }
                    }
                }
                return ms.ToArray();
            }

        }

        public void Load(byte[] data)
        {
            if (data.Length > 0)
                using (MemoryStream ms = new MemoryStream(data))
                {
                    using (BinaryReader r = new BinaryReader(ms))
                    {
                        string storyData = r.ReadString();
                        JsonUtility.FromJsonOverwrite(storyData, this);

                        int count = r.ReadInt32();

                        Dictionary<Mission, List<string>> missionNextDictionary = new Dictionary<Mission, List<string>>();

                        //加载Mission
                        for (int i = 0; i < count; i++)
                        {
                            int length = r.ReadInt32();
                            byte[] missionData = r.ReadBytes(length);

                            Mission mission = new Mission();
                            mission.Load(missionData);
                            NodeModifier.SetContent(mission, this);

                            //Load Next Node Name
                            List<string> nextNames = new List<string>();
                            int num = r.ReadInt32();
                            for (int j = 0; j < num; j++)
                            {
                                nextNames.Add(r.ReadString());
                            }

                            missionNextDictionary.Add(mission, nextNames);
                        }


                        NodeModifier[] allMissions = Nodes;
                        //设置已连接的节点
                        foreach (var item in missionNextDictionary)
                        {
                            NodeModifier[] nextList = System.Array.FindAll<NodeModifier>(allMissions, (m) =>
                            {
                                return item.Value.Find((name) => name == m._name) != null;
                            });

                            Mission curMission = item.Key;

                            for (int i = 0; i < nextList.Length; i++)
                            {
                                Mission next = nextList[i] as Mission;
                                if (next == null) continue;
                                if (!curMission.IsParent(next))
                                    Mission.SetParent(next, curMission);
                                else curMission.AddNextNode(next);
                            }
                        }
                    }
                }
        }

    }
}