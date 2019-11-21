using System.Collections.Generic;
using System.Xml.Serialization;

namespace Models
{
    [XmlRoot("EnemyAbilityList")]
    public class EnemyAbilityList
    {
        [XmlElement("Ability")]
        public List<EnemyAbility> Abilities;
    }
    public class EnemyAbility
    {
        public string Name { get; set; }
        public string Element { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int CriticalChance { get; set; }
        public int MinBlock { get; set; }
        public int MaxBlock { get; set; }
        public int MinHeal { get; set; }
        public int MaxHeal { get; set; }
        public bool ArmorPiercing { get; set; }
        public bool Stun { get; set; }
        public bool Dot { get; set; }
        public int DotDamage { get; set; }
        public int Time { get; set; }
        public int DamageReduction { get; set; }
        public int SelfDamage { get; set; }
    }
}