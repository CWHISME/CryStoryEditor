/**********************************************************
*Author: wangjiaying
*Date: 2016.8.2
*Func:
**********************************************************/


using System.Collections.Generic;
using UnityEngine;

namespace CryStory.Runtime
{
    public class TriggerManager : Singleton<TriggerManager>
    {
        public TriggerObject CreateShpereTrigger(Vector3 pos, float radius, System.Func<GameObject, bool> checkTrigger, System.Action callBack)
        {
            GameObject o = new GameObject("Trigger");
            o.transform.SetParent(Root.transform);
            o.transform.position = pos;
            TriggerObject trigger = o.AddComponent<TriggerObject>();
            trigger.SetParams(radius, checkTrigger, callBack);

            return trigger;
        }


        private GameObject _root;
        public GameObject Root
        {
            get
            {
                if (!_root)
                    _root = new GameObject("[TriggerSystem]");
                return _root;
            }
        }
    }
}