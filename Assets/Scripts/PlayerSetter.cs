using Photon.Pun;
using UnityEngine;

namespace MetaBoxing
{
    public class PlayerSetter : MonoBehaviour
    {
        public BodyLookAt hips;
        public FollowTarget left;
        public FollowTarget right;

        private void Start()
        {
            if (GetComponent<PhotonView>().IsMine)
                SetPlayer();
        }

        public void SetPlayer()
        {
            hips.isPlayer = true;
            left.isPlayer = true;
            right.isPlayer = true;
        }
    }
}