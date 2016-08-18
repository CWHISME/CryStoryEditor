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
            if (StoryWindow != null)
            {
                StoryWindow.Show();
                return;
            }
            StoryWindow = EditorWindow.CreateInstance<StoryEditorWindow>();
            StoryWindow.titleContent = new GUIContent("Story Editor");
            float h = Screen.height * 0.7f;
            float w = Screen.width * 0.7f;
            StoryWindow.position = new Rect(Screen.width - w, Screen.height - h, w, h);

            StoryWindow.Show();
        }

        public StoryObject _storyObject;
        public Story _Story { get { return _storyObject._Story; } }
        public Mission EditMission { get { return _storyObject.EditMission; } set { _storyObject.EditMission = value; } }
        public Vector2 CurrentContentCenter
        {
            get
            {
                return EditMission == null ? _Story.graphCenter : EditMission.graphCenter;
            }
            set
            {
                if (EditMission == null)
                    _Story.graphCenter = value;
                else EditMission.graphCenter = value;
            }
        }

        public Rect _windowRect;
        public Rect _contentRect;

        public float _leftWidth = 230f;
        public float _topHeight = 60f;
        public float _titleHeight = 35f;
        public float _selectionGridHeight { get { return _titleHeight + 5; } }

        private int pageSelect = 0;

        public const float _zoomMin = 0.4f;
        public const float _zoomMax = 2f;
        public float Zoom
        {
            get
            {
                if (_storyObject)
                    return _storyObject._zoom;
                return 1;
            }
            set { if (_storyObject) _storyObject._zoom = value; }
        }

        void OnEnable()
        {
            StoryWindow = this;
            Repaint();
        }

        void Update()
        {
            Repaint();
            StoryWindow = this;
            //Debug.Log("dd");
        }

        void OnGUI()
        {
            //Update cache window size
            _windowRect = new Rect(0, 0, position.width, position.height);
            _contentRect = new Rect(_leftWidth, _topHeight, position.width, position.height);

            //========Story Editor ==============
            if (MainPageEditor.GetInstance.OnGUI(this)) return;

            //Show Pages
            switch (pageSelect)
            {
                case 1:
                    RepairPages();
                    break;
                case 2:
                    SettingPage();
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
            //========Version          ==============
            ShowVersion();
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

        private Vector2 _storySettingScroll;
        private void SettingPage()
        {
            //EditorGUI.LabelField(new Rect(_windowRect.center.x - 38, _windowRect.center.y - 50, 500, 20), "Story Description:");
            //_storyObject._description = EditorGUI.TextArea(new Rect(_windowRect.center.x - 38, _windowRect.center.y, 500, 200), _storyObject._description);
            GUILayout.Space(_topHeight + 10);

            EditorGUILayout.BeginHorizontal();

            //Story Setting
            _storySettingScroll = GUILayout.BeginScrollView(_storySettingScroll, false, true);
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Mission Description Template:");
            Type[] types = ReflectionHelper.GetTypeSubclass(typeof(MissionDescription));
            string[] str = Array.ConvertAll<Type, string>(types, (i) => i.Name);
            int index = Array.FindIndex<Type>(types, (t) => t.FullName == _Story._missionDescriptionType);
            index = EditorGUILayout.Popup(index, str);
            _Story._missionDescriptionType = types[index].FullName;

            GUILayout.Label("Story Description:");
            GUILayout.Space(10);
            _storyObject._description = GUILayout.TextArea(_storyObject._description, GUILayout.MaxHeight(100));
            EditorGUILayout.EndVertical();
            GUILayout.EndScrollView();

            GUILayout.Space(20);
            //Editor Setting
            EditorGUILayout.BeginVertical();
            _storyObject._debugMode = EditorGUILayout.Toggle("DebugMode: ", _storyObject._debugMode);
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        private Vector2 helpScrolPos;
        private bool langugeCN = true;
        private void HelpPage()
        {
            GUILayout.Space(_topHeight + 20);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(_windowRect.width * 0.25f);
            helpScrolPos = EditorGUILayout.BeginScrollView(helpScrolPos);

            EditorGUILayout.BeginHorizontal();
            langugeCN = EditorGUILayout.ToggleLeft("中文", langugeCN);
            langugeCN = !EditorGUILayout.ToggleLeft("English", !langugeCN);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);

            GUIStyle style = new GUIStyle();
            style.fontSize = 12;
            style.normal.textColor = Color.white;
            GUILayout.Label(langugeCN ? ResourcesManager.GetInstance.HelpCN : ResourcesManager.GetInstance.HelpEN, style);
            GUILayout.Space(50);
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();
        }

        private void AboutPage()
        {
            EditorGUI.LabelField(new Rect(_windowRect.center.x - 100, _windowRect.center.y - 100, 300, 20), "Cry Story Editor", ResourcesManager.GetInstance.skin.GetStyle("Title"));
            EditorGUI.LabelField(new Rect(_windowRect.center.x - 33, _windowRect.center.y - 50, 500, 20), "By CWHISME");
            EditorGUI.LabelField(new Rect(_windowRect.center.x - 55, _windowRect.center.y - 20, 500, 20), "Email: cwhisme@qq.com");
            EditorGUI.LabelField(new Rect(_windowRect.center.x - 20, _windowRect.center.y, 500, 20), Version.FullVersion);

            //Web Github and Blog
            EditorGUI.LabelField(new Rect(_windowRect.center.x - 80, _windowRect.center.y + 30, 500, 20), "It's a Open Source project.\n    For more information:", ResourcesManager.GetInstance.GetFontStyle(14, Color.green));
            EditorGUI.SelectableLabel(new Rect(_windowRect.center.x - 60, _windowRect.center.y + 80, 300, 20), "http://www.cwhisme.com");
            EditorGUI.SelectableLabel(new Rect(_windowRect.center.x - 110, _windowRect.center.y + 100, 300, 20), "https://github.com/CWHISME/CryStoryEditor");
        }

        private void MainPage()
        {
            EditorGUI.DrawTextureTransparent(_contentRect, ResourcesManager.GetInstance.texBackground);

            //更新ValueManager中MissionConstainer，避免Mission页面无法找到Mission
            ValueManagerWindow.missionContainer = EditMission;

            if (EditMission == null)
            {
                if (Application.isPlaying) StoryEditorRuntime.GetInstance.OnGUI(this, _storyObject._Story.Nodes);
                else StoryEditor.GetInstance.OnGUI(this, _storyObject._Story.Nodes);
            }
            else
            {
                if (Application.isPlaying) MissionEditorRuntime.GetInstance.OnGUI(this, EditMission.Nodes);
                else MissionEditor.GetInstance.OnGUI(this, EditMission.Nodes);
            }


            //处理缩放
            if (UnityEngine.Event.current != null)
            {
                if (UnityEngine.Event.current.type == EventType.ScrollWheel)
                {
                    _storyObject._zoom -= UnityEngine.Event.current.delta.y * 0.01f;
                    _storyObject._zoom = Mathf.Clamp(_storyObject._zoom, _zoomMin, _zoomMax);
                    //Debug.Log(_zoom);
                }
            }

            //显示清理损毁节点信息
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

            if (EditMission != null)
            {
                btnW = 35;
                if (GUI.Button(new Rect(2, 3, btnW + 2, _titleHeight - 3), "<-", ResourcesManager.GetInstance.skin.button))
                    EditMission = null;
            }

            EditorGUI.LabelField(new Rect(btnW, 0, _windowRect.xMax, _titleHeight), new GUIContent(" Story Editor" + (_storyObject ? "->" + (EditMission == null ? _storyObject.name : _storyObject.name + " -> " + EditMission._name) : "")), style);

            GUIStyle buttonStyle = new GUIStyle(ResourcesManager.GetInstance.skin.button);
            if (GUI.Button(new Rect(_contentRect.width - 90, 3, 80, _titleHeight - 3), "Values", buttonStyle))
            {
                ValueManagerWindow.Open();
            }

            //运行时不允许存储加载
            if (Application.isPlaying) return;

            buttonStyle.normal.textColor = new Color32(255, 64, 180, 255);
            if (GUI.Button(new Rect(_contentRect.width - 180, 3, 80, _titleHeight - 3), "Reload", buttonStyle))
            {
                EditMission = null;
                _storyObject.Load();
            }
            buttonStyle.normal.textColor = new Color32(0, 255, 0, 255);
            if (GUI.Button(new Rect(_contentRect.width - 270, 3, 80, _titleHeight - 3), "Save Story", buttonStyle))
                _storyObject.Save();

            //EditorGUILayout.EndHorizontal();
        }

        private void ShowTopMenuUI()
        {
            Rect rect = new Rect(0, _titleHeight, _windowRect.xMax, _topHeight - _titleHeight);
            GUI.Box(rect, "", ResourcesManager.GetInstance.StyleBackground);
            pageSelect = GUI.SelectionGrid(new Rect(0, _selectionGridHeight, _windowRect.xMax, _topHeight - _titleHeight - 5), pageSelect, new GUIContent[5] { new GUIContent("Main Page"), new GUIContent("Repair"), new GUIContent("Setting"), new GUIContent("Help"), new GUIContent("About") }, 5, ResourcesManager.GetInstance.skin.button);
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
