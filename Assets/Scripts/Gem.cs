using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gem : MonoBehaviour,IInventoryItem
{
    public Sprite Icon => throw new System.NotImplementedException();

    string IInventoryItem.Name => throw new System.NotImplementedException();

    public void OnPickUp()
    {
        InventoryManager.instance.NumberofGems++;
        UIManager.instance.UpdateGems();
    }
    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            AudioManager.instance.Play("Gem");
            OnPickUp();
            gameObject.SetActive(false);
            GameManager.instance.deactivatedGems.Add(this.gameObject);
        }
    }

}
