using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor.TerrainTools;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; } = null;
    public bool IsPaused { get; private set; } = false;
    public bool IsUsingComputer { get; private set; } = false;
    public bool isPowerOn { get; private set; }

    [SerializeField]
    private List<Quest> gameQuests;
    private bool allQuestsFinished;

    private HUDManager hud = null;
    public bool canPlayerMove { get; private set; }
    public bool isQuestRunning { get; private set; }

    public Quest CurrentQuest;



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

        foreach (QuestObject qObject in QuestManager.instance.quests) 
        {
            gameQuests.Add(qObject.objectQuest);
        }
    }

    // Update is called once per frame
    void Update()
    {
            CheckGameStatus();
    }

    public void LoadLevel(int levelToLoad)
    {
        MainMenuUIManager.instance.LoadLevel();
        StartCoroutine(LoadLevelAsync(levelToLoad));
    }

    IEnumerator LoadLevelAsync(int sceneIndex)
    {
        AsyncOperation levelLoadOperation = SceneManager.LoadSceneAsync(sceneIndex);
         while (!levelLoadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(levelLoadOperation.progress / 0.9f);
            MainMenuUIManager.instance.loadingSlider.value = progressValue;
            yield return null;
        }
         if (sceneIndex != 0)
        {
            MainMenuUIManager.instance.HideLoadingScreen();
        }
         else
        {
            MainMenuUIManager.instance.ShowMainMenu();
        }
    }

    public void ToggleComputerUse(bool value)
    {
        if (value == true)
        {
            IsUsingComputer = true;
        }
        else
        {
            IsUsingComputer = false;

        }
    }



    public void PauseGame()
    {
        Time.timeScale = 0;
        IsPaused = true;
        HUDManager.instance.ShowPauseCanvas(IsPaused);   
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        IsPaused = false;
        HUDManager.instance.ReturnToMainPauseMenu();
        HUDManager.instance.ShowPauseCanvas(IsPaused);
        if (IsUsingComputer)
        {
            HUDManager.instance.ShowMouseCursor();
        }
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
        EndInteraction();
    }

    public void FinishQuest()
    {
        isQuestRunning = false;
        CurrentQuest = null;
        HUDManager.instance.ResetPauseModeQuestInfo();

    }

    public void CheckGameStatus()
    {
        allQuestsFinished = true;
        foreach (Quest q in gameQuests)
        {
            if (q.progress != Quest.QuestStatus.FINISHED)
            {
                allQuestsFinished = false;
                Debug.Log("There are quests to be finished");
                return;
            }
            else
            {
                Debug.Log("All quests finished");
            }
        }
    }

    public void QuitToSystem()
    {
        Application.Quit();
    }
}
