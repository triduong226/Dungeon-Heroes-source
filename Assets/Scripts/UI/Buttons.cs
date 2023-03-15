using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    [Header("Main Menu")]
    public GameObject mainMenu;
    [Header("Leaderboard")]
    public GameObject topPanel;
    public GameObject rowPrefab;
    public Transform rowsParent;
    
    [Header("Set Display Name")]
    public GameObject setDisplayNamePanel;
    public InputField displayNameInput;
    public void ToStartScreen()
    {
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;
    }

    public void ToGameScreen()
    {
        SceneManager.LoadScene("StageSelectScene");
    }

    public void ToSelectCharacterScreen()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ToLoginScreen()
    {
        SceneManager.LoadScene("LoginScene");
    }

    public void BtnExitOnclick()
    {
        SceneManager.LoadScene("LoginScene");
        PlayerPrefs.DeleteAll();
    }

    public void Pause()
    {
        PauseMenu.isPaused = true;
    }

    public void Resume()
    {
        PauseMenu.isPaused = false;
    }
    
    public void ShowLeaderboard()
    {
        SceneManager.LoadScene("LeaderboardScene");
    }

    public void BackMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void ShowDisplayName()
    {
        mainMenu.SetActive(false);
        setDisplayNamePanel.SetActive(true);
    }
    public void Exit()
    {
        topPanel.SetActive(false);
        setDisplayNamePanel.SetActive(false);
        mainMenu.SetActive(true);
    }
    
    void SelectCharacter()
    {
        //TODO
    }

    public void SetDisplayName()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = displayNameInput.text
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Successfully updated display name");
        Exit();
    }
    public void getLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "Score",
            StartPosition = 0,
            MaxResultsCount = 6
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }
    
    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            GameObject newGO = Instantiate(rowPrefab, rowsParent);
            Text[] texts = newGO.GetComponentsInChildren<Text>();
            texts[0].text = item.Position.ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();
        }
    }
    
}
