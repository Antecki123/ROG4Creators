using UnityEngine;

public interface IDamageable<T>
{
    public void TakeDamage(T damage);
}

public interface IDoor
{
    public void MovePlayer(GameObject player);
}