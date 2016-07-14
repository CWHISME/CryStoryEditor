/**********************************************************
*Author: wangjiaying
*Date: 2016.6.16
*Func:
**********************************************************/

using System.Collections.Generic;

namespace CryStory.Runtime
{
    public class StoryManager
    {
        private Story _story;

        public StoryManager(Story story)
        {
            _story = story;
        }

        private bool _storyEnd = false;
        public void Tick()
        {
            if (_storyEnd) return;
            if (_story.Tick() != EnumResult.Running)
                _storyEnd = true;
        }
    }
}