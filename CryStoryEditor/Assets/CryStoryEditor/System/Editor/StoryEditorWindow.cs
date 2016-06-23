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

        public Rect _windowRect;
        public Rect _contentRect;

        public float _leftWidth = 230f;
        public float _topHeight = 60f;
        public float _titleHeight = 35f;
        public float _selectionGridHeight { get { return _titleHeight + 5; } }

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
            LoadTextures();

            //Update cache window size
            _windowRect = new Rect(0, 0, position.width, position.height);
            _contentRect = new Rect(_leftWidth, _topHeight, position.width, position.height);

            //========Version          ==============
            ShowVersion();
            //========Story Editor ==============
            if (StoryEditor.GetInstance.OnGUI(this)) return;

            MissionEditor.GetInstance.OnGUI(this);

            //========Top Button ==============
            ShowTitle();
            ShowTopMenuUI();
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

            EditorGUI.LabelField(new Rect(0, 0, _windowRect.xMax, _titleHeight), new GUIContent(" Story Editor" + (_storyObject ? "->" + _storyObject.name : "")), skin.GetStyle("Title"));
            //GUILayout.Space(5);
        }

        private int s;
        private void ShowTopMenuUI()
        {
            //GUIStyle selectStyle = new GUIStyle();
            //selectStyle.alignment = TextAnchor.MiddleCenter;
            //selectStyle.normal.background = texGrid;//Texture2D.blackTexture;
            //selectStyle.onNormal.background = texTargetArea;//Texture2D.whiteTexture;
            Rect rect = new Rect(0, _titleHeight, _windowRect.xMax, _topHeight - _titleHeight);
            GUI.Box(rect, "", StyleBackground);
            s = GUI.SelectionGrid(new Rect(0, _selectionGridHeight, _windowRect.xMax, _topHeight - _titleHeight - 5), s, new GUIContent[5] { new GUIContent("1"), new GUIContent("1"), new GUIContent("1"), new GUIContent(""), new GUIContent("") }, 5, skin.button);

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

        //Resources
        public Texture2D texLogo;
        public Texture2D texInputSlot;
        public Texture2D texInputSlotActive;
        public Texture2D texOutputSlot;
        public Texture2D texOutputSlotActive;
        public Texture2D texTargetArea;
        public Texture2D texSave;
        public Texture2D texGrid;

        public GUISkin skin;

        public GUIStyle StyleBackground { get { return skin.GetStyle("Background"); } }

        public void LoadTextures()
        {
            if (texLogo == null)
                texLogo = Resources.Load("cf_logo") as Texture2D;
            if (texInputSlot == null)
                texInputSlot = Resources.Load("cf_input_slot") as Texture2D;
            if (texInputSlotActive == null)
                texInputSlotActive = Resources.Load("cf_input_slot_active") as Texture2D;
            if (texOutputSlot == null)
                texOutputSlot = Resources.Load("cf_output_slot") as Texture2D;
            if (texOutputSlotActive == null)
                texOutputSlotActive = Resources.Load("cf_output_slot_active") as Texture2D;
            if (texTargetArea == null)
                texTargetArea = Resources.Load("cf_target_area") as Texture2D;
            if (texSave == null)
                texSave = Resources.Load("cf_save") as Texture2D;
            if (texGrid == null)
                texGrid = Resources.Load("cf_grid") as Texture2D;
            if (skin == null) skin = Resources.Load<GUISkin>("Skin");
        }
    }
}
