using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    public virtual float damage { get; set; }
    public float GetDamage() { return damage; }
}
