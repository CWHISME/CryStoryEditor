/**********************************************************
*Author: wangjiaying
*Date: 2016.7.28
*Func:
**********************************************************/
using UnityEngine;
using System.Collections;
using CryStory.Runtime;

namespace CryStory.Examples
{
    public class UISaveAndLoad : MonoBehaviour
    {
        private StoryRunner _runner;

        private byte[] _saveData;

        void Start()
        {
            _runner = GetComponent<StoryRunner>();
        }

        public void Save()
        {
            _saveData = _runner.StoryManager.CurrentStory.Save();
        }

        public void Load()
        {
            if (_saveData != null)
                _runner.LoadStory(_saveData);
        }
    }
}