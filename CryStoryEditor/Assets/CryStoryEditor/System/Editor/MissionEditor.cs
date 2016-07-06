/**********************************************************
*Author: wangjiaying
*Date: 2016.6.17
*Func:
**********************************************************/
using UnityEditor;
using UnityEngine;
using Event = UnityEngine.Event;

namespace CryStory.Editor
{
    public class MissionEditor : NodeEditor<MissionEditor>
    {

        protected override void InternalOnGUI()
        {
            CreateMissionNode();
        }

        private void CreateMissionNode()
        {
            if (Event.current != null)
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    if (Event.current.button == 1)
                    {
                        //Debug.Log(Event.current.mousePosition + "     Rect:" + _contentRect);
                        Vector2 mousePos = Event.current.mousePosition;
                        if (mousePos.x < _contentRect.x) return;
                        if (mousePos.y < _contentRect.y) return;
                        GenericMenu menu = new GenericMenu();
                        menu.AddItem(new GUIContent("Create/New Mission"), false, () =>
                        {
                            _currentNode = _window._storyObject.AddNewMission();
                            _currentNode._position = CalcVirtualPosition(mousePos);
                            _currentNode._name = "New Mission";
                        });
                        menu.ShowAsContext();
                    }
                }
            }
        }

    }
}
