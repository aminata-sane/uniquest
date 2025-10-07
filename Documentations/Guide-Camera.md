# 📹 Guide du SimpleCameraController

## ✅ Problèmes Résolus

### 1. Position automatique non désirée
**Avant :** La caméra se repositionnait automatiquement à `(0, 0, -10)` dès l'ajout du script.
**Maintenant :** Le script préserve la position X/Y actuelle de la caméra et n'ajuste que la position Z si nécessaire.

### 2. Suivi automatique intrusif
**Avant :** `followPlayer = true` par défaut, la caméra suivait immédiatement le joueur.
**Maintenant :** `followPlayer = false` par défaut, vous devez l'activer manuellement.

## 🎮 Contrôles

### Déplacement Manuel
- **I** : Haut
- **J** : Gauche
- **K** : Bas
- **L** : Droite

### Zoom
- **+** : Zoom in (rapprocher)
- **-** : Zoom out (éloigner)

### Raccourcis
- **R** : Revenir instantanément au joueur
- **T** : Activer/désactiver le suivi automatique du joueur
- **Y** : Vue d'ensemble (position optimale entre joueur et obstacles)

## 🔧 Configuration

### Dans l'Inspector Unity :
- `Follow Player` : Suivi automatique (désactivé par défaut)
- `Follow Speed` : Vitesse de suivi (5.0 par défaut)
- `Move Speed` : Vitesse de déplacement manuel (5.0 par défaut)
- `Camera Distance` : Position Z de la caméra (10.0 par défaut)
- `Enable Manual Control` : Active les contrôles manuels (activé par défaut)

### Méthodes disponibles dans l'Inspector :
- **Center on Player** : Centrer instantanément sur le joueur
- **Show Overview** : Vue d'ensemble de la scène
- **Toggle Follow Player** : Basculer le suivi automatique
- **Reset Camera Position** : Remettre à zéro la position de la caméra

## 🐛 Debug

### CameraDebugHelper
Ajouter ce script à un GameObject pour avoir des informations de debug :
- Position de la caméra en temps réel
- Position du joueur
- Distance entre caméra et joueur
- Rappel des contrôles à l'écran

### Gizmos
Le `CameraDebugHelper` dessine aussi :
- Une croix rouge au centre de la caméra
- Une ligne jaune entre la caméra et le joueur

## 💡 Conseils d'Utilisation

1. **Lors du développement** : Garder `followPlayer = false` et utiliser les contrôles manuels
2. **Pour tester le gameplay** : Activer `followPlayer = true` ou utiliser la touche **T**
3. **Pour déboguer** : Ajouter le `CameraDebugHelper` temporairement
4. **Vue d'ensemble** : Utiliser la touche **Y** pour voir joueur et obstacles ensemble

---

**Bon développement ! 🎥✨**
