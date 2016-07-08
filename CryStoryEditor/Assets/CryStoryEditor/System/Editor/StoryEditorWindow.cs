/**********************************************************
*Author: wangjiaying
*Date: 
*Func:
**********************************************************/
using UnityEngine;
using UnityEditor;
using CryStory.Runtime;
using System;

namespace CryStory.Editor
{
    public class StoryEditorWindow : EditorWindow
    {

        public static StoryEditorWindow StoryWindow;
        [MenuItem("StoryEditor/Open Story Editor")]
        public static void Open()
        {
            StoryWindow = EditorWindow.CreateInstance<StoryEditorWindow>();
            StoryWindow.titleContent = new GUIContent("Story Editor");
            float h = Screen.height * 0.7f;
            float w = Screen.width * 0.7f;
            StoryWindow.position = new Rect(Screen.width - w, Screen.height - h, w, h);
            StoryWindow.Show();
        }

        public StoryObject _storyObject;
        public Story _Story { get { return _storyObject._Story; } }
        public Mission _editMission;

        public Rect _windowRect;
        public Rect _contentRect;

        public float _leftWidth = 230f;
        public float _topHeight = 60f;
        public float _titleHeight = 35f;
        public float _selectionGridHeight { get { return _titleHeight + 5; } }

        private int pageSelect = 0;

        void OnEnable()
        {
            StoryWindow = this;
            Repaint();
        }

        void Update()
        {
            Repaint();
            //Debug.Log("dd");
        }

        void OnGUI()
        {
            //Update cache window size
            _windowRect = new Rect(0, 0, position.width, position.height);
            _contentRect = new Rect(_leftWidth, _topHeight, position.width, position.height);

            //========Version          ==============
            ShowVersion();
            //========Story Editor ==============
            if (MainPageEditor.GetInstance.OnGUI(this)) return;

            //Show Pages
            switch (pageSelect)
            {
                case 1:
                case 2:
                    NotingPage();
                    break;
                case 3:
                    HelpPage();
                    break;
                case 4:
                    AboutPage();
                    break;
                case 0:
                default:
                    MainPage();
                    break;
            }
            //LoadTextures();


            //========Top Button ==============
            ShowTitle();
            ShowTopMenuUI();
        }

        private void NotingPage()
        {
            EditorGUI.LabelField(new Rect(_windowRect.center.x - 38, _windowRect.center.y - 50, 500, 20), "There is noting.");
        }

        private Vector2 helpScrolPos;
        private void HelpPage()
        {
            GUILayout.Space(_topHeight + 20);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(_windowRect.width * 0.25f);
            helpScrolPos = EditorGUILayout.BeginScrollView(helpScrolPos);
            GUIStyle style = new GUIStyle();
            style.fontSize = 12;
            style.normal.textColor = Color.white;
            GUILayout.Label(ResourcesManager.GetInstance.HelpCN, style);
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();
        }

        private void AboutPage()
        {
            EditorGUI.LabelField(new Rect(_windowRect.center.x - 100, _windowRect.center.y - 100, 300, 20), "Cry Story Editor", ResourcesManager.GetInstance.skin.GetStyle("Title"));
            EditorGUI.LabelField(new Rect(_windowRect.center.x - 33, _windowRect.center.y - 50, 500, 20), "By CWHISME");
            EditorGUI.LabelField(new Rect(_windowRect.center.x - 20, _windowRect.center.y, 500, 20), Version.FullVersion);
        }

        private void MainPage()
        {
            EditorGUI.DrawTextureTransparent(_contentRect, ResourcesManager.GetInstance.texBackground);

            if (_editMission == null)
                StoryEditor.GetInstance.OnGUI(this, _storyObject._Story.Nodes);
            else MissionEditor.GetInstance.OnGUI(this, _editMission.Nodes);

            if (_storyObject._haveNullData)
            {
                GUIStyle style = new GUIStyle();
                style.fontSize = 16;
                EditorGUI.LabelField(new Rect(_windowRect.center.x - 33, _windowRect.center.y + 200, 500, 20), "<color=red>You have some Mission File Lost!</color>", style);
                if (GUI.Button(new Rect(_windowRect.center.x - 5, _windowRect.center.y + 250, 200, 40), "Remove It From Story", ResourcesManager.GetInstance.skin.button))
                {
                    _storyObject.RemoveAllMissingMissionData();
                }
            }
        }

