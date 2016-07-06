/**********************************************************
*Author: wangjiaying
*Date: 2016.6.6
*Func:
**********************************************************/

using System.Collections.Generic;

namespace CryStory.Runtime
{
    abstract public class NodeBase
    {
        public virtual EnumResult Tick() { return EnumResult.Success; }

        private NodeBase _lastNode = null;

        public List<NodeBase> _nextNodeList = new List<NodeBase>();

        /// <summary>
        /// 重设其父节点
        /// </summary>
        /// <param name="node"></param>
        public void SetParent(NodeBase node)
        {
            if (node == this) return;
            DeleteParent();
            node.AddNextNode(this);
            _lastNode = node;
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
        public void AddNextNode(NodeBase next)
        {
            if (!_nextNodeList.Contains(next)) _nextNodeList.Add(next);
        }

        /// <summary>
        /// 删除指定子节点
        /// </summary>
        /// <param name="node"></param>
        public void Remove(NodeBase node)
        {
            _nextNodeList.Remove(node);
        }

        /// <summary>
        /// 是否属于自身的父节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool IsParent(NodeBase node)
        {
            if (_lastNode == null) return false;
            if (_lastNode == node) return true;
            return _lastNode.IsParent(node);
        }

        public bool HaveParent { get { return _lastNode != null; } }
        public NodeBase Parent { get { return _lastNode; } }

        //For Editor
        public UnityEngine.Vector2 _position;
        public string _name;
    }
}
