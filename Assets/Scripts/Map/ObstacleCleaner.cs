using UnityEngine;

namespace UniQuest.Map
{
    /// <summary>
    /// Utilitaire pour nettoyer les anciens obstacles de test
    /// </summary>
    public class ObstacleCleaner : MonoBehaviour
    {
        [ContextMenu("Remove Old Test Obstacles")]
        public void RemoveOldTestObstacles()
        {
            // Chercher tous les WallObstacle dans la scène
            WallObstacle[] wallObstacles = FindObjectsByType<WallObstacle>(FindObjectsSortMode.None);
            
            int removedCount = 0;
            foreach (var wall in wallObstacles)
            {
                if (wall.gameObject.name.Contains("Wall_Test") || 
                    wall.gameObject.name.Contains("Test") ||
                    wall.gameObject.name.Contains("Wall"))
                {
                    Debug.Log($"🗑️ Suppression de l'obstacle: {wall.gameObject.name}");
                    if (Application.isPlaying)
                        Destroy(wall.gameObject);
                    else
                        DestroyImmediate(wall.gameObject);
                    removedCount++;
                }
            }
            
            // Chercher d'autres GameObjects avec "Wall" ou "Obstacle" dans le nom
            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            
            foreach (var obj in allObjects)
            {
                if ((obj.name.Contains("Wall") || obj.name.Contains("Obstacle")) && 
                    obj.name.Contains("Test"))
                {
                    Debug.Log($"🗑️ Suppression de l'objet: {obj.name}");
                    if (Application.isPlaying)
                        Destroy(obj);
                    else
                        DestroyImmediate(obj);
                    removedCount++;
                }
            }
            
            Debug.Log($"✅ {removedCount} anciens obstacles de test supprimés!");
        }
        
        [ContextMenu("List All Obstacles")]
        public void ListAllObstacles()
        {
            WallObstacle[] walls = FindObjectsByType<WallObstacle>(FindObjectsSortMode.None);
            
            Debug.Log($"📋 {walls.Length} obstacles WallObstacle trouvés:");
            foreach (var wall in walls)
            {
                Debug.Log($"  - {wall.gameObject.name} à {wall.transform.position}");
            }
            
            // Chercher autres obstacles potentiels
            GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            Debug.Log($"📋 {obstacles.Length} objets avec tag 'Obstacle' trouvés:");
            foreach (var obstacle in obstacles)
            {
                Debug.Log($"  - {obstacle.name} à {obstacle.transform.position}");
            }
        }
    }
}