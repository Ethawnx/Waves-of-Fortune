using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Statue : MonoBehaviour, IEntractable
{
    public string Color;
    public Transform firePos;
    public float ActivatedTime;
    public float interactionDistance = 5f;

    public bool IsActivated { get; private set; }
    private GameObject player;
    private float TimeToDeactivate;

    [Header("Visuall Effects")]
    public GameObject ActivatedStatueVFX;

    public void ShowMessage(string message)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateUI()
    {
        UIManager.instance.UpdateActivatedStatueUI();
    }
    private void Start()
    {
        TimeToDeactivate = ActivatedTime;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        if (IsActivated)
        {
            TimeToDeactivate -= Time.deltaTime;
        }
        if (TimeToDeactivate <= 0f && IsActivated)
        {
            DeactivateStatue();
        }
    }
    public void OnMouseDown()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= interactionDistance)
        {
             if (InventoryManager.instance.NumberofGems > 0 && !IsActivated)
            {
                ActivateStatue();
                StartCoroutine(UIManager.instance.UpdateStatuUI(ActivatedTime, false));
                InventoryManager.instance.NumberofGems--;
                UIManager.instance.UpdateGems();
            }
            else if (IsActivated)
            {
                StartCoroutine(UIManager.instance.UpdateStatuUI(((int)TimeToDeactivate), false));
            }
            else
            {
                StartCoroutine(UIManager.instance.UpdateStatuUI(ActivatedTime, true));
            }
        }
    }

    private void ActivateStatue()
    {
        if (!IsActivated)
        {
            IsActivated = true;
            GameObject tempVFX = Instantiate(ActivatedStatueVFX, firePos.position, quaternion.identity);
            Destroy(tempVFX, ActivatedTime);
            GameManager.instance.activeStatueIndex++;
        }
    }
    private void DeactivateStatue()
    {
        IsActivated = false;
        GameManager.instance.activeStatueIndex--;
        TimeToDeactivate = ActivatedTime;
        GameManager.instance.ReactivateGemForStatue();
    }
}
