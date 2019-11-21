using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Models
{
    public enum Elements
    {
        Physical,
        Fire,
        Ice,
        Lightning,
        Water,
        Earth,
        Poison,
        Light,
        Dark,
    }

    public enum Properties
    {
        target
    }
    [XmlRoot("CardList")]
    public class CardList
    {    
        [XmlElement("Card")]
        public List<Card> Cards { get; set; }
    }
    
    [Serializable]
    public class Card
    {
        #region CardProps
        public string Name { get; set; }
        public Elements Element { get; set; }
        public int Copies { get; set; }
        public bool InDeck { get; set; }

        #endregion
        
        #region PlayerEffects
        public int SelfBlock { get; set; }
        public int CriticalChance { get; set; }
        public int DamageBuff { get; set; }
        public int BlockRate { get; set; }
        public int Heal { get; set; }
        
        #endregion

        #region Area
        
        public int Targets { get; set; }
        public bool Aoe { get; set; }
        public bool F1 { get; set; }
        public bool F2 { get; set; }
        public bool F3 { get; set; }
        public bool F4 { get; set; }

        #endregion

        #region Enemy Effects
        
        public int Damage { get; set; }
        public bool Dot { get; set; }
        public int? Time { get; set; }
        public int? DotDamage { get; set; }
        public int DamageReduction { get; set; }
        public bool ArmorPiercing { get; set; }
        public bool Stun { get; set; }
        
        #endregion

        #region Stackable
        
        public bool ComboBreaker { get; set; }
        public bool Stackable { get; set; }
        public string StackProperty { get; set; }
        public int StackAmount { get; set; }
        
        #endregion
    }
}
