using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArenaController : MonoBehaviour
{
    [field: SerializeField] public UnityEvent OnArenaCleared { get; private set; } = new();

    public List<EnvironmentalHazard> _environmentalHazards;
    public int CargoCount { get; set; }
    private readonly HashSet<EnvironmentalHazard> _activeHazards = new();

    [SerializeField] private EnvironmentalHazard _tutorialHazard;

    private int HazardCount => Mathf.Min(_rerollCount / _increaseHazardCountEveryNthReroll + 1, MAX_HAZARD_COUNT);
    private readonly int _increaseHazardCountEveryNthReroll = 3;
    private int _rerollCount = 0;

    private const int MAX_HAZARD_COUNT = 5;

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

        OnArenaCleared.Invoke();

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

    //My dum dum idea na inny spawn system.
    //Na arenie s� spawnery, oko�o 15.
    //Zielone to gdzie obiekt mo�e si� spawni�
    //Czerwone to gdzie granice obiektu mog� si�gn�� (nie dok�adne bo twins jest very fat)
    //W ka�dej rundzie gracz jest teleportowany na d� mapy gdzie jest wolne miejsce
    //Time stop dop�ki gracz si� nie ruszy, ma chwile �eby si� zastanosi�
    //Spawnery s� zar�wno dla cargo i hazzard�w (arena spawnable)
    //Spawnable maj� koszt, wszystkie s� na 1, a twins na 2 bo s� silne.
    //Cargo nie ma (assume 1)
    //Co rerroll spawnione s� randomowe rzeczy z listy tak aby nie wyj�� poza koszt
    //Bud�et cargo zwi�ksza si�, kiedy _cargoDelay jest r�wny _cargoAddDelay. _cargoDelay zwi�kszany na reroll.
    //(Tak �eby by�a wariacja spawnowania shitu)
    //(Nie my�la�em o ratio hazzard�w do cargo jak b�dzie ju� za ma�o spawner�w)
    //(Mo�na te� doda� jakie� max warto�ci bud�et�w i guess)
    [SerializeField] private List<ArenaSpawner> _arenaSpawners = new();
    [SerializeField] private List<ArenaSpawnable> _hazzards = new();
    [SerializeField] private GameObject _cargoPrefab;
#pragma warning disable CS0414 // Dodaj modyfikator tylko do odczytu
    private float _hazzardBudget = 1;
    private float _budgetIncrese = 0.65f;
    private float _cargoBudget = 1;
    private float _cargoIncrease = 1.5f;
    private int _cargoDelay = 0;
    private int _cargoAddDelay = 1;
#pragma warning restore CS0414 // Dodaj modyfikator tylko do odczytu

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
