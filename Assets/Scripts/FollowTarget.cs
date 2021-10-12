using UnityEngine;

namespace MetaBoxing
{
    public class FollowTarget : MonoBehaviour
    {
        public bool isPlayer;
        public bool isLeft = false;
        public Transform target;

        public void Start()
        {
            if (!isPlayer) return;

            if (isLeft)
                target = GameObject.Find("LeftControllerTarget").transform;
            else
                target = GameObject.Find("RightControllerTarget").transform;
        }

        private void Update()
        {
            if (!isPlayer) return;

            transform.position = target.position;
            transform.rotation = target.rotation;
        }
    }
}