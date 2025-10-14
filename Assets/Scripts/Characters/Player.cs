using UnityEngine;

namespace UniQuest.Characters
{
    public class Player : Character
    {
    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public bool canMove = true;
    
    [Header("Player Appearance")]
    public float playerScale = 1f; // Taille du Player (les sprites sont déjà à la bonne taille)
    
    [Header("Player Sprites")]
    public Sprite idleFrontSprite;
    public Sprite idleBackSprite; 
    public Sprite idleLeftSprite;
    public Sprite idleRightSprite;
    public Sprite walkFrontSprite;
    public Sprite walkBackSprite;
    public Sprite walkLeftSprite;
    public Sprite walkRightSprite;
    public Sprite shadowSprite;
    
    [Header("Visual Effects")]
    public Color damageFlashColor = Color.red;
    public float flashDuration = 0.2f;
    
    [Header("Player Components")]
    private SpriteRenderer spriteRenderer;
    private GameObject shadowObject;
    private SpriteRenderer shadowRenderer;
    private ParticleSystem dustParticles;
    private Color originalColor;        [Header("Correction des axes (si nécessaire)")]
        public bool invertHorizontal = false;
        public bool invertVertical = true;  // Corrigé par défaut pour Unity
        
        private Rigidbody2D rb;
        private Vector2 movement;

        [Header("Terrain Effects")]
        public float currentTerrainSpeedMultiplier = 1f;
        // TerrainManager supprimé - non nécessaire avec SimpleMap

        protected override void Start()
        {
            base.Start(); // Appeler l'initialisation du Character
            
            // Récupérer le Rigidbody2D pour le mouvement
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
            }
            
            // Configuration du Rigidbody2D
            rb.gravityScale = 0f; // Pas de gravité pour un jeu 2D top-down
            rb.freezeRotation = true; // Empêcher la rotation
            
            // S'assurer qu'il y a un Collider2D pour les collisions
            BoxCollider2D playerCollider = GetComponent<BoxCollider2D>();
            if (playerCollider == null)
            {
                playerCollider = gameObject.AddComponent<BoxCollider2D>();
                playerCollider.size = new Vector2(0.8f, 0.8f); // Taille du Player
            }
            
            // TerrainManager supprimé - non nécessaire avec SimpleMap
            
            // S'assurer que le Player a le bon tag
            if (!gameObject.CompareTag("Player"))
            {
                gameObject.tag = "Player";
            }
            
            // Appliquer la taille du Player
            transform.localScale = new Vector3(playerScale, playerScale, 1f);
            
            // Initialiser le SpriteRenderer
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            }
            
            // Définir le sprite par défaut (idle face)
            if (idleFrontSprite != null)
            {
                spriteRenderer.sprite = idleFrontSprite;
            }
            
            // Sauvegarder la couleur originale pour les effets
            originalColor = spriteRenderer.color;
            
            // Créer l'ombre
            CreateShadow();
            
            // Créer les particules de poussière
            CreateDustParticles();
            
            // Configuration simplifiée après création
            
