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

            DrawRunModeLable(node, nodeRect);

            DrawDescription(nodeRect, (node as StoryNode).ToDescription());

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
                    _window.EditMission = null;
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

            menu.AddItem(new GUIContent("Duplicate"), false, () =>
            {
                DuplicateNode(_currentHover);
            });

            menu.AddItem(new GUIContent("Delete"), false, () =>
            {
                Runtime.NodeModifier.Delete(_currentHover);
                if (_currentNode == _currentHover) _currentNode = null;
                _currentHover = null;
            });
            menu.ShowAsContext();
        }

        private void AddEventMenu(GenericMenu menu, Type[] type, Assembly asm)
        {
            Type baseType = typeof(CryStory.Runtime.Event);
            Type[] typeList = Array.FindAll<Type>(type, (t) => t.IsSubclassOf(baseType));

            AddMenu(typeList, menu, "Create/Event/");

            //for (int i = 0; i < typeList.Length; i++)
            //{
            //    Type nodeType = typeList[i];
            //    CategoryAttribute[] category = nodeType.GetCustomAttributes(typeof(CategoryAttribute), true) as CategoryAttribute[];
            //    string cat = category.Length > 0 ? category[0].Category : "";
            //    menu.AddItem(new GUIContent("Create/Event/" + cat + "/" + nodeType.Name), false, () =>
            //        {
            //            //Debug.Log("Add Node: " + name);
            //            CreateNode(asm, nodeType);
            //        });
            //}
        }

        private void AddConditionMenu(GenericMenu menu, Type[] type, Assembly asm)
        {
            Type baseType = typeof(CryStory.Runtime.Condition);
            Type[] typeList = Array.FindAll<Type>(type, (t) => t.IsSubclassOf(baseType));

            AddMenu(typeList, menu, "Create/Condition/");

            //for (int i = 0; i < typeList.Length; i++)
            //{
            //    Type nodeType = typeList[i];
            //    menu.AddItem(new GUIContent("Create/Condition/" + nodeType.Name), false, () =>
            //    {
            //        //Debug.Log("Add Node: " + name);
            //        CreateNode(asm, nodeType);
            //    });
            //}
        }

        private void AddActionMenu(GenericMenu menu, Type[] type, Assembly asm)
        {
            Type baseType = typeof(CryStory.Runtime.Action);
            Type[] typeList = Array.FindAll<Type>(type, (t) => t.IsSubclassOf(baseType));

            AddMenu(typeList, menu, "Create/Action/");

            //for (int i = 0; i < typeList.Length; i++)
            //{
            //    Type nodeType = typeList[i];
            //    menu.AddItem(new GUIContent("Create/Action/" + nodeType.Name), false, () =>
            //    {
            //        //Debug.Log("Add Node: " + name);
            //        CreateNode(asm, nodeType);
            //    });
            //}
        }

        private void AddMenu(Type[] typeList, GenericMenu menu, string prefix)
        {
            for (int i = 0; i < typeList.Length; i++)
            {
                Type nodeType = typeList[i];
                if (nodeType.Name.StartsWith("_")) continue;
                CategoryAttribute[] category = nodeType.GetCustomAttributes(typeof(CategoryAttribute), true) as CategoryAttribute[];
                string cat = category.Length > 0 ? category[0].Category + "/" : "";
                menu.AddItem(new GUIContent(prefix + cat + nodeType.Name), false, () =>
               {
                   //Debug.Log("Add Node: " + name);
                   CreateNode(nodeType, _window.EditMission, CalcVirtualPosition(_mousePosition));
               });
            }
        }

        private void CreateNode(Type type, DragModifier content, Vector3 pos)
        {
            object o = ReflectionHelper.Asm.CreateInstance(type.FullName);
            if (o != null)
            {
                Runtime.StoryNode node = o as Runtime.StoryNode;
                if (node == null) return;
                node._name = type.Name;
                node._position = pos;
                node.SetID(content.GenerateID());
                Runtime.NodeModifier.SetContent(node, content);
            }
        }
    }
}