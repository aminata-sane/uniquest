using UnityEngine;
using System.Collections.Generic;

namespace UniQuest.Map
{
    /// <summary>
    /// Gestionnaire du tileset - Charge et organise les sprites du tileset
    /// </summary>
    [CreateAssetMenu(fileName = "TilesetManager", menuName = "UniQuest/Map/Tileset Manager")]
    public class TilesetManager : ScriptableObject
    {
        [Header("Tileset Configuration")]
        public Texture2D tilesetTexture;
        public int tileSize = 32; // Taille d'une tuile en pixels
        public Vector2Int tilesPerRow = new Vector2Int(8, 8); // Colonnes x Lignes
        
        [Header("Generated Tiles")]
        public List<MapTileData> availableTiles = new List<MapTileData>();
        
        [Header("Quick Access Tiles")]
        public MapTileData grassTile;
        public MapTileData stoneTile;
        public MapTileData waterTile;
        public MapTileData wallTile;
        public MapTileData sandTile;
        
        // Cache des sprites pour éviter la recréation
        private Dictionary<int, Sprite> spriteCache = new Dictionary<int, Sprite>();
        
        /// <summary>
        /// Génère tous les sprites à partir du tileset
        /// </summary>
        [ContextMenu("Generate Tiles from Tileset")]
        public void GenerateTilesFromTileset()
        {
            if (tilesetTexture == null)
            {
                Debug.LogError("Aucun tileset assigné!");
                return;
            }
            
            availableTiles.Clear();
            spriteCache.Clear();
            
            int tilesGenerated = 0;
            
            // Parcourir le tileset et créer les sprites
            for (int y = 0; y < tilesPerRow.y; y++)
            {
                for (int x = 0; x < tilesPerRow.x; x++)
                {
                    int tileIndex = y * tilesPerRow.x + x;
                    
                    // Créer le sprite pour cette tuile
                    Sprite tileSprite = CreateSpriteFromTileset(x, y, tileIndex);
                    
                    if (tileSprite != null)
                    {
                        // Déterminer le type de terrain basé sur la position (exemple)
                        TerrainType terrain = DetermineTerrainType(x, y, tileIndex);
                        bool walkable = DetermineWalkable(x, y, tileIndex);
                        
                        MapTileData tileData = new MapTileData(tileSprite, terrain, walkable);
                        availableTiles.Add(tileData);
                        
                        // Assigner aux tuiles d'accès rapide (exemples)
                        if (tileIndex == 0 && grassTile == null) grassTile = tileData;
                        if (tileIndex == 1 && stoneTile == null) stoneTile = tileData;
                        if (tileIndex == 2 && waterTile == null) waterTile = tileData;
                        
                        tilesGenerated++;
                    }
                }
            }
            
            Debug.Log($"✅ {tilesGenerated} tuiles générées à partir du tileset!");
        }
        
        private Sprite CreateSpriteFromTileset(int x, int y, int index)
        {
            if (spriteCache.ContainsKey(index))
                return spriteCache[index];
                
            try
            {
                // Calculer la position dans le tileset
                Rect spriteRect = new Rect(
                    x * tileSize, 
                    (tilesPerRow.y - 1 - y) * tileSize, // Inverser Y (Unity vs tileset)
                    tileSize, 
                    tileSize
                );
                
                // Créer le sprite
                Sprite sprite = Sprite.Create(
                    tilesetTexture, 
                    spriteRect, 
                    new Vector2(0.5f, 0.5f), // Pivot au centre
                    tileSize
                );
                
                sprite.name = $"Tile_{x}_{y}";
                spriteCache[index] = sprite;
                
                return sprite;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Erreur création sprite [{x},{y}]: {e.Message}");
                return null;
            }
        }
        
        private TerrainType DetermineTerrainType(int x, int y, int index)
        {
            // Logique pour déterminer le type de terrain basé sur la position
            // Vous pouvez personnaliser ceci selon votre tileset
            
            if (index < 8) return TerrainType.Grass;      // Première ligne = herbe
            if (index < 16) return TerrainType.Stone;     // Deuxième ligne = pierre  
            if (index < 24) return TerrainType.Water;     // Troisième ligne = eau
            if (index < 32) return TerrainType.Sand;      // Quatrième ligne = sable
            
            return TerrainType.Grass; // Par défaut
        }
        
        private bool DetermineWalkable(int x, int y, int index)
        {
            // Logique pour déterminer si la tuile est marchable
            // Exemple : les murs ne sont pas marchables
            
            if (y == 0 && x > 4) return false; // Dernières tuiles de la première ligne = murs
            
            return true; // Par défaut marchable
        }
        
        /// <summary>
        /// Obtient une tuile aléatoire du type spécifié
        /// </summary>
        public MapTileData GetRandomTileOfType(TerrainType terrainType)
        {
            var tilesOfType = availableTiles.FindAll(t => t.terrainType == terrainType);
            
            if (tilesOfType.Count > 0)
            {
                return tilesOfType[Random.Range(0, tilesOfType.Count)];
            }
            
            return grassTile; // Fallback
        }
        
        /// <summary>
        /// Obtient une tuile par index
        /// </summary>
        public MapTileData GetTileByIndex(int index)
        {
            if (index >= 0 && index < availableTiles.Count)
            {
                return availableTiles[index];
            }
            
            return grassTile; // Fallback
        }
    }
}
