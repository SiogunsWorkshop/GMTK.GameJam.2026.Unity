using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour
{
    [SerializeField, Required] private HealthComponent _playerHealth;

    [SerializeField] private List<GameObject> _healthFills = new();

    private void OnEnable()
    {
        _playerHealth.OnHealthChanged.AddListener(UpdateHealthDisplay);
    }

    private void OnDisable()
    {
        _playerHealth.OnHealthChanged.RemoveListener(UpdateHealthDisplay);
    }

    private void UpdateHealthDisplay(int arg0)
    {
        for (int i = 0; i < _healthFills.Count; i++)
        {
            _healthFills[i].SetActive(i < _playerHealth.CurrentHealth);
        }
    }
}
