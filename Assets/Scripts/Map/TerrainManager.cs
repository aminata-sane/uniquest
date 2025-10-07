using UnityEngine;
using System.Collections.Generic;

namespace UniQuest.Map
{
    public class TerrainManager : MonoBehaviour
    {
        [Header("Terrain Generation")]
        public int mapWidth = 20;
        public int mapHeight = 15;
        public float tileSize = 1f;

        [Header("Terrain Types")]
        public TerrainTile[] terrainTypes;

        [Header("Generation Settings")]
        public bool generateOnStart = true;
        public bool showDebugInfo = true;

        private Dictionary<Vector2Int, TerrainType> terrainMap;
        private Transform terrainParent;

        private void Start()
        {
            if (generateOnStart)
            {
                GenerateMap();
            }
        }

        public void GenerateMap()
        {
            InitializeTerrainSystem();
            GenerateBasicMap();
            if (showDebugInfo)
            {
                Debug.Log($"Map générée: {mapWidth}x{mapHeight} tiles");
            }
        }

        private void InitializeTerrainSystem()
        {
            // Créer un parent pour organiser les terrains
            GameObject terrainContainer = new GameObject("Terrain");
            terrainContainer.transform.parent = transform;
            terrainParent = terrainContainer.transform;

            // Initialiser la map
            terrainMap = new Dictionary<Vector2Int, TerrainType>();

            // Vérifier les types de terrain
            if (terrainTypes == null || terrainTypes.Length == 0)
            {
                CreateDefaultTerrainTypes();
            }
        }

        private void CreateDefaultTerrainTypes()
        {
            terrainTypes = new TerrainTile[]
            {
                new TerrainTile 
                { 
                    type = TerrainType.Grass, 
                    color = new Color(0.3f, 0.8f, 0.3f), 
                    movementSpeedMultiplier = 1f,
                    isWalkable = true 
                },
                new TerrainTile 
                { 
                    type = TerrainType.Stone, 
                    color = Color.gray, 
                    movementSpeedMultiplier = 0.8f,
                    isWalkable = true 
                },
                new TerrainTile 
                { 
                    type = TerrainType.Water, 
                    color = new Color(0.2f, 0.4f, 0.8f), 
                    movementSpeedMultiplier = 0.5f,
                    isWalkable = true 
                },
                new TerrainTile 
                { 
                    type = TerrainType.Lava, 
                    color = new Color(1f, 0.3f, 0f), 
                    movementSpeedMultiplier = 0.3f,
                    isWalkable = true,
                    damagePerSecond = 10 
                }
            };
        }

        private void GenerateBasicMap()
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    Vector2Int gridPos = new Vector2Int(x, y);
                    TerrainType terrainType = DetermineTerrainType(x, y);
                    
