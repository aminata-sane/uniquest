using UnityEngine;

public class NpcInteraction : MonoBehaviour
{
    [Header("Infos PNJ")]
    public string npcName = "Villageois";
    [TextArea] public string[] dialogueLines;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        foreach (string line in dialogueLines)
        {
            Debug.Log(npcName + " : " + line);
        }

        // TODO : Plus tard -> appeler UI Dialogue
        // TODO : Plus tard -> déclencher quête
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
