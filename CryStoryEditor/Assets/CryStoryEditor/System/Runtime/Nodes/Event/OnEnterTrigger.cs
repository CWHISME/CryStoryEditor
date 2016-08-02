/**********************************************************
*Author: CWHISME with PC DESKTOP-TCL3N2O
*Date: 8/2/2016 4:53:46 PM
*Func:
**********************************************************/

using System;
using UnityEngine;

namespace CryStory.Runtime
{
    public class OnEnterTrigger : Event
    {

        public Vector3 Position;
        public float Radius;

        public string TargetTag = "Player";

        protected override void RegistEvent()
        {
            TriggerManager.GetInstance.CreateShpereTrigger(Position, Radius, Check, TriggerEvent);
        }

        protected override void UnRegistEvent()
        {

        }

        private bool Check(GameObject other)
        {
            return other.CompareTag(TargetTag);
        }
    }
}