using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _healthIconPrefab;
    [SerializeField] private RectTransform _healthIconContainer;
    [SerializeField, Required] private HealthComponent _playerHealth;

    private readonly List<GameObject> _healthIcons = new();

    private void OnEnable()
    {
        _playerHealth.OnHealthChanged.AddListener(UpdateHealthDisplay);
    }

    private void OnDisable()
    {
        _playerHealth.OnHealthChanged.RemoveListener(UpdateHealthDisplay);
    }

    private void Awake()
    {
        RemoveChildren();
        for (int i = 0; i < _playerHealth.MaxHealth; i++)
        {
            AddHealthIcon();
        }

        void RemoveChildren()
        {
            for (int i = _healthIconContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(_healthIconContainer.GetChild(i).gameObject);
            }
        }
        void AddHealthIcon()
        {
            GameObject healthIcon = Instantiate(_healthIconPrefab, _healthIconContainer);
            _healthIcons.Add(healthIcon);
        }
    }

    private void UpdateHealthDisplay(int arg0)
    {
        for (int i = 0; i < _healthIcons.Count; i++)
        {
            _healthIcons[i].SetActive(i < _playerHealth.CurrentHealth);
        }
    }
}
