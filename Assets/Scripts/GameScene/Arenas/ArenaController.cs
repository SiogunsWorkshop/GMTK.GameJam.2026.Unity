using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public List<EnvironmentalHazard> _environmentalHazards;
    public int CargoCount { get; set; }
    private readonly HashSet<EnvironmentalHazard> _activeHazards = new();

    [SerializeField] private EnvironmentalHazard _tutorialHazard;

    private int HazardCount => _rerollCount / _increaseHazardCountEveryNthReroll + 1;
    private readonly int _increaseHazardCountEveryNthReroll = 2;
    private int _rerollCount = 0;

    private void Awake()
    {
        foreach (var hazard in _environmentalHazards)
        {
            hazard.gameObject.SetActive(false);
        }
        _tutorialHazard.gameObject.SetActive(true);
    }

    public void RerollEnvironmentalHazards()
    {
        _tutorialHazard.gameObject.SetActive(false);

        _rerollCount++;

        if (HazardCount <= 0 || _environmentalHazards.Count == 0)
        {
            foreach (var hazard in _activeHazards)
            {
                hazard.gameObject.SetActive(false);
            }
            _activeHazards.Clear();
            return;
        }

        foreach (var hazard in _activeHazards)
        {
            hazard.gameObject.SetActive(false);
        }

        _activeHazards.Clear();
        for (int i = 0; i < HazardCount; i++)
        {
            var hazard = _environmentalHazards[Random.Range(0, _environmentalHazards.Count)];
            hazard.gameObject.SetActive(true);
            _activeHazards.Add(hazard);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Collect Environmental Hazards From Children")]
    private void CollectEnvironmentalHazardsFromChildren()
    {
        _environmentalHazards.Clear();
        foreach (var hazard in GetComponentsInChildren<EnvironmentalHazard>(includeInactive: true))
        {
            if (hazard == _tutorialHazard) continue;
            _environmentalHazards.Add(hazard);
        }

        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}
