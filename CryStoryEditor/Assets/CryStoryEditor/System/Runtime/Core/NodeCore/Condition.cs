/**********************************************************
*Author: wangjiaying
*Date: 2016.6.16
*Func:
**********************************************************/

using System;

namespace CryStory.Runtime
{

    abstract public class Condition : StoryNode
    {

        public abstract bool OnCheck();

        public sealed override EnumResult Tick()
        {
            if (OnCheck())
                return EnumResult.Success;
            return EnumResult.Failed;
        }
    }
}