                    CreateTerrainTile(gridPos, terrainType);
                    terrainMap[gridPos] = terrainType;
                }
            }
        }

        private TerrainType DetermineTerrainType(int x, int y)
        {
            // Logique simple de génération de terrain
            
            // Bordures de la map = murs/pierres
            if (x == 0 || y == 0 || x == mapWidth - 1 || y == mapHeight - 1)
            {
                return TerrainType.Stone;
            }

            // Zone d'eau au centre
            int centerX = mapWidth / 2;
            int centerY = mapHeight / 2;
            float distanceFromCenter = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
            
            if (distanceFromCenter < 2f)
            {
                return TerrainType.Water;
            }

            // Quelques zones de lave (danger)
            if ((x == 3 && y == 3) || (x == mapWidth - 4 && y == mapHeight - 4))
            {
                return TerrainType.Lava;
            }

            // Le reste = herbe
            return TerrainType.Grass;
        }

        private void CreateTerrainTile(Vector2Int gridPos, TerrainType terrainType)
        {
            // Trouver les données du terrain
            TerrainTile terrainData = GetTerrainData(terrainType);
            if (terrainData == null) return;

            // Créer le GameObject
            GameObject tile = new GameObject($"Terrain_{terrainType}_{gridPos.x}_{gridPos.y}");
            tile.transform.parent = terrainParent;

            // Position mondiale
            Vector3 worldPos = new Vector3(gridPos.x * tileSize, gridPos.y * tileSize, 0);
            tile.transform.position = worldPos;

            // Ajouter SpriteRenderer
            SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
            sr.sprite = CreateSquareSprite();
            sr.color = terrainData.color;
            sr.sortingOrder = -1; // Derrière les autres objets

            // Ajouter le component TerrainTile
            TerrainTileComponent tileComponent = tile.AddComponent<TerrainTileComponent>();
            tileComponent.Initialize(terrainData);

            // Ajouter collider si nécessaire
            if (!terrainData.isWalkable)
            {
                BoxCollider2D collider = tile.AddComponent<BoxCollider2D>();
                collider.size = Vector2.one * tileSize;
            }
            else if (terrainData.type == TerrainType.Water || terrainData.type == TerrainType.Lava)
            {
                // Trigger pour les effets spéciaux
                BoxCollider2D trigger = tile.AddComponent<BoxCollider2D>();
                trigger.size = Vector2.one * tileSize;
                trigger.isTrigger = true;
            }
        }

        private TerrainTile GetTerrainData(TerrainType type)
        {
            foreach (var terrain in terrainTypes)
            {
                if (terrain.type == type)
                    return terrain;
            }
            return null;
        }

        private Sprite CreateSquareSprite()
        {
            // Utiliser le sprite carré par défaut d'Unity
            return Resources.GetBuiltinResource<Sprite>("UI/Skin/Background.psd");
        }

        // Méthodes utilitaires
        public TerrainType GetTerrainAt(Vector2 worldPosition)
        {
            Vector2Int gridPos = WorldToGrid(worldPosition);
            return terrainMap.ContainsKey(gridPos) ? terrainMap[gridPos] : TerrainType.Grass;
        }

        public Vector2Int WorldToGrid(Vector2 worldPosition)
        {
            return new Vector2Int(
                Mathf.FloorToInt(worldPosition.x / tileSize),
                Mathf.FloorToInt(worldPosition.y / tileSize)
            );
        }

        public Vector2 GridToWorld(Vector2Int gridPosition)
        {
            return new Vector2(gridPosition.x * tileSize, gridPosition.y * tileSize);
        }

        public float GetMovementMultiplierAt(Vector2 worldPosition)
        {
            TerrainType terrainType = GetTerrainAt(worldPosition);
            TerrainTile terrainData = GetTerrainData(terrainType);
            return terrainData?.movementSpeedMultiplier ?? 1f;
        }

        // Pour le level design - placer des obstacles
        public void PlaceObstacle(Vector2Int gridPos, ObstacleType obstacleType)
        {
            Vector3 worldPos = new Vector3(gridPos.x * tileSize, gridPos.y * tileSize, 0);
            
            GameObject obstacle = new GameObject($"Obstacle_{obstacleType}_{gridPos.x}_{gridPos.y}");
            obstacle.transform.position = worldPos;
            obstacle.transform.parent = transform;

            WallObstacle wallComponent = obstacle.AddComponent<WallObstacle>();
            wallComponent.SetObstacleType(obstacleType);
        }

        // Debug - afficher la grille dans l'éditeur
        private void OnDrawGizmos()
        {
            if (!showDebugInfo) return;

            Gizmos.color = Color.white;
            for (int x = 0; x <= mapWidth; x++)
            {
                Vector3 start = new Vector3(x * tileSize, 0, 0);
                Vector3 end = new Vector3(x * tileSize, mapHeight * tileSize, 0);
                Gizmos.DrawLine(start, end);
            }

            for (int y = 0; y <= mapHeight; y++)
            {
                Vector3 start = new Vector3(0, y * tileSize, 0);
                Vector3 end = new Vector3(mapWidth * tileSize, y * tileSize, 0);
                Gizmos.DrawLine(start, end);
            }
        }
    }

    // Types TerrainTile et TerrainType déplacés vers TerrainTypes.cs
}
