using UnityEngine;
#if UNITY_TEXTMESHPRO
using TMPro;
#endif

namespace UniQuest.Utils
{
    /// <summary>
    /// Utilitaire pour corriger les problèmes de Font Asset TextMesh Pro
    /// </summary>
    public class FontAssetFixer : MonoBehaviour
    {
        [Header("TextMesh Pro Fix")]
        [TextArea(3, 5)]
        public string instructions = @"Si vous voyez 'No Font Asset assigned':
1. Window → TextMeshPro → Import TMP Essential Resources
2. Ou désactiver ce GameObject temporairement";
        
        [ContextMenu("Import TMP Essential Resources")]
        public void ImportTMPResources()
        {
            Debug.Log("💡 Allez dans Window → TextMeshPro → Import TMP Essential Resources");
            Debug.Log("Ou dans Package Manager → TextMeshPro → Import samples");
        }
        
        [ContextMenu("Disable TextMesh Pro Components")]
        public void DisableAllTMPComponents()
        {
#if UNITY_TEXTMESHPRO
            // Chercher tous les TextMeshPro dans la scène
            var tmpComponents = FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);
            
            int disabledCount = 0;
            foreach (var tmp in tmpComponents)
            {
                if (tmp.font == null)
                {
                    tmp.gameObject.SetActive(false);
                    Debug.Log($"🔇 Désactivé GameObject '{tmp.gameObject.name}' (pas de font assignée)");
                    disabledCount++;
                }
            }
            
            Debug.Log($"✅ Vérification terminée pour {tmpComponents.Length} composants TextMesh Pro, {disabledCount} désactivés");
#else
            Debug.LogWarning("⚠️ TextMesh Pro n'est pas installé dans ce projet");
            Debug.Log("💡 Pour installer: Window → Package Manager → TextMeshPro → Install");
#endif
        }
    }
}