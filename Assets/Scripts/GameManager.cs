using System.Collections.Generic;
using System.Linq;
using Model;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static CharacterData selectedCharacterData;
    public static PlayerData PlayerData;
    [SerializeField] private CharacterDataList characterDataList;
    
    public static CharacterDataList CharacterDataList => Instance.characterDataList;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnLoginFailure(PlayFabError obj)
    {
        Debug.LogError(obj.GenerateErrorReport());
    }

    private void OnLoginSuccess(LoginResult obj)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
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
            SceneManager.LoadScene("MainScene");
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }
    
    public static void UpdatePlayFabData()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Characters", string.Join(",", PlayerData.Characters)},
                {"Souls", PlayerData.Souls.ToString()},
                {"Score", PlayerData.Score.ToString()},
                {"CurrentCharacter", PlayerData.CurrentCharacter},
                {"Stage", string.Join(",", PlayerData.Stage)}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, result => { Debug.Log("Successfully updated user data"); },
            error => { Debug.LogError(error.GenerateErrorReport()); });
    }
    public static void UpdateSoulsData(string souls)
    {
        GameManager.PlayerData.Souls = int.Parse(souls);
    }
    public static void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
    
    public void BtnStart_OnClick()
    {
        if (PlayerPrefs.HasKey("Username"))
        {
            Debug.Log(PlayerPrefs.GetString("Username"));
            Debug.Log(PlayerPrefs.GetString("Password"));
            
            var username = PlayerPrefs.GetString("Username");
            var password = PlayerPrefs.GetString("Password");
            var request = new LoginWithPlayFabRequest
            {
                Username = username,
                Password = password
            };
            PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
        }
        else
        {
            SceneManager.LoadScene("LoginScene");
        }
    }
    
    public void BtnLogin_OnClick()
    {
        SceneManager.LoadScene("LoginScene");
    }
}
