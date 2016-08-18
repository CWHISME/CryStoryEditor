/**********************************************************
*Author: wangjiaying
*Date: 2016.7.13
*Func:
**********************************************************/
using UnityEngine;
using Event = UnityEngine.Event;
using UnityEditor;
using System.Reflection;
using System;
using CryStory.Runtime;

namespace CryStory.Editor
{
    public class MissionEditorRuntime : NodeContentEditorRuntime<MissionEditorRuntime>
    {
        protected override void InternalOnGUI()
        {
            base.InternalOnGUI();
            CheckReturnStoryEditor();

            DrawHelp(_currentNode);
        }


        protected override Rect DrawNodeRect(NodeModifier node, bool coreNode = false)
        {
            Vector2 pos = CalcRealPosition(node._position);
            Rect nodeRect = Tools.GetNodeRect(pos);

            GUIStyle style = ResourcesManager.GetInstance.Node;
            GUIStyle selectStyle = ResourcesManager.GetInstance.NodeOn;
            if (node is CryStory.Runtime.Event)
            {
                style = ResourcesManager.GetInstance.EventNode;
                selectStyle = ResourcesManager.GetInstance.EventNodeOn;
            }
            else if (node is CryStory.Runtime.Condition)
            {
                style = ResourcesManager.GetInstance.ConditionNode;
                selectStyle = ResourcesManager.GetInstance.ConditionNodeOn;
            }
            else if (node is CryStory.Runtime.Action)
            {
                style = ResourcesManager.GetInstance.ActionNode;
                selectStyle = ResourcesManager.GetInstance.ActionNodeOn;
            }

            style = new GUIStyle(style);
            selectStyle = new GUIStyle(selectStyle);
            style.fontSize = (int)(style.fontSize * Tools.Zoom);
            selectStyle.fontSize = (int)(selectStyle.fontSize * Tools.Zoom);

            GUI.Box(nodeRect, coreNode ? "<color=#00FF00>" + node._name + "</color>" : node._name, _currentNode == node ? selectStyle : style);

            DragNodeEvent(node, nodeRect);

            DrawRunModeLable(node, nodeRect);

            if (coreNode) DrawRunningNodeLabel(nodeRect);
            else DrawDescription(nodeRect, (node as StoryNode).ToDescription());

            return nodeRect;
        }

        /// <summary>
        /// 检查是否应当返回上级
        /// </summary>
        private void CheckReturnStoryEditor()
        {
            if (Tools.MouseDoubleClick && !Tools.IsValidMouseAABB(_currentNodeRect))
            {
                if (Event.current.button == 0)
                    _window.EditMission = null;
            }
        }
    }
}