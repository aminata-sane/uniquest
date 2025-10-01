using UnityEngine;

namespace UniQuest.Characters
{
    public class Player : Character
    {
        [Header("Player Movement")]
        public float moveSpeed = 5f;
        public bool canMove = true;
        
        [Header("Player Components")]
        private Rigidbody2D rb;
        private Vector2 movement;

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
            movement.x = Input.GetAxisRaw("Horizontal"); // A/D ou flèches gauche/droite
            movement.y = Input.GetAxisRaw("Vertical");   // W/S ou flèches haut/bas
            
            // Normaliser pour éviter le mouvement diagonal plus rapide
            movement = movement.normalized;
        }

        private void MovePlayer()
        {
            // Appliquer le mouvement
            Vector2 targetVelocity = movement * moveSpeed;
            rb.velocity = targetVelocity;
        }

        // Méthodes utiles pour le gameplay
        public void SetCanMove(bool canMove)
        {
            this.canMove = canMove;
            if (!canMove)
            {
                rb.velocity = Vector2.zero; // Arrêter le mouvement
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

        // Override pour ajouter des comportements spécifiques au joueur
        protected override void OnCharacterKO()
        {
            base.OnCharacterKO();
            SetCanMove(false);
            Debug.Log("Le joueur est KO! Game Over possible...");
        }
    }
}
