using UnityEngine;

namespace UniQuest.Utils
{
    public class PlayerDamageTest : MonoBehaviour
    {
        [Header("Test Damage Effect")]
        public KeyCode damageTestKey = KeyCode.Space;
        public float testDamage = 10f;
        
        private UniQuest.Characters.Player player;
        
        void Start()
        {
            // Trouver le joueur
            player = FindFirstObjectByType<UniQuest.Characters.Player>();
            
            if (player != null)
            {
                Debug.Log("🧪 Test des dégâts activé ! Appuie sur ESPACE pour tester l'effet de flash.");
            }
        }
        
        void Update()
        {
            // Test de l'effet de dégâts avec la barre d'espace
            if (Input.GetKeyDown(damageTestKey) && player != null)
            {
                Debug.Log($"💥 Test dégâts : {testDamage} points !");
                player.TakeDamage(testDamage);
            }
        }
        
        void OnGUI()
        {
            // Affichage des instructions à l'écran
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = 16;
            
            GUI.Label(new Rect(10, 10, 300, 20), "Appuie sur ESPACE pour tester l'effet de dégâts", style);
        }
    }
}