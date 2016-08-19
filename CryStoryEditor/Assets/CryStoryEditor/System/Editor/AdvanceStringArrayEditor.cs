/**********************************************************
*Author: wangjiaying
*Date: 2016.8.19
*Func:
**********************************************************/
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CryStory.Editor
{
    public class AdvanceStringArrayEditor : EditorWindow
    {

        private static object _target;
        private static FieldInfo _field;
        private static bool _init;

        private List<string> _valueList = new List<string>();
        private string _valueText;

        public static void Open(object target, FieldInfo field)
        {
            AdvanceStringArrayEditor a = EditorWindow.GetWindow<AdvanceStringArrayEditor>();
            a.titleContent = new GUIContent("Advance Editor");
            //a.position = new Rect(100, 100, 600, 700);

            InitValue(target, field);

            a.Show();
        }

        void Update()
        {
            if (!_init)
            {
                ReloadValue();
                _init = true;
            }
        }

        private Vector2 _textScrollPos;
        private Vector2 _arrayScrollPos;
        void OnGUI()
        {
            _textScrollPos = EditorGUILayout.BeginScrollView(_textScrollPos, GUI.skin.box, GUILayout.Height(200));

            _valueText = EditorGUILayout.TextArea(_valueText, GUILayout.Height(200));

            EditorGUILayout.EndScrollView();

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            if (GUILayout.Button("Text Convert To Array <color=#00FF00>↓↓</color>", ResourcesManager.GetInstance.skin.button))
            {
                _valueList.Clear();
                _valueList.AddRange(TextToArray(_valueText, "\n"));
            }

            if (GUILayout.Button("Array Convert To Text <color=#EE82EE>↑↑</color>", ResourcesManager.GetInstance.skin.button))
            {
                _valueText = ArrayToText(_valueList.ToArray());
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            _arrayScrollPos = EditorGUILayout.BeginScrollView(_arrayScrollPos, GUI.skin.box);
            for (int i = 0; i < _valueList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                //GUILayout.LabelField((i + 1) + ". ");
                _valueList[i] = EditorGUILayout.TextField(_valueList[i], GUILayout.Height(20));
                if (GUILayout.Button("<color=red>X</color>", ResourcesManager.GetInstance.skin.button, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    _valueList.RemoveAt(i);
                    break;
                }

                if (GUILayout.Button("<color=#00FF00>↑</color>", ResourcesManager.GetInstance.skin.button, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    int index = i - 1;
                    if (index >= 0)
                    {
                        string temp = _valueList[index];
                        _valueList[index] = _valueList[i];
                        _valueList[i] = temp;
                    }
                    break;
                }

                if (GUILayout.Button("<color=#00FF00>↓</color>", ResourcesManager.GetInstance.skin.button, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    int index = i + 1;
                    if (index < _valueList.Count)
                    {
                        string temp = _valueList[index];
                        _valueList[index] = _valueList[i];
                        _valueList[i] = temp;
                    }
                    break;
                }


                EditorGUILayout.EndHorizontal();
                GUILayout.Space(3);
            }
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("<color=#00FF00>Save</color>", ResourcesManager.GetInstance.skin.button, GUILayout.Width(100), GUILayout.Height(50)))
            {
                if (!HaveTarget())
                {
                    EditorUtility.DisplayDialog("Error!", "Save Failed! The Value Target Was Lost!", "OK");
                    return;
                }

                _field.SetValue(_target, _valueList.ToArray());
            }
        }

        private void ReloadValue()
        {
            if (!HaveTarget()) return;

            _valueList.Clear();
            _valueList.AddRange(_field.GetValue(_target) as string[]);

            _valueText = ArrayToText(_valueList.ToArray());
        }

        private string ArrayToText(string[] array)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                builder.AppendLine(array[i]);
            }
            return builder.ToString();
        }

        private string[] TextToArray(string text, params string[] spltChar)
        {
            return text.Split(spltChar, System.StringSplitOptions.None);
        }

        private bool HaveTarget()
        {
            return _target != null && _field != null;
        }

        private static void InitValue(object target, FieldInfo field)
        {
            _target = target;
            _field = field;
            _init = false;
        }
    }
}
