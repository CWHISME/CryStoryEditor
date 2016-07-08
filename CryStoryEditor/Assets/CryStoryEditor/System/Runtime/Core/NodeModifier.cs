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

        public NodeModifier[] NextNodes { get { return _nextNodeList.ToArray(); } }
        /// <summary>
        /// 重设其父节点
        /// </summary>
        /// <param name="node"></param>
        public void SetParent(NodeModifier node)
        {
            if (node == this) return;
            DeleteParent();
            node.AddNextNode(this);
            _lastNode = node;

            RemoveFromContent();
        }

        /// <summary>
        /// 删除其父节点
        /// </summary>
        public void DeleteParent()
        {
            if (_lastNode == null) return;
            _lastNode.Remove(this);
            _lastNode = null;
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
        }

        /// <summary>
        /// 是否属于自身的父节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool IsParent(NodeModifier node)
        {
            if (_lastNode == null) return false;
            if (_lastNode == node) return true;
            return _lastNode.IsParent(node);
        }

        /// <summary>
        /// 设置父容器
        /// </summary>
        /// <param name="content"></param>
        public void SetContent(NodeContent content)
        {
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