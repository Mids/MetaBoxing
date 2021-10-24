using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotator : MonoBehaviour
{
    public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var rot = Quaternion.Euler(0, speed * Time.deltaTime, 0);

        transform.rotation = rot * transform.rotation;


    }
}
