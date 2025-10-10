using UnityEngine;

namespace UniQuest.Map
{
    /// <summary>
    /// Générateur d'obstacles pour créer des chemins balisés
    /// </summary>
    public class ObstacleGenerator : MonoBehaviour
    {
        [Header("Obstacle Configuration")]
        public GameObject obstaclePrefab;
        public Transform obstaclesContainer;
        
        [Header("Manual Obstacle Positions")]
        [Tooltip("Positions où placer des obstacles (arbres, murs, etc.)")]
        public Vector2[] obstaclePositions = new Vector2[]
        {
            // Exemples de positions - à ajuster selon votre carte
            new Vector2(-3, 2), new Vector2(-2, 2), new Vector2(-1, 2),
            new Vector2(1, 1), new Vector2(2, 1), new Vector2(3, 1),
            new Vector2(-2, -1), new Vector2(0, -2), new Vector2(2, -2)
        };
        
        [ContextMenu("Generate Obstacles")]
        public void GenerateObstacles()
        {
            if (obstaclesContainer == null)
            {
                // Créer le conteneur s'il n'existe pas
                GameObject container = new GameObject("ObstaclesContainer");
                obstaclesContainer = container.transform;
            }
            
            // Supprimer les anciens obstacles
            ClearObstacles();
            
            // Créer les nouveaux obstacles
            for (int i = 0; i < obstaclePositions.Length; i++)
            {
                CreateObstacle(obstaclePositions[i], i);
            }
            
            Debug.Log($"✅ {obstaclePositions.Length} obstacles créés!");
        }
        
        [ContextMenu("Clear All Obstacles")]
        public void ClearObstacles()
        {
            if (obstaclesContainer != null)
            {
                for (int i = obstaclesContainer.childCount - 1; i >= 0; i--)
                {
                    if (Application.isPlaying)
                        Destroy(obstaclesContainer.GetChild(i).gameObject);
                    else
                        DestroyImmediate(obstaclesContainer.GetChild(i).gameObject);
                }
            }
        }
        
        private void CreateObstacle(Vector2 position, int index)
        {
            GameObject obstacle;
            
            if (obstaclePrefab != null)
            {
                // Utiliser le prefab s'il est assigné
                obstacle = Instantiate(obstaclePrefab, obstaclesContainer);
            }
            else
            {
                // Créer un obstacle simple
                obstacle = new GameObject($"Obstacle_{index:00}");
                obstacle.transform.SetParent(obstaclesContainer);
                
                // Ajouter le collider
                BoxCollider2D collider = obstacle.AddComponent<BoxCollider2D>();
                collider.size = new Vector2(0.9f, 0.9f);
                
                // Optionnel : Ajouter un SpriteRenderer pour voir l'obstacle
                SpriteRenderer sr = obstacle.AddComponent<SpriteRenderer>();
                sr.color = new Color(1f, 0f, 0f, 0.3f); // Rouge transparent
                sr.sprite = CreateSquareSprite();
            }
            
            // Positionner l'obstacle
            obstacle.transform.position = new Vector3(position.x, position.y, 0f);
            
            // Configurer le tag
            obstacle.tag = "Obstacle";
            
            Debug.Log($"🚧 Obstacle créé à ({position.x:F1}, {position.y:F1})");
        }
        
        private Sprite CreateSquareSprite()
        {
            // Créer une texture carrée simple
            Texture2D texture = new Texture2D(32, 32);
            Color[] pixels = new Color[32 * 32];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = Color.white;
            }
            texture.SetPixels(pixels);
            texture.Apply();
            
            return Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f));
        }
        
        void Start()
        {
            // Générer automatiquement au démarrage si souhaité
            // GenerateObstacles();
        }
    }
}