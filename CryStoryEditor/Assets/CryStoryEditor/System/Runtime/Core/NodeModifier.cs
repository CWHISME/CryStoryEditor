/**********************************************************
*Author: wangjiaying
*Date: 2016.7.7
*Func:
**********************************************************/
using System;
using System.Collections.Generic;
using System.IO;

namespace CryStory.Runtime
{

    public class NodeModifier : NodeBase, ISerialize
    {
        private NodeModifier _lastNode = null;

        private NodeContent _content = null;

        private List<NodeModifier> _nextNodeList = new List<NodeModifier>();

        public EnumRunMode RunMode = EnumRunMode.UntilSuccess;
        //public EnumRunMode RunMode { get { return _runMode; } set { _runMode = value; } }

        //protected bool _coreNode = true;

        ///// <summary>
        ///// 是否是核心节点，即：起始节点
        ///// </summary>
        //public bool IsCoreNode { get { return _coreNode; } set { _coreNode = value; } }

        public NodeModifier[] NextNodes { get { return _nextNodeList.ToArray(); } }

        public NodeModifier[] GetNextNodes(NodeModifier node)
        {
            return _nextNodeList.FindAll((n) => !node.IsParent(n) && n.Parent == node).ToArray();
        }
        /// <summary>
        /// 重设其父节点
        /// </summary>
        /// <param name="node"></param>
        public void SetParent(NodeModifier node)
        {
            //if (node == this) return false;
            //if (node.IsChild(this, false)) return false;
            node.AddNextNode(this);
            //if (node.IsParent(this)) return true;
            //RemoveFromContent();
            //DeleteParent();
            _lastNode = node;
            //return true;
        }

        public bool CanSetParent(NodeModifier node)
        {
            if (node == this) return false;
            if (node.IsChild(this, false)) return false;
            if (node.IsParent(this)) return false;
            return true;
        }

        /// <summary>
        /// 删除其父节点
        /// </summary>
        public void DeleteParent()
        {
            if (_lastNode == null) return;
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
        /// 检查子节点列表中，是否具有父节点存在（循环）
        /// </summary>
        /// <returns></returns>
        public bool HaveParentNodeInNext()
        {
            return _nextNodeList.Find((n) => IsParent(n)) != null;
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

        public int ParentToLayer(NodeModifier parent, int layer = 0)
        {
            layer++;
            if (this == parent) return layer;
            if (_lastNode == null) return 0;
            return _lastNode.ParentToLayer(parent, layer);
        }

        public NodeModifier LayerToParent(int layer)
        {
            layer--;
            if (layer == 0) return this;
            if (_lastNode != null) return _lastNode.LayerToParent(layer);
            return null;
        }

        /// <summary>
        /// 设置父容器
        /// </summary>
        /// <param name="content"></param>
        public void SetContent(NodeContent content)
        {
            if (content == null) return;
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

        public static void Delete(NodeModifier node)
        {
            NodeModifier[] nodes = node.NextNodes;
            for (int i = 0; i < nodes.Length; i++)
            {
                node.Remove(nodes[i]);
            }

            node.DeleteParent();
            node.RemoveFromContent();
        }

        public static bool SetParent(NodeModifier node, NodeModifier parentNode)
        {
            if (node.CanSetParent(parentNode))
            {
                node.RemoveFromContent();
                node.DeleteParent();
                node.SetParent(parentNode);
                return true;
            }
            return false;
        }

        public static void SetContent(NodeModifier node, NodeContent content)
        {
            node.RemoveFromContent();
            node.DeleteParent();
            node.SetContent(content);
        }

        public static void SetToDefaltContent(NodeModifier node)
        {
            SetContent(node, node.GetContentNode());
        }

        public void Serialize(BinaryWriter w)
        {
            w.Write(UnityEngine.JsonUtility.ToJson(this));
            w.Write(_nextNodeList.Count);
            for (int i = 0; i < _nextNodeList.Count; i++)
            {
                NodeModifier node = _nextNodeList[i];
                w.Write(ParentToLayer(node));
                bool isSingleNode = node.Parent != this;
                w.Write(isSingleNode);
                if (!isSingleNode)
                {
                    w.Write(node.GetType().FullName);
                    node.Serialize(w);
                }

            }
        }

        public void Deserialize(BinaryReader r)
        {
            UnityEngine.JsonUtility.FromJsonOverwrite(r.ReadString(), this);
            int count = r.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                int layer = r.ReadInt32();
                bool isSingleNode = r.ReadBoolean();
                NodeModifier node;
                if (isSingleNode)
                {
                    node = LayerToParent(layer);
                    if (node != null)
                        _nextNodeList.Add(node);
                }
                else {
                    string fullName = r.ReadString();
                    node = ReflectionHelper.CreateInstance<NodeModifier>(fullName);
                    if (node == null)
                    {
                        UnityEngine.Debug.LogError("Error: The Mission Node [" + fullName + "] Was Lost!");
                        return;
                    }

                    SetParent(node, this);
                    node.Deserialize(r);
                }
            }
        }

        public bool HaveParent { get { return _lastNode != null; } }
        public NodeModifier Parent { get { return _lastNode; } }



    }
}