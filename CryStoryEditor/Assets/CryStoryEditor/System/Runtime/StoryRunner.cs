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
        void Start()
        {
            _storyManager = new StoryManager(_runStory._Story);
        }

        void Update()
        {
            _storyManager.Tick();
        }

    }
}