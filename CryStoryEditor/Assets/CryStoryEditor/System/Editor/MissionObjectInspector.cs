/**********************************************************
*Author: wangjiaying
*Date: 2016.7.29
*Func:
**********************************************************/
using UnityEditor;
using UnityEngine;
using CryStory.Runtime;

namespace CryStory.Editor
{
    [CustomEditor(typeof(MissionObject))]
    public class MissionObjectInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Mission Source", ResourcesManager.GetInstance.skin.GetStyle("Title"));

            GUILayout.Space(10);

            if (GUILayout.Button("Value >>>", GUILayout.Height(40), GUILayout.Width(80)))
                CryStory.Editor.ValueManagerWindow.Open();

            GUILayout.Space(10);

            MissionObject m = (MissionObject)target;
            if (m._nextMissionNodeList.Count > 0)
            {
                EditorGUILayout.LabelField("Next Missions:", ResourcesManager.GetInstance.GetFontStyle(12, new Color32(238, 130, 238, 255)));
                for (int i = 0; i < m._nextMissionNodeList.Count; i++)
                {
                    NextMissionData data = m._nextMissionNodeList[i];
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    EditorGUILayout.LabelField(data.Name, data.IsSingleNode ? "[<color=#00FF00>Single</color>]" : "[<color=#00BFFF>Double</color>]", ResourcesManager.GetInstance.GetFontStyle(11));
                    EditorGUILayout.EndHorizontal();
                }
            }

            GUILayout.Space(10);

            string path = Application.dataPath + "/../" + AssetDatabase.GetAssetPath(target);
            System.DateTime modifyDate = System.IO.File.GetLastAccessTime(path);
            EditorGUILayout.LabelField("Modify Date:", modifyDate.ToString("yyyy.MM.dd   H:m:s"));
        }
    }
}