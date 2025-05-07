using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Door : MonoBehaviour
{

    public bool AllActivated { get; private set; }

    [Header("Requirements")]
    public List<string> RequiredStatueColors;

    [Header("Visual Feedback")]
    public ParticleSystem activationParticles;
    public Animator DoorAnimator;
    public GameObject[] colorIndicators; // Should match RequiredStatueColors count

    private Dictionary<string, bool> _activatedColors = new Dictionary<string, bool>();
    private static readonly int OpenCloseHash = Animator.StringToHash("OpenClose");

    void Start()
    {
        if (DoorAnimator == null)
            DoorAnimator = GetComponent<Animator>();

        InitializeColorTracking();
        InitializeIndicators();

        Statue.OnActivated += OnStatueActivated;
        Statue.OnDeactivated += OnStatueDeactivated;
    }

    void OnDestroy()
    {
        Statue.OnActivated -= OnStatueActivated;
        Statue.OnDeactivated -= OnStatueDeactivated;
    }

    void InitializeColorTracking()
    {
        foreach (string color in RequiredStatueColors)
        {
            _activatedColors[color] = false;
        }
    }

    void InitializeIndicators()
    {
        // Safety check for indicator count
        if (colorIndicators.Length != RequiredStatueColors.Count)
        {
            Debug.LogError("Color indicators count doesn't match required colors!");
            return;
        }

        // Start with all indicators disabled
        foreach (GameObject indicator in colorIndicators)
        {
            if (indicator != null) indicator.SetActive(false);
        }
    }

    void OnStatueActivated(Statue statue)
    {
        if (!RequiredStatueColors.Contains(statue.Color)) return;

        _activatedColors[statue.Color] = true;
        UpdateIndicator(statue.Color, true);
        CheckDoorState();
    }

    void OnStatueDeactivated(Statue statue)
    {
        if (!RequiredStatueColors.Contains(statue.Color)) return;

        _activatedColors[statue.Color] = false;
        UpdateIndicator(statue.Color, false);
        CheckDoorState();
    }

    void UpdateIndicator(string color, bool active)
    {
        int index = RequiredStatueColors.IndexOf(color);
        if (index >= 0 && index < colorIndicators.Length)
        {
            // Use Safe indicator access
            if (colorIndicators[index] != null)
            {
                colorIndicators[index].gameObject.SetActive(active);
                if (active && activationParticles != null)
                {
                    activationParticles.Play();
                }
            }
        }
    }

    void CheckDoorState()
    {
        bool newState = _activatedColors.Values.All(active => active);

        if (newState != AllActivated)
        {
            AllActivated = newState;
            UpdateDoorAnimation();
        }
    }

    public void UpdateDoorAnimation()
    {
        if (DoorAnimator != null)
        {
            DoorAnimator.SetBool(OpenCloseHash, AllActivated);
        }
    }
}