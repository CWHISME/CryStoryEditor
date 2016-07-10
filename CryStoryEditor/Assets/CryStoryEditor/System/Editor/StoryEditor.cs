/**********************************************************
*Author: wangjiaying
*Date: 2016.6.17
*Func:
**********************************************************/
using CryStory.Runtime;
using UnityEditor;
using UnityEngine;
using Event = UnityEngine.Event;

namespace CryStory.Editor
{
    public class StoryEditor : NodeContentEditor<StoryEditor>
    {

        protected override void InternalOnGUI()
        {
            base.InternalOnGUI();
            CreateMissionNode();
            OnDoubleClickMission();
        }

        protected override void OnNodeNameChange(NodeModifier node, string oldName)
        {
            base.OnNodeNameChange(node, oldName);
            if (!_window._storyObject.RenameMission(oldName, node._name))
            {
                node._name = oldName;
            }
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
                            MissionObject mo = ScriptableObject.CreateInstance<MissionObject>();
                            mo._mission = _window._storyObject.AddNewMission();

                            _currentNode = mo._mission;
                            _currentNode._position = CalcVirtualPosition(mousePos);
                            _currentNode._name = "New Mission";
                            _currentNode.SetContent(_window._Story);

                            int index = 1;
                            while (_window._storyObject.GetMissionSaveDataByName(mo._mission._name) != null)
                            {
                                mo._mission._name = mo._mission._name + "_" + index++;
                            }

                            while (!CreateMissionObjectFile(mo))
                                mo._mission._name = mo._mission._name + "_" + index++;

                            _window._storyObject.AssignNewMissionObject(mo);
                        });
                        menu.ShowAsContext();
                    }
                }
            }
        }

        public void OnDoubleClickMission()
        {
            if (IsDoubleClick() && Tools.IsValidMouseAABB(_currentNodeRect))
            {
                //Debug.Log(_totalFrame + "    " + _nextFrame);
                //if (_totalFrame < _nextFrame)
                _window._editMission = _currentNode as Mission;
                //_nextFrame = _totalFrame + 60;
                //Debug.Log(_totalFrame + "    " + _nextFrame);
            }
        }

        private bool CreateMissionObjectFile(MissionObject o)
        {
            string fullPath = Application.dataPath + "/../" + _window._storyObject.GetFullMissionPath(o._mission._name);
            if (System.IO.File.Exists(fullPath)) return false;

            string dirPath = _window._storyObject.GetFullMissionDirectoryPath();
            if (!AssetDatabase.IsValidFolder(dirPath))
            {
                System.IO.Directory.CreateDirectory(Application.dataPath + "/../" + dirPath);
            }

            o.Save();
            AssetDatabase.CreateAsset(o, _window._storyObject.GetFullMissionPath(o._mission._name));
            return true;
        }
    }
}
