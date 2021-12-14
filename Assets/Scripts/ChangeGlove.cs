using Photon.Pun;
using UnityEngine;

public class ChangeGlove : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        if (GetComponentInParent<PhotonView>().IsMine)
        {
            var blueMat = Resources.Load<Material>("Meshes/Characters/Gloves_MAT_Blue");
            GetComponent<SkinnedMeshRenderer>().material = blueMat;
        }
    }
}