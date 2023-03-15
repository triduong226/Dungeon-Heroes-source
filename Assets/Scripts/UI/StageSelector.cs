using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelector : MonoBehaviour
{
    [SerializeField] public Button stage1;
    [SerializeField] public Button stage2;
    [SerializeField] public Button stage3;
    [SerializeField] public Text modalText;

    void Start()
    {
        stage1.GetComponent<Image>().color = Color.green;
        CheckStatusButton("Stage2",stage2);
        CheckStatusButton("Stage3",stage3);
        
    }
    public void Stage1()
    {
        modalText.text = "Loading....";
        SceneManager.LoadScene("Stage1");
    }

    void CheckStatusButton(string stage, Button stageBtn)
    {
        if (!GameManager.PlayerData.Stage.Contains(stage))
        {
            stageBtn.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            stageBtn.GetComponent<Image>().color = Color.green;
        }
    }
    public void Stage2()
    {
        if (GameManager.PlayerData.Stage.Contains("Stage2"))
        {
            modalText.text = "Loading....";
            SceneManager.LoadScene("Stage2");
        }
        else
        {
            modalText.text = "The Stage 2 is locked";
        }
    }
    public void Stage3()
    {
        if (GameManager.PlayerData.Stage.Contains("Stage3"))
        {
            modalText.text = "Loading....";
        }
        else
        {
            modalText.text = "Coming soon....";
        }
    }
}
