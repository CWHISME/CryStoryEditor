/**********************************************************
*Author: wangjiaying
*Date: 2016.6.16
*Func:
**********************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace CryStory.Runtime
{

    public class Mission : DragModifier
    {

        private List<int> _idList = new List<int>();

        public int GenerateID()
        {
            int id;
            do { id = UnityEngine.Random.Range(1, int.MaxValue); }
            while (_idList.Contains(id));
            _idList.Add(id);
            return id;
        }

        protected override void OnSaved(System.IO.BinaryWriter w)
        {
            base.OnSaved(w);

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
        }

        public void OnAbort()
        {

        }

    }
}