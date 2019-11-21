namespace Interfaces
{
    public interface IDamageable
    {
        int ReceiveDamage(int amount,bool armorPiercing = false);

        int ApplyDamage(int amount, IDamageable target,bool armorPiercing = false);

        void ApplyDamageDebuff(int amount,IDamageable target);

        void ReceiveDamageDebuff(int amount);

        void ReceiveDot(int damage, int rounds);
        void ApplyDot(int damage, int rounds, IDamageable target);

        void ReceiveHealing(int amount);
        void ApplyHealing(int amount, IDamageable target);
    }
}