/**********************************************************
*Author: wangjiaying
*Date: 2016.7.29
*Func:
**********************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace CryStory.Runtime
{

    [Help("该节点将会移除运行中的所有单子节点(即红色连线连接者)。白色连线不受影响。")]
    public class Killer : Decorator
    {
        public KillMode Mode = KillMode.Node;

        protected override EnumResult OnProcessing(NodeContent content, NodeModifier[] nextNode)
        {
            switch (Mode)
            {
                case KillMode.Node:
                    for (int i = 0; i < nextNode.Length; i++)
                    {
                        if (nextNode[i].Parent != this)
                            content.RemoveContenNode(nextNode[i]);
                    }
                    break;
                case KillMode.NodeAndChild:
                    for (int i = 0; i < nextNode.Length; i++)
                    {
                        if (nextNode[i].Parent != this)
                        {
                            RemoveChild(content, nextNode[i]);
                        }
                    }
                    break;
            }

            return EnumResult.Success;
        }

        protected void RemoveChild(NodeContent content, NodeModifier node)
        {
            if (content.RemoveContenNode(node))
                node.ForceStop();

            NodeModifier[] nodes = node.NextNodes;
            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i].Parent != node) continue;
                RemoveChild(content, nodes[i]);
            }
        }

        public override void GetNextNodes(List<NodeModifier> nodes)
        {
            NodeModifier node = _nextNodeList.Find(n => n.Parent == this);
            if (node != null)
                nodes.Add(node);
        }

        public override Color ColorLine
        {
            get
            {
                return Color.red;
            }
        }

        public override string ToDescription()
        {
            switch (Mode)
            {
                case KillMode.Node:
                    return "停止指定运行中的节点。";
                case KillMode.NodeAndChild:
                    return "停止指定运行中节点及指定节点所有运行中的子节点。";
            }
            return "?";
        }
    }

    public enum KillMode
    {
        Node, NodeAndChild
    }
}