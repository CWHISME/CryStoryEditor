/**********************************************************
*Author: wangjiaying
*Date: 2016.6.17
*Func:
**********************************************************/
using UnityEditor;
using CryStory.Runtime;
using Event = UnityEngine.Event;
using UnityEngine;

namespace CryStory.Editor
{
    public class StoryEditor : Singleton<StoryEditor>
    {

        private StoryEditorWindow _window;
        public bool OnGUI(StoryEditorWindow window)
        {
            //if (!Selection.activeObject) { ShowTips(window._windowRect.center); return true; }

            StoryObject story = Selection.activeObject as StoryObject;
            window._storyObject = story;
            if (story == null)
            {
                _window = window;
                ShowTips(window._windowRect.center);
                return true;
            }

            return false;
        }

        private void ShowTips(Vector2 center)
        {
            ShowRightClickPopupMenu();
            //GUIStyle title = new GUIStyle();
            EditorGUI.LabelField(new Rect(center.x - 100, center.y - 100, 500, 20), "Cry Story Editor",_window.skin.GetStyle("Title"));
            EditorGUI.LabelField(new Rect(center.x - 38, center.y - 50, 500, 20), "—By CWHISME");
            EditorGUI.LabelField(new Rect(center.x - 150, center.y - 20, 500, 20), "You are not select any story file,but you can create a new story.");
            if (GUI.Button(new Rect(center.x - 20, center.y + 30, 60, 40), "Create", _window.skin.button))
            {
                CreateNewStory();
            }

            EditorGUI.LabelField(new Rect(center.x - 20, center.y + 100, 500, 20), Version.FullVersion);
        }

        private void ShowRightClickPopupMenu()
        {
            if (Event.current != null && Event.current.type == EventType.mouseDown)
            {
                if (Event.current.button == 1)
                {
                    if (StoryEditorWindow.StoryWindow != null)
                        if (Event.current.mousePosition.y < StoryEditorWindow.StoryWindow._contentRect.y) return;
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Create/New Story"), false, () =>
                    {
                        CreateNewStory();
                    }
               );
                    menu.ShowAsContext();
                }
            }
        }

        private void CreateNewStory()
        {
            string path = EditorUtility.SaveFilePanelInProject("Create Story", "New Story", "asset", "Create new story");
            if (!string.IsNullOrEmpty(path))
            {
                StoryObject o = ScriptableObject.CreateInstance<StoryObject>();//  new CryStory.Runtime.StoryObject();
                AssetDatabase.CreateAsset(o, path);
                Selection.activeObject = o;
            }
        }
    }
}