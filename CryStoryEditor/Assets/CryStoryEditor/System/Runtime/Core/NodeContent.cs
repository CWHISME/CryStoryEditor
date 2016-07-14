/**********************************************************
*Author: wangjiaying
*Date: 2016.7.7
*Func:
**********************************************************/
using System.Collections.Generic;

namespace CryStory.Runtime
{

    public class NodeContent : UpdateNode
    {
        protected List<NodeModifier> _contenNodeList = new List<NodeModifier>(2);

        private List<NodeModifier> _toRemoveNode = new List<NodeModifier>();
        private List<NodeModifier> _toAddNode = new List<NodeModifier>();

        public NodeModifier[] Nodes { get { return _contenNodeList.ToArray(); } }

        private NodeModifier[] _tempNodeList;
        /// <summary>
        /// 将节点添加至该容器。注意：不会更改节点本身容器数据！
        /// 使用节点SetContent代替
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool AddContentNode(NodeModifier node)
        {
            if (_contenNodeList.Contains(node) || node == null) return false;
            _contenNodeList.Add(node);
            //node.SetContent(this);
            return true;
        }

        public void AddContentNode(NodeModifier[] nodes)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                AddContentNode(nodes[i]);
            }
        }

        public bool RemoveContenNode(NodeModifier node)
        {
            return _contenNodeList.Remove(node);
        }

        protected override EnumResult OnStart()
        {
            _tempNodeList = _contenNodeList.ToArray();
            return base.OnStart();
        }

        protected override EnumResult OnUpdate()
        {
            if (_contenNodeList.Count == 0) return EnumResult.Success;
            //Run
            for (int i = 0; i < _contenNodeList.Count; i++)
            {
                //UnityEngine.Debug.Log("Run Node:" + _contenNodeList[i]._name);
                EnumResult result = _contenNodeList[i].Tick();
                if (result != EnumResult.Running)
                {
                    NodeModifier node = _contenNodeList[i];
                    if (result == EnumResult.Failed)
                        switch (RunMode)
                        {
                            case EnumRunMode.UntilSuccess:
                                continue;
                            case EnumRunMode.ReturnParentNode:
                                _toRemoveNode.Add(node);
                                _toAddNode.Add(node.Parent);
                                continue;
                            case EnumRunMode.StopNodeList:
                                _toRemoveNode.Add(node);
                                continue;
                        }

                    _toRemoveNode.Add(node);
                    _toAddNode.AddRange(node.NextNodes);
                }
            }

            ProcessNode();

            return EnumResult.Running;
        }

        protected void ProcessNode()
        {
            //Remove
            if (_toRemoveNode.Count > 0)
            {
                for (int i = 0; i < _toRemoveNode.Count; i++)
                {
                    //若处于最后一个节点，则结束时，将其初始节点重新添加
                    //if (_contenNodeList.Count == 1 && _toAddNode.Count == 0)
                    //    _finalNode = _contenNodeList[0];

                    _contenNodeList.Remove(_toRemoveNode[i]);
                }
                _toRemoveNode.Clear();
            }

            //Add
            if (_toAddNode != null)
            {
                AddContentNode(_toAddNode.ToArray());
                _toAddNode.Clear();
            }
        }

        protected override void OnEnd()
        {
            base.OnEnd();
            _contenNodeList.Clear();
            _contenNodeList.AddRange(_tempNodeList);
        }
    }
}