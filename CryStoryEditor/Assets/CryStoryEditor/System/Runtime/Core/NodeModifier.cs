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
        /// <summary>
        /// 处于Mission中的唯一性ID
        /// </summary>
        public int _id;

        /// <summary>
        /// 运行节点的模式
        /// </summary>
        public EnumRunMode RunMode = EnumRunMode.UntilSuccess;

        /// <summary>
        /// 父节点引用
        /// </summary>
        protected NodeModifier _lastNode = null;

        /// <summary>
        /// 容器节点引用
        /// </summary>
        protected NodeContent _content = null;

        /// <summary>
        /// 子节点列表
        /// </summary>
        protected List<NodeModifier> _nextNodeList = new List<NodeModifier>();

        /// <summary>
        /// 获取子节点列表
        /// </summary>
        public NodeModifier[] NextNodes { get { return _nextNodeList.ToArray(); } }

        /// <summary>
        /// 获取指定节点的父节点是自身，并且指定节点并非子节点的父节点的子节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public NodeModifier[] GetNextNodes(NodeModifier node)
        {
            return _nextNodeList.FindAll((n) => !node.IsParent(n) && n.Parent == node).ToArray();
        }

        /// <summary>
        ///设置父节点
        /// </summary>
        /// <param name="node"></param>
        public void SetParent(NodeModifier node)
        {
            node.AddNextNode(this);
            _lastNode = node;
        }

        /// <summary>
        /// 获取子节点列表
        /// </summary>
        /// <param name="nodes"></param>
        public virtual void GetNextNodes(List<NodeModifier> nodes)
        {
            nodes.AddRange(_nextNodeList);
        }

        /// <summary>
        /// 是否可以将指定节点设置为自身的父节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取容器节点
        /// </summary>
        /// <returns></returns>
        public NodeContent GetContent()
        {
            if (_content != null) return _content;
            if (_lastNode != null) return _lastNode.GetContent();
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

        /// <summary>
        /// 父节点转化为Layer，即层级数(用于保存)
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public int ParentToLayer(NodeModifier parent, int layer = 0)
        {
            layer++;
            if (this == parent) return layer;
            if (_lastNode == null) return 0;
            return _lastNode.ParentToLayer(parent, layer);
        }

        /// <summary>
        /// 通过层级数转化为父节点引用
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
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
        /// 获取顶级父容器
        /// </summary>
        public NodeModifier GetTopParent()
        {
            if (Parent != null)
                return Parent.GetTopParent();
            return this;
        }

        /// <summary>
        /// 从父容器中删除自己
        /// </summary>
        public void RemoveFromContent()
        {
            if (_content != null)
            {
                _content.RemoveContenNode(this);
                _content = null;
            }
        }

        //========ID================================
        /// <summary>
        /// 通过ID获取节点引用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NodeModifier GetNextNodeByID(int id)
        {
            if (_id == id) return this;

            for (int i = 0; i < _nextNodeList.Count; i++)
            {
                NodeModifier node = _nextNodeList[i];
                if (node == null) continue;
                //if (IsParent(node)) continue;
                if (node.Parent != this) continue;
                if (CompareNodeID(node, id))
                    return node;

                NodeModifier node2 = node.GetNextNodeByID(id);
                if (node2 != null)
                    return node2;
            }

            return null;
        }

        public void SetID(int id)
        {
            _id = id;
        }

        private bool CompareNodeID(NodeModifier node, int id)
        {
            if (node == null) return false;
            return node._id == id;
        }

        //Static=============================================
        //以下静态方法将会同时处理设置中的各项引用、依赖关系
        //<<<===============================================
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
            SetContent(node, node.GetContent());
        }

        //Save===========================================

        public virtual void Serialize(BinaryWriter w)
        {
            w.Write(UnityEngine.JsonUtility.ToJson(this));
            w.Write(_nextNodeList.Count);
            for (int i = 0; i < _nextNodeList.Count; i++)
            {
                NodeModifier node = _nextNodeList[i];
                w.Write(node._id);//(ParentToLayer(node));
                bool isSingleNode = node.Parent != this;
                w.Write(isSingleNode);
                if (!isSingleNode)
                {
                    w.Write(node.GetType().FullName);
                    node.Serialize(w);
                }

            }

            OnSaved(w);
        }

        public virtual void Deserialize(BinaryReader r)
        {
            UnityEngine.JsonUtility.FromJsonOverwrite(r.ReadString(), this);
            int count = r.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                //int layer = r.ReadInt32();
                int id = r.ReadInt32();
                bool isSingleNode = r.ReadBoolean();
                NodeModifier node;
                if (isSingleNode)
                {
                    node = GetContent().GetNodeByID(id);// GetTopParent().GetNextNodeByID(id);//LayerToParent(layer);
                    if (node != null)
                        _nextNodeList.Add(node);
                    else
                        GetContent().OnNodeLoaded += (con) =>
                    {
                        NodeModifier child = con.GetNodeByID(id);
                        if (child != null)
                            _nextNodeList.Add(child);
                    };
                }
                else {
                    string fullName = r.ReadString();
                    node = ReflectionHelper.CreateInstance<NodeModifier>(fullName);
                    if (node == null)
                    {
#if UNITY_EDITOR
                        UnityEngine.Debug.LogError("Error: The Mission Node [" + fullName + "] Was Lost!");
#endif
                        //return;
                        node = ReflectionHelper.CreateInstance<NodeModifier>("CryStory.Runtime._MissingNode");
                    }

                    SetParent(node, this);
                    node.Deserialize(r);
                }
            }

            OnLoaded(r);
        }

        public bool HaveParent { get { return _lastNode != null; } }
        public NodeModifier Parent { get { return _lastNode; } }
        public NodeContent Content { get { return _content; } }

        protected virtual void OnSaved(BinaryWriter w) { }
        protected virtual void OnLoaded(BinaryReader r) { }

    }
}