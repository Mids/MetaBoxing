using System;
using UnityEngine;

namespace MetaBoxing
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;

        public void Start()
        {
            target = GameObject.Find("LeftControllerTarget").transform;
        }

        private void Update()
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
        }
    }
}