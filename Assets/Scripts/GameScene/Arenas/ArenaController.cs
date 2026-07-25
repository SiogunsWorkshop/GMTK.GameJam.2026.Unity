using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public List<EnvironmentalHazard> _environmentalHazards;
    private readonly HashSet<EnvironmentalHazard> _activeHazards = new();

    [SerializeField] private EnvironmentalHazard _tutorialHazard;

    private void Awake()
    {
        foreach (var hazard in _environmentalHazards)
        {
            hazard.gameObject.SetActive(false);
        }
        _tutorialHazard.gameObject.SetActive(true);
    }

    public void RerollEnvironmentalHazards(int hazardCount)
    {
        _tutorialHazard.gameObject.SetActive(false);

        if (hazardCount <= 0 || _environmentalHazards.Count == 0)
        {
            foreach (var hazard in _activeHazards)
            {
                hazard.gameObject.SetActive(false);
            }
            _activeHazards.Clear();
            return;
        }

        var staleHazards = _environmentalHazards.FindAll(hazard => hazard != _tutorialHazard);
        var newHazards = new HashSet<EnvironmentalHazard>();

        for (int i = 0; i < hazardCount; i++)
        {
            int randomIndex = Random.Range(0, _environmentalHazards.Count);
            var selectedHazard = _environmentalHazards[randomIndex];
            newHazards.Add(selectedHazard);
        }

        foreach (var hazard in staleHazards)
        {
            if (!newHazards.Contains(hazard))
            {
                hazard.gameObject.SetActive(false);
                _activeHazards.Remove(hazard);
            }
        }

        foreach (var hazard in newHazards)
        {
            if (!_activeHazards.Contains(hazard))
            {
                hazard.gameObject.SetActive(true);
                _activeHazards.Add(hazard);
            }
        }
    }
}
