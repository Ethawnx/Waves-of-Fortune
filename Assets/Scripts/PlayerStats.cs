using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public int MaxHealth = 5;
    public float ImmunityTime = 3f;
    public float MashroomedTime = 15f;

    [Header("Visuall Effects")]
    //the effect that pops on the screen when we take mashroom
    public GameObject mashrooVFX;
    //the effect that pops on the screen when we take a hit from snake
    public GameObject HitVFX;

    [HideInInspector]
    public bool _ishitBySnake;
    [HideInInspector]
    public bool _isJustTookHeart;

    private float mashroomConsumedTime;

    private Animator _owlAnimator;

    public int HumanCurrentHealth { get; private set; }
    public bool IsImmune { get; private set; }
    public bool IsOnMashroom { get; private set; }

    void Start()
    {
        _owlAnimator = GameManager.instance.Owl.GetComponent<Animator>();
        HumanCurrentHealth = MaxHealth;
        UIManager.instance.UpdateHealth();
        mashroomConsumedTime = MashroomedTime;
        _ishitBySnake = false;
        _isJustTookHeart = false;
    }
    
    void Update()
    {
        if (HumanCurrentHealth <= 0f)
        {
            GameManager.instance.GameOver();
            HumanCurrentHealth = MaxHealth;
        }

        if (IsOnMashroom)
        {
            mashroomConsumedTime -= Time.deltaTime;
        }

        if (mashroomConsumedTime <= 0f && _ishitBySnake == false || mashroomConsumedTime <= 0f && _isJustTookHeart == false)
        {
            GameManager.instance.GameOver();
            mashrooVFX.SetActive(false);
            IsOnMashroom = false;
            mashroomConsumedTime = MashroomedTime;
        }
        if (IsOnMashroom && _ishitBySnake == true || _isJustTookHeart == true)
        {
            mashrooVFX.SetActive(false);
            IsOnMashroom = false;
            mashroomConsumedTime = MashroomedTime;
            _ishitBySnake = false;
            _isJustTookHeart = false;
        }
        
    }
    IEnumerator TemporaryImmune()
    {
        if (!GameManager.instance.isHuman)
        {
            _owlAnimator.SetLayerWeight(1, 1f);
        }
        IsImmune = true;

        yield return new WaitForSeconds(ImmunityTime);

        IsImmune = false;

        if (!GameManager.instance.isHuman)
        {
            _owlAnimator.SetLayerWeight(1, 0f);
        }
    }
    void MashroomEffect()
    {
        if (!IsImmune && !IsOnMashroom)
        {
            mashrooVFX.SetActive(true);
            IsOnMashroom = true;
            mashroomConsumedTime = MashroomedTime;
            if (mashroomConsumedTime <= 0f)
            {
                //isMashroomed = false;
                mashrooVFX.SetActive(false);
                mashroomConsumedTime = MashroomedTime;
            }

        }
    }
     public void TakeDamage(int amount)
    {
        if (!IsImmune)
        {
            AudioManager.instance.Play("TakeDamage");

            HumanCurrentHealth -= amount;
            StartCoroutine(TemporaryImmune());
            if (amount != 0f)
            {
                StartCoroutine(HitEffect());
            }
        }
    }
    private IEnumerator HitEffect()
    {
        HitVFX.SetActive (true);
        yield return new WaitForSeconds(ImmunityTime);
        HitVFX.SetActive (false);
    }
    public int GetCurrentHealth()
    {
        return HumanCurrentHealth;
    }
    public void AddHealth(int amount)
    {
        if(HumanCurrentHealth < MaxHealth)
        {
            HumanCurrentHealth += amount;
            UIManager.instance.UpdateHealth();
            if (HumanCurrentHealth >= MaxHealth) 
                HumanCurrentHealth = MaxHealth;
        }
    }
    public void SetHealth(int amount)
    {
        HumanCurrentHealth = amount;
        UIManager.instance.UpdateHealth();
    }

    public void TakeMashroom()
    {
        MashroomEffect();
    }
    public IEnumerator ResetIsHitState(float time)  
    {
        yield return new WaitForSeconds(time);
        _ishitBySnake = false;
    }
    public IEnumerator ResetIsHeartTakenState(float time)
    {
        yield return new WaitForSeconds(time);
        _isJustTookHeart = false;
    }
}
