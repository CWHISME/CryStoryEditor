/**********************************************************
*Author: wangjiaying
*Date: 2016.6.6
*Func:
**********************************************************/

using System.Collections.Generic;

namespace CryStory.Runtime
{
    abstract public class NodeBase
    {
        public virtual EnumResult Tick() { return EnumResult.Success; }

        public NodeBase _next;
    }
}
