using Cinemachine;
using UnityEngine;

public class VCamFollower : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("ISPHYSICS") == 0 || PlayerPrefs.GetInt("IS3PP") == 1)
            GetComponent<CinemachineVirtualCamera>().enabled = false;
    }
}