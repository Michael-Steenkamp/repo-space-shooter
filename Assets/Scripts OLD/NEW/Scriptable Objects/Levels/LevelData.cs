using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Level Information")]
    public int Chapter;
    public int Level;

    [Header("Game Rules")]
    public int NumberOfEnemies;
    public bool BossFight;

    public DynamicEnemyLogic[] DynamicEnemies;
    //public StaticEnemyLogic[] StaticEnemies;
    //public BossLogic Boss;

    [Tooltip("Seconds")]
    public float TimeLimit;
    [Tooltip("Seconds")]
    public float StaticEnemySpawnRate;
}
