using UnityEngine;

namespace UniQuest.Debug
{
    /// <summary>
    /// Helper pour déboguer et tester la caméra
    /// Affiche des informations sur la position de la caméra et les objets dans la scène
    /// </summary>
    public class CameraDebugHelper : MonoBehaviour
    {
        [Header("Debug Settings")]
        public bool showDebugInfo = true;
        public bool showGizmos = true;
        
        private Camera mainCamera;
        private Transform player;
        
        private void Start()
        {
            mainCamera = Camera.main;
            
            // Trouver le player
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
        
        private void OnGUI()
        {
            if (!showDebugInfo) return;
            
            GUILayout.BeginArea(new Rect(10, 10, 300, 200));
            GUILayout.Label("=== DEBUG CAMÉRA ===");
            
            if (mainCamera != null)
            {
                GUILayout.Label($"Position Caméra: {mainCamera.transform.position}");
                GUILayout.Label($"Orthographic Size: {mainCamera.orthographicSize:F1}");
            }
            
            if (player != null)
            {
                GUILayout.Label($"Position Player: {player.position}");
                float distance = Vector3.Distance(mainCamera.transform.position, player.position);
                GUILayout.Label($"Distance au Player: {distance:F1}");
            }
            
            GUILayout.Space(10);
            GUILayout.Label("CONTRÔLES CAMÉRA:");
            GUILayout.Label("IJKL = Déplacer");
            GUILayout.Label("+/- = Zoom");
            GUILayout.Label("R = Revenir au Player");
            GUILayout.Label("T = Toggle Suivi");
            GUILayout.Label("Y = Vue d'ensemble");
            
            GUILayout.EndArea();
        }
        
        private void OnDrawGizmos()
        {
            if (!showGizmos) return;
            
            // Dessiner une croix au centre de la caméra
            if (mainCamera != null)
            {
                Gizmos.color = Color.red;
                Vector3 camPos = mainCamera.transform.position;
                Gizmos.DrawLine(camPos + Vector3.left * 0.5f, camPos + Vector3.right * 0.5f);
                Gizmos.DrawLine(camPos + Vector3.up * 0.5f, camPos + Vector3.down * 0.5f);
            }
            
            // Dessiner une ligne entre la caméra et le player
            if (mainCamera != null && player != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(mainCamera.transform.position, player.position);
            }
        }
    }
}
