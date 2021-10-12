using TMPro;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public float score = 0;
    public TextMeshPro tmp;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Hand"))
        {
            score += 1;
            tmp.text = "" + score;
        }
    }
}