using UnityEngine;

public class ArenaSpawner : MonoBehaviour
{
    public float Width => _width;
    public float Height => _height;
    [SerializeField] private float _width = 1;
    [SerializeField] private float _height = 1;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireCube(gameObject.transform.position, new Vector3(_width, _height, 0.5f));
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(gameObject.transform.position, new Vector3(_width + 1, _height + 1, 0.5f));
    }
#endif
}
