using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public List<Level> levels = new List<Level>();
    public PlayerController player;
    public AIController ai;
    Level currentLevel;

    int level = 1;

    void Start()
    {
        UIManager.Instance.OpenMainMenuUI();
        LoadLevel();
    }

    public void LoadLevel()
    {
        LoadLevel(level);
        OnInit();
    }
    public void LoadLevel(int index)
    {
        if(currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }
        currentLevel = Instantiate(levels[index-1]);
    }

    public void OnInit()
    {
        player.transform.position = currentLevel.startPointPlayer;
        ai.transform.position = currentLevel.startPointAI;
    }

    public void OnStart()
    {
        GameManager.Instance.ChangeState(GameState.GamePlay);
    }

    public void OnFinish()
    {
        UIManager.Instance.OpenFinishUI();
        GameManager.Instance.ChangeState(GameState.Finish);
    }

    public void NextLevel()
    {
        if(level < 3)
        {
            level++;
        }

        else
        {
            level = 1;
        }
        LoadLevel();
    }
}
