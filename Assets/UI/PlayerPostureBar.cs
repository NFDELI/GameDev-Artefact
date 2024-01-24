using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPostureBar : MonoBehaviour
{
    public Slider postureSlider;
    public Slider easePostureSlider;
    public float maxPosture = 6f;
    public float currentPosture;

    public PlayerStateManager player;
    private float lerpSpeed = 0.025f;

    // Start is called before the first frame update
    void Start()
    {
        postureSlider.maxValue = player.postureDefault;
        easePostureSlider.maxValue = player.postureDefault;
    }

    // Update is called once per frame
    void Update()
    {
        if (postureSlider.value != player.postureCurrent)
        {
            postureSlider.value = player.postureCurrent;
        }

        if (postureSlider.value != easePostureSlider.value)
        {
            easePostureSlider.value = Mathf.Lerp(easePostureSlider.value, player.postureCurrent, lerpSpeed);
        }
    }
}
