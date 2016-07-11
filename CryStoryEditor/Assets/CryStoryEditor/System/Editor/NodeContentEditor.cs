/**********************************************************
*Author: wangjiaying
*Date: 2016.7.8
*Func:
**********************************************************/
using UnityEngine;
using System.Collections;
using CryStory.Runtime;

namespace CryStory.Editor
{
    public class NodeContentEditor<T> : NodeEditor<T> where T : class, new()
    {
        //private int _totalFrame;
        //private int _nextFrame;

        //protected override void InternalOnGUI()
        //{
        //    _totalFrame++;
        //}

        //protected bool IsDoubleClick()
        //{
        //    if (Tools.MouseDown)
        //    {
        //        if (_totalFrame < _nextFrame)
        //            return true;
        //        _nextFrame = _totalFrame + 60;
        //        return false;
        //    }
        //    return false;
        //}
    }
}