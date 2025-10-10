using UnityEngine;

namespace UniQuest.Map
{
    /// <summary>
    /// Visualiseur pour identifier tous les colliders qui bloquent le Player
    /// </summary>
    public class ColliderVisualizer : MonoBehaviour
    {
        [Header("Visualization")]
        public bool showColliders = true;
        public Color colliderColor = Color.red;
        public bool showInGameView = true;
        
        [ContextMenu("Find All Colliders")]
        public void FindAllColliders()
        {
            Collider2D[] allColliders = FindObjectsByType<Collider2D>(FindObjectsSortMode.None);
            
            Debug.Log($"🔍 {allColliders.Length} colliders trouvés dans la scène:");
            
            int obstacleCount = 0;
            foreach (var collider in allColliders)
            {
                if (collider.gameObject != FindFirstObjectByType<UniQuest.Characters.Player>()?.gameObject)
                {
                    Debug.Log($"  🚧 {collider.gameObject.name} à {collider.transform.position} (Tag: {collider.tag})");
                    obstacleCount++;
                    
                    // Ajouter un visualiseur si pas déjà présent
                    if (showInGameView && collider.GetComponent<ColliderGizmo>() == null)
                    {
                        collider.gameObject.AddComponent<ColliderGizmo>();
                    }
                }
            }
            
            Debug.Log($"📊 Total obstacles: {obstacleCount}");
        }
        
        [ContextMenu("Remove All Tile Colliders")]
        public void RemoveAllTileColliders()
        {
            // Chercher tous les objets avec "Tile" dans le nom
            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            
            int removedCount = 0;
            foreach (var obj in allObjects)
            {
                if (obj.name.Contains("Tile") && obj.GetComponent<Collider2D>() != null)
                {
                    Collider2D collider = obj.GetComponent<Collider2D>();
                    if (Application.isPlaying)
                        Destroy(collider);
                    else
                        DestroyImmediate(collider);
                    
                    Debug.Log($"🗑️ Collider supprimé de: {obj.name}");
                    removedCount++;
                }
            }
            
            Debug.Log($"✅ {removedCount} colliders de tuiles supprimés!");
        }
        
        [ContextMenu("Remove ALL Colliders (Except Player)")]
        public void RemoveAllCollidersExceptPlayer()
        {
            Collider2D[] allColliders = FindObjectsByType<Collider2D>(FindObjectsSortMode.None);
            UniQuest.Characters.Player player = FindFirstObjectByType<UniQuest.Characters.Player>();
            
            int removedCount = 0;
            foreach (var collider in allColliders)
            {
                // Ne pas supprimer le collider du Player
                if (player != null && collider.gameObject == player.gameObject)
                    continue;
                    
                Debug.Log($"🗑️ Suppression collider de: {collider.gameObject.name}");
                
                if (Application.isPlaying)
                    Destroy(collider.gameObject);
                else
                    DestroyImmediate(collider.gameObject);
                    
                removedCount++;
            }
            
            Debug.Log($"✅ {removedCount} colliders supprimés (Player préservé)!");
        }
        
        [ContextMenu("Toggle Tile Colliders")]
        public void ToggleTileColliders()
        {
            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            
            int toggledCount = 0;
            foreach (var obj in allObjects)
            {
                if (obj.name.Contains("Tile"))
                {
                    Collider2D collider = obj.GetComponent<Collider2D>();
                    if (collider != null)
                    {
                        collider.enabled = !collider.enabled;
                        toggledCount++;
                    }
                }
            }
            
            Debug.Log($"🔄 {toggledCount} colliders de tuiles basculés!");
        }
    }
    
    /// <summary>
    /// Composant pour visualiser les colliders en jeu
    /// </summary>
    public class ColliderGizmo : MonoBehaviour
    {
        void OnDrawGizmos()
        {
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                Gizmos.color = Color.red;
                if (col is BoxCollider2D boxCol)
                {
                    Gizmos.DrawWireCube(transform.position, boxCol.size);
                }
            }
        }
    }
}