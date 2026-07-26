using UnityEngine;

public class ArenaSpawnable : MonoBehaviour
{
    public int Cost => _cost;
    [SerializeField] private int _cost;
}
