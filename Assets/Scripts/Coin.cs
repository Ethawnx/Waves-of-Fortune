using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
     void OnTriggerEnter(Collider other) 
    {
    // Code to execute when an object enters the trigger
        if (other.CompareTag("Owl"))
        {
            AudioManager.instance.Play("Coin");
            gameObject.SetActive(false);
            InventoryManager.instance.NumberOfGolds++;
            InventoryManager.instance.Calculate();
            UIManager.instance.UpdateGold();
        }
    }
}
