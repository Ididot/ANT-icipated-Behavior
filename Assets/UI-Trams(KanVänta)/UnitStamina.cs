using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStamina 
{
    // Fields
    float _currentStamina;
    float _currentMaxStamina;
    float _staminaRegenSpeed;
    bool _pauseStaminaRegen = false;

    // Properties
    public float Stamina
    {
        get
        {
            return _currentStamina;
        }
        set
        {
            _currentStamina = value;
        }
    }
    public float MaxStamina
    {
        get
        {
            return _currentMaxStamina;
        }
        set
        {
            _currentMaxStamina = value;
        }
    }
    public float StaminaRegenSpeed
    {
        get
        {
            return _staminaRegenSpeed;
        }
        set
        {
            _staminaRegenSpeed = value;
        }
    }
    public bool pauseStaminaRegen
    {
        get
        {
            return pauseStaminaRegen;
        }
        set
        {
            pauseStaminaRegen = value;
        }
    }

    public UnitStamina(float stamina, float maxStamina, float staminaRegenSpeed, bool pauseStaminaRegen)
    {
        _currentStamina= stamina;
        _currentMaxStamina= maxStamina;
        _staminaRegenSpeed = staminaRegenSpeed;
        _pauseStaminaRegen = pauseStaminaRegen;
    }

    public void UseStamina(float staminaAmount)
    {
        if (_currentStamina > 0)
        {
            _currentStamina -= staminaAmount * Time.deltaTime;
        }
    }

    public void RegenStamina()
    {
        if (_currentStamina < _currentMaxStamina && !_pauseStaminaRegen)
        {
            _currentStamina += _staminaRegenSpeed * Time.deltaTime;
        }
    }
    

    


    
}
