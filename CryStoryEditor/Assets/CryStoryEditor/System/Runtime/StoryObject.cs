/**********************************************************
*Author: wangjiaying
*Date: 2016.6.20
*Func:
**********************************************************/
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CryStory.Runtime
{
    public class StoryObject : ScriptableObject
    {
        private Story _story;
        public event System.Action OnStoryReload;

        public Story _Story
        {
            get
            {
                if (_story == null)
                    Load();
                return _story;
            }
            set
            {
                _story = value;
                if (OnStoryReload != null)
                    OnStoryReload.Invoke();
                if (_editMission != null)
                    _editMission = _story.GetNodeByID(_editMission._id) as Mission;
            }
        }

        public Story CreateNewStoryFromData()
        {
            _story = new Story();
            Load();
            return _story;
        }

        private Mission _editMission;
        public Mission EditMission
        {
            get
            {
                return _editMission;
            }
            set
            {
                _editMission = value;
            }
        }

        public Vector2 StoryCenter { get { return _story._graphCenter; } set { _story._graphCenter = value; } }

        public string _description = "This Story have not description.";

        public byte[] _SaveData = new byte[] { };

        public float _saveVersion = Story.SaveVersion;


        [SerializeField]
        private List<MissionData> _missionSaveList = new List<MissionData>();

#if UNITY_EDITOR

        public MissionData[] MisisonDatas { get { return _missionSaveList.ToArray(); } }

        public bool _haveNullData;

        public float _zoom = 1f;

        public bool _debugMode = false;
#endif
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

        public MissionData GetMissionSaveDataByName(string missionName)
        {
            return _missionSaveList.Find((m) => m.Name == missionName);
        }

#if UNITY_EDITOR
        public bool AssignNewMissionObject(MissionObject mo)
        {
            //if (_missionSaveList == null) _missionSaveList = new List<MissionData>();
            mo.Load();
            if (_missionSaveList.Find((m) => m.Name == mo._mission._name) != null) return false;
            _missionSaveList.Add(new MissionData(mo));
            return true;
        }

        public bool DeleteMissionData(MissionData mo)
        {
            if (!_missionSaveList.Contains(mo)) return false;
            //string path = GetFullMissionPath(mo._name);
            //UnityEditor.AssetDatabase.DeleteAsset(path);
            mo.Delete();
            _missionSaveList.Remove(mo);
            return true;
        }

        public void RemoveAllMissingMissionData()
        {
            if (!UnityEditor.EditorUtility.DisplayDialog("Caution!", "If Delete It Now,You Will Be Not Recovery It!Event You Recovery The Mission File!", "OK", "Cancel")) return;

            List<MissionData> toRemove = new List<MissionData>();
            for (int i = 0; i < _missionSaveList.Count; i++)
            {
                if (_missionSaveList[i].MissionObject == null)
                    toRemove.Add(_missionSaveList[i]);
            }

            for (int i = 0; i < toRemove.Count; i++)
            {
                _missionSaveList.Remove(toRemove[i]);
            }

            Load();
        }


        public MissionData[] GetMissionSaveDataByMission(NodeBase[] missions)
        {
            List<MissionData> datas = new List<MissionData>();
            for (int i = 0; i < missions.Length; i++)
            {
                MissionData data = GetMissionSaveDataByName(missions[i]._name);
                if (data != null)
                    datas.Add(data);
            }
            return datas.ToArray();
        }

        #region Editor Method
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

        /// <summary>
        /// 对Mission进行重命名
        /// 该方法同时修改MissionObject，即实际文件的名字
        /// 保证Mission的名字与Mision文件的名字同步一致
        /// </summary>
        /// <param name="missionName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public bool RenameMission(string missionName, string newName)
        {
            //string oldPath = GetFullMissionPath(missionName);

            MissionData data = _missionSaveList.Find((m) => m.Name == missionName);
            if (null != data)
            {
                return data.Rename(newName);
            }
            return false;
        }

        public void RepairMissionDataByName()
        {
            for (int i = 0; i < _missionSaveList.Count; i++)
            {
                UnityEditor.EditorUtility.DisplayProgressBar("Repair Mission Data .....", "Current Repair Mission: [" + _missionSaveList[i].Name + "]", (i + 1) / _missionSaveList.Count);
                //MissionObject o = UnityEditor.AssetDatabase.LoadAssetAtPath<MissionObject>(GetFullMissionPath(_missionSaveList[i]._name));
                //if (o)
                //{
                //    _missionSaveList[i].MissionObject = o;
                //}
                _missionSaveList[i].ReloadObject();
            }

            UnityEditor.EditorUtility.ClearProgressBar();
        }

        private void RefreshDataName()
        {
            for (int i = 0; i < _missionSaveList.Count; i++)
            {
                if (_missionSaveList[i].MissionObject == null) continue;
                _missionSaveList[i].RefreshExtraData();
                //_missionSaveList[i].Name = _missionSaveList[i].MissionObject.name;
            }
        }

        public void Save()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter w = new BinaryWriter(ms))
                {
                    _saveVersion = Story.SaveVersion;

                    w.Write(Story.SaveVersion);

                    w.Write(JsonUtility.ToJson(_story));

                    _story.SaveOnlyThisNode(w);
                }
                _SaveData = ms.ToArray();
            }


            _haveNullData = false;

            //Upadte Mission Refrence
            NodeModifier[] Nodes = _Story.Nodes;
            for (int i = 0; i < Nodes.Length; i++)
            {
                MissionData data = _missionSaveList.Find((n) => n.Name == Nodes[i]._name);
                if (data != null)
                    if (data.MissionObject != null)
                        data.MissionObject._mission = Nodes[i] as Mission;
            }

            List<MissionData> toDelete = new List<MissionData>();

            for (int i = 0; i < _missionSaveList.Count; i++)
            {
                MissionObject mo = _missionSaveList[i].MissionObject;
                if (mo == null)
                {
                    _haveNullData = true;
                    continue;
                }
                //Update Mission Insure Data
                //mo._mission = _Story.GetNode(mo._mission._name) as Mission;
                //Update Name For insure
                //_missionSaveList[i].Name = mo.name;
                //mo._mission = System.Array.Find<NodeModifier>(Nodes, (n) => n._name == mo.name) as Mission;
                //如果标志Mission已经为空，则代表删除
                if (mo._mission == null)
                {
                    toDelete.Add(_missionSaveList[i]);
                    continue;
                }

                _missionSaveList[i].RefreshExtraData();


                //UnityEditor.EditorUtility.SetDirty(mo);
                NodeModifier[] nextMissions = mo._mission.NextNodes;
                MissionData data = GetMissionSaveDataByName(mo._mission._name);
                data.ClearNextMissionObject();
                if (nextMissions.Length > 0)
                {
                    data.AddNextMissionObject(GetMissionSaveDataByMission(nextMissions));
                }

                mo.Save();
            }

            //删除
            for (int i = 0; i < toDelete.Count; i++)
            {
                DeleteMissionData(toDelete[i]);
            }

            UnityEditor.EditorUtility.SetDirty(this);

            if (!_haveNullData)
                UnityEditor.EditorUtility.DisplayDialog("Tips", "Save Successfully!", "OK");
            else UnityEditor.EditorUtility.DisplayDialog("Tips", "Some Data Save Failed!You should check out it.", "OK");
        }

        private bool _queryloaded = false;
        public void Load()
        {
            if (_SaveData.Length > 0)
                using (MemoryStream ms = new MemoryStream(_SaveData))
                {
                    using (BinaryReader r = new BinaryReader(ms))
                    {
                        float ver = r.ReadSingle();
                        if (ver != Story.SaveVersion)
                        {
                            Debug.LogError("Error:Archive data version not same! Curent: " + Story.SaveVersion + " Data: " + ver);
#if UNITY_EDITOR
                            if (!_queryloaded)
                            {
                                _queryloaded = true;
                                if (!UnityEditor.EditorUtility.DisplayDialog("Error!", "Error:Archive data version not the same! Curent Version: " + Story.SaveVersion + " Data Version:" + ver, "Force Load", "Cancel"))
                                    return;
                            }
#endif
                        }

                        _queryloaded = false;

                        string storyData = r.ReadString();
                        _story = JsonUtility.FromJson<Story>(storyData);
                        _story.LoadOnlyThisNode(r);
                    }
                }

            if (_story == null)
                _story = new Story();

            _haveNullData = false;
            for (int i = 0; i < _missionSaveList.Count; i++)
            {
                MissionData data = _missionSaveList[i];
                if (!data.MissionObject)
                {
                    Debug.Log("Mission Not Found: " + data.Name);
                    _haveNullData = true;
                    continue;
                }

                data.MissionObject.Load();
                data.MissionObject._mission._name = data.Name;

                NodeModifier.SetContent(data.MissionObject._mission, _story);
            }

            //设置已连接的节点
            for (int i = 0; i < _missionSaveList.Count; i++)
            {
                MissionData data = _missionSaveList[i];
                if (!data.MissionObject) continue;
                if (data.MissionObject._nextMissionNodeList.Count > 0)
                {
                    foreach (var item in data.MissionObject._nextMissionNodeList)
                    {
                        MissionData next = GetMissionSaveDataByName(item.Name);
                        if (next == null) continue;
                        if (next.MissionObject == null) continue;
                        if (item.IsSingleNode)
                        {
                            data.MissionObject._mission.AddNextNode(next.MissionObject._mission);
                        }
                        else Mission.SetParent(next.MissionObject._mission, data.MissionObject._mission);
                    }
                }
            }

            if (_haveNullData)
            {
                UnityEditor.EditorUtility.DisplayDialog("Attention!", "You have some Mission File Lost! ", "OK");
            }
        }

        //public void Load(byte[] data)
        //{
        //    _story = new Story();
        //    _story.Load(data);
        //}
        #endregion
