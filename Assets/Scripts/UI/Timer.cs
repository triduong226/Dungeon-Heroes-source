using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    TextMeshProUGUI timeText;
    [SerializeField] GameObject GameOverWindow;
    int minute;
    int second;

    private Timer() { }

    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        StartCoroutine(CountTime());
    }

    void Initialize()
    {
        timeText = GetComponent<TextMeshProUGUI>();
        minute = 0;
        second = 0;
    }

    IEnumerator CountTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            ++second;
            if (second >= 60)
            {
                ++minute;
                second = 0;

                if (minute != 0 && minute != 10 && minute % 2 == 0)
                    EnemySpawner.GetInstance().IncreaseStage();
            }

            if (second < 10)
                timeText.text = minute.ToString() + " : 0" + second.ToString();
            else
                timeText.text = minute.ToString() + " : " + second.ToString();

            if (minute >= 10)
            {
                switch (SceneManager.GetActiveScene().name)
                {
                    case "Stage1":
                        GameManager.PlayerData.Stage.Add("Stage2");
                        GameManager.UpdatePlayFabData();
                        break;
                    case "Stage2":
                        GameManager.PlayerData.Stage.Add("Stage3");
                        GameManager.UpdatePlayFabData();
                        break;
                }
                GameOverWindow.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    public int GetMinute()
    {
        return minute;
    }
}
