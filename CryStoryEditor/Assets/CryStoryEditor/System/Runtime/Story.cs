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
    public class Story : ScriptableObject
    {
        public Mission _mission = new Mission();
        public int ID;
    }
}