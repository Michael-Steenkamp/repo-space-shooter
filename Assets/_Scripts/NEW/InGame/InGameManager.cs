using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;

public class InGameManager : MonoBehaviour
{
    [Header("Utility")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject globalVolume;
    [SerializeField] private GameObject globalLight2D;

    [Header("Enviroment")]
    [SerializeField] private new Camera camera;
    [SerializeField] private CinemachineCamera cinemachine;
    [SerializeField] private GameObject background;

    [Header("InGame")]
    [SerializeField] private InGameCanvasLogic inGame;

    [Header("Overlays")]
    [SerializeField] private SettingsManager settings;
    [SerializeField] private DeathScreenLogic deathScreen;
    [SerializeField] private CompleteScreenLogic completeScreen;
    [SerializeField] private PauseMenuLogic pauseMenu;

    [Header("Game Data")]
    [SerializeField] private PlayerController spaceship;
    [SerializeField] private LevelData levelData;
    [SerializeField] private DynamicEnemyLogic[] dynamicEnemies;

    [SerializeField] private int numEnemies;

    [Header("States")]
    [SerializeField] private bool isPaused;
    [SerializeField] private bool isSettingsActive;
    [SerializeField] private bool isPlayerAlive;

    //TEMP
    public Systems systems;


    private void OnDestroy()
    {
        PauseMenuLogic.OnResumeGame -= ResumeGame;
    }

    private void Awake()
    {
        PauseMenuLogic.OnResumeGame += ResumeGame;

        spaceship = GameDataSystem.spaceship;
        levelData = GameDataSystem.Instance.GetLevelData();
        dynamicEnemies = levelData.DynamicEnemies;

        /* Instantiate */
        // Utility
        eventSystem = Instantiate(eventSystem);
        globalLight2D = Instantiate(globalLight2D);
        globalVolume = Instantiate(globalVolume);

        // Enviroment
        camera = Instantiate(camera);
        cinemachine = Instantiate(cinemachine);
        background = Instantiate(background);
        
        // InGame
        inGame = Instantiate(inGame);



        // Overlays
        //settings = Instantiate(settings);
        //deathScreen = Instantiate(deathScreen);
        //completeScreen = Instantiate(completeScreen);
        //pauseMenu = Instantiate(pauseMenu);



        // TEMP
        //Instantiate(systems).Initialize();


        isPaused = false;
        isSettingsActive = false;
        isPlayerAlive = true;
    }

    private void Start()
    {
        // Player
        spaceship = Instantiate(spaceship);
        spaceship.Initialize(playerInput);
        cinemachine.Follow = spaceship.transform;

        string levelName = $"Chapter {GameDataSystem.currentChapter} - Level {GameDataSystem.currentLevel}";
        Debug.Log($"Level Data: {levelName}, Enemies: {levelData.NumberOfEnemies}, Time Limit: {levelData.TimeLimit}");
        numEnemies = levelData.NumberOfEnemies;
        float timer = levelData.TimeLimit;
        inGame.Initialize(levelName, numEnemies, timer);

        DynamicEnemyLogic enemyToSpawn = dynamicEnemies[0];
        enemyToSpawn.SetTarget(spaceship.gameObject);
        Vector3 spawnPos = new Vector3(spaceship.transform.position.x, spaceship.transform.position.y + 10, spaceship.transform.position.z);
        Instantiate(enemyToSpawn, spawnPos, transform.rotation);

    }

    private void OnSettingsClosedHandler()
    {
        isSettingsActive = false;
        settings.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(true);
    }

    private void OnOpenSettingsHandler()
    {
        LogSystem.Instance.Log("Opening Settings", LogType.Info, "InGameManager");
        isSettingsActive = true;
        pauseMenu.gameObject.SetActive(false);
        settings.gameObject.SetActive(true);
    }

    public void OnPauseHandler(InputAction.CallbackContext context)
    {

        //if (context.performed && isPlayerAlive && !isSettingsActive)
        //{
        //    if (isPaused)
        //    {
        //        ResumeGame();
        //    }
        //    else
        //    {
        //        PauseGame();
        //    }
        //}
    }

    private void PauseGame()
    {
        LogSystem.Instance.Log("Game Paused", LogType.Info, "GameManager");
        Time.timeScale = 0f;
        pauseMenu.gameObject.SetActive(true);
        isPaused = !isPaused;
    }

    private void ResumeGame()
    {
        LogSystem.Instance.Log("Game Resumed", LogType.Info, "GameManager");
        Time.timeScale = 1f;
        pauseMenu.gameObject.SetActive(false);
        isPaused = !isPaused;
    }
}
