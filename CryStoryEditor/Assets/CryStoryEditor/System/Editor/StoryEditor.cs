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
        public bool OnGUI(StoryEditorWindow window)
        {
            if (!Selection.activeObject) { ShowTips(window._windowRect.center); return true; }

            Story story = Selection.activeObject as Story;
            if (story == null)
            {
                ShowTips(window._windowRect.center);
                return true;
            }
            story.ID++;
            window._story = story;

            return false;
        }

        private void ShowTips(Vector2 center)
        {
            ShowRightClickPopupMenu();
            EditorGUI.LabelField(new Rect(center.x - 150, center.y - 20, 500, 20), "You are not select any story file,but you can create a new story.");
            if (GUI.Button(new Rect(center.x - 20, center.y + 30, 60, 40), "Create"))
            {
                CreateNewStory();
            }
        }

        private void ShowRightClickPopupMenu()
        {
            if (Event.current != null && Event.current.type == EventType.mouseDown)
            {
                if (Event.current.button == 1)
                {
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
                AssetDatabase.CreateAsset(new CryStory.Runtime.Story(), path);
        }
    }
}