using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data List", menuName = "Scriptable Object/Character Data List", order = int.MaxValue )]
public class CharacterDataList : ScriptableObject
{
    [SerializeField] private List<CharacterData> characterDataList;
    
    public List<CharacterData> getCharacterDataList()
    {
        return characterDataList;
    }
    
    public CharacterData GetCharacterData(int index)
    {
        return characterDataList[index];
    }
    
    public CharacterData FindByCharacterTypeString(string characterTypeString)
    {
        var characterType = (CharacterData.CharacterType) Enum.Parse(typeof(CharacterData.CharacterType), characterTypeString);
        return characterDataList.Find(x => x.GetCharacterType() == characterType);
    }
    
    public int GetSelectedCharacterIndex(CharacterData characterData)
    {
        return characterDataList.FindIndex(x => x.GetCharacterType() == characterData.GetCharacterType());
    }
    
    public int GetSelectedCharacterIndex(string characterTypeString)
    {
        var characterType = (CharacterData.CharacterType) Enum.Parse(typeof(CharacterData.CharacterType), characterTypeString);
        return characterDataList.FindIndex(x => x.GetCharacterType() == characterType);
    }
    
    public int Count()
    {
        return characterDataList.Count;
    }
}
