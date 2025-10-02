using UnityEngine;

namespace UniQuest.Map
{
    public class WallObstacle : MonoBehaviour
    {
        [Header("Obstacle Settings")]
        public ObstacleType obstacleType = ObstacleType.Wall;
        public bool isDestructible = false;
        public int durability = 1;
        
        [Header("Visual Settings")]
        public Color obstacleColor = Color.gray;
        
        [Header("Audio Settings")]
        public AudioClip collisionSound;

        private SpriteRenderer spriteRenderer;
        private BoxCollider2D obstacleCollider;

        private void Start()
        {
            InitializeObstacle();
        }

        private void InitializeObstacle()
        {
            // Récupérer ou créer les composants nécessaires
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            }

            obstacleCollider = GetComponent<BoxCollider2D>();
            if (obstacleCollider == null)
            {
                obstacleCollider = gameObject.AddComponent<BoxCollider2D>();
            }

            // Configuration de l'obstacle
            ConfigureObstacle();
        }

        private void ConfigureObstacle()
        {
            // Sprite par défaut (carré blanc Unity)
            if (spriteRenderer.sprite == null)
            {
                spriteRenderer.sprite = CreateDefaultSprite();
            }

            // Couleur selon le type d'obstacle
            spriteRenderer.color = GetObstacleColor();

            // Configuration du collider
            obstacleCollider.isTrigger = false; // Collision solide
            
            // Layer pour les obstacles
            gameObject.layer = LayerMask.NameToLayer("Default"); // On utilisera les layers plus tard

            Debug.Log($"Obstacle {obstacleType} initialisé à la position {transform.position}");
        }

        private Sprite CreateDefaultSprite()
        {
            // Utiliser le sprite carré par défaut d'Unity
            return Resources.GetBuiltinResource<Sprite>("UI/Skin/Knob.psd");
        }

        private Color GetObstacleColor()
        {
            return obstacleType switch
            {
                ObstacleType.Wall => Color.gray,
                ObstacleType.Rock => new Color(0.4f, 0.3f, 0.2f), // Marron
                ObstacleType.Tree => new Color(0.2f, 0.8f, 0.2f), // Vert
                ObstacleType.Water => new Color(0.2f, 0.4f, 0.8f), // Bleu
                ObstacleType.Fence => new Color(0.6f, 0.4f, 0.2f), // Marron clair
                _ => obstacleColor
            };
        }

        // Appelé quand le Player entre en collision
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                HandlePlayerCollision(collision);
            }
        }

        private void HandlePlayerCollision(Collision2D collision)
        {
            Debug.Log($"Player collision avec {obstacleType} à {transform.position}");

            // Jouer un son si configuré
            if (collisionSound != null)
            {
                AudioSource.PlayClipAtPoint(collisionSound, transform.position);
            }

            // Logique spécifique selon le type d'obstacle
            switch (obstacleType)
            {
                case ObstacleType.Wall:
                case ObstacleType.Rock:
                    // Obstacles solides - pas d'action spéciale
                    break;

                case ObstacleType.Water:
                    // Ralentir le joueur dans l'eau
                    var player = collision.gameObject.GetComponent<Characters.Player>();
                    if (player != null)
                    {
                        // On ajoutera cette fonctionnalité au Player plus tard
                        Debug.Log("Player dans l'eau - vitesse réduite");
                    }
                    break;

                case ObstacleType.Tree:
                    // Possibilité de destruction
                    if (isDestructible)
                    {
                        TakeDamage(1);
                    }
                    break;
            }
        }

        // Système de destruction pour les obstacles destructibles
        public void TakeDamage(int damage)
        {
            if (!isDestructible) return;

            durability -= damage;
            Debug.Log($"Obstacle {obstacleType} prend {damage} dégâts. Durabilité restante: {durability}");

            if (durability <= 0)
            {
                DestroyObstacle();
            }
            else
            {
                // Effet visuel de dégâts
                StartCoroutine(DamageFlash());
            }
        }

        private void DestroyObstacle()
        {
            Debug.Log($"Obstacle {obstacleType} détruit!");
            
            // Effet de destruction (optionnel)
            CreateDestructionEffect();
            
            // Supprimer l'obstacle
            Destroy(gameObject);
        }

        private System.Collections.IEnumerator DamageFlash()
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
        }

        private void CreateDestructionEffect()
        {
            // Ici on pourrait ajouter des particules, des sons, etc.
            Debug.Log("💥 Effet de destruction!");
        }

        // Méthodes utilitaires pour le level design
        public void SetObstacleType(ObstacleType newType)
        {
            obstacleType = newType;
            if (spriteRenderer != null)
            {
                spriteRenderer.color = GetObstacleColor();
            }
        }

        public void SetSize(Vector2 size)
        {
            transform.localScale = new Vector3(size.x, size.y, 1f);
        }
    }

    [System.Serializable]
    public enum ObstacleType
    {
        Wall,      // Mur solide
        Rock,      // Rocher
        Tree,      // Arbre (potentiellement destructible)
        Water,     // Eau (ralentit le joueur)
        Fence      // Barrière
    }
}