            Debug.Log($"Joueur {characterName} initialisé!");
        }

        private void Update()
        {
            if (canMove && IsAlive())
            {
                HandleInput();
            }
        }

        private void FixedUpdate()
        {
            if (canMove && IsAlive())
            {
                MovePlayer();
            }
        }

        private void HandleInput()
        {
            // Récupérer les entrées du joueur (WASD ou flèches)
            float horizontalInput = Input.GetAxisRaw("Horizontal"); // A/D ou flèches gauche/droite
            float verticalInput = Input.GetAxisRaw("Vertical");     // W/S ou flèches haut/bas
            
            // Appliquer la correction d'inversion si nécessaire
            if (invertHorizontal) horizontalInput = -horizontalInput;
            if (invertVertical) verticalInput = -verticalInput;
            
            // Appliquer les axes correctement
            movement.x = horizontalInput;  // Positif = droite, Négatif = gauche
            movement.y = verticalInput;    // Positif = haut, Négatif = bas
            
            // Debug pour vérifier les entrées
            if (movement != Vector2.zero)
            {
                Debug.Log($"Input brut - H: {Input.GetAxisRaw("Horizontal")}, V: {Input.GetAxisRaw("Vertical")}");
                Debug.Log($"Input corrigé - H: {horizontalInput}, V: {verticalInput}, Movement final: {movement}");
            }
            
            // Normaliser pour éviter le mouvement diagonal plus rapide
            movement = movement.normalized;
        }

        private void MovePlayer()
        {
            // Calculer la vitesse avec modificateur de terrain
            float effectiveSpeed = moveSpeed * currentTerrainSpeedMultiplier;
            
            // Appliquer le mouvement
            Vector2 targetVelocity = movement * effectiveSpeed;
            rb.linearVelocity = targetVelocity;
            
            // Mettre à jour le modificateur de terrain
            UpdateTerrainEffects();
            
            // Mettre à jour le sprite selon la direction
            UpdateSprite();
            
            // Gérer les particules de poussière
            UpdateDustParticles(targetVelocity);
            
            // Debug pour vérifier le mouvement
            // Debug occasionnel seulement
            if (targetVelocity != Vector2.zero && Time.frameCount % 60 == 0) // Une fois par seconde environ
            {
                Debug.Log($"🔵 Player se déplace: Vitesse={effectiveSpeed:F1}, Terrain={currentTerrainSpeedMultiplier:F1}");
            }
        }

        // Méthodes utiles pour le gameplay
        public void SetCanMove(bool canMove)
        {
            this.canMove = canMove;
            if (!canMove)
            {
                rb.linearVelocity = Vector2.zero; // Arrêter le mouvement
            }
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void SetPosition(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        // Gestion des collisions/triggers
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("Rencontre avec un ennemi!");
                // Ici, Nelson ajoutera la logique des combats aléatoires
            }
            else if (other.CompareTag("Chest"))
            {
                Debug.Log("Coffre trouvé!");
                // Logique d'ouverture de coffre
            }
            else if (other.CompareTag("NPC"))
            {
                Debug.Log("PNJ rencontré!");
                // Logique de dialogue
            }
        }

        // Gestion des effets de terrain
        private void UpdateTerrainEffects()
        {
            // Vitesse constante avec SimpleMap - pas de modificateur de terrain
            currentTerrainSpeedMultiplier = 1f;
        }

        public void SetTerrainSpeedMultiplier(float multiplier)
        {
            currentTerrainSpeedMultiplier = Mathf.Clamp(multiplier, 0.1f, 2f);
        }

        public float GetCurrentSpeed()
        {
            return moveSpeed * currentTerrainSpeedMultiplier;
        }

        // Override pour ajouter des comportements spécifiques au joueur
        protected override void OnCharacterKO()
        {
            base.OnCharacterKO();
            SetCanMove(false);
            Debug.Log("Le joueur est KO! Game Over possible...");
        }

        private void UpdateSprite()
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) return;

            // Si le joueur ne bouge pas, utiliser les sprites idle
            if (movement.magnitude < 0.1f)
            {
                // Garder la dernière direction pour l'idle, ou front par défaut
                if (idleFrontSprite != null)
                {
                    spriteRenderer.sprite = idleFrontSprite;
                }
            }
            else
            {
                // Déterminer la direction principale (la plus forte)
                if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
                {
                    // Mouvement horizontal dominant
                    if (movement.x > 0 && walkRightSprite != null) // Droite
                    {
                        spriteRenderer.sprite = walkRightSprite;
                    }
                    else if (movement.x < 0 && walkLeftSprite != null) // Gauche
                    {
                        spriteRenderer.sprite = walkLeftSprite;
                    }
                }
                else
                {
                    // Mouvement vertical dominant
                    if (movement.y > 0 && walkBackSprite != null) // Haut
                    {
                        spriteRenderer.sprite = walkBackSprite;
                    }
                    else if (movement.y < 0 && walkFrontSprite != null) // Bas
                    {
                        spriteRenderer.sprite = walkFrontSprite;
                    }
                }
            }
        }

        private void UpdateDustParticles(Vector2 velocity)
        {
            if (dustParticles == null) return;
            
            var emission = dustParticles.emission;
            
            // Activer les particules seulement si le joueur bouge
            if (velocity.magnitude > 0.1f)
            {
                if (!emission.enabled)
                {
                    emission.enabled = true;
                    emission.rateOverTime = 30f; // Plus de particules par seconde - plus visible
                }
            }
            else
            {
                emission.enabled = false;
            }
        }

        private void CreateShadow()
        {
            if (shadowSprite == null) return;
            
            // Créer un GameObject enfant pour l'ombre
            shadowObject = new GameObject("PlayerShadow");
            shadowObject.transform.SetParent(transform);
            
            // Positionner l'ombre légèrement en dessous du personnage
            shadowObject.transform.localPosition = new Vector3(0, -0.2f, 0.1f);
            shadowObject.transform.localScale = Vector3.one;
            
            // Ajouter le SpriteRenderer pour l'ombre
            shadowRenderer = shadowObject.AddComponent<SpriteRenderer>();
            shadowRenderer.sprite = shadowSprite;
            shadowRenderer.sortingOrder = -1; // Derrière le personnage
            shadowRenderer.color = new Color(0, 0, 0, 0.5f); // Ombre semi-transparente
        }

        private void CreateDustParticles()
        {
            // Créer un GameObject enfant pour les particules
            GameObject dustObject = new GameObject("DustParticles");
            dustObject.transform.SetParent(transform);
            dustObject.transform.localPosition = new Vector3(0, -0.3f, -0.1f); // Z négatif pour être devant
            
            // Ajouter le système de particules
            dustParticles = dustObject.AddComponent<ParticleSystem>();
            
            var main = dustParticles.main;
            main.startLifetime = 1f;        // Plus longue durée de vie
            main.startSpeed = 3f;           // Plus rapide
            main.startSize = 0.3f;          // Plus grandes particules
            main.startColor = Color.yellow; // Couleur simple et stable
            main.maxParticles = 20;         // Plus de particules
            
            // Désactiver Color over Lifetime pour garder la couleur fixe
            var colorOverLifetime = dustParticles.colorOverLifetime;
            colorOverLifetime.enabled = false;
            
            var emission = dustParticles.emission;
            emission.enabled = false; // Désactivé par défaut, activé quand on bouge
            
            var shape = dustParticles.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.2f;
            
            var velocityOverLifetime = dustParticles.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.radial = new ParticleSystem.MinMaxCurve(-1f, -0.5f);
            
            // Configuration simple du rendu
            var renderer = dustParticles.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
            {
                renderer.sortingOrder = 1; // Simple sorting order
                renderer.material = null;  // Pas de matériau spécial (utilise le défaut)
            }
        }

        public void TakeDamage(float damage)
        {
            // Appeler la méthode de base du Character (convertir en int)
            base.TakeDamage((int)damage);
            
            // Debug pour vérifier que la méthode est appelée
            Debug.Log($"🩸 Player TakeDamage appelé avec {damage} dégâts !");
            
            // Déclencher l'effet de surbrillance
            StartCoroutine(FlashEffect());
        }

        private System.Collections.IEnumerator FlashEffect()
        {
            // Récupérer le SpriteRenderer du GameObject
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            
            if (renderer != null)
            {
                Debug.Log("💥 Flash de dégâts - changement de couleur !");
                
                // Sauvegarder la couleur actuelle
                Color currentColor = renderer.color;
                
                // Appliquer la couleur de flash (rouge vif)
                renderer.color = Color.red;
                
                // Attendre plus longtemps pour bien voir l'effet
                yield return new UnityEngine.WaitForSeconds(0.5f);
                
                // Revenir à la couleur originale
                renderer.color = currentColor;
                
                Debug.Log("✅ Flash de dégâts terminé !");
            }
            else
            {
                Debug.LogError("❌ SpriteRenderer non trouvé pour l'effet de flash !");
            }
        }
    }
}
