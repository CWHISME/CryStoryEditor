/**********************************************************
*Author: wangjiaying
*Date: 
*Func:
**********************************************************/
using UnityEngine;
using UnityEditor;
using CryStory.Runtime;

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

        public Story _story;

        public Rect _windowRect;
        public Rect _contentRect;

        public float _leftWidth = 230f;

        void OnGUI()
        {
            LoadTextures();

            //Update cache window size
            _windowRect = new Rect(0, 0, position.width, position.height);
            _contentRect = new Rect(_leftWidth, 60, position.width, position.height);

            //========Top Button ==============
            ShowTitle();
            ShowTopMenuUI();

            //========Story Editor ==============
            if (StoryEditor.GetInstance.OnGUI(this)) return;

            MissionEditor.GetInstance.OnGUI(this);
        }

        private void ShowTitle()
        {
            GUILayout.Space(5);
            GUIStyle style = new GUIStyle();
            style.fontSize = 25;
            style.normal.textColor = Color.white;
            style.fontStyle = FontStyle.Bold;
            GUILayout.Label(new GUIContent(" Story Editor"), style);
            GUILayout.Space(5);
        }

        private int s;
        private void ShowTopMenuUI()
        {
            GUIStyle selectStyle = new GUIStyle();
            selectStyle.alignment = TextAnchor.MiddleCenter;
            selectStyle.normal.background = texGrid;//Texture2D.blackTexture;
            selectStyle.onNormal.background = texTargetArea;//Texture2D.whiteTexture;
            s = GUILayout.SelectionGrid(s, new GUIContent[5] { new GUIContent("1"), new GUIContent("1"), new GUIContent("1"), new GUIContent(""), new GUIContent("") }, 5, selectStyle, GUILayout.Height(20));
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
        }
    }
}
