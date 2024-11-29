using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinEvent : MonoBehaviour
{
    public MeterScript healthMeter; //this allows you to link this script to the meter's script via the inspector
    public int currentHealth; //defines the variable you'd like to keep track of
    public int maxHealth = 30; //defines the maximum value of your variable (keep it at a multiple of 8 so that it matches the meter's sections)
    void Start()
    {
        currentHealth = 0; //sets your variable to maximum from the start
        healthMeter.SetMaxHealth(maxHealth); //sets your meter's fill to maximum from the start

    }

    void FixedUpdate()
    {
        healthMeter.SetHealth(currentHealth); //links your variable to the meter's fill

    }

    public void Increase(int n)
    {

        currentHealth += n; //increases the variable's value by 10
    }

    public void Decrease(int n)
    {

        currentHealth -= n; //decreases the variable's value by 10
    }
}
