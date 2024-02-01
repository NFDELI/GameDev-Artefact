using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSuperBarScript : MonoBehaviour
{
    public Slider superSlider;
    public Slider easeSuperSlider;
    public Image fillColor;
    public Color defaultFillColor;
    public float maxSuper = 12f;
    public float currentSuper;

    public PlayerStateManager player;
    private float lerpSpeed = 0.025f;

    // Start is called before the first frame update
    void Start()
    {
        superSlider.maxValue = player.superMax;
        easeSuperSlider.maxValue = player.superMax;
        defaultFillColor = fillColor.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (superSlider.value != player.superCurrent)
        {
            superSlider.value = player.superCurrent;
        }

        if (superSlider.value != easeSuperSlider.value)
        {
            easeSuperSlider.value = Mathf.Lerp(easeSuperSlider.value, player.superCurrent, lerpSpeed);
        }

        if(player.superCurrent >= player.superMax)
        {
            fillColor.color = Color.green;
        }
        else
        {
            fillColor.color = defaultFillColor;
        }
    }
}
