/**********************************************************
*Author: wangjiaying
*Date: 
*Func:
**********************************************************/
using UnityEngine;
using UnityEditor;

namespace CryStory.Editor
{
    public class StoryEditorWindow : EditorWindow
    {
        [MenuItem("StoryEditor/Open Story Editor")]
        static void Open()
        {
            StoryEditorWindow window = EditorWindow.CreateInstance<StoryEditorWindow>();
            window.titleContent = new GUIContent("Story Editor");
            float h = Screen.height * 0.7f;
            float w = Screen.width * 0.7f;
            window.position = new Rect(Screen.width - w, Screen.height - h, w, h);
            window.Show();
        }

        private Vector2 _leftScrollPosition;
        private float _leftWidth = 230f;

        public Rect windowRect;
        public Rect contentRect;

        void OnGUI()
        {
            LoadTextures();

            //Update cache window size
            windowRect = new Rect(0, 0, position.width, position.height);
            contentRect = new Rect(_leftWidth, 60, position.width, position.height);

            //========Top Button ==============
            ShowTitle();
            ShowTopMenuUI();
            //========Left Slider Area===========
            ShowLeftSliderArea();
            //========Right Area===============
            DrawRightGrid();

            ShowRightClickPopupMenu();
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

        private void ShowLeftSliderArea()
        {
            _leftScrollPosition = GUILayout.BeginScrollView(_leftScrollPosition, false, true, GUILayout.Width(_leftWidth));
            for (int i = 0; i < 12; i++)
            {
                GUILayout.Label("Test===================");
            }
            GUILayout.EndScrollView();
        }

        public void DrawRightGrid()
        {
            Rect coord = new Rect();
            Vector2 nodepos = Content2Node(Vector2.zero);
            coord.x = nodepos.x / 128.0f;
            coord.y = nodepos.y / 128.0f;
            coord.width = contentRect.width / 128.0f;
            coord.height = -contentRect.height / 128.0f;
            GUI.color = new Color(1f, 1f, 1f, 0.1f);
            GUI.DrawTextureWithTexCoords(contentRect, texGrid, coord);
            GUI.color = Color.white;
        }

        private int s;
        private void ShowTopMenuUI()
        {
            GUIStyle selectStyle = new GUIStyle();
            selectStyle.alignment = TextAnchor.MiddleCenter;
            selectStyle.normal.background = Texture2D.blackTexture;
            selectStyle.onNormal.background = Texture2D.whiteTexture;
            s = GUILayout.SelectionGrid(s, new GUIContent[5] { new GUIContent("1"), new GUIContent("1"), new GUIContent("1"), new GUIContent(""), new GUIContent("") }, 5, selectStyle, GUILayout.Height(20));
        }

        private void ShowRightClickPopupMenu()
        {
            if (Event.current != null && Event.current.type == EventType.mouseDown)
            {
                if (Event.current.button == 1)
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Create/Story"), false, () =>
                    {
                        string path = EditorUtility.SaveFilePanelInProject("Create Story", "Story", "asset", "Create new story");
                        if (!string.IsNullOrEmpty(path))
                            AssetDatabase.CreateAsset(new CryStory.Runtime.Story(), path);
                    });
                    //menu.Popup(Event.current.mousePosition, Content2Node(Event.current.mousePosition));
                    menu.ShowAsContext();
                }
            }
        }

        //Tool===========
        Vector2 Content2Node(Vector2 pos)
        {
            Vector2 center = contentRect.size * 0.5f;
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
