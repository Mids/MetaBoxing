using MetaBoxing;
using Photon.Pun;
using UnityEngine;

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