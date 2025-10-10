using UnityEngine;
using UniQuest.Map; // Pour accéder à TilesetManager

namespace UniQuest.Utils
{
    /// <summary>
    /// Créateur manuel de TilesetManager
    /// </summary>
    public class TilesetManagerCreator : MonoBehaviour
    {
        [Header("Manual TilesetManager Creation")]
        public Texture2D tilesetTexture;
        public string assetName = "AncientRuinsTileset";
        
        [Header("Generated TilesetManager")]
        public TilesetManager createdTilesetManager;
        
        [ContextMenu("Create TilesetManager Asset")]
        public void CreateTilesetManagerAsset()
        {
            if (tilesetTexture == null)
            {
                Debug.LogError("❌ Veuillez assigner une Tileset Texture d'abord!");
                return;
            }
            
            // Créer l'instance
            var tilesetManager = ScriptableObject.CreateInstance<Map.TilesetManager>();
            
            // Configuration de base
            tilesetManager.tilesetTexture = tilesetTexture;
            tilesetManager.tileSize = 32;
            tilesetManager.tilesPerRow = new Vector2Int(8, 8);
            
            // Sauvegarder l'asset dans le Project
            string path = $"Assets/{assetName}.asset";
            
            #if UNITY_EDITOR
            UnityEditor.AssetDatabase.CreateAsset(tilesetManager, path);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            
            // Sélectionner l'asset créé
            UnityEditor.Selection.activeObject = tilesetManager;
            #endif
            
            // Référencer dans ce script
            createdTilesetManager = tilesetManager;
            
            Debug.Log($"✅ TilesetManager créé : {path}");
            Debug.Log($"💡 Vous pouvez maintenant l'assigner à votre MapRenderer!");
        }
        
        [ContextMenu("Generate Tiles in TilesetManager")]
        public void GenerateTilesInTilesetManager()
        {
            if (createdTilesetManager == null)
            {
                Debug.LogError("❌ Créez d'abord le TilesetManager avec 'Create TilesetManager Asset'");
                return;
            }
            
            createdTilesetManager.GenerateTilesFromTileset();
            
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(createdTilesetManager);
            UnityEditor.AssetDatabase.SaveAssets();
            #endif
            
            Debug.Log($"✅ Tuiles générées dans le TilesetManager!");
        }
    }
}