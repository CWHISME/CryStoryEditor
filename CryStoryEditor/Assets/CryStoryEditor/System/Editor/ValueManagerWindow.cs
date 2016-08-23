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
            float w = 650;//Screen.width * 0.95f;
            _instance.position = new Rect(Screen.width - w, Screen.height - h, w, h);
            _instance.maxSize = new Vector2(w, Screen.height);

            _instance.Show();
        }

        void OnGUI()
        {
            GUILayout.Space(5);
            EditorGUILayout.LabelField("Value Manager", ResourcesManager.GetInstance.GetFontStyle(22));
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal(GUILayout.Width(640));
            //Story Value Manager

            StoryValue();

            ShowSeperator();

            //Mission Value Manager
            MissionValue();
            EditorGUILayout.EndHorizontal();
        }

        void Update()
        {
            Repaint();
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
                EditorGUILayout.LabelField("Not select story!", GUILayout.Width(300));
                return;
            }
            if (story._Story != null)
            {
                EditorGUILayout.BeginVertical(GUILayout.Width(300));

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("<color=#FF00FF>" + story.name + "</color>", ResourcesManager.GetInstance.GetFontStyle(18));

                if (GUILayout.Button("<color=#00FF00>Add Value</color>", ResourcesManager.GetInstance.skin.button, GUILayout.Height(25)))
                    ValueAdder.Open(story._Story);
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(10);

                ShowContainer(story._Story);

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
                EditorGUILayout.LabelField("Not select Mission!", GUILayout.Width(300));
                return;
            }

            EditorGUILayout.BeginVertical(GUILayout.Width(300));

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<color=#FF00FF>" + container._name + "</color>", ResourcesManager.GetInstance.GetFontStyle(18));

            if (GUILayout.Button("<color=#00FF00>Add Value</color>", ResourcesManager.GetInstance.skin.button, GUILayout.Height(25)))
                ValueAdder.Open(container);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            ShowContainer(container);

            EditorGUILayout.EndVertical();
        }

        private void ShowContainer(ValueContainer container)
        {
            foreach (var val in container._valueContainer)
            {
                if (ShowValue(val.Key, val.Value))
                {
                    container._valueContainer.Remove(val.Key);
                    return;
                }
            }
        }

        private bool ShowValue(string name, Value val)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("<color=#00FFFF>" + name + "</color>[<color=yellow>" + val.ValueType + "</color>]", ResourcesManager.GetInstance.GetFontStyle(12));
            switch (val.ValueType)
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

            if (GUILayout.Button("<color=red>X</color>", ResourcesManager.GetInstance.skin.button, GUILayout.Height(18), GUILayout.Width(18)))
            {
                if (EditorUtility.DisplayDialog("Caution!", "You will delete value [" + name + "] !", "Confirm", "Cancel"))
                    return true;
            }

            EditorGUILayout.EndHorizontal();
            return false;
        }

    }
}
