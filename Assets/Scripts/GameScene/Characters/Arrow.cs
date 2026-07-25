using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : DamageComponent
{
    [SerializeField] private float _speed = 5.5f;
    private Rigidbody2D _rigidBody;
    private Team _team;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        GetComponent<CapsuleCollider2D>().isTrigger = true;
    }
    public void Spawn(Team team, Vector2 direction)
    {
        _team = team;

        gameObject.transform.rotation = Quaternion.FromToRotation(Vector2.up, direction);
        _rigidBody.linearVelocity = direction.normalized * _speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 25)
            Destroy(gameObject);

        if (!collision.gameObject.TryGetComponent(out HealthComponent otherHealth))
            return;
        var otherBouncer = otherHealth.GetComponent<UniversalBouncer>();
        if (!CanBeDamaged(_team, otherBouncer))
            return;

        otherHealth.TakeDamage(_damageAmount);
        Destroy(gameObject);
    }
}
