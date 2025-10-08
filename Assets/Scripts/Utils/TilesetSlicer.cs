using UnityEngine;
using UniQuest.Map; // Pour accéder à TerrainType

namespace UniQuest.Utils
{
    /// <summary>
    /// Utilitaire pour découper automatiquement un tileset sans Sprite Editor
    /// </summary>
    public class TilesetSlicer : MonoBehaviour
    {
        [Header("Tileset Configuration")]
        public Texture2D tilesetTexture;
        public int tileSize = 32;
        public int tilesPerRow = 8;
        public int tilesPerColumn = 8;
        
        [Header("Output")]
        public Sprite[] generatedSprites;
        
        [ContextMenu("Slice Tileset Automatically")]
        public void SliceTilesetAutomatically()
        {
            if (tilesetTexture == null)
            {
                Debug.LogError("❌ Aucun tileset assigné!");
                return;
            }
            
            Debug.Log($"🔪 Découpage du tileset {tilesetTexture.name}...");
            
            // Créer le tableau de sprites
            int totalTiles = tilesPerRow * tilesPerColumn;
            generatedSprites = new Sprite[totalTiles];
            
            int spritesCreated = 0;
            
            // Découper chaque tuile
            for (int y = 0; y < tilesPerColumn; y++)
            {
                for (int x = 0; x < tilesPerRow; x++)
                {
                    int index = y * tilesPerRow + x;
                    
                    // Calculer la position dans la texture
                    // Note: Unity utilise le coin bas-gauche comme origine
                    Rect spriteRect = new Rect(
                        x * tileSize,                           // X
                        tilesetTexture.height - ((y + 1) * tileSize), // Y (inversé)
                        tileSize,                               // Width
                        tileSize                                // Height
                    );
                    
                    // Créer le sprite
                    Sprite newSprite = Sprite.Create(
                        tilesetTexture,
                        spriteRect,
                        new Vector2(0.5f, 0.5f), // Pivot au centre
                        tileSize,                 // Pixels per unit
                        0,                        // Extrude
                        SpriteMeshType.FullRect   // Mesh type
                    );
                    
                    newSprite.name = $"{tilesetTexture.name}_Tile_{x}_{y}";
                    generatedSprites[index] = newSprite;
                    spritesCreated++;
                }
            }
            
            Debug.Log($"✅ {spritesCreated} sprites créés à partir du tileset!");
            Debug.Log($"💡 Vous pouvez maintenant utiliser generatedSprites[] dans votre TilesetManager");
        }
        
        [ContextMenu("Create TilesetManager with Generated Sprites")]
        public void CreateTilesetManagerWithSprites()
        {
            if (generatedSprites == null || generatedSprites.Length == 0)
            {
                Debug.LogError("❌ Aucun sprite généré! Utilisez d'abord 'Slice Tileset Automatically'");
                return;
            }
            
            // Créer un ScriptableObject TilesetManager
            var tilesetManager = ScriptableObject.CreateInstance<TilesetManager>();
            tilesetManager.tilesetTexture = tilesetTexture;
            tilesetManager.tileSize = tileSize;
            tilesetManager.tilesPerRow = new Vector2Int(tilesPerRow, tilesPerColumn);
            
            // Créer les MapTileData à partir des sprites générés
            tilesetManager.availableTiles.Clear();
            
            for (int i = 0; i < generatedSprites.Length; i++)
            {
                if (generatedSprites[i] != null)
                {
                    // Déterminer le type de terrain (vous pouvez personnaliser ceci)
                    TerrainType terrain = DetermineTerrainFromIndex(i);
                    bool walkable = DetermineWalkableFromIndex(i);
                    
                    var tileData = new MapTileData(generatedSprites[i], terrain, walkable);
                    tilesetManager.availableTiles.Add(tileData);
                    
                    // Assigner les tuiles d'accès rapide
                    if (i == 0 && tilesetManager.grassTile == null) tilesetManager.grassTile = tileData;
                    if (i == 8 && tilesetManager.stoneTile == null) tilesetManager.stoneTile = tileData;
                    if (i == 16 && tilesetManager.waterTile == null) tilesetManager.waterTile = tileData;
                    if (i == 24 && tilesetManager.wallTile == null) tilesetManager.wallTile = tileData;
                }
            }
            
            Debug.Log($"✅ TilesetManager configuré avec {tilesetManager.availableTiles.Count} tuiles!");
            Debug.Log($"💡 Assignez ce TilesetManager à votre MapRenderer dans l'Inspector");
        }
        
        private TerrainType DetermineTerrainFromIndex(int index)
        {
            // Logique basée sur la position dans le tileset
            // Personnalisez selon votre tileset
            
            if (index < 8) return TerrainType.Grass;      // Première ligne
            if (index < 16) return TerrainType.Stone;     // Deuxième ligne
            if (index < 24) return TerrainType.Water;     // Troisième ligne
            if (index < 32) return TerrainType.Sand;      // Quatrième ligne
            if (index < 40) return TerrainType.Lava;      // Cinquième ligne
            
            return TerrainType.Grass; // Par défaut
        }
        
        private bool DetermineWalkableFromIndex(int index)
        {
            // Les murs/obstacles sont généralement en fin de tileset
            // Personnalisez selon votre tileset
            
            int row = index / tilesPerRow;
            int col = index % tilesPerRow;
            
            // Exemple: dernière ligne = murs
            if (row >= tilesPerColumn - 1) return false;
            
            // Exemple: dernières colonnes = obstacles
            if (col >= tilesPerRow - 2) return false;
            
            return true; // Par défaut walkable
        }
        
        [ContextMenu("Preview Tileset Info")]
        public void PreviewTilesetInfo()
        {
            if (tilesetTexture != null)
            {
                Debug.Log($"📋 INFOS TILESET:");
                Debug.Log($"   Nom: {tilesetTexture.name}");
                Debug.Log($"   Taille: {tilesetTexture.width}x{tilesetTexture.height}");
                Debug.Log($"   Tuiles: {tilesPerRow}x{tilesPerColumn} = {tilesPerRow * tilesPerColumn}");
                Debug.Log($"   Taille tuile: {tileSize}x{tileSize}");
            }
        }
    }
}
