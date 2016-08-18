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
            RightClickMenuCheck();
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

        protected override void DrawLeftArribute(object o)
        {
            EditorGUI.LabelField(GetGUILeftScrollAreaRect(0, 150, 18), "Mission Description:");
            LeftHeightSpace(10);
            System.Reflection.FieldInfo info = o.GetType().GetField("_missionDescription");
            object o_o = info.GetValue(o);
            if (o_o == null)
            {
                o_o = ReflectionHelper.CreateInstance(_window._Story._missionDescriptionType);
                info.SetValue(o, o_o);
            }

            if (_window._storyObject._debugMode)
                base.DrawLeftArribute(o);

            base.DrawLeftArribute(o_o);
        }

        /// <summary>
        /// 右键点击时显示的东西
        /// </summary>
        private void RightClickMenuCheck()
        {
            if (Event.current != null)
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    if (Event.current.button == 1)
                    {
                        if (_currentHover == null)
                        {
                            ShowCreateMissionMenu();
                            return;
                        }

                        //Check id click node
                        Rect rect = Tools.GetNodeRect(CalcRealPosition(_currentHover._position));
                        if (Tools.IsValidMouseAABB(rect))
                        {
                            ShowDeleteNodeMenu();
                        }
                        else ShowCreateMissionMenu();
                    }
                }
            }
        }

        /// <summary>
        /// 双击进入某个Mission中
        /// </summary>
        public void OnDoubleClickMission()
        {
            if (Tools.MouseDoubleClick && Tools.IsValidMouseAABB(_currentNodeRect))
            {
                if (Event.current.button == 0)
                {
                    _window.EditMission = _currentNode as Mission;
                    _currentNode = null;
                }
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

        private void ShowCreateMissionMenu()
        {
            Vector2 mousePos = Event.current.mousePosition;
            if (mousePos.x < _contentRect.x) return;
            if (mousePos.y < _contentRect.y) return;
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Create/New Mission"), false, () =>
            {
                MissionObject mo = ScriptableObject.CreateInstance<MissionObject>();
                mo._mission = new Mission();

                _currentNode = mo._mission;
                _currentNode._position = CalcVirtualPosition(mousePos);
                _currentNode._name = "New Mission";
                _currentNode.SetID(_window._Story.GenerateID());
                Mission.SetContent(_currentNode, _window._Story);

                int index = 1;
                while (_window._storyObject.GetMissionSaveDataByName(mo._mission._name) != null)
                {
                    mo._mission._name = mo._mission._name + "_" + index++;
                }

                while (!CreateMissionObjectFile(mo))
                    mo._mission._name = mo._mission._name + "_" + index++;

                _window._storyObject.AssignNewMissionObject(mo);

                //避免新的实例与Object实例不同
                _currentNode.RemoveFromContent();
                _currentNode = mo._mission;
                Mission.SetContent(_currentNode, _window._Story);
            });
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
                NodeModifier.Delete(_currentHover);
                MissionData data = _window._storyObject.GetMissionSaveDataByName(_currentHover._name);
                data._missionObject._mission = null;
                if (_currentNode == _currentHover) _currentNode = null;
                _currentHover = null;
            });

            menu.ShowAsContext();
        }
    }
}
