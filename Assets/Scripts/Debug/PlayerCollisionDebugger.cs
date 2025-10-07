using UnityEngine;

namespace UniQuest.Debugging
{
    /// <summary>
    /// Script de debug pour visualiser et tester les collisions du Player
    /// Attachez ce script au Player pour avoir des informations détaillées
    /// </summary>
    public class PlayerCollisionDebugger : MonoBehaviour
    {
        [Header("Debug Settings")]
        public bool enableDebugLogs = true;
        public bool enableVisualDebug = true;
        public Color collisionGizmoColor = Color.red;
        
        [Header("Collision Info")]
        public string lastCollisionObject = "None";
        public Vector2 lastCollisionPoint = Vector2.zero;
        public float lastCollisionTime = 0f;
        
        private BoxCollider2D playerCollider;
        private CircleCollider2D playerCircleCollider;
        
        private void Start()
        {
            // Récupérer les colliders du Player
            playerCollider = GetComponent<BoxCollider2D>();
            playerCircleCollider = GetComponent<CircleCollider2D>();
            
            if (playerCollider == null && playerCircleCollider == null)
            {
                Debug.LogWarning("⚠️ Aucun collider trouvé sur le Player! Ajoutez un BoxCollider2D ou CircleCollider2D.");
            }
            
            Debug.Log("🔍 PlayerCollisionDebugger initialisé");
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!enableDebugLogs) return;
            
            lastCollisionObject = collision.gameObject.name;
            lastCollisionTime = Time.time;
            
            if (collision.contacts.Length > 0)
            {
                lastCollisionPoint = collision.contacts[0].point;
            }
            
            Debug.Log($"🔴 COLLISION ENTRÉE avec {collision.gameObject.name}");
            Debug.Log($"   Position: {lastCollisionPoint}");
            Debug.Log($"   Tag: {collision.gameObject.tag}");
            Debug.Log($"   Layer: {LayerMask.LayerToName(collision.gameObject.layer)}");
            
            // Vérifier si c'est un obstacle
            var obstacle = collision.gameObject.GetComponent<UniQuest.Map.WallObstacle>();
            if (obstacle != null)
            {
                Debug.Log($"   Type d'obstacle: {obstacle.obstacleType}");
                Debug.Log($"   Destructible: {obstacle.isDestructible}");
            }
        }
        
        private void OnCollisionStay2D(Collision2D collision)
        {
            // Info moins fréquente pour éviter le spam
            if (enableDebugLogs && Time.time - lastCollisionTime > 1f)
            {
                Debug.Log($"🟡 COLLISION CONTINUE avec {collision.gameObject.name}");
                lastCollisionTime = Time.time;
            }
        }
        
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!enableDebugLogs) return;
            
            Debug.Log($"🟢 COLLISION SORTIE de {collision.gameObject.name}");
            
            if (collision.gameObject.name == lastCollisionObject)
            {
                lastCollisionObject = "None";
            }
        }
        
        // Triggers (pour les zones d'eau par exemple)
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!enableDebugLogs) return;
            
            Debug.Log($"🔵 TRIGGER ENTRÉE avec {other.gameObject.name}");
            
            // Vérifier si c'est un terrain
            var terrain = other.GetComponent<UniQuest.Map.TerrainTileComponent>();
            if (terrain != null)
            {
                Debug.Log($"   Type de terrain: {terrain.terrainType}");
                Debug.Log($"   Modificateur de vitesse: {terrain.movementSpeedMultiplier}x");
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!enableDebugLogs) return;
            
            Debug.Log($"🔵 TRIGGER SORTIE de {other.gameObject.name}");
        }
        
        // Affichage visuel dans l'éditeur
        private void OnDrawGizmosSelected()
        {
            if (!enableVisualDebug) return;
            
            // Dessiner le collider du Player
            Gizmos.color = Color.green;
            
            if (playerCollider != null)
            {
                Gizmos.DrawWireCube(transform.position, playerCollider.size);
            }
            
            if (playerCircleCollider != null)
            {
                Gizmos.DrawWireSphere(transform.position, playerCircleCollider.radius);
            }
            
            // Dessiner le dernier point de collision
            if (lastCollisionPoint != Vector2.zero)
            {
                Gizmos.color = collisionGizmoColor;
                Gizmos.DrawSphere(lastCollisionPoint, 0.1f);
            }
        }
        
        // Méthodes utilitaires pour les tests
        [ContextMenu("Test Collision Info")]
        public void TestCollisionInfo()
        {
            Debug.Log("=== INFO DE COLLISION ===");
            Debug.Log($"Dernier objet touché: {lastCollisionObject}");
            Debug.Log($"Position de collision: {lastCollisionPoint}");
            Debug.Log($"Temps depuis dernière collision: {Time.time - lastCollisionTime}s");
            
            // Info sur les colliders
            if (playerCollider != null)
            {
                Debug.Log($"BoxCollider2D - Taille: {playerCollider.size}");
            }
            
            if (playerCircleCollider != null)
            {
                Debug.Log($"CircleCollider2D - Rayon: {playerCircleCollider.radius}");
            }
        }
        
        [ContextMenu("Check Nearby Obstacles")]
        public void CheckNearbyObstacles()
        {
            var obstacles = FindObjectsByType<UniQuest.Map.WallObstacle>(FindObjectsSortMode.None);
            Debug.Log($"=== OBSTACLES PROCHES (Rayon 5 unités) ===");
            
            int count = 0;
            foreach (var obstacle in obstacles)
            {
                float distance = Vector2.Distance(transform.position, obstacle.transform.position);
                if (distance < 5f)
                {
                    Debug.Log($"{obstacle.name} ({obstacle.obstacleType}) - Distance: {distance:F1}");
                    count++;
                }
            }
            
            if (count == 0)
            {
                Debug.Log("Aucun obstacle dans un rayon de 5 unités");
            }
        }
    }
}
