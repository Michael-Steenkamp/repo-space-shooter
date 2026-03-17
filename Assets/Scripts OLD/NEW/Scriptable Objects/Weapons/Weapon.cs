using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Components")]
    public Sprite Sprite;

    [Header("Properties")]
    public float Damage;
    public float Speed;
    public float Lifetime;

    [Header("SFX")]
    public AudioClip ACLP_Fire;
}
