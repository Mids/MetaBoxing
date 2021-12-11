using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class VCamFollower : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("ISPHYSICS") == 0 || PlayerPrefs.GetInt("IS3PP") == 1 || !GetComponentInParent<PhotonView>().IsMine)
            GetComponent<CinemachineVirtualCamera>().enabled = false;
    }
}