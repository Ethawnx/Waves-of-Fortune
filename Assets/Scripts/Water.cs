using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            GameManager.instance.GameOver();
        }
        else if(collider.CompareTag("Snake"))
        {
            Destroy(collider.gameObject);
        }
    }
}
