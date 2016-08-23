/**********************************************************
*Author: wangjiaying
*Date: 2016.7.8
*Func:
**********************************************************/
using UnityEngine;
using System.Collections;

namespace CryStory.Runtime
{
    public class StoryRunner : MonoBehaviour
    {

        public StoryObject _runStory;


        private StoryManager _storyManager;
        public StoryManager StoryManager { get { return _storyManager; } }
        void Awake()
        {
            _storyManager = new StoryManager(_runStory._Story);
        }

        void Update()
        {
            _storyManager.Tick();
        }

        public void LoadStory(byte[] saveData)
        {
            StoryManager.LoadStory(saveData);
            _runStory._Story = StoryManager.CurrentStory;
        }

        public void OnApplicationQuit()
        {
            _runStory.Load();
        }
    }
}