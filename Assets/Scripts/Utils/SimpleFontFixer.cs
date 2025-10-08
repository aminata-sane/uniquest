using UnityEngine;
using UnityEngine.UI;

namespace UniQuest.Utils
{
    /// <summary>
    /// Alternative simple au FontAssetFixer - Sans TextMesh Pro
    /// </summary>
    public class SimpleFontFixer : MonoBehaviour
    {
        [Header("Simple Font Fix")]
        [TextArea(3, 5)]
        public string instructions = @"Si vous avez des erreurs de Font:
1. Ce script évite TextMesh Pro complètement
2. Utilise seulement les Text UI standard de Unity
3. Aucune dépendance externe requise";
        
        [ContextMenu("Fix Standard UI Text Components")]
        public void FixStandardUITexts()
        {
            // Chercher tous les Text standard dans la scène
            var textComponents = FindObjectsByType<Text>(FindObjectsSortMode.None);
            
            int fixedCount = 0;
            foreach (var text in textComponents)
            {
                if (text.font == null)
                {
                    // Assigner la font par défaut d'Unity
                    text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                    Debug.Log($"🔧 Font par défaut assignée à '{text.gameObject.name}'");
                    fixedCount++;
                }
            }
            
            Debug.Log($"✅ Vérification terminée pour {textComponents.Length} composants Text, {fixedCount} corrigés");
        }
        
        [ContextMenu("List All Text Components")]
        public void ListAllTextComponents()
        {
            var textComponents = FindObjectsByType<Text>(FindObjectsSortMode.None);
            
            Debug.Log($"📋 {textComponents.Length} composants Text trouvés:");
            
            foreach (var text in textComponents)
            {
                string status = text.font != null ? "✅ OK" : "❌ Pas de font";
                Debug.Log($"  - '{text.gameObject.name}': {status}");
            }
        }
        
        [ContextMenu("Disable Objects with Missing Fonts")]
        public void DisableObjectsWithMissingFonts()
        {
            var textComponents = FindObjectsByType<Text>(FindObjectsSortMode.None);
            
            int disabledCount = 0;
            foreach (var text in textComponents)
            {
                if (text.font == null)
                {
                    text.gameObject.SetActive(false);
                    Debug.Log($"🔇 Désactivé '{text.gameObject.name}' (font manquante)");
                    disabledCount++;
                }
            }
            
            Debug.Log($"✅ {disabledCount} objets avec fonts manquantes désactivés");
        }
    }
}