using UnityEngine;

namespace UniQuest.Utils
{
    /// <summary>
    /// Script utilitaire pour créer rapidement des objets de test
    /// </summary>
    public class QuickTestSetup : MonoBehaviour
    {
        [Header("Quick Setup Info")]
        public string info = "Utiliser le menu contextuel (clic droit) pour les actions";
        
        [ContextMenu("Create Test Player")]
        public void CreateTestPlayer()
        {
            // Créer un GameObject Player
            GameObject player = new GameObject("Player");
            player.tag = "Player";
            
            // Ajouter un SpriteRenderer
            SpriteRenderer sr = player.AddComponent<SpriteRenderer>();
            sr.sprite = CreateSimpleSprite(Color.blue);
            
            // Ajouter Rigidbody2D
            Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            
            // Ajouter BoxCollider2D
            BoxCollider2D col = player.AddComponent<BoxCollider2D>();
            col.size = new Vector2(0.8f, 0.8f);
            
            // Ajouter le script Player
            player.AddComponent<Characters.Player>();
            
            // Positionner
            player.transform.position = Vector3.zero;
            
            Debug.Log("✅ Player de test créé!");
        }
        
        [ContextMenu("Create Test Wall")]
        public void CreateTestWall()
        {
            // Créer un mur de test
            GameObject wall = new GameObject("TestWall");
            wall.transform.position = new Vector3(2f, 0f, 0f);
            wall.AddComponent<Map.WallObstacle>();
            
            Debug.Log("✅ Mur de test créé!");
        }
        
        [ContextMenu("Setup Main Camera")]
        public void SetupMainCamera()
        {
            Camera mainCam = Camera.main;
            if (mainCam == null)
            {
                mainCam = FindFirstObjectByType<Camera>();
            }
            
            if (mainCam != null)
            {
                // Ajouter SimpleCameraController s'il n'existe pas
                if (mainCam.GetComponent<Map.SimpleCameraController>() == null)
                {
                    mainCam.gameObject.AddComponent<Map.SimpleCameraController>();
                }
                
                // Configurer la caméra pour le 2D
                mainCam.orthographic = true;
                mainCam.orthographicSize = 5f;
                mainCam.transform.position = new Vector3(0f, 0f, -10f);
                
                Debug.Log("✅ Caméra configurée!");
            }
            else
            {
                Debug.LogError("❌ Aucune caméra trouvée!");
            }
        }
        
        private Sprite CreateSimpleSprite(Color color)
        {
            Texture2D texture = new Texture2D(32, 32);
            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 32; y++)
                {
                    texture.SetPixel(x, y, color);
                }
            }
            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
        }
    }
}
