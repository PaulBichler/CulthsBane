using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class Enemy
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public Dictionary<EnemyAbility,int> EnemyAbilities { get; set; }
        
        public bool IsBoss { get; set; }
    }
}