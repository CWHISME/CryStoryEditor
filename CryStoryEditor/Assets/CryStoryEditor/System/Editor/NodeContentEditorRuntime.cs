/**********************************************************
*Author: wangjiaying
*Date: 2016.7.13
*Func:
**********************************************************/
using UnityEngine;
using System.Collections;
using CryStory.Runtime;

namespace CryStory.Editor
{
    public class NodeContentEditorRuntime<T> : NodeEditor<T> where T : class, new()
    {
        protected override void DrawNodes(NodeModifier[] nodeList, bool coreNode = false)
        {

            //DrawChildNodes(nodeList);
            for (int i = 0; i < nodeList.Length; i++)
            {
                NodeModifier node = nodeList[i];

                DrawChildNodes(node.NextNodes);
                DrawParentNodes(node, coreNode);
            }
        }

        private void DrawChildNodes(NodeModifier[] nodeList, bool coreNode = false)
        {
            for (int i = 0; i < nodeList.Length; i++)
            {
                NodeModifier node = nodeList[i];
                DrawNodeRect(node, coreNode);

                //Draw conection bazier line
                DrawBazierLine(node);
                //Draw Debug Line
                //DrawDebugBazierLine(node);
                //Draw Node
                //DrawNodeSlot(node, nodeRect);

                DrawChildNodes(node.GetNextNodes(node));

                //if (_isConnecting) continue;

                //DragNodeEvent(node, nodeRect);
            }
        }

        private void DrawParentNodes(NodeModifier node, bool coreNode = false)
        {
            if (node == null) return;

            DrawNodeRect(node, coreNode);

            //Draw conection bazier line
            DrawDebugBazierLine(node);
            //Draw Debug Line
            //DrawDebugBazierLine(node);
            //Draw Node
            //DrawNodeSlot(node, nodeRect);

            DrawParentNodes(node.Parent);

        }

        protected override Rect DrawNodeRect(NodeModifier node, bool coreNode = false)
        {
            Rect rect = base.DrawNodeRect(node, coreNode);
            DragNodeEvent(node, rect);
            //if (node.Parent != null)
            //    DrawNodeRect(node.Parent);
            return rect;
        }
    }
}