/**********************************************************
*Author: wangjiaying
*Date: 2016.8.2
*Func:
**********************************************************/
using UnityEngine;
using System.Collections;

namespace CryStory.Runtime
{
    public class TriggerObject : MonoBehaviour
    {

        private System.Func<GameObject, bool> _checkTrigger;
        private System.Action _callBack;
        private float _radius;

        public void Awake()
        {
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        public void SetParams(float radius, System.Func<GameObject, bool> checkTrigger, System.Action callBack)
        {
            _checkTrigger = checkTrigger;
            _callBack = callBack;
            _radius = radius;

            SphereCollider co = gameObject.AddComponent<SphereCollider>();
            co.radius = radius;
            co.isTrigger = true;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (_checkTrigger.Invoke(other.gameObject))
            {
                _callBack.Invoke();
                Destroy(this.gameObject);
            }
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 1, 0, 0.5f);
            Gizmos.DrawSphere(transform.position, _radius);
        }
    }
}