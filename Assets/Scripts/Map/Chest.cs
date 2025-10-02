using UnityEngine;
using UniQuest.Inventory; // Namespace de ton InventoryManager et Item

public class Chest : MonoBehaviour
{
    [Header("Récompense")]
    public Item item;      // Item ScriptableObject assigné dans l'Inspector
    public int quantity = 1;

    private bool opened = false;
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !opened)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        opened = true;

        InventoryManager inventory = Object.FindFirstObjectByType<InventoryManager>();
        if (inventory != null)
        {
            inventory.AddItem(item, quantity);
            Debug.Log($"🎁 Vous avez trouvé {quantity}x {item.itemName} !");
        }
        else
        {
            Debug.LogWarning("InventoryManager introuvable !");
        }

        // TODO : changer le sprite du coffre pour un coffre ouvert
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = false;
    }
}
