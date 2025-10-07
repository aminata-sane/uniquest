using UnityEngine;

namespace UniQuest.Map
{
    /// <summary>
    /// Contrôleur de caméra simple avec suivi du joueur et contrôles manuels
    /// 
    /// CONTRÔLES:
    /// - IJKL : Déplacer la caméra manuellement
    /// - +/- : Zoom in/out
    /// - R : Revenir au player
    /// - T : Toggle suivi du player (on/off)
    /// - Y : Vue d'ensemble (overview)
    /// 
    /// PARAMÈTRES:
    /// - followPlayer : Active/désactive le suivi automatique du joueur
    /// - followSpeed : Vitesse de suivi du joueur
    /// - moveSpeed : Vitesse de déplacement manuel
    /// - cameraDistance : Distance de la caméra (position Z)
    /// </summary>
    public class SimpleCameraController : MonoBehaviour
    {
    [Header("Camera Controls")]
    public Transform player;
    public float followSpeed = 5f;
    public float cameraDistance = 10f;
    public bool followPlayer = false;  // Désactivé par défaut pour ne pas perturber
        
        [Header("Manual Camera Controls")]
        public float moveSpeed = 5f;
        public bool enableManualControl = true;
        
        private void Start()
        {
            // Si pas de player assigné, le trouver automatiquement
            if (player == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                {
                    player = playerObj.transform;
                }
            }
            
            // Ajuster seulement la position Z si nécessaire, garder X et Y actuels
            Vector3 currentPos = transform.position;
            if (Mathf.Abs(currentPos.z) < 1f) // Si la caméra est trop proche du plan Z=0
            {
                transform.position = new Vector3(currentPos.x, currentPos.y, -cameraDistance);
            }
        }
        
        private void Update()
        {
            // Contrôles manuels de la caméra avec les touches IJKL
            if (enableManualControl)
            {
                HandleManualCameraControls();
            }
        }
        
        private void LateUpdate()
        {
            // Suivre le joueur si activé
            if (followPlayer && player != null)
            {
                FollowPlayer();
            }
        }
        
        private void HandleManualCameraControls()
        {
            Vector3 moveDirection = Vector3.zero;
            
            // Utiliser IJKL pour déplacer la caméra
            if (Input.GetKey(KeyCode.I)) moveDirection += Vector3.up;       // I = Haut
            if (Input.GetKey(KeyCode.K)) moveDirection += Vector3.down;     // K = Bas  
            if (Input.GetKey(KeyCode.J)) moveDirection += Vector3.left;     // J = Gauche
            if (Input.GetKey(KeyCode.L)) moveDirection += Vector3.right;    // L = Droite
            
            // Zoom avec + et -
            if (Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Plus))
            {
                Camera.main.orthographicSize = Mathf.Max(1f, Camera.main.orthographicSize - 2f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.KeypadMinus) || Input.GetKey(KeyCode.Minus))
            {
                Camera.main.orthographicSize += 2f * Time.deltaTime;
            }
            
            // Appliquer le mouvement
            if (moveDirection != Vector3.zero)
            {
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
                // Maintenir la distance Z
                transform.position = new Vector3(transform.position.x, transform.position.y, -cameraDistance);
            }
            
            // Touches de raccourci
            if (Input.GetKeyDown(KeyCode.R) && player != null)
            {
                // R = Revenir au player
                transform.position = new Vector3(player.position.x, player.position.y, -cameraDistance);
            }
            
            if (Input.GetKeyDown(KeyCode.T))
            {
                // T = Toggle follow player
                ToggleFollowPlayer();
            }
            
            if (Input.GetKeyDown(KeyCode.Y))
            {
                // Y = Show overview
                ShowOverview();
            }
        }
        
        private void FollowPlayer()
        {
            Vector3 targetPosition = new Vector3(player.position.x, player.position.y, -cameraDistance);
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
        
        // Méthodes utiles
        [ContextMenu("Center on Player")]
        public void CenterOnPlayer()
        {
            if (player != null)
            {
                transform.position = new Vector3(player.position.x, player.position.y, -cameraDistance);
            }
        }
        
        [ContextMenu("Show Overview")]
        public void ShowOverview()
        {
            transform.position = new Vector3(1f, 0f, -cameraDistance); // Position entre Player et obstacle
            Camera.main.orthographicSize = 8f; // Zoom pour voir les deux
        }
        
        [ContextMenu("Toggle Follow Player")]
        public void ToggleFollowPlayer()
        {
            followPlayer = !followPlayer;
            Debug.Log($"Follow Player: {(followPlayer ? "Activé" : "Désactivé")}");
        }
        
        // Méthode appelable depuis l'Inspector pour reset la position
        [ContextMenu("Reset Camera Position")]
        public void ResetCameraPosition()
        {
            transform.position = new Vector3(0, 0, -cameraDistance);
            Camera.main.orthographicSize = 5f; // Taille par défaut
        }
    }
}
