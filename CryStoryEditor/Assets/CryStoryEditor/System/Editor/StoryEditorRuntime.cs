/**********************************************************
*Author: wangjiaying
*Date: 2016.7.13
*Func:
**********************************************************/
using CryStory.Runtime;
using UnityEditor;
using UnityEngine;
using Event = UnityEngine.Event;

namespace CryStory.Editor
{
    public class StoryEditorRuntime : NodeContentEditorRuntime<StoryEditorRuntime>
    {

        protected override void InternalOnGUI()
        {
            base.InternalOnGUI();
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

        //protected override void DrawLeftArribute(object o)
        //{
        //    EditorGUI.LabelField(GetGUILeftScrollAreaRect(0, 150, 18), "Mission Description:");
        //    LeftHeightSpace(10);
        //    System.Reflection.FieldInfo info = o.GetType().GetField("_missionDescription");
        //    object o_o = info.GetValue(o);
        //    base.DrawLeftArribute(o_o);
        //}

        /// <summary>
        /// 双击进入某个Mission中
        /// </summary>
        public void OnDoubleClickMission()
        {
            if (Tools.MouseDoubleClick && Tools.IsValidMouseAABB(_currentNodeRect))
            {
                if (Event.current.button == 0)
                    _window.EditMission = _currentNode as Mission;
            }

        }
    }
}
