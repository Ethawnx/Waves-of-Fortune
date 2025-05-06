using UnityEngine;
using UnityEngine.UI;

public class SliderDecreaser : MonoBehaviour
{
    Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
    }
    private void OnEnable()
    {
        if (slider != null)
            slider.value = slider.maxValue;
    }
    private void OnDisable()
    {
        if (slider != null)
            slider.value = slider.maxValue;
    }
    // Update is called once per frame
    void Update()
    {
        if (isActiveAndEnabled)
        {
            slider.value -= Time.deltaTime;
        }
    }
}
