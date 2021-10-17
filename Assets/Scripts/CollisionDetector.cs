using Photon.Pun;
using TMPro;
using UnityEngine;

namespace MetaBoxing
{
    public class CollisionDetector : MonoBehaviour
    {
        public float score = 0;
        public TextMeshPro tmp;

        private void OnCollisionEnter(Collision other)
        {
            if (gameObject.GetComponentInParent<PhotonView>().IsMine !=
                other.gameObject.GetComponentInParent<PhotonView>().IsMine)
            {
                score += 1;
                tmp.text = "" + score;
            }
        }
    }
}