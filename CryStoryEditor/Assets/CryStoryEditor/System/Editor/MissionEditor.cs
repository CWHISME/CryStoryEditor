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

namespace CryStory.Editor
{
    public class MissionEditor : NodeContentEditor<MissionEditor>
    {
        protected override void InternalOnGUI()
        {
            base.InternalOnGUI();
            CheckReturnStoryEditor();
            CreateNodeMenu();
        }

        private void CheckReturnStoryEditor()
        {
            if (Tools.MouseDoubleClick && !Tools.IsValidMouseAABB(_currentNodeRect))
            {
                if (Event.current.button == 0)
                    _window._editMission = null;
            }
        }

        private Vector2 _mousePosition;

        private void CreateNodeMenu()
        {
            if (Event.current != null)
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    _mousePosition = Event.current.mousePosition;
                    if (Event.current.button == 1)
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
                }
            }
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