using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Models;
using TMPro;
using UIScripts;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyBehavior : MonoBehaviour , IDamageable
{

    private class Dot
    {
        public readonly int Damage;
        public int Rounds;

        public Dot(int damage, int rounds)
        {
            Damage = damage;
            Rounds = rounds;
        }
    }

    private string abName;
    public Enemy enemyData;
    public int health;
    public int shield = 0;
    public bool stunned;
    public int damageMultiplier;
    private List<Dot> _dots;


    public Slider healthbar;
    public TextMeshProUGUI shieldbar;
    
    
    
    private int GetHealth()
    {
        return health;
    }

    private void TakeDotDamage()
    {
        foreach (var dot in _dots)
        {
            this.ReceiveDamage(dot.Damage);
            dot.Rounds =- 1;
            if (dot.Rounds == 0)
            {
                _dots.Remove(dot);
            }
        }
    }
    
    private void Die()
    {
        Game.CurrentCombatSystem.Enemies.Remove(this);
        Destroy(this.GetComponent<EnemyMouseEvents>().popUpRef);
        Destroy(this.gameObject);
        Destroy(healthbar.gameObject);
        Destroy(shieldbar.gameObject);
    }

    private int ApplyDamageMultiplier(int damage)
    {
        if (damageMultiplier != 0)
        {
            var damageModifier = (float) damageMultiplier / 100;
            damageMultiplier = 0;
            return damage * (int) Math.Round(damage * (1 + (float) (damageModifier)));
        }
        
        UpdateBars();

        return damage;
    }
    
    
    public void Init(Enemy enemyModel)
    {
        this.enemyData = enemyModel;
        this._dots = new List<Dot>();
        this.stunned = false;
        this.shield = 0;
        this.damageMultiplier = 0;
        this.health = enemyData.Health;
    }
    
    
    private void IncreaseShield(int amount)
    {
        this.shield += amount;
        
        UpdateBars();
    }

    private void UpdateText()
    {
        Game.Notification.GetComponent<TextMeshProUGUI>().text = this.enemyData.Name + " used " + abName;
    }
    public void PerformCombatAction()
    {
        TakeDotDamage();
        EnemyAbility ab = WeightedRandomizer.From(enemyData.EnemyAbilities).TakeOne();
        abName = ab.Name;
        Invoke(nameof(UpdateText),1);
        
        if (ab.MinHeal > 0)
        {
            EnemyBehavior enemyToHeal = Game.CurrentCombatSystem.Enemies.First(e => e.health == Game.CurrentCombatSystem.Enemies.Max(b => b.health));
            enemyToHeal.ReceiveHealing(Random.Range(ab.MinHeal,ab.MaxHeal));
        }

        if (ab.MinDamage > 0)
        {
            this.ApplyDamage(Random.Range(ab.MinDamage,ab.MaxDamage + 1) , Game.Player, ab.ArmorPiercing);
        }

        if (ab.MinBlock > 0)
        {
            this.IncreaseShield(Random.Range(ab.MinBlock,ab.MaxBlock));
        }

        if (ab.Stun)
        {
            Game.Player.stunned = true;
        }

        if (ab.DamageReduction > 0)
        {
            this.ApplyDamageDebuff(-ab.DamageReduction, Game.Player);
        }

        if (ab.SelfDamage > 0)
        {
            this.ReceiveDamage(ab.SelfDamage);
        }

        if (ab.Dot)
        {
            this.ReceiveDot(ab.DotDamage,ab.Time);
        }
        
        UpdateBars();
        
    }

    public int ReceiveDamage(int amount, bool armorPiercing = false)
    {
        if (armorPiercing)
        {
            health -=  amount;
        }
        else
        {
            var shieldDamage = this.shield - amount;
            if (shieldDamage < 0)
            {
                this.health -= Math.Abs(shieldDamage);
                healthbar.value = health;
            }
            else
            {
                this.shield = Math.Abs(shieldDamage);
                shieldbar.text = shield.ToString();
            }
        }

        if (this.health < 1)
        {
            Die();
        }

        UpdateBars();
        
        return amount;
    }


    private void UpdateBars()
    {
        healthbar.maxValue = enemyData.Health;
        
        healthbar.value = health;
        healthbar.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = health + " / " + enemyData.Health;
        
        shieldbar.text = "Shield " + shield;
    }
    
    public int ApplyDamage(int amount, IDamageable target, bool armorPiercing = false)
    {
        target.ReceiveDamage(ApplyDamageMultiplier(amount),armorPiercing);
        UpdateBars();
        return amount;
    }

    public void ApplyDamageDebuff(int amount, IDamageable target)
    {
        target.ReceiveDamageDebuff(amount);
        UpdateBars();
    }

    public void ReceiveDamageDebuff(int amount)
    {
        this.damageMultiplier = amount;
        UpdateBars();
    }

    public void ReceiveDot(int damage, int rounds)
    {
        this._dots.Add(new Dot(damage,rounds));
        UpdateBars();
    }

    public void ApplyDot(int damage, int rounds, IDamageable target)
    {
        target.ReceiveDot(damage,rounds);
        UpdateBars();
    }

    public void ReceiveHealing(int amount)
    {
        if (this.health + amount > this.enemyData.Health)
        {
            this.health = this.enemyData.Health;
        }
        else
        {
            this.health += amount;
        }
        
        UpdateBars();
    }

    public void ApplyHealing(int amount, IDamageable target)
    {
        target.ReceiveHealing(amount);
        UpdateBars();
    }
}