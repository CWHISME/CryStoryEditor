/**********************************************************
*Author: wangjiaying
*Date: 2016.6.20
*Func:
**********************************************************/
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace CryStory.Runtime
{
    public class DragModifier : ValueContainer
    {
        /// <summary>
        /// 唯一ID，当然，只针对每一个Mission而言
        /// </summary>
        private List<int> _idList = new List<int>();

        /// <summary>
        /// 为节点计算一个唯一ID
        /// </summary>
        /// <returns></returns>
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
        public void SaveThisNode(System.IO.BinaryWriter w)
        {
            SaveID(w);

            OnSaved(w);
        }

        public void LoadThisNode(BinaryReader r)
        {
            LoadID(r);

            OnLoaded(r);
        }

        private void SaveID(BinaryWriter w)
        {
            List<int> notUsedID = new List<int>();
            for (int i = 0; i < _idList.Count; i++)
            {
                if (GetNextNodeByID(_idList[i]) == null)
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
        }

        private void LoadID(BinaryReader r)
        {
            int count = r.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                _idList.Add(r.ReadInt32());
            }
        }

#if UNITY_EDITOR
        public override void SaveInEditor(BinaryWriter w)
        {
            base.SaveInEditor(w);
            SaveID(w);
        }

        public override void LoadInEditor(BinaryReader r)
        {
            base.LoadInEditor(r);
            LoadID(r);
        }
#endif

        //===For Editor=============
        public Vector2 _graphCenter;
    }
}