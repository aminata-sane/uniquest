using UnityEngine;
using System.Collections.Generic;

namespace UniQuest.Map
{
    /// <summary>
    /// Outil pour identifier et placer des obstacles en cliquant sur la carte
    /// </summary>
    public class ObstaclePlacer : MonoBehaviour
    {
        [Header("Obstacle Placement")]
        public bool placementMode = true;
        public GameObject obstaclePreview;
        
        [Header("Identified Positions")]
        [Tooltip("Positions identifiées en cliquant sur la carte")]
        public List<Vector2> identifiedPositions = new List<Vector2>();
        
        [Header("Visual Helpers")]
        public Color previewColor = Color.red;
        public float previewSize = 0.5f;
        
        private Camera mainCamera;
        private List<GameObject> previewObjects = new List<GameObject>();
        
        void Start()
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
                mainCamera = FindFirstObjectByType<Camera>();
            
            if (mainCamera != null)
            {
                Debug.Log("🎯 ObstaclePlacer activé! Cliquez sur la carte pour identifier les positions d'obstacles.");
                Debug.Log("📋 Instructions:");
                Debug.Log("   - Clic gauche: Ajouter obstacle");
                Debug.Log("   - Clic droit: Supprimer obstacle proche");
                Debug.Log("   - Espace: Afficher/masquer les positions");
                Debug.Log($"📷 Caméra trouvée: {mainCamera.name}");
            }
            else
            {
                Debug.LogError("❌ Aucune caméra trouvée! ObstaclePlacer ne peut pas fonctionner.");
                enabled = false;
            }
        }
        
        void Update()
        {
            if (!placementMode) return;
            
            HandleInput();
        }
        
        void HandleInput()
        {
            // Clic gauche pour ajouter un obstacle
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 worldPos = GetMouseWorldPosition();
                AddObstaclePosition(worldPos);
            }
            
            // Clic droit pour supprimer un obstacle proche
            if (Input.GetMouseButtonDown(1))
            {
                Vector2 worldPos = GetMouseWorldPosition();
                RemoveNearestObstacle(worldPos);
            }
            
            // Espace pour basculer l'affichage
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TogglePreviewVisibility();
            }
            
            // Afficher la position de la souris
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                Vector2 mousePos = GetMouseWorldPosition();
                Debug.Log($"🎯 Position souris: ({mousePos.x:F1}, {mousePos.y:F1})");
            }
        }
        
        Vector2 GetMouseWorldPosition()
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
            return new Vector2(mouseWorldPos.x, mouseWorldPos.y);
        }
        
        void AddObstaclePosition(Vector2 position)
        {
            // Arrondir à 0.5 pour un placement plus précis
            Vector2 roundedPos = new Vector2(
                Mathf.Round(position.x * 2f) / 2f,
                Mathf.Round(position.y * 2f) / 2f
            );
            
            // Éviter les doublons
            if (!identifiedPositions.Contains(roundedPos))
            {
                identifiedPositions.Add(roundedPos);
                CreatePreviewObstacle(roundedPos);
                Debug.Log($"✅ Obstacle ajouté à ({roundedPos.x:F1}, {roundedPos.y:F1}) - Total: {identifiedPositions.Count}");
            }
        }
        
        void RemoveNearestObstacle(Vector2 position)
        {
            if (identifiedPositions.Count == 0) return;
            
            float minDistance = float.MaxValue;
            int nearestIndex = -1;
            
            for (int i = 0; i < identifiedPositions.Count; i++)
            {
                float distance = Vector2.Distance(position, identifiedPositions[i]);
                if (distance < minDistance && distance < 1f) // Dans un rayon de 1 unité
                {
                    minDistance = distance;
                    nearestIndex = i;
                }
            }
            
            if (nearestIndex >= 0)
            {
                Vector2 removedPos = identifiedPositions[nearestIndex];
                identifiedPositions.RemoveAt(nearestIndex);
                
                // Supprimer l'aperçu visuel
                if (nearestIndex < previewObjects.Count)
                {
                    DestroyImmediate(previewObjects[nearestIndex]);
                    previewObjects.RemoveAt(nearestIndex);
                }
                
                Debug.Log($"❌ Obstacle supprimé à ({removedPos.x:F1}, {removedPos.y:F1}) - Restant: {identifiedPositions.Count}");
            }
        }
        
        void CreatePreviewObstacle(Vector2 position)
        {
            GameObject preview = new GameObject($"Preview_{identifiedPositions.Count - 1}");
            preview.transform.position = new Vector3(position.x, position.y, -1f);
            preview.transform.SetParent(transform);
            
            // Ajouter un sprite renderer pour la visualisation
            SpriteRenderer sr = preview.AddComponent<SpriteRenderer>();
            sr.sprite = CreatePreviewSprite();
            sr.color = previewColor;
            sr.sortingOrder = 10; // Au-dessus de tout
            
            previewObjects.Add(preview);
        }
        
        Sprite CreatePreviewSprite()
        {
            Texture2D texture = new Texture2D(16, 16);
            Color[] pixels = new Color[16 * 16];
            
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = Color.white;
            }
            
            texture.SetPixels(pixels);
            texture.Apply();
            
            return Sprite.Create(texture, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f));
        }
        
        void TogglePreviewVisibility()
        {
            foreach (var preview in previewObjects)
            {
                if (preview != null)
                    preview.SetActive(!preview.activeSelf);
            }
        }
        
        [ContextMenu("Export Positions to ObstacleGenerator")]
        public void ExportPositions()
        {
            ObstacleGenerator generator = FindFirstObjectByType<ObstacleGenerator>();
            if (generator != null)
            {
                generator.obstaclePositions = identifiedPositions.ToArray();
                Debug.Log($"✅ {identifiedPositions.Count} positions exportées vers ObstacleGenerator!");
            }
            else
            {
                Debug.LogError("❌ ObstacleGenerator introuvable! Créez d'abord un ObstacleManager avec le script ObstacleGenerator.");
            }
        }
        
        [ContextMenu("Clear All Positions")]
        public void ClearAllPositions()
        {
            identifiedPositions.Clear();
            
            foreach (var preview in previewObjects)
            {
                if (preview != null)
                    DestroyImmediate(preview);
            }
            previewObjects.Clear();
            
            Debug.Log("🧹 Toutes les positions supprimées!");
        }
        
        [ContextMenu("Print Positions for Code")]
        public void PrintPositionsForCode()
        {
            Debug.Log("📋 Positions pour le code:");
            Debug.Log("new Vector2[] {");
            foreach (var pos in identifiedPositions)
            {
                Debug.Log($"    new Vector2({pos.x:F1}f, {pos.y:F1}f),");
            }
            Debug.Log("};");
        }
        
        void OnDisable()
        {
            Debug.Log($"🎯 ObstaclePlacer désactivé. {identifiedPositions.Count} positions identifiées.");
        }
    }
}