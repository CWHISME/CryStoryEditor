﻿/**********************************************************
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

        public Vector2 StoryCenter { get { return _story.graphCenter; } set { _story.graphCenter = value; } }

        public string _description = "This Story have not description.";

        public byte[] _SaveData = new byte[] { };

        public float _saveVersion = Story.SaveVersion;

#if UNITY_EDITOR

        [SerializeField]
        private List<MissionData> _missionSaveList = new List<MissionData>();

        public MissionData[] MisisonDatas { get { return _missionSaveList.ToArray(); } }

        public bool _haveNullData;

        public float _zoom = 1f;

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

#if UNITY_EDITOR
        public bool AssignNewMissionObject(MissionObject mo)
        {
            //if (_missionSaveList == null) _missionSaveList = new List<MissionData>();
            mo.Load();
            if (_missionSaveList.Find((m) => m._name == mo._mission._name) != null) return false;
            _missionSaveList.Add(new MissionData() { _name = mo._mission._name, _missionObject = mo });
            return true;
        }

        public bool DeleteMissionData(MissionData mo)
        {
            if (!_missionSaveList.Contains(mo)) return false;
            string path = GetFullMissionPath(mo._name);
            UnityEditor.AssetDatabase.DeleteAsset(path);
            _missionSaveList.Remove(mo);
            return true;
        }

        public void RemoveAllMissingMissionData()
        {
            if (!UnityEditor.EditorUtility.DisplayDialog("Caution!", "If Delete It Now,You Will Be Not Recovery It!Event You Recovery The Mission File!", "OK", "Cancel")) return;

            List<MissionData> toRemove = new List<MissionData>();
            for (int i = 0; i < _missionSaveList.Count; i++)
            {
                if (_missionSaveList[i]._missionObject == null)
                    toRemove.Add(_missionSaveList[i]);
            }

            for (int i = 0; i < toRemove.Count; i++)
            {
                _missionSaveList.Remove(toRemove[i]);
            }

            Load();
        }


        public MissionData GetMissionSaveDataByName(string missionName)
        {
            return _missionSaveList.Find((m) => m._name == missionName);
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

        public void RepairMissionDataByName()
        {
            for (int i = 0; i < _missionSaveList.Count; i++)
            {
                UnityEditor.EditorUtility.DisplayProgressBar("Repair Mission Data .....", "Current Repair Mission: [" + _missionSaveList[i]._name + "]", (i + 1) / _missionSaveList.Count);
                MissionObject o = UnityEditor.AssetDatabase.LoadAssetAtPath<MissionObject>(GetFullMissionPath(_missionSaveList[i]._name));
                if (o)
                {
                    _missionSaveList[i]._missionObject = o;
                }
            }

            UnityEditor.EditorUtility.ClearProgressBar();
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
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter w = new BinaryWriter(ms))
                {
                    _saveVersion = Story.SaveVersion;

                    w.Write(Story.SaveVersion);

                    w.Write(JsonUtility.ToJson(_story));

                    _story.SaveInEditor(w);
                }
                _SaveData = ms.ToArray();
            }


            _haveNullData = false;

            //Upadte Mission Refrence
            NodeModifier[] Nodes = _Story.Nodes;
            for (int i = 0; i < Nodes.Length; i++)
            {
                MissionData data = _missionSaveList.Find((n) => n._name == Nodes[i]._name);
                if (data != null)
                    if (data._missionObject != null)
                        data._missionObject._mission = Nodes[i] as Mission;
            }

            List<MissionData> toDelete = new List<MissionData>();

            for (int i = 0; i < _missionSaveList.Count; i++)
            {
                MissionObject mo = _missionSaveList[i]._missionObject;
                if (mo == null)
                {
                    _haveNullData = true;
                    continue;
                }
                //Update Mission Insure Data
                //mo._mission = _Story.GetNode(mo._mission._name) as Mission;
                //Update Name For insure
                _missionSaveList[i]._name = mo.name;

                //mo._mission = System.Array.Find<NodeModifier>(Nodes, (n) => n._name == mo.name) as Mission;
                //如果标志Mission已经为空，则代表删除
                if (mo._mission == null)
                {
                    toDelete.Add(_missionSaveList[i]);
                    continue;
                }

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
                        _story.LoadInEditor(r);
                    }
                }

            if (_story == null)
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

                NodeModifier.SetContent(data._missionObject._mission, _story);
            }

            //设置已连接的节点
            for (int i = 0; i < _missionSaveList.Count; i++)
            {
                MissionData data = _missionSaveList[i];
                if (!data._missionObject) continue;
                if (data._missionObject._nextMissionNodeList.Count > 0)
                {
                    foreach (var item in data._missionObject._nextMissionNodeList)
                    {
                        MissionData next = GetMissionSaveDataByName(item.Name);
                        if (next == null) continue;
                        if (next._missionObject == null) continue;
                        if (item.IsSingleNode)
                        {
                            data._missionObject._mission.AddNextNode(next._missionObject._mission);
                        }
                        else Mission.SetParent(next._missionObject._mission, data._missionObject._mission);
                    }
                }
            }

            if (_haveNullData)
            {
                UnityEditor.EditorUtility.DisplayDialog("Attention!", "You have some Mission File Lost! ", "OK");
            }
        }
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
#endif
    }

#if UNITY_EDITOR
    [System.Serializable]
    public class MissionData
    {
        public string _name;
        public MissionObject _missionObject;

        public void AddNextMissionObject(MissionData[] os)
        {
            for (int i = 0; i < os.Length; i++)
            {
                _missionObject.AddNextMissionName(os[i]._name, os[i]._missionObject._mission.Parent != _missionObject._mission);
            }
        }

        //public void RemoveNextMissionObject(string o)
        //{
        //    _missionObject._nextMissionNodeLIst.RemoveAll();
        //    _missionObject._nextMissionNodeLIst.Remove(o);
        //}

        public void ClearNextMissionObject()
        {
            _missionObject._nextMissionNodeList.Clear();
        }
    }

#endif
}