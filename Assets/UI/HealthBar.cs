using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth = 100f;
    public float currentHealth;

    public PlayerStateManager player;
    private float lerpSpeed = 0.025f;

    // Start is called before the first frame update
    void Start()
    {
        //currentHealth = player.health;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSlider.value != player.health)
        {
            healthSlider.value = player.health;
        }

        if(healthSlider.value != easeHealthSlider.value) 
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, player.health, lerpSpeed);
        }
    }
}
