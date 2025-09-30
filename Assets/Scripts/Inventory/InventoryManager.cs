using System.Collections.Generic;
using UnityEngine;

namespace UniQuest.Inventory
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Item", menuName = "UniQuest/Item")]
    public class Item : ScriptableObject
    {
        [Header("Item Info")]
        public string itemName;
        public string description;
        public ItemType type;
        public Sprite icon;

        [Header("Item Effects")]
        public int healingValue = 0;
        public int manaRestoreValue = 0;
        public int attackBoost = 0;
        public int defenseBoost = 0;
        public int speedBoost = 0;

        [Header("Item Properties")]
        public bool consumable = true;
        public bool usableInCombat = true;
        public bool usableOutOfCombat = true;

        public virtual bool UseItem(Characters.Character target)
        {
            bool itemUsed = false;

            if (healingValue > 0)
            {
                int currentHP = target.currentHP;
                target.currentHP = Mathf.Min(target.maxHP, target.currentHP + healingValue);
                int actualHealing = target.currentHP - currentHP;
                
                if (actualHealing > 0)
                {
                    Debug.Log($"{target.characterName} récupère {actualHealing} PV grâce à {itemName}!");
                    itemUsed = true;
                }
            }

            if (manaRestoreValue > 0)
            {
                target.RestoreMP(manaRestoreValue);
                Debug.Log($"{target.characterName} récupère {manaRestoreValue} PM grâce à {itemName}!");
                itemUsed = true;
            }

            // Les boosts temporaires seraient gérés par un système de buffs plus complexe
            if (attackBoost > 0 || defenseBoost > 0 || speedBoost > 0)
            {
                Debug.Log($"{itemName} applique des bonus temporaires à {target.characterName}!");
                itemUsed = true;
                // TODO: Implémenter le système de buffs
            }

            return itemUsed;
        }
    }

    public enum ItemType
    {
        Consumable,
        Key,
        Boost,
        Special
    }

    [System.Serializable]
    public class InventorySlot
    {
        public Item item;
        public int quantity;

        public InventorySlot(Item newItem, int newQuantity)
        {
            item = newItem;
            quantity = newQuantity;
        }
    }

    public class InventoryManager : MonoBehaviour
    {
        [Header("Inventory Settings")]
        public int maxSlots = 30;
        public List<InventorySlot> inventory = new List<InventorySlot>();

        public bool AddItem(Item item, int quantity = 1)
        {
            // Chercher si l'item existe déjà
            InventorySlot existingSlot = inventory.Find(slot => slot.item == item);
            
            if (existingSlot != null)
            {
                existingSlot.quantity += quantity;
                Debug.Log($"Ajouté {quantity}x {item.itemName} à l'inventaire (Total: {existingSlot.quantity})");
                return true;
            }
            else if (inventory.Count < maxSlots)
            {
                inventory.Add(new InventorySlot(item, quantity));
                Debug.Log($"Nouvel item ajouté: {quantity}x {item.itemName}");
                return true;
            }

            Debug.Log("Inventaire plein!");
            return false;
        }

        public bool UseItem(Item item, Characters.Character target)
        {
            InventorySlot slot = inventory.Find(s => s.item == item);
            
            if (slot != null && slot.quantity > 0)
            {
                bool success = item.UseItem(target);
                
                if (success && item.consumable)
                {
                    slot.quantity--;
                    if (slot.quantity <= 0)
                    {
                        inventory.Remove(slot);
                    }
                }
                
                return success;
            }

            return false;
        }

        public bool HasItem(Item item)
        {
            InventorySlot slot = inventory.Find(s => s.item == item);
            return slot != null && slot.quantity > 0;
        }

        public int GetItemQuantity(Item item)
        {
            InventorySlot slot = inventory.Find(s => s.item == item);
            return slot?.quantity ?? 0;
        }
    }
}
