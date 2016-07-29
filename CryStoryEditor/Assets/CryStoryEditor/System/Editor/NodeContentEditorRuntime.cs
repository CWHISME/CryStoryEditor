/**********************************************************
*Author: wangjiaying
*Date: 2016.7.13
*Func:
**********************************************************/
using UnityEngine;
using System.Collections;
using CryStory.Runtime;
using UnityEditor;
using System.Collections.Generic;

namespace CryStory.Editor
{
    public class NodeContentEditorRuntime<T> : NodeEditor<T> where T : class, new()
    {

        private List<NodeModifier> _alreadyDrawNode = new List<NodeModifier>(10);

        protected override void DrawNodes(NodeModifier[] nodeList, bool coreNode = false)
        {
            _alreadyDrawNode.Clear();

            //DrawChildNodes(nodeList);
            for (int i = 0; i < nodeList.Length; i++)
            {
                NodeModifier node = nodeList[i];

                //DrawChildNodes(node.NextNodes);
                DrawParentNodes(node, coreNode);
            }
        }

        private void DrawChildNodes(NodeModifier[] nodeList, bool coreNode = false)
        {
            for (int i = 0; i < nodeList.Length; i++)
            {
                NodeModifier node = nodeList[i];

                if (_alreadyDrawNode.Contains(node)) continue;

                DrawNodeRect(node, coreNode);

                //Draw conection bazier line
                if (node is Decorator)
                {
                    DrawBazierLine(node, (node as Decorator).ColorLine);
                }
                else
                    DrawBazierLine(node);
                //Draw Debug Line
                //DrawDebugBazierLine(node);
                //Draw Node
                //DrawNodeSlot(node, nodeRect);

                _alreadyDrawNode.Add(node);

                DrawChildNodes(node.GetNextNodes(node));

            }
        }

        private void DrawParentNodes(NodeModifier node, bool coreNode = false)
        {
            if (node == null) return;

            DrawNodeRect(node, coreNode);

            //Draw conection bazier line
            DrawDebugBazierLine(node);

            DrawChildNodes(node.NextNodes);

            DrawParentNodes(node.Parent);

        }

        protected override Rect DrawNodeRect(NodeModifier node, bool coreNode = false)
        {
            Rect rect = base.DrawNodeRect(node, coreNode);
            DragNodeEvent(node, rect);

            if (coreNode) DrawRunningNodeLabel(rect);
            //if (node.Parent != null)
            //    DrawNodeRect(node.Parent);
            return rect;
        }

        protected virtual void DrawRunningNodeLabel(Rect rect)
        {
            Rect labelRect = new Rect(rect);
            labelRect.position = new Vector2(labelRect.x + (labelRect.width / 2 - 30), labelRect.y + labelRect.height);
            EditorGUI.LabelField(labelRect, "<color=#7CFC00>Running...</color>", ResourcesManager.GetInstance.GetFontStyle((int)(13 * Tools.Zoom)));
        }
    }
}