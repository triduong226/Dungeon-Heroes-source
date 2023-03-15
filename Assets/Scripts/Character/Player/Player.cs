using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Events;
using UnityEngine.SceneManagement;

public class Player : Character
{
    [SerializeField] Slider hpSlider;
    [SerializeField] ParticleSystem bleeding;
    [Header("Game Over")]
    [SerializeField] GameObject GameOverWindow;
    [SerializeField] Text soulText, nameText;
    
    static Player instance;
    float attackSpeed;
    float expAdditional;
    int luck;
    bool isColliding;
    private int maxScore;
    private int currentSouls;
    
    private Player() { }

    void Awake()
    {
        Initialize();
    }

    protected override void Initialize()
    {
        characterData = GameManager.selectedCharacterData;
        base.Initialize();
        GameOverWindow.SetActive(false);
        instance = this;
        attackSpeed = 100f;
        expAdditional = 100f;
        luck = 0;
        hpSlider.maxValue = GetHealthPoint();
        hpSlider.value = GetHealthPoint();
        isColliding = false;
        GetCharacterData();
        GetFirstWeapon();
    }

    
    public static Player GetInstance()
    {
        return instance;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public float GetExpAdditional()
    {
        return expAdditional;
    }

    public int GetLuck()
    {
        return luck;
    }

    public void DecreaseAttackSpeed(float value)
    {
        attackSpeed -= value;
    }

    public void IncreaseExpAdditional(float value)
    {
        expAdditional += value;
    }

    public void IncreaseLuck(int value)
    {
        luck += value;
    }

    public override void Die()
    {
        PlayerMove.GetInstance().isDead = true;
        updateSouls();
        if (EnemySpawner.killCount > maxScore)
        {
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    { "Score", EnemySpawner.killCount.ToString() }
                }
            };
            PlayFabClientAPI.UpdateUserData(request,OnSetCharacterData , OnError);
            sendLeaderboard(EnemySpawner.killCount);
            Debug.Log("Game Over");
        }
        
        StartCoroutine(DieAnimation());
    }

    public void updateSouls()
    {
        string total = (currentSouls + EnemySpawner.killCount).ToString();
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "Souls",  total}
            }
        };
        GameManager.UpdateSoulsData(total);
        PlayFabClientAPI.UpdateUserData(request,OnSetCharacterData , OnError);
    }
    public void sendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Score",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnSendLeaderboard, OnError);
    }
    void OnSendLeaderboard(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Send Leaderboard Success");
    }
    public void GetCharacterData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnGetCharacterData, OnError);
    }
    void OnGetCharacterData(GetUserDataResult result)
    {
        Debug.Log("Get Character Data Success");
        if(result.Data != null && result.Data.ContainsKey("Score"))
        {
            maxScore = int.Parse(result.Data["Score"].Value);
            currentSouls = int.Parse(result.Data["Souls"].Value);
        }
        else
        {
            Debug.Log("No Score");
        }
    }
    void OnSetCharacterData(UpdateUserDataResult result)
    {
        Debug.Log("Set Character Data Success");
    }
    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
    protected override IEnumerator DieAnimation()
    {
        
        GetAnimator().SetBool("Death", true);
        
        yield return new WaitForSeconds(1f);
        
        ShowGameOverWindow();
        Time.timeScale = 0f;
    }
    void GetFirstWeapon()
    {
        switch (GetComponentInParent<Player>().GetCharacterType())
        {
            case CharacterData.CharacterType.Knight:
                Inventory.GetInstance().AddWeapon(WeaponData.WeaponType.Whip);
                break;
            case CharacterData.CharacterType.Bandit:
                Inventory.GetInstance().AddWeapon(WeaponData.WeaponType.Axe);
                break;
            case CharacterData.CharacterType.Mage:
                Inventory.GetInstance().AddWeapon(WeaponData.WeaponType.FireWand);
                break;
            case CharacterData.CharacterType.Necromancer:
                Inventory.GetInstance().AddWeapon(WeaponData.WeaponType.Lightning);
                break;
            case CharacterData.CharacterType.ThunderLord:
                Inventory.GetInstance().AddWeapon(WeaponData.WeaponType.Lightning);
                break;
            case CharacterData.CharacterType.Priest:
                Inventory.GetInstance().AddWeapon(WeaponData.WeaponType.MagicWand);
                break;
            case CharacterData.CharacterType.Rouge:
                Inventory.GetInstance().AddWeapon(WeaponData.WeaponType.Whip);
                break;    
        }
    }

    public override void ReduceHealthPoint(int damage)
    {
        if (!PlayerMove.GetInstance().isDead)
        {
            base.ReduceHealthPoint(damage);

            hpSlider.value = GetHealthPoint();
            bleeding.Play();

            isColliding = true;

            if (hitCoroutine == null)
                hitCoroutine = StartCoroutine(UnderAttack());
        }
    }
    public override void RecoverHealthPoint(int amount)
    {
        base.RecoverHealthPoint(amount);
        hpSlider.value = GetHealthPoint();
    }

    protected override IEnumerator UnderAttack()
    {
        spriteRenderer.color = Color.red;

        do
        {
            isColliding = false;
            yield return new WaitForSeconds(0.2f);
        }
        while (isColliding);

        spriteRenderer.color = Color.white;
        hitCoroutine = null;
    }
    void ShowGameOverWindow()
    {
        nameText.text = "Username: " + PlayerPrefs.GetString("Username");
        soulText.text = "Souls: " + EnemySpawner.killCount;
        GameOverWindow.SetActive(true);
    }
}