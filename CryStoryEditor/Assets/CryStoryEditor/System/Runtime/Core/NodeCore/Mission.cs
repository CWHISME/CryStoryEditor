/**********************************************************
*Author: wangjiaying
*Date: 2016.6.16
*Func:
**********************************************************/
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CryStory.Runtime
{

    public class Mission : DragModifier
    {

        public MissionDescription _missionDescription;

        private List<int> _idList = new List<int>();

        public int GenerateID()
        {
            int id;
            do { id = UnityEngine.Random.Range(1, int.MaxValue); }
            while (_idList.Contains(id));
            _idList.Add(id);
            return id;
        }

        /// <summary>
        /// 校验及保存ID
        /// </summary>
        public override void Serialize(System.IO.BinaryWriter w)
        {
            List<int> notUsedID = new List<int>();
            for (int i = 0; i < _idList.Count; i++)
            {
                if (GetNodeByID(_idList[i]) == null)
                    notUsedID.Add(_idList[i]);
            }

            for (int i = 0; i < notUsedID.Count; i++)
            {
                _idList.Remove(notUsedID[i]);
            }

            w.Write(_idList.Count);
            for (int i = 0; i < _idList.Count; i++)
            {
                w.Write(_idList[i]);
            }

            w.Write(_missionDescription.GetType().FullName);
            _missionDescription.Serialize(w);
        }

        public override void Deserialize(BinaryReader r)
        {
            int count = r.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                _idList.Add(r.ReadInt32());
            }

            string name = r.ReadString();
            _missionDescription = ReflectionHelper.CreateInstance<MissionDescription>(name);
            _missionDescription.Deserialize(r);
        }

        public void OnAbort()
        {

        }

    }
}