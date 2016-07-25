/**********************************************************
*Author: wangjiaying
*Date: 2016.7.8
*Func:
**********************************************************/
using CryStory.Runtime;
using System;
using UnityEngine;

namespace CryStory.Editor
{
    public class NodeContentEditor<T> : NodeEditor<T> where T : class, new()
    {
        public ValueContainer CurrentValueContainer
        {
            get
            {
                if (_currentNode == null) return null;
                return _currentNode as ValueContainer;
            }
        }

    }
}