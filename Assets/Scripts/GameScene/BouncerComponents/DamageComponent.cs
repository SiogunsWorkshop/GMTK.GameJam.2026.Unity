using UnityEngine;

public class DamageComponent : MonoBehaviour
{
    [SerializeField] protected bool _allowFriendlyFire;
    [SerializeField] protected int _damageAmount = 1;

    protected bool CanBeDamaged(UniversalBouncer self, UniversalBouncer other)
    {
        bool sameTeam = self.Team == other.Team;
        return !sameTeam || _allowFriendlyFire;
    }
    protected bool CanBeDamaged(Team team, UniversalBouncer other)
    {
        bool sameTeam = team == other.Team;
        return !sameTeam || _allowFriendlyFire;
    }
}
