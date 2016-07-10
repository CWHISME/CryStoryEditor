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
                    RepairPages();
                    break;
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

        private Vector2 _repairPageScroll;
        private void RepairPages()
        {
            GUILayout.Space(_topHeight + 20);

            //EditorGUILayout.BeginHorizontal();
            _repairPageScroll = EditorGUILayout.BeginScrollView(_repairPageScroll);
            MissionData[] datas = _storyObject.MisisonDatas;

            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = 16;
            EditorGUILayout.LabelField("This list is exist data but the <color=#00E3E3>File Connection</color> lost:", style);
            style.fontSize = 14;
            style.clipping = TextClipping.Overflow;
            GUILayout.Space(20);
            for (int i = 0; i < datas.Length; i++)
            {
                MissionData data = datas[i];

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Mission:  <color=#F9F900>" + data._name + "</color>", style);
                if (data._missionObject == null || data._missionObject.name != data._name)
                {
                    EditorGUILayout.LabelField("<color=red>✘</color>", style);
                }
                else EditorGUILayout.LabelField("<color=#28FF28>✔</color>", style);

                if (GUILayout.Button("<color=red>Delete</color>", ResourcesManager.GetInstance.skin.button, GUILayout.MaxWidth(50), GUILayout.MaxHeight(30)))
                {
                    _storyObject.DeleteMissionData(data);
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("<color=#28FF28>Try Reconnect File</color>", ResourcesManager.GetInstance.skin.button, GUILayout.MaxWidth(200), GUILayout.MaxHeight(50)))
            {
                _storyObject.RepairMissionDataByName();
            }

            GUILayout.Space(50);

            style.fontSize = 16;
            EditorGUILayout.LabelField("This list is exist Mission File but the <color=#00E3E3>Mission Data</color> lost:", style);
            style.fontSize = 14;
            string path = Application.dataPath + "/../" + _storyObject.GetFullMissionDirectoryPath();
            path = path.Replace("Assets/../", "");

            if (System.IO.Directory.Exists(path))
            {
                string[] files = System.IO.Directory.GetFiles(path, "*.asset");
                bool haveLost = false;
                for (int i = 0; i < files.Length; i++)
                {
                    string name = System.IO.Path.GetFileNameWithoutExtension(files[i]);
                    if (_storyObject.GetMissionSaveDataByName(name) != null) continue;
                    haveLost = true;
                    EditorGUILayout.LabelField("Mission:  " + name, "<color=red>Mission Data Not Found!</color>", style);
                    if (GUILayout.Button("<color=#28FF28>Create Mission Data For It</color>", ResourcesManager.GetInstance.skin.button, GUILayout.MaxWidth(180), GUILayout.MaxHeight(30)))
                    {
                        string mp = files[i];//.Replace("Assets/../", "");
                        MissionObject o = AssetDatabase.LoadAssetAtPath<MissionObject>(FileUtil.GetProjectRelativePath(mp));
                        if (!o)
                        {
                            EditorUtility.DisplayDialog("Error!", "Recreate Mission Data Faild!Because the file was damaged!", "OK");
                            continue;
                        }
                        _storyObject.AssignNewMissionObject(o);
                    }

                    if (GUILayout.Button("<color=red>Delete It</color>", ResourcesManager.GetInstance.skin.button, GUILayout.MaxWidth(180), GUILayout.MaxHeight(30)))
                    {
                        string mp = files[i];//.Replace("Assets/../", "");
                        if (!AssetDatabase.DeleteAsset(FileUtil.GetProjectRelativePath(mp)))
                        {
                            EditorUtility.DisplayDialog("Error!", "Delete Faild!", "OK");
                        }
                    }
                }

                GUILayout.Space(10);

                if (haveLost)
                    if (GUILayout.Button("<color=#28FF28>Recreate All The Mission Data</color>", ResourcesManager.GetInstance.skin.button, GUILayout.MaxWidth(200), GUILayout.MaxHeight(50)))
                    {
                        for (int i = 0; i < files.Length; i++)
                        {
                            string name = System.IO.Path.GetFileNameWithoutExtension(files[i]);
                            if (_storyObject.GetMissionSaveDataByName(name) != null) continue;
                            string mp = files[i];//.Replace("Assets/../", "");
                            MissionObject o = AssetDatabase.LoadAssetAtPath<MissionObject>(FileUtil.GetProjectRelativePath(mp));
                            if (!o)
                            {
                                EditorUtility.DisplayDialog("Error!", "Recreate Mission [" + name + "] Data Faild!Because the file was damaged!", "OK");
                                continue;
                            }
                            _storyObject.AssignNewMissionObject(o);
                        }
                    }

            }

            EditorGUILayout.EndScrollView();
            //EditorGUILayout.EndHorizontal();
            //_leftScrollPosition = GUI.BeginScrollView(new Rect(0, _window._topHeight, _window._leftWidth, _window._windowRect.height - _window._topHeight), _leftScrollPosition, new Rect(0, _window._topHeight, _window._leftWidth - 30, _window._windowRect.height - _window._topHeight), false, true, ResourcesManager.GetInstance.skin.horizontalScrollbar, ResourcesManager.GetInstance.skin.verticalScrollbar);
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
            EditorGUI.LabelField(new Rect(_windowRect.center.x - 55, _windowRect.center.y - 20, 500, 20), "Email: cwhisme@qq.com");
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
                EditorGUI.LabelField(new Rect(_windowRect.center.x - 33, _windowRect.center.y + 150, 500, 20), "<color=red>You have some Mission File Lost!</color>", style);

                if (GUI.Button(new Rect(_windowRect.center.x - 50, _windowRect.center.y + 200, 150, 40), "<color=#28FF28>Try Repair</color>", ResourcesManager.GetInstance.skin.button))
                {
                    pageSelect = 1;
                }
                if (GUI.Button(new Rect(_windowRect.center.x + 120, _windowRect.center.y + 200, 150, 40), "<color=red>Remove It From Story</color>", ResourcesManager.GetInstance.skin.button))
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
            EditorGUI.DrawTextureTransparent(new Rect(0, 0, position.width, _topHeight), ResourcesManager.GetInstance.texBackground);

            //EditorGUILayout.BeginHorizontal();
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

            //EditorGUILayout.EndHorizontal();
        }

        private void ShowTopMenuUI()
        {
            Rect rect = new Rect(0, _titleHeight, _windowRect.xMax, _topHeight - _titleHeight);
            GUI.Box(rect, "", ResourcesManager.GetInstance.StyleBackground);
            pageSelect = GUI.SelectionGrid(new Rect(0, _selectionGridHeight, _windowRect.xMax, _topHeight - _titleHeight - 5), pageSelect, new GUIContent[5] { new GUIContent("Main Page"), new GUIContent("Repair"), new GUIContent("Nothing"), new GUIContent("Help"), new GUIContent("About") }, 5, ResourcesManager.GetInstance.skin.button);
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
