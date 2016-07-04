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

        public NodeBase _lastNode = null;
        public List<NodeBase> _nextNodeList = new List<NodeBase>();

        public void AddNextNode(NodeBase next)
        {
            if (!_nextNodeList.Contains(next)) _nextNodeList.Add(next);
        }

        //将其从父节点删除
        public void RemoveFromLastNode()
        {
            if (_lastNode == null) return;
            _lastNode.Remove(this);
        }

        public void Remove(NodeBase node)
        {
            _nextNodeList.Remove(node);
        }

        //For Editor
        public UnityEngine.Vector2 _position;
        public string _name;
    }
}
