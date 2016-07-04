/**********************************************************
*Author: wangjiaying
*Date: 2016.6.16
*Func:
**********************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryStory.Runtime
{
    public class Mission : DragModifier
    {

        [HideInInspector]
        public int _prototypeId;

        private List<NodeBase> _nodeList = new List<NodeBase>(2);

        public override EnumResult Tick()
        {
            if (_nodeList.Count == 0) return EnumResult.Success;

            for (int i = 0; i < _nodeList.Count; i++)
            {
                if (_nodeList[i].Tick() != EnumResult.Running)
                {
                    NodeBase node = _nodeList[i];
                    _nodeList.RemoveAt(i);
                    for (int j = 0; j < node._nextNodeList.Count; j++)
                    {
                        _nodeList.Add(node._nextNodeList[j]);
                    }
                }
            }

            return EnumResult.Running;
        }


        public void OnAbort()
        {

        }

    }
}