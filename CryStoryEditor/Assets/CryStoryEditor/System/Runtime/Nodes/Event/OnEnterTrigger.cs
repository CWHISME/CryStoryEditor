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
        public float Radius = 0.5f;

        public string TargetTag = "Player";

        private TriggerObject _triggerObject;

        protected override void RegistEvent()
        {
            _triggerObject = TriggerManager.GetInstance.CreateShpereTrigger(Position, Radius, Check, TriggerEvent);
        }

        protected override void UnRegistEvent()
        {

        }

        public override void ForceStop()
        {
            GameObject.Destroy(_triggerObject.gameObject);
        }

        private bool Check(GameObject other)
        {
            return other.CompareTag(TargetTag);
        }
    }
}