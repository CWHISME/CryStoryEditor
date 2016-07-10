/**********************************************************
*Author: wangjiaying
*Date: 2016.7.7
*Func:
**********************************************************/
using System.Collections.Generic;

namespace CryStory.Runtime
{

    public class NodeModifier : NodeBase
    {
        private NodeModifier _lastNode = null;

        private NodeContent _content = null;

        private List<NodeModifier> _nextNodeList = new List<NodeModifier>();

        //protected bool _coreNode = true;

        ///// <summary>
        ///// 是否是核心节点，即：起始节点
        ///// </summary>
        //public bool IsCoreNode { get { return _coreNode; } set { _coreNode = value; } }

        public NodeModifier[] NextNodes { get { return _nextNodeList.ToArray(); } }

        public NodeModifier[] GetNextNodes(NodeModifier node)
        {
            return _nextNodeList.FindAll((n) => !node.IsParent(n)).ToArray();
        }
        /// <summary>
        /// 重设其父节点
        /// </summary>
        /// <param name="node"></param>
        public bool SetParent(NodeModifier node)
        {
            if (node == this) return false;
            if (node.IsChild(this, false)) return false;
            node.AddNextNode(this);
            if (node.IsParent(this)) return true;
            DeleteParent();
            _lastNode = node;

            RemoveFromContent();
            return true;
        }

        /// <summary>
        /// 删除其父节点
        /// </summary>
        public void DeleteParent()
        {
            if (_lastNode == null) return;
            SetContent(_lastNode.GetContentNode());
            _lastNode.Remove(this);
            _lastNode = null;

            DeleteSingleConnectChildNode();
        }

        /// <summary>
        /// 删除自己单方面链接的子节点
        /// </summary>
        public void DeleteSingleConnectChildNode()
        {
            List<NodeModifier> toRemove = new List<NodeModifier>();
            for (int i = 0; i < _nextNodeList.Count; i++)
            {
                if (_nextNodeList[i]._lastNode != this)
                    toRemove.Add(_nextNodeList[i]);
            }

            for (int i = 0; i < toRemove.Count; i++)
            {
                Remove(toRemove[i]);
            }
        }

        public NodeContent GetContentNode()
        {
            if (_content != null) return _content;
            if (_lastNode != null) return _lastNode.GetContentNode();
            return null;
        }

        /// <summary>
        /// 添加一个子节点
        /// </summary>
        /// <param name="next"></param>
        public void AddNextNode(NodeModifier next)
        {
            if (!_nextNodeList.Contains(next)) _nextNodeList.Add(next);
        }

        /// <summary>
        /// 删除指定子节点
        /// </summary>
        /// <param name="node"></param>
        public void Remove(NodeModifier node)
        {
            _nextNodeList.Remove(node);
            if (node._lastNode == this)
                node.SetContent(GetContentNode());
        }

        /// <summary>
        /// 是否属于自身的父节点
        /// </summary>
        /// <param name="node">可能是父节点的节点</param>
        /// <returns></returns>
        public bool IsParent(NodeModifier node)
        {
            if (_lastNode == null) return false;
            if (_lastNode == node) return true;
            return _lastNode.IsParent(node);
        }

        /// <summary>
        /// 是否是属于该节点的子节点，将检查所有的链表
        /// </summary>
        /// <param name="node"></param>
        /// <param name="includeSingleNode">是否包括单方面链接的节点(父级具有子级，子级不具有父级)</param>
        /// <returns></returns>
        public bool IsChild(NodeModifier node, bool includeSingleNode = true)
        {
            if (includeSingleNode)
                if (node._lastNode != this) return false;
            if (_nextNodeList.Contains(node))
                return true;
            for (int i = 0; i < _nextNodeList.Count; i++)
            {
                if (_nextNodeList[i].IsChild(node))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 设置父容器
        /// </summary>
        /// <param name="content"></param>
        public void SetContent(NodeContent content)
        {
            if (content == null) return;
            RemoveFromContent();
            _content = content;
            content.AddContentNode(this);
        }

        /// <summary>
        /// 从父容器中删除自己
        /// </summary>
        private void RemoveFromContent()
        {
            if (_content != null)
            {
                _content.RemoveContenNode(this);
                _content = null;
            }
        }

        public bool HaveParent { get { return _lastNode != null; } }
        public NodeBase Parent { get { return _lastNode; } }
    }
}