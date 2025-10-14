using UnityEngine;

namespace UniQuest.Map
{
    public class SimpleTerrainManager : MonoBehaviour
    {
        [Header("Terrain Effects")]
        public float defaultSpeedMultiplier = 1f;
        
        void Start()
        {
            Debug.Log("🌿 SimpleTerrainManager initialisé!");
        }
        
        // Méthode simple pour obtenir le multiplicateur de vitesse
        public float GetSpeedMultiplier(Vector3 position)
        {
            return defaultSpeedMultiplier;
        }
        
        // Méthode alternative pour le Player
        public float GetTerrainSpeedMultiplier(Vector3 position)
        {
            return defaultSpeedMultiplier;
        }
    }
}