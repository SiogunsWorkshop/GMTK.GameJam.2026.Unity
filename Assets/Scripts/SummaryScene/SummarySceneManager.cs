using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class SummarySceneManager : MonoBehaviour
{
    [Inject] private readonly LoadingWindow _loadingWindow;

    [SerializeField] private GameObject _cargoSpherePrefab;
    [SerializeField] private Transform _cargoSphereSpawnPoint;
    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _delayBetweenSpawns = 0.3f;

    [Inject] private readonly PlaythroughSummarySnapshotService _playthroughSummarySnapshotService;

    private void Awake()
    {
        _loadingWindow.Hide();

        var cargoCount = _playthroughSummarySnapshotService.ReadSnapshot().Score;

#if UNITY_EDITOR
        if (cargoCount <= 0)
        {
            cargoCount = 10;
        }
#endif

        HandleCargoSpawnAsync(cargoCount).Forget();
    }

    private async UniTaskVoid HandleCargoSpawnAsync(int cargoCount)
    {
        for (int i = 0; i < cargoCount; i++)
        {
            var cargo = SpawnCargoSphere();
            await UniTask.Delay(System.TimeSpan.FromSeconds(_delayBetweenSpawns));

            // apply small force
            cargo.AddForce(Random.insideUnitSphere * 2f, ForceMode.Impulse);
        }
    }

    private Rigidbody SpawnCargoSphere()
    {
        var go = Instantiate(_cargoSpherePrefab, GetRandomPositionInSphere(_cargoSphereSpawnPoint.position, _radius), Quaternion.identity, _cargoSphereSpawnPoint);
        return go.GetComponent<Rigidbody>();

        static Vector3 GetRandomPositionInSphere(Vector3 center, float radius)
        {
            Vector3 randomDirection = Random.insideUnitSphere.normalized;
            float randomDistance = Random.Range(0f, radius);
            return center + randomDirection * randomDistance;
        }
    }
}
