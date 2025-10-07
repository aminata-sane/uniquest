using UnityEngine;

namespace UniQuest.Map
{
    /// <summary>
    /// Types de terrain disponibles dans le jeu
    /// </summary>
    public enum TerrainType
    {
        Grass,    // Herbe normale
        Stone,    // Pierre (ralentit)
        Water,    // Eau (ralentit beaucoup)
        Sand,     // Sable
        Lava,     // Lave (dégâts)
        Ice       // Glace (glisse)
    }
    
    /// <summary>
    /// Données d'un type de terrain
    /// </summary>
    [System.Serializable]
    public class TerrainTile
    {
        [Header("Terrain Properties")]
        public TerrainType type;
        public Color color = Color.white;
        
        [Header("Movement Properties")]
        public float movementSpeedMultiplier = 1f;
        public bool isWalkable = true;
        
        [Header("Effects")]
        public int damagePerSecond = 0;
        public AudioClip walkSound;
        
        [Header("Visual")]
        public Sprite terrainSprite;
    }
}
