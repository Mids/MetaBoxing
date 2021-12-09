using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KinematicController : MonoBehaviour
{
    public XRBaseController controller;
    public bool isLeft = false;
    public bool isRight = false;

    private void Start()
    {
        if (isLeft)
            controller = GameObject.Find("LeftHand Controller")?.GetComponent<XRController>();
        if (isRight)
            controller = GameObject.Find("RightHand Controller")?.GetComponent<XRController>();
    }
}