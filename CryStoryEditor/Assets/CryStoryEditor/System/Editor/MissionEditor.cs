/**********************************************************
*Author: wangjiaying
*Date: 2016.7.7
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
    public class MissionEditor : NodeContentEditor<MissionEditor>
    {
        protected override void InternalOnGUI()
        {
            base.InternalOnGUI();
            CheckReturnStoryEditor();
            ShowRightClickMenu();
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

            GUI.Box(nodeRect, coreNode ? "<color=#00FF00>" + node._name + "</color>" : node._name, _currentNode == node ? selectStyle : style);

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
                {
                    _currentNode = null;
                    _window._editMission = null;
                }
            }
        }

        private Vector2 _mousePosition;

        private void ShowRightClickMenu()
        {
            if (Event.current != null)
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    _mousePosition = Event.current.mousePosition;
                    if (Event.current.button == 1)
                    {
                        if (_currentHover == null)
                        {
                            CreateNodeMenu();
                            return;
                        }

                        if (!Tools.IsValidMouseAABB(Tools.GetNodeRect(CalcRealPosition(_currentHover._position))))
                            CreateNodeMenu();
                        else ShowDeleteNodeMenu();
                    }
                }
            }
        }

        private void CreateNodeMenu()
        {
            Vector2 mousePos = Event.current.mousePosition;
            if (mousePos.x < _contentRect.x) return;
            if (mousePos.y < _contentRect.y) return;
            GenericMenu menu = new GenericMenu();

            Assembly asm = Assembly.GetAssembly(typeof(CryStory.Runtime.Story));
            Type[] types = asm.GetTypes();

            AddEventMenu(menu, types, asm);
            AddConditionMenu(menu, types, asm);
            AddActionMenu(menu, types, asm);

            menu.ShowAsContext();
        }

        private void ShowDeleteNodeMenu()
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Delete"), false, () =>
            {
                Runtime.NodeModifier.Delete(_currentHover);
                _currentHover = null;
            });
            menu.ShowAsContext();
        }

        private void AddEventMenu(GenericMenu menu, Type[] type, Assembly asm)
        {
            Type baseType = typeof(CryStory.Runtime.Event);
            Type[] typeList = Array.FindAll<Type>(type, (t) => t.IsSubclassOf(baseType));

            for (int i = 0; i < typeList.Length; i++)
            {
                Type nodeType = typeList[i];
                menu.AddItem(new GUIContent("Create/Event/" + nodeType.Name), false, () =>
                {
                    //Debug.Log("Add Node: " + name);
                    CreateNode(asm, nodeType);
                });
            }
        }

        private void AddConditionMenu(GenericMenu menu, Type[] type, Assembly asm)
        {
            Type baseType = typeof(CryStory.Runtime.Condition);
            Type[] typeList = Array.FindAll<Type>(type, (t) => t.IsSubclassOf(baseType));

            for (int i = 0; i < typeList.Length; i++)
            {
                Type nodeType = typeList[i];
                menu.AddItem(new GUIContent("Create/Condition/" + nodeType.Name), false, () =>
                {
                    //Debug.Log("Add Node: " + name);
                    CreateNode(asm, nodeType);
                });
            }
        }

        private void AddActionMenu(GenericMenu menu, Type[] type, Assembly asm)
        {
            Type baseType = typeof(CryStory.Runtime.Action);
            Type[] typeList = Array.FindAll<Type>(type, (t) => t.IsSubclassOf(baseType));

            for (int i = 0; i < typeList.Length; i++)
            {
                Type nodeType = typeList[i];
                menu.AddItem(new GUIContent("Create/Action/" + nodeType.Name), false, () =>
                {
                    //Debug.Log("Add Node: " + name);
                    CreateNode(asm, nodeType);
                });
            }
        }

        private void CreateNode(Assembly asm, Type type)
        {
            object o = asm.CreateInstance(type.FullName);
            if (o != null)
            {
                Runtime.NodeModifier node = o as Runtime.NodeModifier;
                if (node == null) return;
                node._name = type.Name;
                node._position = CalcVirtualPosition(_mousePosition);
                Runtime.NodeModifier.SetContent(node, _window._editMission);
                //node.SetContent(_window._editMission);
            }
        }
    }
}