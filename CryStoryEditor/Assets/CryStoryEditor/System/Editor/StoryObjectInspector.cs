/**********************************************************
*Author: wangjiaying
*Date: 
*Func:
**********************************************************/
using UnityEditor;
using UnityEngine;
using CryStory.Runtime;

namespace CryStory.Editor
{
    [CustomEditor(typeof(StoryObject))]
    public class StoryObjectInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            //if (CryStory.Editor.StoryEditorWindow.StoryWindow)
            //    CryStory.Editor.StoryEditorWindow.StoryWindow.Focus();

            EditorGUILayout.LabelField("Story Source", ResourcesManager.GetInstance.skin.GetStyle("Title"));

            GUILayout.Space(10);

            EditorGUILayout.HelpBox(((StoryObject)target)._description, MessageType.Info);

            GUILayout.Space(10);
            if (GUILayout.Button("Edit >>>", GUILayout.Height(40), GUILayout.Width(80)))
                CryStory.Editor.StoryEditorWindow.Open();

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Archive Version: ", ((StoryObject)target)._saveVersion.ToString());

            string path = Application.dataPath + "/../" + AssetDatabase.GetAssetPath(target);
            //System.DateTime createDate = System.IO.File.GetCreationTime(path);
            System.DateTime modifyDate = System.IO.File.GetLastAccessTime(path);
            //EditorGUILayout.LabelField("Create Date:", createDate.ToString("yyyy.MM.dd   H:m:s"));
            EditorGUILayout.LabelField("Modify Date:", modifyDate.ToString("yyyy.MM.dd   H:m:s"));
        }
    }
}