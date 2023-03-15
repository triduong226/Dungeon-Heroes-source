using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using Model;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private InputField txtUsername;
    [SerializeField] private InputField txtPassword;
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private Text txtMessage;
    [SerializeField] private Button btnClose;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void BtnLogin_OnClick()
    {
        var username = txtUsername.text;
        var password = txtPassword.text;
        
        var request = new LoginWithPlayFabRequest
        {
            Username = username,
            Password = password
        };
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
    }
    
    public void BtnRegister_OnClick()
    {
        SceneManager.LoadScene("RegisterScene");
    }

    private void OnLoginFailure(PlayFabError obj)
    {
        txtMessage.text = obj.ErrorMessage;
        messagePanel.SetActive(true);
    }
    
    public void BtnClose_OnClick()
    {
        messagePanel.SetActive(false);
    }

    private void OnLoginSuccess(LoginResult obj)
    {
        GameManager.ResetPlayerPrefs();
        PlayerPrefs.SetString("Username", txtUsername.text);
        PlayerPrefs.SetString("Password", txtPassword.text);
        var request = new GetUserDataRequest();
        
        PlayFabClientAPI.GetUserData(request, result =>
        {
            var data = new PlayerData
            {
                PlayFabId = obj.PlayFabId,
                Characters = result.Data["Characters"].Value.Split(',').ToList(),
                Souls = int.Parse(result.Data["Souls"].Value),
                Score = int.Parse(result.Data["Score"].Value),
                CurrentCharacter = result.Data["CurrentCharacter"].Value,
                Stage = result.Data["Stage"].Value.Split(',').ToList()
            };
            Debug.Log(data.CurrentCharacter);
            GameManager.PlayerData = data;
            GameManager.selectedCharacterData =
                GameManager.CharacterDataList.FindByCharacterTypeString(data.CurrentCharacter);
            SceneManager.LoadScene("MainScene");
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }
}
