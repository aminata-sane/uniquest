using System.Collections.Generic;
using UnityEngine;

namespace UniQuest.Characters
{
    [System.Serializable]
    public abstract class Character : MonoBehaviour
    {
        [Header("Character Stats")]
        public string characterName;
        public int level = 1;
        public int currentHP;
        public int maxHP;
        public int currentMP;
        public int maxMP;
        public int attack;
        public int defense;
        public int speed;
        public int precision;
        public int experience = 0;
        public int experienceToNextLevel = 100;

        [Header("Character Type")]
        public CharacterType type;

        [Header("Available Attacks")]
        public List<Attack> availableAttacks = new List<Attack>();

        protected virtual void Start()
        {
            InitializeCharacter();
        }

        protected virtual void InitializeCharacter()
        {
            currentHP = maxHP;
            currentMP = maxMP;
        }

        public virtual void TakeDamage(int damage)
        {
            int finalDamage = Mathf.Max(1, damage - defense);
            currentHP = Mathf.Max(0, currentHP - finalDamage);
            
            if (currentHP <= 0)
            {
                OnCharacterKO();
            }
        }

        public virtual void RestoreMP(int amount)
        {
            currentMP = Mathf.Min(maxMP, currentMP + amount);
        }

        public virtual void GainExperience(int exp)
        {
            experience += exp;
            CheckLevelUp();
        }

        protected virtual void CheckLevelUp()
        {
            while (experience >= experienceToNextLevel)
            {
                LevelUp();
            }
        }

        protected virtual void LevelUp()
        {
            level++;
            experience -= experienceToNextLevel;
            experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * 1.2f);

            // Augmenter les stats
            maxHP += Random.Range(5, 15);
            maxMP += Random.Range(3, 8);
            attack += Random.Range(2, 6);
            defense += Random.Range(1, 4);
            speed += Random.Range(1, 3);
            precision += Random.Range(1, 3);

            currentHP = maxHP; // Heal complet au level up
            currentMP = maxMP;

            Debug.Log($"{characterName} monte au niveau {level}!");
        }

        public virtual bool CanUseAttack(Attack attack)
        {
            return currentMP >= attack.mpCost && availableAttacks.Contains(attack);
        }

        protected virtual void OnCharacterKO()
        {
            Debug.Log($"{characterName} est KO!");
        }

        public bool IsAlive()
        {
            return currentHP > 0;
        }
    }

    public enum CharacterType
    {
        Warrior,
        Mage,
        Archer,
        Healer
    }
}
