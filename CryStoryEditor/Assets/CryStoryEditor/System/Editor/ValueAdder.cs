/**********************************************************
*Author: wangjiaying
*Date: 2016.7.20
*Func:
**********************************************************/
using CryStory.Runtime;
using UnityEditor;
using UnityEngine;

namespace CryStory.Editor
{
    public class ValueAdder : EditorWindow
    {

        private static ValueContainer _container;
        private string _name = "New Varlue";
        private string _var;
        private VarType _varType = VarType.STRING;

        public static void Open(ValueContainer container)
        {
            ValueAdder win = EditorWindow.CreateInstance<ValueAdder>();
            win.titleContent = new GUIContent("Value Adder");
            float h = Screen.height * 0.3f;
            float w = Screen.width * 0.4f;
            win.position = new Rect(Screen.width - w, Screen.height - h, w, h);

            _container = container;
            win.ShowAuxWindow();
        }

        void OnGUI()
        {
            GUILayout.Space(10);
            _name = EditorGUILayout.TextField("Name:", _name);
            GUILayout.Space(10);
            _varType = (VarType)EditorGUILayout.EnumPopup(_varType);
            GUILayout.Space(10);

            switch (_varType)
            {
                case VarType.INT:
                case VarType.FLOAT:
                case VarType.STRING:
                    _var = EditorGUILayout.TextField("Value:", _var);
                    break;
                case VarType.BOOL:
                    if (string.IsNullOrEmpty(_var)) _var = bool.TrueString;
                    try
                    {
                        _var = EditorGUILayout.Toggle(bool.Parse(_var)).ToString();
                    }
                    catch (System.Exception)
                    {
                        _var = bool.TrueString;
                    }
                    break;
            }

            GUILayout.Space(10);
            if (GUILayout.Button("Add"))
            {
                if (_container.HaveKey(_name))
                {
                    EditorUtility.DisplayDialog("Error!", "The name already exist! ", "OK");
                    return;
                }

                Value value = null;
                switch (_varType)
                {
                    case VarType.INT:
                        int result;
                        if (int.TryParse(_var, out result))
                            value = new Value(result);
                        break;
                    case VarType.FLOAT:
                        float resultF;
                        if (float.TryParse(_var, out resultF))
                            value = new Value(resultF);
                        break;
                    case VarType.BOOL:
                        value = new Value(bool.Parse(_var));
                        break;
                    case VarType.STRING:
                        value = new Value(_var);
                        break;
                }

                if (value == null)
                {
                    EditorUtility.DisplayDialog("Error!", "Value Type Was Wrong! ", "OK");
                    return;
                }

                _container.AddValue(_name, value);
                Close();
            }
        }
    }
}
