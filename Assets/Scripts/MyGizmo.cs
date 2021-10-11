using UnityEngine;

public class MyGizmo : MonoBehaviour
{
    public Color color = Color.yellow;

    public float radius = 0.2f;


    // Start is called before the first frame update
    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
    }
}