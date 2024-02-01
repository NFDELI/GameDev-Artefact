using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeScript : MonoBehaviour
{
    Animator cameraAnim;

    private void Start()
    {
        cameraAnim = GetComponent<Animator>();
    }

    public void CameraShake()
    {
        cameraAnim.SetTrigger("triggerShake");
    }
}
