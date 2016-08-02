/**********************************************************
*Author: wangjiaying
*Date: 2016.8.2
*Func:
**********************************************************/
using UnityEngine;
using System.Collections;

namespace CryStory.Example
{
    public class PlayerController : MonoBehaviour
    {

        public float _distance = 0.5f;

        private Rigidbody _rig;
        void Start()
        {
            _rig = GetComponent<Rigidbody>();
        }

        void Update()
        {
            float h = Input.GetAxis("Horizontal") * _distance;
            float v = Input.GetAxis("Vertical") * _distance;
            Vector3 pos = transform.position + Vector3.forward * v + Vector3.right * h;
            transform.LookAt(pos);
            _rig.MovePosition(pos);
        }
    }
}