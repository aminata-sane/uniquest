using UnityEngine;
using System.Collections.Generic;

namespace UniQuest.Map
{
    /// <summary>
    /// Rendu optimisé de la carte avec tileset
    /// </summary>
    public class MapRenderer : MonoBehaviour
    {
        [Header("Map Configuration")]
        public TilesetManager tilesetManager;
        public int mapWidth = 50;
        public int mapHeight = 30;
        public float tileWorldSize = 1f;
        
        [Header("Rendering")]
        public bool useOptimizedRendering = true;
        public int chunkSize = 16; // Taille des chunks pour l'optimisation
        
        [Header("Generation")]
        public bool generateOnStart = true;
        public int seed = 12345;
        
        private Dictionary<Vector2Int, GameObject> tileObjects = new Dictionary<Vector2Int, GameObject>();
        private Transform tilesParent;
        
        private void Start()
        {
            if (generateOnStart)
            {
                GenerateMap();
            }
        }
        
        private void InitializeMap()
        {
            // Créer le parent pour organiser les tuiles
            if (tilesParent == null)
            {
                GameObject parentObj = new GameObject("Map_Tiles");
                parentObj.transform.SetParent(transform);
                tilesParent = parentObj.transform;
            }
        }
        
        [ContextMenu("Generate Random Map")]
        public void GenerateMap()
        {
            if (tilesetManager == null)
            {
                Debug.LogError("TilesetManager non assigné!");
                return;
            }
            
            InitializeMap();
            ClearExistingMap();
            
            Random.InitState(seed);
            
            Debug.Log($"🗺️ Génération de la carte {mapWidth}x{mapHeight}...");
            
            int tilesCreated = 0;
            
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    Vector2Int tilePos = new Vector2Int(x, y);
                    MapTileData tileData = SelectTileForPosition(x, y);
                    
                    if (tileData != null)
                    {
                        CreateTileAt(tilePos, tileData);
                        tilesCreated++;
                    }
                }
            }
            
            Debug.Log($"✅ Carte générée: {tilesCreated} tuiles créées!");
        }
        
        private MapTileData SelectTileForPosition(int x, int y)
        {
            // Logique de sélection de tuile basée sur la position
            // Vous pouvez personnaliser ceci pour créer des patterns
            
            float noiseValue = Mathf.PerlinNoise(x * 0.1f, y * 0.1f);
            
            // Créer des zones avec différents terrains
            if (noiseValue < 0.3f)
            {
                return tilesetManager.GetRandomTileOfType(TerrainType.Water);
            }
            else if (noiseValue < 0.5f)
            {
                return tilesetManager.GetRandomTileOfType(TerrainType.Sand);
            }
            else if (noiseValue < 0.8f)
            {
                return tilesetManager.GetRandomTileOfType(TerrainType.Grass);
            }
            else
            {
                return tilesetManager.GetRandomTileOfType(TerrainType.Stone);
            }
        }
        
        private void CreateTileAt(Vector2Int tilePos, MapTileData tileData)
        {
            // Position dans le monde
            Vector3 worldPos = new Vector3(
                tilePos.x * tileWorldSize, 
                tilePos.y * tileWorldSize, 
                0
            );
            
            // Créer le GameObject de la tuile
            GameObject tileObj = new GameObject($"Tile_{tilePos.x}_{tilePos.y}");
            tileObj.transform.SetParent(tilesParent);
            tileObj.transform.position = worldPos;
            
            // Ajouter le SpriteRenderer
            SpriteRenderer sr = tileObj.AddComponent<SpriteRenderer>();
            sr.sprite = tileData.tileSprite;
            sr.color = tileData.tileColor;
            sr.sortingLayerName = "Ground"; // Assurez-vous que ce layer existe
            
            // Ajouter les composants de gameplay si nécessaire
            if (!tileData.isWalkable)
            {
                BoxCollider2D collider = tileObj.AddComponent<BoxCollider2D>();
                collider.size = Vector2.one * tileWorldSize;
            }
            
            if (tileData.terrainType != TerrainType.Grass)
            {
                TerrainTileComponent terrainComp = tileObj.AddComponent<TerrainTileComponent>();
                terrainComp.Initialize(new TerrainTile 
                { 
                    type = tileData.terrainType,
                    movementSpeedMultiplier = tileData.speedMultiplier,
                    isWalkable = tileData.isWalkable,
                    damagePerSecond = tileData.damagePerSecond,
                    color = tileData.tileColor
                });
            }
            
            // Stocker dans le dictionnaire
            tileObjects[tilePos] = tileObj;
        }
        
        [ContextMenu("Clear Map")]
        public void ClearExistingMap()
        {
            foreach (var tileObj in tileObjects.Values)
            {
                if (tileObj != null)
                {
                    DestroyImmediate(tileObj);
                }
            }
            
            tileObjects.Clear();
            
            // Nettoyer les enfants restants
            if (tilesParent != null)
            {
                for (int i = tilesParent.childCount - 1; i >= 0; i--)
                {
                    DestroyImmediate(tilesParent.GetChild(i).gameObject);
                }
            }
        }
        
        /// <summary>
        /// Change une tuile à une position spécifique
        /// </summary>
        public void SetTileAt(Vector2Int position, MapTileData newTileData)
        {
            if (tileObjects.ContainsKey(position))
            {
                DestroyImmediate(tileObjects[position]);
                tileObjects.Remove(position);
            }
            
            CreateTileAt(position, newTileData);
        }
        
        /// <summary>
        /// Obtient la position de tuile basée sur une position mondiale
        /// </summary>
        public Vector2Int WorldToTilePosition(Vector3 worldPos)
        {
            return new Vector2Int(
                Mathf.FloorToInt(worldPos.x / tileWorldSize),
                Mathf.FloorToInt(worldPos.y / tileWorldSize)
            );
        }
        
        /// <summary>
        /// Obtient la position mondiale d'une tuile
        /// </summary>
        public Vector3 TileToWorldPosition(Vector2Int tilePos)
        {
            return new Vector3(
                tilePos.x * tileWorldSize,
                tilePos.y * tileWorldSize,
                0
            );
        }
    }
}
