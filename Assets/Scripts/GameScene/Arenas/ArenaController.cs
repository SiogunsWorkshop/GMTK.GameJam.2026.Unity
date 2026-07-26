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

    //My dum dum idea na inny spawn system.
    //Na arenie s¹ spawnery, oko³o 15.
    //Zielone to gdzie obiekt mo¿e siê spawniæ
    //Czerwone to gdzie granice obiektu mog¹ siêgn¹æ (nie dok³adne bo twins jest very fat)
    //W ka¿dej rundzie gracz jest teleportowany na dó³ mapy gdzie jest wolne miejsce
    //Time stop dopóki gracz siê nie ruszy, ma chwile ¿eby siê zastanosiæ
    //Spawnery s¹ zarówno dla cargo i hazzardów (arena spawnable)
    //Spawnable maj¹ koszt, wszystkie s¹ na 1, a twins na 2 bo s¹ silne.
    //Cargo nie ma (assume 1)
    //Co rerroll spawnione s¹ randomowe rzeczy z listy tak aby nie wyjœæ poza koszt
    //Bud¿et cargo zwiêksza siê, kiedy _cargoDelay jest równy _cargoAddDelay. _cargoDelay zwiêkszany na reroll.
    //(Tak ¿eby by³a wariacja spawnowania shitu)
    //(Nie myœla³em o ratio hazzardów do cargo jak bêdzie ju¿ za ma³o spawnerów)
    //(Mo¿na te¿ dodaæ jakieœ max wartoœci bud¿etów i guess)
    [SerializeField] private List<ArenaSpawner> _arenaSpawners = new();
    [SerializeField] private List<ArenaSpawnable> _hazzards = new();
    [SerializeField] private GameObject _cargoPrefab;
    private float _hazzardBudget = 1;
    private float _budgetIncrese = 0.65f;
    private float _cargoBudget = 1;
    private float _cargoIncrease = 1.5f;
    private int _cargoDelay = 0;
    private int _cargoAddDelay = 1;

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
