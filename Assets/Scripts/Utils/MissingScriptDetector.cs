using UnityEngine;

namespace UniQuest.Utils
{
    /// <summary>
    /// Utilitaire pour détecter et nettoyer les scripts manquants dans la scène
    /// </summary>
    public class MissingScriptDetector : MonoBehaviour
    {
        [Header("Detection Settings")]
        public bool autoDetectOnStart = true;
        
        [Header("Results")]
        [TextArea(5, 10)]
        public string detectionResults = "Appuyez sur 'Find Missing Scripts' pour analyser";
        
        private void Start()
        {
            if (autoDetectOnStart)
            {
                FindMissingScripts();
            }
        }
        
        [ContextMenu("Find Missing Scripts")]
        public void FindMissingScripts()
        {
            string results = "=== DETECTION SCRIPTS MANQUANTS ===\n\n";
            int missingCount = 0;
            
            // Chercher dans tous les GameObjects de la scène
            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            
            foreach (GameObject obj in allObjects)
            {
                Component[] components = obj.GetComponents<Component>();
                
                for (int i = 0; i < components.Length; i++)
                {
                    if (components[i] == null)
                    {
                        missingCount++;
                        results += $"❌ SCRIPT MANQUANT sur '{obj.name}' (composant #{i})\n";
                        results += $"   Position: {obj.transform.position}\n";
                        results += $"   Parent: {(obj.transform.parent ? obj.transform.parent.name : "Racine")}\n\n";
                    }
                }
            }
            
            if (missingCount == 0)
            {
                results += "✅ Aucun script manquant détecté!\n";
            }
            else
            {
                results += $"\n🚨 TOTAL: {missingCount} script(s) manquant(s) trouvé(s)\n";
                results += "\nSOLUTIONS:\n";
                results += "1. Sélectionner l'objet mentionné\n";
                results += "2. Dans l'Inspector, supprimer le composant 'Missing Script'\n";
                results += "3. Ou réassigner le bon script si vous le connaissez\n";
            }
            
            detectionResults = results;
            Debug.Log(results);
        }
        
        [ContextMenu("List GameObjects with Components")]
        public void ListGameObjectsWithComponents()
        {
            string results = "=== ANALYSE GAMEOBJECTS ===\n\n";
            
            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            
            foreach (GameObject obj in allObjects)
            {
                Component[] components = obj.GetComponents<Component>();
                results += $"📦 {obj.name}:\n";
                
                for (int i = 0; i < components.Length; i++)
                {
                    if (components[i] == null)
                    {
                        results += $"   ❌ [MANQUANT] Position #{i}\n";
                    }
                    else
                    {
                        results += $"   ✅ {components[i].GetType().Name}\n";
                    }
                }
                results += "\n";
            }
            
            detectionResults = results;
            Debug.Log(results);
        }
    }
}
