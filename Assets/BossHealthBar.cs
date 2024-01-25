using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth = 100f;
    public float currentHealth;

    public BossStateManager boss;
    private float lerpSpeed = 0.025f;

    // Start is called before the first frame update
    void Start()
    {
        //currentHealth = boss.health;
        healthSlider.maxValue = boss.health;
        easeHealthSlider.maxValue = boss.health;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSlider.value != boss.health)
        {
            healthSlider.value = boss.health;
        }

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, boss.health, lerpSpeed);
        }
    }
}
