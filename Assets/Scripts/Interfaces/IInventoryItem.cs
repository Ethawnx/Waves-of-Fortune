using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryItem
{
    string Name { get; }
    void OnPickUp();
}
