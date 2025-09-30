using UnityEngine;

namespace UniQuest.Characters
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Attack", menuName = "UniQuest/Attack")]
    public class Attack : ScriptableObject
    {
        [Header("Attack Info")]
        public string attackName;
        public string description;
        public AttackType type;

        [Header("Attack Stats")]
        public int baseDamage;
        public int mpCost;
        public int accuracy = 95; // Précision en %
        public int criticalChance = 5; // Chance de critique en %
        public float criticalMultiplier = 1.5f;

        [Header("Special Effects")]
        public bool canMiss = true;
        public bool isHealingMove = false;
        public int healAmount = 0;

        public virtual AttackResult ExecuteAttack(Character attacker, Character target)
        {
            AttackResult result = new AttackResult();
            result.attackName = attackName;
            result.attacker = attacker.characterName;
            result.target = target.characterName;

            // Vérifier si l'attaque touche
            if (canMiss && !CheckHit(attacker))
            {
                result.missed = true;
                result.message = $"{attacker.characterName} rate son attaque {attackName}!";
                return result;
            }

            // Consommer MP
            attacker.currentMP = Mathf.Max(0, attacker.currentMP - mpCost);

            if (isHealingMove)
            {
                // Attaque de soin
                int healValue = healAmount + (attacker.attack / 4);
                target.currentHP = Mathf.Min(target.maxHP, target.currentHP + healValue);
                result.healing = healValue;
                result.message = $"{attacker.characterName} utilise {attackName} et soigne {healValue} PV!";
            }
            else
            {
                // Calculer les dégâts
                int damage = CalculateDamage(attacker);
                
                // Vérifier critique
                if (CheckCritical())
                {
                    damage = Mathf.RoundToInt(damage * criticalMultiplier);
                    result.isCritical = true;
                }

                target.TakeDamage(damage);
                result.damage = damage;
                
                string critText = result.isCritical ? " CRITIQUE!" : "";
                result.message = $"{attacker.characterName} utilise {attackName} et inflige {damage} dégâts{critText}";
            }

            return result;
        }

        private bool CheckHit(Character attacker)
        {
            int hitChance = accuracy + (attacker.precision - 50);
            return Random.Range(0, 100) < hitChance;
        }

        private bool CheckCritical()
        {
            return Random.Range(0, 100) < criticalChance;
        }

        private int CalculateDamage(Character attacker)
        {
            int damage = baseDamage + (attacker.attack / 2) + Random.Range(-3, 4);
            return Mathf.Max(1, damage);
        }
    }

    [System.Serializable]
    public class AttackResult
    {
        public string attackName;
        public string attacker;
        public string target;
        public int damage;
        public int healing;
        public bool missed;
        public bool isCritical;
        public string message;
    }

    public enum AttackType
    {
        Physical,
        Magic,
        Healing,
        Special
    }
}