#else
        public byte[] Save()
        {
            return _story.Save();
        }

        public void Load(byte[] data)
        {
            _story = new Story();
            _story.Load(data);
        }

         public void Load()
        {
            if (_SaveData.Length > 0)
                using (MemoryStream ms = new MemoryStream(_SaveData))
                {
                    using (BinaryReader r = new BinaryReader(ms))
                    {
                        float ver = r.ReadSingle();
                        if (ver != Story.SaveVersion)
                        {
                            Debug.LogError("Error:Archive data version not same! Curent: " + Story.SaveVersion + " Data: " + ver);
                        }

                        string storyData = r.ReadString();
                        _story = JsonUtility.FromJson<Story>(storyData);
                        _story.LoadOnlyThisNode(r);
                    }
                }

            if (_story == null)
                _story = new Story();

            for (int i = 0; i < _missionSaveList.Count; i++)
            {
                MissionData data = _missionSaveList[i];
                if (!data.MissionObject)
                {
                    Debug.Log("Mission Not Found: " + data.Name);
                    continue;
                }

                data.MissionObject.Load();
                data.MissionObject._mission._name = data.Name;

                NodeModifier.SetContent(data.MissionObject._mission, _story);
            }

            //设置已连接的节点
            for (int i = 0; i < _missionSaveList.Count; i++)
            {
                MissionData data = _missionSaveList[i];
                if (!data.MissionObject) continue;
                if (data.MissionObject._nextMissionNodeList.Count > 0)
                {
                    foreach (var item in data.MissionObject._nextMissionNodeList)
                    {
                        MissionData next = GetMissionSaveDataByName(item.Name);
                        if (next == null) continue;
                        if (next.MissionObject == null) continue;
                        if (item.IsSingleNode)
                        {
                            data.MissionObject._mission.AddNextNode(next.MissionObject._mission);
                        }
                        else Mission.SetParent(next.MissionObject._mission, data.MissionObject._mission);
                    }
                }
            }
        }
