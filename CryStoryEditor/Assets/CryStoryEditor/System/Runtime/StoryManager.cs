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

        public void Tick()
        {
            _story.Tick();
        }
    }
}