using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Object/Character Data", order = int.MaxValue )]
[Serializable]
public class CharacterData : ScriptableObject
{
    [SerializeField] Sprite sprite;
    [SerializeField] RuntimeAnimatorController controller;
    [SerializeField] CharacterType characterType;
    [SerializeField] int healthPoint;
    [SerializeField] int attackPower;
    [SerializeField] string description;
    [SerializeField] string rare;
    [SerializeField] Sprite rareSprite;
    [SerializeField] int defencePower;
    [SerializeField] int speed;
    [SerializeField] int unlockPrice;


    public enum CharacterType
    {
        FlyingEye,
        Goblin,
        Skeleton,
        Mushroom,
        Knight,
        Bandit,
        Mage,
        Necromancer,
        ThunderLord,
        Priest,
        Rouge
    }

    public int getCharacterCount
    {
        get
        {
            return System.Enum.GetValues(typeof(CharacterType)).Length;
        }
    }
    public int GetHealthPoint()
    {
        return healthPoint;
    }

    public string GetDescription()
    {
        return description;
    }
    
    public int GetUnlockPrice()
    {
        return unlockPrice;
    }

    public int GetAttackPower()
    {
        return attackPower;
    }

    public int GetDefencePower()
    {
        return defencePower;
    }

    public int GetSpeed()
    {
        return speed;
    }

    public CharacterType GetCharacterType()
    {
        return characterType;   
    }

    public RuntimeAnimatorController GetController()
    {
        return controller;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }
    
    public Sprite GetRareSprite()
    {
        return rareSprite;
    }
    public string GetRare()
    {
        return rare;
    }
}