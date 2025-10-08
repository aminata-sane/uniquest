# 🔧 Configuration Tileset - Solutions Alternatives

## 🚨 Problème : Sprite Editor Manquant dans Unity 6

### **Solution 1 : Installation Packages 2D**

1. **Window** → **Package Manager** 
2. **Dropdown "In Project"** → **"Unity Registry"**
3. **Installer ces packages** :
   - `2D Sprite` (essentiel)
   - `2D Tilemap Extras`
   - `2D Pixel Perfect`

### **Solution 2 : Script TilesetSlicer (Alternative)**

Si les packages ne s'installent pas, utilisez `TilesetSlicer.cs` :

```csharp
// 1. Créer GameObject + TilesetSlicer
// 2. Assigner votre texture PNG
// 3. Configurer tileSize (32) et grid (8x8)
// 4. Clic droit → "Slice Tileset Automatically"
```

### **Solution 3 : Configuration Manuelle**

Si rien ne fonctionne, configurez manuellement :

1. **Sélectionner votre PNG**
2. **Inspector** :
   - Texture Type: `Sprite (2D and UI)`
   - Sprite Mode: `Single` (pas Multiple)
   - **Apply**

3. **Utiliser le sprite complet** dans MapRenderer avec découpage programmatique

## ✅ **Test de Configuration**

### **Vérifier que votre setup fonctionne :**

1. **Créer GameObject** → "MapTest"
2. **Ajouter MapRenderer**
3. **Créer TilesetManager** (manuellement ou via script)
4. **Assigner au MapRenderer**
5. **Play** → La carte doit se générer

### **Debugging :**

Si la carte ne se génère pas :

```csharp
// Dans MapRenderer, vérifier :
Debug.Log($"TilesetManager: {tilesetManager != null}");
Debug.Log($"Available tiles: {tilesetManager?.availableTiles?.Count}");
```

## 🎨 **Configuration Tileset Spécifique EPIC RPG**

Votre pack contient :
- `Tileset-Terrain2.png` → Terrain principal
- `wall-8 - 2 tiles tall.png` → Murs
- `Tileset-Animated Terrains.png` → Terrains animés

### **Recommandation :**
1. **Commencer par** `Tileset-Terrain2.png`
2. **Taille tuile** : Probablement 32x32 ou 16x16
3. **Grid** : Compter manuellement les tuiles dans l'image

### **Pour vérifier la taille :**
```csharp
// Dans TilesetSlicer, utiliser "Preview Tileset Info"
// Cela affiche la taille de l'image
```

---

**L'important : avoir une carte qui se génère, même avec une seule tuile ! 🗺️**
