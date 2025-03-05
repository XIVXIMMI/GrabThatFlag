using UnityEngine;

public class LevelComponent : MonoBehaviour, IDamageable
{
    public virtual void Damage(PlayerMotor player)
    {
        if (player != null)
        {
            player.GetComponent<Health>().KillPlayer();
        }
    }
}
