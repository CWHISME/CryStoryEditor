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

        private Story _story;
        void Start()
        {
            _story = _runStory._Story;
        }

        void Update()
        {
            if (_story != null) _story.Tick();
        }
    }
}