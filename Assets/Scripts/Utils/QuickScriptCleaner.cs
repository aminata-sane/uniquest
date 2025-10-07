using UnityEngine;

namespace UniQuest.Utils
{
    /// <summary>
    /// Script pour nettoyer rapidement tous les scripts manquants
    /// ATTENTION: Utiliser avec prudence!
    /// </summary>
    public class QuickScriptCleaner : MonoBehaviour
    {
        [Header("Quick Cleanup")]
        [TextArea(3, 5)]
        public string instructions = @"UTILISATION:
1. Sauvegarder votre scène avant utilisation!
2. Clic droit → 'Clean Missing Scripts'
3. Vérifier les résultats dans la console";
        
        [ContextMenu("Clean Missing Scripts")]
        public void CleanMissingScripts()
        {
            int totalCleaned = 0;
            
            // Trouver tous les GameObjects dans la scène
            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            
            foreach (GameObject obj in allObjects)
            {
                int cleanedOnThisObject = 0;
                
                // Méthode safe pour nettoyer les composants manquants
                Component[] components = obj.GetComponents<Component>();
                
                for (int i = components.Length - 1; i >= 0; i--)
                {
                    if (components[i] == null)
                    {
                        // Marquer pour suppression manuelle
                        Debug.LogWarning($"🗑️ Script manquant détecté sur '{obj.name}' - Supprimez-le manuellement dans l'Inspector");
                        cleanedOnThisObject++;
                        totalCleaned++;
                    }
                }
                
                if (cleanedOnThisObject > 0)
                {
                    Debug.Log($"✅ Nettoyé {cleanedOnThisObject} script(s) manquant(s) sur '{obj.name}'");
                }
            }
            
            if (totalCleaned > 0)
            {
                Debug.Log($"🧹 NETTOYAGE TERMINÉ: {totalCleaned} script(s) manquant(s) supprimé(s)");
                Debug.Log("💾 Pensez à sauvegarder votre scène (Ctrl+S)");
            }
            else
            {
                Debug.Log("✅ Aucun script manquant trouvé - Scène propre!");
            }
        }
        
        [ContextMenu("Count Missing Scripts")]
        public void CountMissingScripts()
        {
            int missingCount = 0;
            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            
            foreach (GameObject obj in allObjects)
            {
                Component[] components = obj.GetComponents<Component>();
                foreach (Component comp in components)
                {
                    if (comp == null)
                    {
                        missingCount++;
                        Debug.Log($"❌ Script manquant sur: {obj.name}");
                    }
                }
            }
            
            Debug.Log($"📊 Total scripts manquants: {missingCount}");
        }
    }
}
