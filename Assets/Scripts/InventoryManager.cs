using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public int NumberofGems;
    public int NumberOfGolds;

    private List<IInventoryItem> items = new List<IInventoryItem>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void AddItem(IInventoryItem item)
    {
        items.Add(item);
        item.OnPickUp();
        // Update UI, etc.
    }

    public void Calculate()
    {
        if (NumberOfGolds == 10)
        {
            NumberofGems++;    
            NumberOfGolds = 0;
            UIManager.instance.UpdateGold();
            UIManager.instance.UpdateGems();
        }
    }
    // Other inventory methods...
    void Update ()
    {
    }
}