#endif
    }

    [System.Serializable]
    public class MissionData
    {
        [SerializeField]
        private string _name;
#if UNITY_EDITOR
        [SerializeField]
        private int _instanceID;
#endif
        [SerializeField]
        private MissionObject _missionObject;
        public string Name { get { return _name; } }
        public MissionObject MissionObject
        {
            get
            {
#if UNITY_EDITOR
                if (!_missionObject) ReloadObject();
#endif
                return _missionObject;
            }
            set { _missionObject = value; }
        }

        public MissionData(MissionObject obj)
        {
            _missionObject = obj;
#if UNITY_EDITOR
            RefreshExtraData();
#endif
        }

        public void AddNextMissionObject(MissionData[] os)
        {
            for (int i = 0; i < os.Length; i++)
            {
                MissionObject.AddNextMissionName(os[i]._name, os[i].MissionObject._mission.Parent != MissionObject._mission);
            }
        }

        public void RefreshExtraData()
        {
            _name = MissionObject._mission._name;
#if UNITY_EDITOR
            _instanceID = MissionObject.GetInstanceID();
#endif
        }

#if UNITY_EDITOR
        public void Delete()
        {
            UnityEditor.AssetDatabase.DeleteAsset(FilePath);
        }

        public bool Rename(string newName)
        {
            //重命名成功
            string msg = UnityEditor.AssetDatabase.RenameAsset(FilePath, newName);
            if (string.IsNullOrEmpty(msg))
            {
                _name = newName;
                MissionObject._mission._name = newName;
                return true;
            }

            UnityEditor.EditorUtility.DisplayDialog("错误！", "Modify Name Failed! Msg:" + msg, "OK");
            return false;
        }

        public void ReloadObject()
        {
            _missionObject = UnityEditor.AssetDatabase.LoadAssetAtPath<MissionObject>(FilePath);
        }

        private string FilePath { get { return UnityEditor.AssetDatabase.GetAssetPath(_instanceID); } }
#endif

        public void ClearNextMissionObject()
        {
            MissionObject._nextMissionNodeList.Clear();
        }
    }
}