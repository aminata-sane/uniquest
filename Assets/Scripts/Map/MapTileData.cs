using UnityEngine;

namespace UniQuest.Map
{
    /// <summary>
    /// Données d'une tuile de carte avec sprite du tileset
    /// </summary>
    [System.Serializable]
    public class MapTileData
    {
        [Header("Visual Properties")]
        public Sprite tileSprite;
        public Color tileColor = Color.white;
        
        [Header("Gameplay Properties")]
        public bool isWalkable = true;
        public bool isObstacle = false;
        public float movementCost = 1f;
        
        [Header("Terrain Effects")]
        public TerrainType terrainType = TerrainType.Grass;
        public float speedMultiplier = 1f;
        public int damagePerSecond = 0;
        
        [Header("Audio")]
        public AudioClip stepSound;
        
        [Header("Interaction")]
        public bool isInteractable = false;
        public string interactionMessage = "";
        
        public MapTileData(Sprite sprite, TerrainType terrain, bool walkable = true)
        {
            tileSprite = sprite;
            terrainType = terrain;
            isWalkable = walkable;
            isObstacle = !walkable;
            
            // Configuration par défaut selon le terrain
            switch (terrain)
            {
                case TerrainType.Grass:
                    speedMultiplier = 1f;
                    break;
                case TerrainType.Stone:
                    speedMultiplier = 0.8f;
                    break;
                case TerrainType.Water:
                    speedMultiplier = 0.5f;
                    break;
                case TerrainType.Sand:
                    speedMultiplier = 0.9f;
                    break;
                case TerrainType.Lava:
                    speedMultiplier = 0.3f;
                    damagePerSecond = 10;
                    break;
                case TerrainType.Ice:
                    speedMultiplier = 1.2f; // Plus rapide mais peut glisser
                    break;
            }
        }
    }
}
