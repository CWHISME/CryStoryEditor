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
            _saveData = _runner.StoryManager.Story.Save();
        }

        public void Load()
        {
            _runner.LoadStory(_saveData);
        }
    }
}