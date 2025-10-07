using UnityEngine;

namespace UniQuest.Map
{
    /// <summary>
    /// Script utilitaire pour créer rapidement des obstacles de test dans la scène
    /// Utilisez ce script pour tester facilement les différents types d'obstacles
    /// </summary>
    public class ObstacleTestManager : MonoBehaviour
    {
        [Header("Test Configuration")]
        public bool createTestObstacles = true;
        public Vector2 testAreaSize = new Vector2(10f, 10f);
        
        [Header("Obstacles to Create")]
        public bool createWalls = true;
        public bool createRocks = true;
        public bool createTrees = true;
        public bool createWater = false; // Water sera géré différemment
        
        private void Start()
        {
            if (createTestObstacles)
            {
                CreateTestObstacles();
            }
        }
        
        private void CreateTestObstacles()
        {
            Debug.Log("🔨 Création des obstacles de test...");
            
            if (createWalls)
            {
                // Mur horizontal au nord
                CreateObstacle(ObstacleType.Wall, new Vector3(0, 3, 0), new Vector2(4, 1), false);
                
                // Mur vertical à l'est
                CreateObstacle(ObstacleType.Wall, new Vector3(3, 0, 0), new Vector2(1, 3), false);
            }
            
            if (createRocks)
            {
                // Rocher au sud-ouest
                CreateObstacle(ObstacleType.Rock, new Vector3(-2, -2, 0), new Vector2(1, 1), false);
                
                // Rocher au nord-est
                CreateObstacle(ObstacleType.Rock, new Vector3(2, 2, 0), new Vector2(1.5f, 1.5f), false);
            }
            
            if (createTrees)
            {
                // Arbre destructible
                CreateObstacle(ObstacleType.Tree, new Vector3(-3, 1, 0), new Vector2(1, 1), true, 3);
                
                // Groupe d'arbres non-destructibles
                CreateObstacle(ObstacleType.Tree, new Vector3(-1, 3, 0), new Vector2(1, 1), false);
                CreateObstacle(ObstacleType.Tree, new Vector3(1, 4, 0), new Vector2(1, 1), false);
            }
            
            Debug.Log("✅ Obstacles de test créés!");
        }
        
        private void CreateObstacle(ObstacleType type, Vector3 position, Vector2 size, bool destructible, int durability = 1)
        {
            // Créer un nouveau GameObject
            GameObject obstacleGO = new GameObject($"{type}_Test_{position.x}_{position.y}");
            
            // Positionner
            obstacleGO.transform.position = position;
            obstacleGO.transform.localScale = new Vector3(size.x, size.y, 1f);
            
            // Ajouter le script WallObstacle
            WallObstacle obstacle = obstacleGO.AddComponent<WallObstacle>();
            
            // Configurer l'obstacle
            obstacle.obstacleType = type;
            obstacle.isDestructible = destructible;
            obstacle.durability = durability;
            
            Debug.Log($"Obstacle {type} créé à {position} (Destructible: {destructible})");
        }
        
        // Méthode utilitaire pour nettoyer les obstacles de test
        [ContextMenu("Clear Test Obstacles")]
        public void ClearTestObstacles()
        {
            // Trouver tous les obstacles avec "_Test_" dans le nom
            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            int count = 0;
            
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.Contains("_Test_"))
                {
                    if (Application.isPlaying)
                    {
                        Destroy(obj);
                    }
                    else
                    {
                        DestroyImmediate(obj);
                    }
                    count++;
                }
            }
            
            Debug.Log($"🗑️ {count} obstacles de test supprimés");
        }
        
        // Affichage visuel des zones de test dans l'éditeur
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector3(testAreaSize.x, testAreaSize.y, 0));
        }
    }
}
