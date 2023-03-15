using System;
using System.Collections.Generic;

namespace Model
{
    public class PlayerData
    {
        public string Username { get; set; }
        public string PlayFabId { get; set; }
        public List<string> Characters { get; set; }
        public List<string> Stage { get; set; }
        public string CurrentCharacter { get; set; }
        public int Souls { get; set; }
        public int Score { get; set; }
        
    }
}