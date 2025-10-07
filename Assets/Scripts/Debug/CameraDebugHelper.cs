using UnityEngine;

namespace UniQuest.Debug
{
    /// <summary>
    /// Helper simple pour déboguer la caméra - version allégée
    /// </summary>
    public class CameraDebugHelper : MonoBehaviour
    {
        [Header("Debug Settings")]
        public bool showDebugInfo = true;
        
        private Camera mainCamera;
        private Transform player;
        
        private void Start()
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
                mainCamera = FindFirstObjectByType<Camera>();
            
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
            if (mainCamera == null) return;
            
            GUI.Box(new Rect(10, 10, 300, 150), "");
            GUILayout.BeginArea(new Rect(15, 15, 290, 140));
            
            GUILayout.Label("=== DEBUG CAMERA ===");
            GUILayout.Label($"Position: {mainCamera.transform.position:F1}");
            GUILayout.Label($"Zoom: {mainCamera.orthographicSize:F1}");
            
            if (player != null)
            {
                GUILayout.Label($"Player: {player.position:F1}");
                float distance = Vector3.Distance(mainCamera.transform.position, player.position);
                GUILayout.Label($"Distance: {distance:F1}");
            }
            
            GUILayout.Space(5);
            GUILayout.Label("Controles: IJKL +/- R T Y");
            
            GUILayout.EndArea();
        }
        
        // Version simplifiée des gizmos
        private void OnDrawGizmos()
        {
            if (mainCamera != null)
            {
                Gizmos.color = Color.red;
                Vector3 pos = mainCamera.transform.position;
                Gizmos.DrawWireCube(pos, Vector3.one * 0.5f);
            }
        }
    }
}
