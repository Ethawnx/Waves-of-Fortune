using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    //the amount of heal a Heart can give
    public int HealAmount = 1;
    //the time that need to pass so we can take another heart
    public float TakingHeartResetTimer = 3f;
    private PlayerStats playerStats;
    public GameObject heartVFX;

    void Start()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Code to execute when an object enters the trigger
        if (other.CompareTag("Owl") || other.CompareTag("Player") && playerStats.GetCurrentHealth() != playerStats.MaxHealth || playerStats.IsOnMashroom)
        {
            playerStats._isJustTookHeart = true;
            StartCoroutine(playerStats.ResetIsHeartTakenState(TakingHeartResetTimer));
            AudioManager.instance.Play("Heart");
            gameObject.SetActive(false);
            GameObject tempObj = Instantiate(heartVFX, transform.position, Quaternion.identity);
            Destroy(tempObj, 2f);
            playerStats.AddHealth(HealAmount);
            UIManager.instance.UpdateHealth();
        }
    }
}