        private void ShowVersion()
        {
            GUI.Label(new Rect(_windowRect.xMax - 120, _windowRect.yMax - 40, 120, 20), new GUIContent("Made By CWHISME"));
            GUI.Label(new Rect(_windowRect.xMax - 90, _windowRect.yMax - 20, 100, 20), new GUIContent(Version.FullVersion));
        }

        private void ShowTitle()
        {
            //GUILayout.Space(5);
            //GUIStyle style = new GUIStyle();
            //style.fontSize = 25;
            //style.normal.textColor = Color.white;
            //style.fontStyle = FontStyle.Bold;
            EditorGUI.DrawTextureTransparent(new Rect(0, 0, position.width, _topHeight), ResourcesManager.GetInstance.texBackground);

            EditorGUILayout.BeginHorizontal();
            GUIStyle style = ResourcesManager.GetInstance.skin.GetStyle("Title");
            float btnW = 0;

            if (_editMission != null)
            {
                btnW = 35;
                if (GUI.Button(new Rect(2, 3, btnW + 2, _titleHeight - 3), "<-", ResourcesManager.GetInstance.skin.button))
                    _editMission = null;
            }

            EditorGUI.LabelField(new Rect(btnW, 0, _windowRect.xMax, _titleHeight), new GUIContent(" Story Editor" + (_storyObject ? "->" + (_editMission == null ? _storyObject.name : _storyObject.name + " -> " + _editMission._name) : "")), style);

            GUIStyle saveStyle = new GUIStyle(ResourcesManager.GetInstance.skin.button);
            saveStyle.normal.textColor = new Color32(255, 64, 180, 255);
            if (GUI.Button(new Rect(_contentRect.width - 180, 3, 80, _titleHeight - 3), "Reload", saveStyle))
                _storyObject.Load();
            saveStyle.normal.textColor = new Color32(0, 255, 0, 255);
            if (GUI.Button(new Rect(_contentRect.width - 90, 3, 80, _titleHeight - 3), "Save Story", saveStyle))
                _storyObject.Save();

            EditorGUILayout.EndHorizontal();
            //GUILayout.Space(5);
        }

        private void ShowTopMenuUI()
        {
            //GUIStyle selectStyle = new GUIStyle();
            //selectStyle.alignment = TextAnchor.MiddleCenter;
            //selectStyle.normal.background = texGrid;//Texture2D.blackTexture;
            //selectStyle.onNormal.background = texTargetArea;//Texture2D.whiteTexture;
            Rect rect = new Rect(0, _titleHeight, _windowRect.xMax, _topHeight - _titleHeight);
            GUI.Box(rect, "", ResourcesManager.GetInstance.StyleBackground);
            pageSelect = GUI.SelectionGrid(new Rect(0, _selectionGridHeight, _windowRect.xMax, _topHeight - _titleHeight - 5), pageSelect, new GUIContent[5] { new GUIContent("Main Page"), new GUIContent("Nothing"), new GUIContent("Nothing"), new GUIContent("Help"), new GUIContent("About") }, 5, ResourcesManager.GetInstance.skin.button);

            // s = GUILayout.SelectionGrid(s, new GUIContent[5] { new GUIContent("1"), new GUIContent("1"), new GUIContent("1"), new GUIContent(""), new GUIContent("") }, 5, skin.button, GUILayout.Height(20),);
        }

        //Tool===========
        Vector2 Content2Node(Vector2 pos)
        {
            Vector2 center = _contentRect.size * 0.5f;
            center.x = (int)(center.x);
            center.y = (int)(center.y);

            return pos;// + modifier.graphCenter - center;
        }

    }
}
