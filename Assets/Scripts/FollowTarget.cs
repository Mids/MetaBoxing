using UnityEngine;

namespace MetaBoxing
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;

        private void Update()
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
        }
    }
}