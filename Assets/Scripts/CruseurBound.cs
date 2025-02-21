using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CruseurBound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if(transform.position.x < -0.5f)
        {
            transform.position = new Vector3(-0.5f, transform.position.y, transform.position.z);
        }

        if (transform.position.x > 15f)
        {
            transform.position = new Vector3(15f, transform.position.y, transform.position.z);
        }

        if (transform.position.z < 0f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y,0f);
        }

        if (transform.position.z > 8.4f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 8.4f);
        }
    }
}
