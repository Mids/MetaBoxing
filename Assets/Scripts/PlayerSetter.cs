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
            {
                SetPlayer();
                foreach (var component in GetComponentsInChildren<Collider>())
                {
                    if(LayerMask.LayerToName(component.gameObject.layer) == "Player 1")
                        component.gameObject.layer = LayerMask.NameToLayer("Agent 1");
                    else if(LayerMask.LayerToName(component.gameObject.layer) == "Player 2")
                        component.gameObject.layer = LayerMask.NameToLayer("Agent 2");
                    else if(LayerMask.LayerToName(component.gameObject.layer) == "Player 3")
                        component.gameObject.layer = LayerMask.NameToLayer("Agent 3");
                    else if(LayerMask.LayerToName(component.gameObject.layer) == "Player 4")
                        component.gameObject.layer = LayerMask.NameToLayer("Agent 4");
                    else if(LayerMask.LayerToName(component.gameObject.layer) == "Player 5")
                        component.gameObject.layer = LayerMask.NameToLayer("Agent 5");
                }
            }
        }

        public void SetPlayer()
        {
            hips.isPlayer = true;
            left.isPlayer = true;
            right.isPlayer = true;
        }
    }
}