using UnityEngine;
using System.Linq;

namespace UniQuest.Diagnostic
{
    /// <summary>
    /// Script de diagnostic pour identifier les problèmes de compilation
    /// </summary>
    public class UnityDiagnostic : MonoBehaviour
    {
        [Header("Diagnostic Unity")]
        [TextArea(5, 10)]
        public string diagnosticResults = "Appuyez sur 'Run Diagnostic' pour analyser le projet";
        
        [ContextMenu("Run Diagnostic")]
        public void RunDiagnostic()
        {
            string results = "";
            
            // Test 1: Version Unity
            results += $"Unity Version: {Application.unityVersion}\n";
            
            // Test 2: Namespaces en conflit
            results += "\n=== SCRIPTS DETECTES ===\n";
            // Compter les scripts (sans les énumérer pour éviter les erreurs)
            int scriptCount = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).Length;
            results += $"Nombre de scripts actifs: {scriptCount}\n";
            
            // Test 3: Caméras
            results += "\n=== CAMERAS ===\n";
            var cameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
            foreach (var cam in cameras)
            {
                results += $"- {cam.name}: {cam.orthographic} (Size: {cam.orthographicSize})\n";
            }
            
            // Test 4: GameObjects avec tag Player
            results += "\n=== PLAYERS ===\n";
            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in players)
            {
                results += $"- {player.name} à {player.transform.position}\n";
            }
            
            // Test 5: Composants critiques
            results += "\n=== COMPOSANTS ===\n";
            if (FindFirstObjectByType<Characters.Player>() != null)
                results += "✅ Player script trouvé\n";
            else
                results += "❌ Player script manquant\n";
                
            if (FindFirstObjectByType<Map.WallObstacle>() != null)
                results += "✅ WallObstacle script trouvé\n";
            else
                results += "❌ WallObstacle script manquant\n";
                
            if (FindFirstObjectByType<Map.SimpleCameraController>() != null)
                results += "✅ SimpleCameraController trouvé\n";
            else
                results += "❌ SimpleCameraController manquant\n";
            
            diagnosticResults = results;
            Debug.Log("=== DIAGNOSTIC COMPLET ===\n" + results);
        }
        
        private void Start()
        {
            Debug.Log("🔍 UnityDiagnostic chargé. Utilisez le menu contextuel pour lancer le diagnostic.");
        }
    }
}
