using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPiece : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TurnPieces()
    {
        foreach(Transform t in transform)
        {
            t.localPosition = Quaternion.AngleAxis(90, Vector3.up) * t.localPosition;
            t.Rotate(0,90,0);
        }
    }
}
