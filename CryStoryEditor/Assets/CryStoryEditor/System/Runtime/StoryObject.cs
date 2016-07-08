/**********************************************************
*Author: wangjiaying
*Date: 2016.6.20
*Func:
**********************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace CryStory.Runtime
{
    public class StoryObject : ScriptableObject
    {
        private Story _story;

        public Story _Story
        {
            get
            {
                if (_story == null)
                    Load();
                return _story;
            }
        }

#if UNITY_EDITOR
        public const string MissionDirectoryExtName = "Missions";
        public string GetFullMissionDirectoryPath()
        {
            string path = UnityEditor.AssetDatabase.GetAssetPath(this);
            path = path.Replace(System.IO.Path.GetFileName(path), "");
            return /*Application.dataPath + "/../" +*/ path + "[" + name + MissionDirectoryExtName + "]/";
        }

        public string GetFullMissionPath(string missionName)
        {
            return GetFullMissionDirectoryPath() + missionName + ".asset";
        }

        public bool RenameMission(string missionName, string newName)
        {
            string oldPath = GetFullMissionPath(missionName);//UnityEditor.FileUtil.GetProjectRelativePath(GetFullMissionPath(missionName));
            //string newPath = GetFullMissionPath(newName);//UnityEditor.FileUtil.GetProjectRelativePath(GetFullMissionPath(newName));

            string msg = UnityEditor.AssetDatabase.RenameAsset(oldPath, newName);
            if (string.IsNullOrEmpty(msg))
            {
                MissionData data = _missionSaveList.Find((m) => m._name == missionName);
                if (data != null)
                    data._name = newName;
                return true;
            }

            UnityEditor.EditorUtility.DisplayDialog("错误！", "Modify Name Failed! Msg:" + msg, "OK");
            return false;
        }
#endif

        public Vector2 StoryCenter { get { return _story.graphCenter; } set { _story.graphCenter = value; } }

        public bool _haveNullData;

        private List<MissionData> _missionSaveList;

        //public List<SaveData> _data;
        /// <summary>
        /// 直接添加一个新的任务
        /// </summary>
        /// <returns></returns>
        public Mission AddNewMission()
        {
            Mission mission = new Mission();
            _story.AddContentNode(mission);
            return mission;
        }

        public bool AssignNewMissionObject(MissionObject mo)
        {
            if (_missionSaveList == null) _missionSaveList = new List<MissionData>();

            if (_missionSaveList.Find((m) => m._name == mo._mission._name) != null) return false;
            _missionSaveList.Add(new MissionData() { _name = mo._mission._name, _missionObject = mo });
            return true;
        }

        public void RemoveAllMissingMissionData()
        {
            if (!UnityEditor.EditorUtility.DisplayDialog("Caution!", "If Delete It Now,You Will Be Not Recovery It!Event You Recovery The Mission File!", "OK", "Cancel")) return;

            List<int> toRemove = new List<int>();
            for (int i = 0; i < _missionSaveList.Count; i++)
            {
                if (_missionSaveList[i]._missionObject == null)
                    toRemove.Add(i);
            }

            for (int i = 0; i < toRemove.Count; i++)
            {
                _missionSaveList.RemoveAt(toRemove[i]);
            }

            Load();
        }

        private void RefreshDataName()
        {
            for (int i = 0; i < _missionSaveList.Count; i++)
            {
                if (_missionSaveList[i]._missionObject == null) continue;
                _missionSaveList[i]._name = _missionSaveList[i]._missionObject.name;
            }
        }

        public void Save()
        {
            //NodeModifier[] nodes = _Story.Nodes;
            //_data = new List<SaveData>();
            //for (int i = 0; i < nodes.Length; i++)
            //{
            //    NodeModifier node = nodes[i];
            //    System.Type type = node.GetType();
            //    //NodeBase n = _story;
            //    //System.Type tt = n.GetType();
            //    SaveData data = new SaveData();
            //    data._type = type.FullName;
            //    data._json = JsonUtility.ToJson(node);

            //    _data.Add(data);
            //}
            _haveNullData = false;
            for (int i = 0; i < _missionSaveList.Count; i++)
            {
                MissionObject mo = _missionSaveList[i]._missionObject;
                if (mo == null)
                {
                    _haveNullData = true;
                    continue;
                }
                mo.Save();
                UnityEditor.EditorUtility.SetDirty(mo);
            }

            UnityEditor.EditorUtility.SetDirty(this);

            if (!_haveNullData)
                UnityEditor.EditorUtility.DisplayDialog("Tips", "Save Successfully!", "OK");
            else UnityEditor.EditorUtility.DisplayDialog("Tips", "Some Data Save Failed!You should check out it.", "OK");
        }

        public void Load()
        {
            _story = new Story();

            _haveNullData = false;
            for (int i = 0; i < _missionSaveList.Count; i++)
            {
                MissionData data = _missionSaveList[i];
                if (!data._missionObject)
                {
                    Debug.Log("Mission Not Found: " + data._name);
                    _haveNullData = true;
                    continue;
                }
                data._missionObject.Load();

                _story.AddContentNode(data._missionObject._mission);
            }

            if (_haveNullData)
            {
                UnityEditor.EditorUtility.DisplayDialog("Attention!", "You have some Mission File Lost! ", "OK");
            }
            //if (_data != null)
            //{
            //    System.Reflection.Assembly asm = System.Reflection.Assembly.GetAssembly(typeof(Story));
            //    for (int i = 0; i < _data.Count; i++)
            //    {
            //        object o = asm.CreateInstance(_data[i]._type);
            //        JsonUtility.FromJsonOverwrite(_data[i]._json, o);
            //        _story.AddContentNode(o as NodeModifier);
            //    }
            //}
        }
    }

    [System.Serializable]
    public class MissionData
    {
        public string _name;
        public MissionObject _missionObject;
    }

}