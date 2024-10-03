using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Staminabar : MonoBehaviour
{

    Slider _staminaSlider;

    // Start is called before the first frame update
    void Start()
    {
        _staminaSlider = GetComponent<Slider>();
        
    }

    public void SetMaxStamina(float maxStamina)
    {
        _staminaSlider.maxValue= maxStamina;
        _staminaSlider.value = maxStamina;
    }

    public void SetStamina(int stamina)
    {
        _staminaSlider.value= stamina;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
