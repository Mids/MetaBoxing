using UnityEngine;

public class BodyLookAt : MonoBehaviour
{
    public Transform head;

    private void Update()
    {
        var dir = head.position - transform.position;

        var projZ = Vector3.ProjectOnPlane(dir, Vector3.forward);
        var projX = Vector3.ProjectOnPlane(dir, Vector3.right);
        var rotZ = Vector3.SignedAngle(Vector3.up, projZ, Vector3.forward);
        var rotX = Vector3.SignedAngle(Vector3.up, projX, Vector3.right);

        transform.localRotation = Quaternion.Euler(rotX, 0, rotZ);
    }
}