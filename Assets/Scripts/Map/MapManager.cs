using UnityEngine;
using UniQuest.Core;

namespace UniQuest.Map
{
    public class MapManager : MonoBehaviour
    {
        [Header("Map Settings")]
        public Transform playerSpawnPoint;
        public Camera mainCamera;
        
        [Header("Map Bounds")]
        public float mapWidth = 20f;
        public float mapHeight = 20f;
        public Vector2 mapCenter = Vector2.zero;
        
        [Header("References")]
        public Characters.Player player;
        
        private void Start()
        {
            InitializeMap();
        }
        
        private void InitializeMap()
        {
            // Spawner le joueur si un point de spawn est défini
            if (playerSpawnPoint != null && player != null)
            {
                player.SetPosition(playerSpawnPoint.position);
                Debug.Log("Joueur spawné à la position: " + playerSpawnPoint.position);
            }
            
            // Configurer la caméra pour suivre le joueur
            if (mainCamera != null && player != null)
            {
                SetupCameraFollow();
            }
            
            Debug.Log("Map initialisée!");
        }
        
        private void SetupCameraFollow()
        {
            // Ajouter le script CameraFollow à la caméra
            CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
            if (cameraFollow == null)
            {
                cameraFollow = mainCamera.gameObject.AddComponent<CameraFollow>();
            }
            
            cameraFollow.target = player.transform;
        }
        
        private void Update()
        {
            // Vérifier que le joueur reste dans les limites de la carte
            if (player != null)
            {
                KeepPlayerInBounds();
            }
        }
        
        private void KeepPlayerInBounds()
        {
            Vector3 playerPos = player.GetPosition();
            
            // Calculer les limites
            float minX = mapCenter.x - mapWidth / 2f;
            float maxX = mapCenter.x + mapWidth / 2f;
            float minY = mapCenter.y - mapHeight / 2f;
            float maxY = mapCenter.y + mapHeight / 2f;
            
            // Contraindre la position
            playerPos.x = Mathf.Clamp(playerPos.x, minX, maxX);
            playerPos.y = Mathf.Clamp(playerPos.y, minY, maxY);
            
            // Appliquer la position corrigée
            player.SetPosition(playerPos);
        }
        
        // Méthode utile pour téléporter le joueur
        public void TeleportPlayer(Vector3 newPosition)
        {
            if (player != null)
            {
                player.SetPosition(newPosition);
                Debug.Log($"Joueur téléporté à: {newPosition}");
            }
        }
        
        // Visualiser les limites de la carte dans l'éditeur
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(mapCenter, new Vector3(mapWidth, mapHeight, 0));
        }
    }
}
