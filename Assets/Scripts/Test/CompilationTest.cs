using UnityEngine;

namespace UniQuest.Test
{
    /// <summary>
    /// Script minimal pour tester la compilation Unity
    /// </summary>
    public class CompilationTest : MonoBehaviour
    {
        [Header("Test de Compilation")]
        public string status = "Script de test - Si vous voyez ceci, la compilation fonctionne";
        
        private void Start()
        {
            Debug.Log("✅ Compilation Test - Unity compile correctement !");
        }
        
        [ContextMenu("Test API Unity")]
        public void TestUnityAPI()
        {
            // Test des API Unity courantes
            Camera cam = Camera.main;
            if (cam != null)
            {
                Debug.Log($"✅ Camera trouvée: {cam.name}");
            }
            
            // Test FindFirstObjectByType (Unity 6)
            var testObj = FindFirstObjectByType<MonoBehaviour>();
            if (testObj != null)
            {
                Debug.Log($"✅ FindFirstObjectByType fonctionne: {testObj.name}");  
            }
            
            // Test GameObject
            GameObject go = new GameObject("TestObject");
            Destroy(go);
            Debug.Log("✅ GameObject Create/Destroy fonctionne");
            
            Debug.Log("✅ Tous les tests API passés !");
        }
    }
}
