using Photon.Pun;
using UnityEngine;

namespace MetaBoxing
{
    public class FollowTarget : MonoBehaviour
    {
        public bool isPlayer;
        public bool isLeft = false;
        public Transform target;
        public Vector3 offset;

        public void Start()
        {
            if (!isPlayer) return;

            if (isLeft)
                target = GameObject.Find("LeftControllerTarget").transform;
            else
                target = GameObject.Find("RightControllerTarget").transform;

            if (PlayerPrefs.GetInt("IS3PP") == 1)
                offset = transform.GetComponentInParent<PhotonView>().transform.rotation * new Vector3(0, -0.5f, 1);
        }

        private void Update()
        {
            if (!isPlayer) return;

            transform.position = target.position + offset;
            transform.rotation = target.rotation;
        }
    }
}