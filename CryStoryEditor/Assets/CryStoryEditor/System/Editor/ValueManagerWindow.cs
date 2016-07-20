/**********************************************************
*Author: wangjiaying
*Date: 2016.7.20
*Func:
**********************************************************/
using System;
using CryStory.Runtime;
using UnityEditor;
using UnityEngine;

namespace CryStory.Editor
{
    /// <summary>
    /// 管理Story及Mission的各种变量
    /// </summary>
    public class ValueManagerWindow : EditorWindow
    {

        private static ValueManagerWindow _instance;
        public static ValueContainer missionContainer;

        [MenuItem("StoryEditor/Open Value Manager")]
        public static void Open()
        {
            if (_instance != null)
            {
                _instance.Focus();
                return;
            }
            _instance = EditorWindow.CreateInstance<ValueManagerWindow>();
            _instance.titleContent = new GUIContent("Value Manager");
            float h = 600;// Screen.height * 0.8f;
            float w = 630;//Screen.width * 0.95f;
            _instance.position = new Rect(Screen.width - w, Screen.height - h, w, h);
            _instance.maxSize = new Vector2(w, Screen.height);

            _instance.Show();
        }

        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            //Story Value Manager
            StoryValue();

            ShowSeperator();

            //Mission Value Manager
            MissionValue();
            EditorGUILayout.EndHorizontal();
        }

        private void ShowSeperator()
        {
            GUILayout.BeginVertical();
            int count = (int)(position.height / 17);
            for (int i = 0; i < count; i++)
            {
                GUILayout.Label("☆★");
            }
            GUILayout.EndVertical();
        }

        private void StoryValue()
        {
            StoryObject story = Selection.activeObject as StoryObject;
            if (story == null)
            {
                EditorGUILayout.LabelField("Not select story!");
                return;
            }
            if (story._Story != null)
            {
                EditorGUILayout.BeginVertical();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(story.name, ResourcesManager.GetInstance.GetFontStyle(15));
                //GUILayout.Space(10);
                if (GUILayout.Button("Add Value"))
                    ValueAdder.Open(story._Story);
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(10);

                foreach (var val in story._Story._valueContainer)
                {
                    ShowValue(val.Key, val.Value);
                }
                EditorGUILayout.EndVertical();
            }
        }

        private void MissionValue()
        {
            MissionObject missionObj = Selection.activeObject as MissionObject;
            ValueContainer container;

            if (missionObj != null)
            {
                if (missionObj._mission == null)
                    missionObj.Load();
                container = missionObj._mission;
            }
            else container = StoryEditor.GetInstance.CurrentValueContainer;

            if (container == null)
                container = missionContainer;

            if (container == null)
            {
                EditorGUILayout.LabelField("Not select Mission!");
                return;
            }

            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(container._name, ResourcesManager.GetInstance.GetFontStyle(15));

            if (GUILayout.Button("Add Value"))
                ValueAdder.Open(container);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            foreach (var val in container._valueContainer)
            {
                ShowValue(val.Key, val.Value);
            }
            EditorGUILayout.EndVertical();
        }

        private void ShowValue(string name, Value val)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(name);
            switch (val.VarlueType)
            {
                case VarType.INT:
                    val.IntValue = EditorGUILayout.IntField(val.IntValue);
                    break;
                case VarType.FLOAT:
                    val.FloatValue = EditorGUILayout.FloatField(val.FloatValue);
                    break;
                case VarType.BOOL:
                    val.BoolValue = EditorGUILayout.Toggle(val.BoolValue);
                    break;
                case VarType.STRING:
                    val.StringValue = EditorGUILayout.TextField(val.StringValue);
                    break;
            }

            EditorGUILayout.EndHorizontal();
        }

    }
}
