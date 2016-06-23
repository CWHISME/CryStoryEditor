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

        //For Editor
        public UnityEngine.Vector2 _position;
        public string _name;
    }
}
