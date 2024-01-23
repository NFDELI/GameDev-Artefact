using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossPostureBar : MonoBehaviour
{
    public Slider postureSlider;
    public Slider easePostureSlider;
    public float maxPosture = 6f;
    public float currentPosture;

    public BossStateManager boss;
    private float lerpSpeed = 0.025f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (postureSlider.value != boss.postureCurrent)
        {
            postureSlider.value = boss.postureCurrent;
        }

        if (postureSlider.value != easePostureSlider.value)
        {
            easePostureSlider.value = Mathf.Lerp(easePostureSlider.value, boss.postureCurrent, lerpSpeed);
        }
    }
}

