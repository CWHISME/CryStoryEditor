/**********************************************************
*Author: wangjiaying
*Date: 2016.7.7
*Func:
**********************************************************/
using System.Collections.Generic;

namespace CryStory.Runtime
{

    public class NodeContent : NodeModifier
    {
        protected List<NodeModifier> _contenNodeList = new List<NodeModifier>(2);

        private List<NodeModifier> _toRemoveNode = new List<NodeModifier>();
        private NodeModifier[] _toAddNode;

        public NodeModifier[] Nodes { get { return _contenNodeList.ToArray(); } }

        /// <summary>
        /// 将节点添加至该容器。注意：不会更改节点本身容器数据！
        /// 使用节点SetContent代替
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool AddContentNode(NodeModifier node)
        {
            if (_contenNodeList.Contains(node)) return false;
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

        public override EnumResult Tick()
        {
            if (_contenNodeList.Count == 0) return EnumResult.Success;
            //Run
            for (int i = 0; i < _contenNodeList.Count; i++)
            {
                UnityEngine.Debug.Log("Run Node:" + _contenNodeList[i]._name);
                if (_contenNodeList[i].Tick() != EnumResult.Running)
                {
                    NodeModifier node = _contenNodeList[i];
                    _toRemoveNode.Add(node);
                    _toAddNode = node.NextNodes;
                }
            }

            //Remove
            if (_toRemoveNode.Count > 0)
            {
                for (int i = 0; i < _toRemoveNode.Count; i++)
                {
                    _contenNodeList.Remove(_toRemoveNode[i]);
                }
                _toRemoveNode.Clear();
            }

            //Add
            if (_toAddNode != null)
            {
                AddContentNode(_toAddNode);
                _toAddNode = null;
            }

            return EnumResult.Running;
        }
    }
}