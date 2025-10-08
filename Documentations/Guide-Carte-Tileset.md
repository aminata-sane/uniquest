# 🗺️ Guide - Système de Carte avec Tileset

## 🎯 Vue d'ensemble

Votre système de carte est maintenant prêt à utiliser votre tileset **EPIC RPG World Pack** ! 

### Scripts créés :
- **`MapTileData.cs`** : Données d'une tuile (sprite, propriétés gameplay)
- **`TilesetManager.cs`** : Gestionnaire du tileset (ScriptableObject)  
- **`MapRenderer.cs`** : Rendu de la carte avec les tuiles

## 🚀 Configuration dans Unity

### Étape 1 : Préparer le Tileset

1. **Dans Unity** → Sélectionner votre image tileset :
   `Assets/Art/Tilesets/.../Tileset-Terrain2.png`

2. **Dans l'Inspector** :
   - **Texture Type** : `Sprite (2D and UI)`
   - **Sprite Mode** : `Multiple`
   - **Filter Mode** : `Point (no filter)` (pour pixel art)
   - **Cliquer "Apply"**

3. **Ouvrir le Sprite Editor** :
   - **Type** : `Grid By Cell Count`
   - **Column & Row** : Selon votre tileset (ex: 8x8)
   - **Cliquer "Slice"** → **"Apply"**

### Étape 2 : Créer le TilesetManager

1. **Clic droit dans Project** → **Create** → **UniQuest/Map/Tileset Manager**
2. **Nommer** : "AncientRuinsTileset"
3. **Dans l'Inspector** :
   - **Tileset Texture** : Votre image slicée
   - **Tile Size** : 32 (ou la taille de vos tuiles)
   - **Tiles Per Row** : 8x8 (ou selon votre tileset)
4. **Clic droit sur le TilesetManager** → **"Generate Tiles from Tileset"**

### Étape 3 : Setup de la carte

1. **Dans votre scène** → Créer un GameObject → "MapSystem"
2. **Ajouter le script `MapRenderer`**
3. **Dans l'Inspector** :
   - **Tileset Manager** : Votre ScriptableObject créé
   - **Map Width/Height** : Taille désirée (ex: 50x30)
   - **Generate On Start** : ✅

### Étape 4 : Layers de rendu

1. **Window** → **2D** → **Sorting Layers**
2. **Ajouter les layers** :
   - `Ground` (index 0) : Tuiles de terrain
   - `Objects` (index 1) : Obstacles, objets
   - `Characters` (index 2) : Player, ennemis
   - `UI` (index 3) : Interface

## 🎮 Utilisation en jeu

### Génération automatique
```csharp
// La carte se génère automatiquement au Start()
// Basée sur du bruit de Perlin pour des patterns naturels
```

### Génération manuelle
```csharp
// Dans l'Inspector, clic droit sur MapRenderer
// → "Generate Random Map"
```

### Customisation
```csharp
// Modifier le seed pour différentes cartes
mapRenderer.seed = 67890;
mapRenderer.GenerateMap();
```

## 🔧 Personnalisation

### Modifier la génération de terrain

Dans `MapRenderer.cs`, fonction `SelectTileForPosition()` :

```csharp
// Exemple : plus d'eau près du centre
float distanceFromCenter = Vector2.Distance(
    new Vector2(x, y), 
    new Vector2(mapWidth/2, mapHeight/2)
);

if (distanceFromCenter < 5f)
{
    return tilesetManager.GetRandomTileOfType(TerrainType.Water);
}
```

### Ajouter des structures

```csharp
// Dans MapRenderer, après la génération de base
CreateCastle(new Vector2Int(25, 15));
CreateForest(new Vector2Int(10, 20), 5);
```

## 🎨 Intégration avec le système existant

### Obstacles et collisions
- Les tuiles non-walkables créent automatiquement des colliders
- Compatible avec votre `WallObstacle.cs` existant

### Effets de terrain  
- Integration automatique avec `TerrainTileComponent.cs`
- Le Player réagit aux différents terrains (vitesse, dégâts)

### Caméra
- Fonctionne avec votre `SimpleCameraController.cs`
- La caméra peut suivre le Player sur la nouvelle carte

## 🚀 Prochaines étapes

1. **Tester la génération** de carte
2. **Ajuster les paramètres** de terrain dans TilesetManager
3. **Créer des patterns** personnalisés (châteaux, forêts, rivières)
4. **Ajouter des décors** avec le dossier Props/
5. **Intégrer les personnages** du dossier Characters/

---

**Votre carte procédurale avec tileset est prête ! 🗺️✨**
