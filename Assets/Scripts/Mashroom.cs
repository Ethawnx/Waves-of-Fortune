using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mashroom : MonoBehaviour
{
    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();
    }
    void OnTriggerEnter(Collider other) 
    {
        // Code to execute when an object enters the trigger
        if (other.CompareTag("Player") && playerStats.IsOnMashroom == false && playerStats.IsImmune == false && playerStats._ishitBySnake == false)
        {
            AudioManager.instance.Play("Mashroom");
            gameObject.SetActive(false);
            playerStats.TakeMashroom();
        }
    }
}
