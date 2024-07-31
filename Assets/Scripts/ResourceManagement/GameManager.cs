using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor.TerrainTools;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; } = null;
    public bool IsPaused { get; private set; } = false;
    public bool isPowerOn { get; private set; }
    private float oldTimeScale;

    [SerializeField]
    private List<Quest> gameQuests = new List<Quest>();
    private bool allQuestsFinished;

    private HUDManager hud = null;
    public bool canPlayerMove { get; private set; }
    public bool isQuestRunning { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        hud = FindObjectOfType<HUDManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isPowerOn = false;
        canPlayerMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (allQuestsFinished == false)
        {
            CheckGameStatus();
        }
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }


    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0;
        HUDManager.instance.TogglePausePanel(true);
       
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1.0f;
        HUDManager.instance.TogglePausePanel(false);
    }

    public void ActivatePower()
    {
        isPowerOn = true;
        Debug.Log("Power on");
    }


    public void EnableMovement()
    {
        canPlayerMove = true;
    }

    public void DisableMovement() 
    {  
        canPlayerMove = false;
    }

    public void EndInteraction()
    {
        EnableMovement();
        HUDManager.instance.HideMouseCursor();
        HUDManager.instance.HideQuestPanel();
    }

    public void StartQuest()
    {
        isQuestRunning = true;
    }

    public void FinishQuest()
    {
        isQuestRunning = false;
    }

    public void CheckGameStatus()
    {
       allQuestsFinished = true;
        foreach(Quest q in gameQuests)
        {
        

        }
    }
}
