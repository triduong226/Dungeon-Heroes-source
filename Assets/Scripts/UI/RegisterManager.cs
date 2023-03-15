using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour
{
    [SerializeField] private InputField txtUsername;
    [SerializeField] private InputField txtPassword;
    [SerializeField] private InputField txtConfirmPassword;

    [SerializeField] private GameObject messageModal;
    [SerializeField] private Button btnClose;
    [SerializeField] private Text message;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BtnRegister_OnClick()
    {
        if (txtPassword.text != txtConfirmPassword.text)
        {
            Debug.Log("Passwords do not match");
            return;
        }

        // get username and password
        var username = txtUsername.text;
        var password = txtPassword.text;
        
        
        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest()
        {
            Username = username,
            Password = password,
            DisplayName = username,
            RequireBothUsernameAndEmail = false
        };
        
        Debug.Log(txtUsername.text);
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }

    public void BtnLogin_OnClick()
    {
        SceneManager.LoadScene("LoginScene");
    }

    public void BtnClose_OnClick()
    {
        messageModal.SetActive(false);
    }

    private void OnRegisterFailure(PlayFabError obj)
    {
        message.text = obj.ErrorMessage;
        messageModal.SetActive(true);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult obj)
    {
        GameManager.ResetPlayerPrefs();
        PlayerPrefs.SetString("Username", txtUsername.text);
        PlayerPrefs.SetString("Password", txtPassword.text);
        InitNewPlayerData();
        message.text = obj.Username + "\n Welcome to Dungeon Heroes";
        messageModal.SetActive(true);
        var playerData = new PlayerData
        {
            PlayFabId = obj.PlayFabId,
            Username = obj.Username,
            Score = 0,
            Souls = 0,
            Characters = new List<string>()
            {
                "Knight"
            },
            Stage = new List<string>()
            {
                "Stage1"
            }
        };
        
        GameManager.PlayerData = playerData;
        btnClose.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainScene");
        });
    }

    private void InitNewPlayerData()
    {
        UpdateUserDataRequest request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {"Souls", "0"},
                {"Score", "0"},
                {"Characters", "Knight"},
                {"CurrentCharacter", "Knight"},
                {"Stage", "Stage1"}
            },
        };
        
        PlayFabClientAPI.UpdateUserData(request, result => Debug.Log("Successfully updated user data"),
            error => {
                Debug.Log("Got error setting user data Ancestor to Arthur");
                Debug.Log(error.GenerateErrorReport());
            });
    }
    public void SetDisplayName(String a)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = a
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, result => Debug.Log("Successfully updated display name") , error => {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });
    }
}