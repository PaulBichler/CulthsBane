using System;
using System.Collections.Generic;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class Player : MonoBehaviour, IDamageable
{
    public GameObject gameOver;
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

    public int maxHealth = 100;
    public int maxShield = 1000;
    public int health;
    public int shield = 0;
    public bool stunned;
    public int damageMultiplier;
    public Slider healthDisplay;
    public Slider shieldDisplay;
    private List<Dot> _dots;


    private int ApplyDamageMultiplier(int damage)
    {
        if (damageMultiplier != 0)
        {
            return damage * (int) Math.Round(damage * (1 + (float) (damageMultiplier / 100)));
        }

        return damage;
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
        this.healthDisplay.value = health;
    }
    
    private void Start()
    {
        health = 100;
        maxHealth = 100;
        healthDisplay.maxValue = maxHealth;
        healthDisplay.value = health;
        shieldDisplay.value = 0;
        healthDisplay.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = health + " / " + health;
        shieldDisplay.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = shieldDisplay.value + " / " + maxShield;
        this._dots = new List<Dot>();
    }

    private void UpdateHealthBar()
    {
        this.healthDisplay.value = health;
        healthDisplay.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = health + " / " + maxHealth;
    }

    private void UpdateShieldBar()
    {
        this.shieldDisplay.value = health;
        shieldDisplay.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = shieldDisplay.value + " / " + maxShield;
    }
    
    public int ReceiveDamage(int amount, bool armorPiercing = false)
    {
        if (armorPiercing)
        {
            health -=  amount;
        }
        else
        {
            var shieldDamage = this.shield - Math.Abs(amount);
            if (shieldDamage < 0)
            {
                this.health -= Math.Abs(shieldDamage);
            }
            else
            {
                this.shield = shieldDamage;
            }
        }

        if (health <= 0)
        {
            gameOver.SetActive(true);
        }
        
        this.UpdateHealthBar();
        return amount;
    }

    public int ApplyDamage(int amount, IDamageable target , bool armorPiercing = false)
    {
        var damage = ApplyDamageMultiplier(amount);
        target.ReceiveDamage(ApplyDamageMultiplier(amount),armorPiercing);
        return damage;
    }

    public void ApplyDamageDebuff(int amount, IDamageable target)
    {
        target.ReceiveDamage(amount);
    }

    public void ReceiveDamageDebuff(int amount)
    {
        this.damageMultiplier = amount;
    }

    public void ReceiveDot(int damage, int rounds)
    {
        this._dots.Add(new Dot(damage,rounds));
    }

    public void ApplyDot(int damage, int rounds, IDamageable target)
    {
        target.ReceiveDot(damage,rounds);
    }

    public void ReceiveHealing(int amount)
    {
        this.health += amount;
        if (this.health > 100)
        {
            health = 100;
        }

        this.UpdateHealthBar();
    }

    public void ApplyHealing(int amount, IDamageable target)
    {
        var hp = this.health + amount;
        this.health = hp <= 100 ? amount : 100;
        this.UpdateHealthBar();
    }
}
