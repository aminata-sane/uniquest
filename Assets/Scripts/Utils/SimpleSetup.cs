using UnityEngine;

namespace UniQuest.Utils
{
    /// <summary>
    /// Version simplifiée du setup rapide - sans erreurs de compilation
    /// </summary>
    public class SimpleSetup : MonoBehaviour
    {
        [Header("Setup Utilities")]
        public bool showInstructions = true;
        
        [Header("Instructions")]
        [TextArea(3, 6)]
        public string instructions = @"UTILISATION:
1. Clic droit sur ce script dans l'Inspector
2. Choisir une action dans le menu contextuel:
   - Create Test Player
   - Create Test Wall  
   - Setup Main Camera";

        [ContextMenu("Create Test Player")]
        public void CreateTestPlayer()
        {
            CreatePlayer();
        }
        
        [ContextMenu("Create Test Wall")]
        public void CreateTestWall()
        {
            CreateWall();
        }
        
        [ContextMenu("Setup Main Camera")]
        public void SetupMainCamera()
        {
            ConfigureCamera();
        }
        
        private void CreatePlayer()
        {
            // Vérifier si un Player existe déjà
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                Debug.LogWarning("⚠️ Un Player existe déjà dans la scène!");
                return;
            }
            
            // Créer le Player
            GameObject player = new GameObject("Player");
            player.tag = "Player";
            player.transform.position = Vector3.zero;
            
            // Components
            var sr = player.AddComponent<SpriteRenderer>();
            sr.sprite = CreateSprite(Color.blue, 32);
            
            var rb = player.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            
            var col = player.AddComponent<BoxCollider2D>();
            col.size = new Vector2(0.8f, 0.8f);
            
            // Script Player
            player.AddComponent<Characters.Player>();
            
            Debug.Log("✅ Player créé avec succès!");
        }
        
        private void CreateWall()
        {
            GameObject wall = new GameObject("TestWall");
            wall.transform.position = new Vector3(2f, 0f, 0f);
            wall.AddComponent<Map.WallObstacle>();
            
            Debug.Log("✅ Mur de test créé!");
        }
        
        private void ConfigureCamera()
        {
            Camera cam = Camera.main ?? FindFirstObjectByType<Camera>();
            
            if (cam == null)
            {
                Debug.LogError("❌ Aucune caméra trouvée!");
                return;
            }
            
            // Configuration caméra 2D
            cam.orthographic = true;
            cam.orthographicSize = 5f;
            cam.transform.position = new Vector3(0f, 0f, -10f);
            
            // Ajouter SimpleCameraController
            if (cam.GetComponent<Map.SimpleCameraController>() == null)
            {
                cam.gameObject.AddComponent<Map.SimpleCameraController>();
            }
            
            Debug.Log("✅ Caméra configurée avec SimpleCameraController!");
        }
        
        private Sprite CreateSprite(Color color, int size)
        {
            Texture2D texture = new Texture2D(size, size);
            
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    texture.SetPixel(x, y, color);
                }
            }
            
            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
        }
    }
}
