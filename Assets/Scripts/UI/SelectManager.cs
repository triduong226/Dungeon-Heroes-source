using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    public Text nameText;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    [SerializeField] private Text name;
    [SerializeField] private Text hp;
    [SerializeField] private Text atk;
    [SerializeField] private Text def;
    [SerializeField] private Text spd;
    [SerializeField] private Image rare;
    [SerializeField] private Text info;
    [SerializeField] private Image useButton;
    [SerializeField] private Text soul;


    public int selectedoption = 0;

    private void Awake()
    {
        soul.text = GameManager.PlayerData.Souls.ToString();
        selectedoption = GameManager.CharacterDataList.GetSelectedCharacterIndex(GameManager.PlayerData.CurrentCharacter);
        GameManager.selectedCharacterData = GameManager.CharacterDataList.GetCharacterData(selectedoption);
    }


    void Start()
    {
        UpdateCharacter();
    }

    public void NextOption()
    {
        selectedoption++;
        if (selectedoption >= GameManager.CharacterDataList.Count())
        {
            selectedoption = 0;
        }
        UpdateCharacter();
    }

    public void BackOption()
    {
        selectedoption--;
        if (selectedoption < 0)
        {
            selectedoption = GameManager.CharacterDataList.Count() - 1;
        }
        UpdateCharacter();
    }

    private void UpdateCharacter()
    {
        var character = GameManager.CharacterDataList.GetCharacterData(selectedoption);
        
        name.text = character.GetCharacterType().ToString();
        rare.sprite = character.GetRareSprite();
        switch (character.GetRare())
        {
            case "Common":
                name.color = new Color(0.09019608f, 0.8196079f, 0.3843137f);
                break;
            case "Uncommon":
                name.color = new Color(0.03137255f, 0.5647059f, 0.827451f);
                break;
            case "Rare":
                name.color = new Color(0.8f, 0.1882f, 0.0627451f);
                break;
            case "Legendary":
                name.color = new Color(0.945098f, 0.8509804f, 0.1843137f);
                break;
        }
        info.text = character.GetDescription();
        soul.text = GameManager.PlayerData.Souls.ToString();
        hp.text = "HP: " + character.GetHealthPoint().ToString();
        atk.text =  "ATK: " +character.GetAttackPower().ToString();
        def.text = "DEF: " + character.GetDefencePower().ToString();
        spd.text = "SPD: " + character.GetSpeed().ToString();
        // if character in use change color of button
        useButton.color = character == GameManager.selectedCharacterData ? Color.green : Color.white;
        
        var useText = useButton.GetComponentInChildren<Text>();

        if (character.GetUnlockPrice() == 0)
        {
            useText.text = !GameManager.PlayerData.Characters.Contains(character.GetCharacterType().ToString()) ? "Get" : "Use";
        }
        else
        {
            useText.text = !GameManager.PlayerData.Characters.Contains(character.GetCharacterType().ToString()) ? character.GetUnlockPrice().ToString() : "Use";
        }

        // if character is unlocked
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = character.GetController();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<SpriteRenderer>().sprite = character.GetSprite();
        spriteRenderer.size = new Vector2(70f, 70f);
        
       
        
    }

    public void Select()
    {
        var character = GameManager.CharacterDataList.GetCharacterData(selectedoption);
        if (!GameManager.PlayerData.Characters.Contains(character.GetCharacterType().ToString()))
        {
            
            if (character.GetUnlockPrice() == 0)
            {
                // open website
                Application.OpenURL("http://unity3d.com/");
            }
            else
            {
                if (GameManager.PlayerData.Souls < character.GetUnlockPrice()) return;
                GameManager.PlayerData.Souls -= character.GetUnlockPrice();
                GameManager.PlayerData.Characters.Add(character.GetCharacterType().ToString());
                soul.text = GameManager.PlayerData.Souls.ToString();
                useButton.GetComponentInChildren<Text>().text = "Use";
            }
        }
        else
        {
            GameManager.selectedCharacterData = character;
            useButton.color = Color.green;
            GameManager.PlayerData.CurrentCharacter = character.GetCharacterType().ToString();
        }
        GameManager.UpdatePlayFabData();
    }
    
}
