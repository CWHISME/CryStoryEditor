/**********************************************************
*Author: wangjiaying
*Date: 
*Func:
**********************************************************/
using UnityEditor;
using UnityEngine;

namespace CryStory.Runtime
{
    [CustomEditor(typeof(Story))]
    public class StoryInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (CryStory.Editor.StoryEditorWindow.StoryWindow)
                CryStory.Editor.StoryEditorWindow.StoryWindow.Focus();
            if (GUILayout.Button("Edit >>>", GUILayout.Height(40), GUILayout.Width(80)))
                CryStory.Editor.StoryEditorWindow.Open();
        }
    }
}