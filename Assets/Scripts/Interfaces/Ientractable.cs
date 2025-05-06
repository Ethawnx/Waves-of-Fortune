using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntractable
{
    void ShowMessage(string message);
    void UpdateUI();
    void OnMouseDown();
}