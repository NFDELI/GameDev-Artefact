using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResizeScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().rect = new Rect(800, 336, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
