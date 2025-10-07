# Guide de Test des Obstacles et Collisions

## Vue d'ensemble
Ce guide vous explique comment tester le système d'obstacles et de collisions dans UniQuest.

## Scripts impliqués
- `WallObstacle.cs` : Gestion des obstacles (murs, rochers, arbres, etc.)
- `Player.cs` : Déplacement du joueur et détection de collision
- `ObstacleTestManager.cs` : Créateur automatique d'obstacles de test
- `PlayerCollisionDebugger.cs` : Debug et visualisation des collisions

## Méthode 1 : Test Manuel Rapide

### Créer un obstacle manuellement
1. **Ouvrir Unity** et la scène `GameScene.unity`
2. **Créer un GameObject vide** :
   - Hierarchy → Clic droit → Create Empty
   - Renommer en "Wall_Test"
3. **Ajouter le script** :
   - Sélectionner "Wall_Test"
   - Inspector → Add Component → "WallObstacle"
4. **Positionner** :
   - Transform → Position (0, 2, 0)
5. **Tester** :
   - Play → Déplacer le Player avec WASD
   - Observer les messages dans la Console

### Résultat attendu
- Le Player ne peut pas traverser le mur
- Message dans la Console : "Player collision avec Wall à [position]"
- L'obstacle apparaît en gris

## Méthode 2 : Test Automatique avec ObstacleTestManager

### Configuration
1. **Créer un GameObject vide** nommé "ObstacleTestManager"
2. **Ajouter le script ObstacleTestManager**
3. **Vérifier les paramètres dans l'Inspector** :
   - ✅ Create Test Obstacles
   - ✅ Create Walls, Create Rocks, Create Trees
   - Test Area Size : (10, 10)

### Utilisation
1. **Play** → Les obstacles se créent automatiquement
2. **Tester la navigation** avec le Player
3. **Nettoyer** : Inspector → Clic droit sur script → "Clear Test Obstacles"

## Méthode 3 : Debug Avancé avec PlayerCollisionDebugger

### Configuration
1. **Sélectionner le Player GameObject**
2. **Ajouter le script PlayerCollisionDebugger**
3. **Configurer dans l'Inspector** :
   - ✅ Enable Debug Logs
   - ✅ Enable Visual Debug

### Fonctionnalités de debug
- **Messages détaillés** dans la Console
- **Visualisation** des colliders en mode Scene
- **Tests manuels** : Clic droit sur script → "Test Collision Info"
- **Détection d'obstacles proches** : "Check Nearby Obstacles"

## Types d'obstacles disponibles

### Wall (Mur)
- **Comportement** : Collision solide, infranchissable
- **Couleur** : Gris
- **Destructible** : Non (par défaut)

### Rock (Rocher)
- **Comportement** : Collision solide, infranchissable
- **Couleur** : Marron
- **Destructible** : Non (par défaut)

### Tree (Arbre)
- **Comportement** : Collision solide
- **Couleur** : Vert
- **Destructible** : Oui (configurable)
- **Durabilité** : 3 (par défaut)

### Water (Eau) - À implémenter
- **Comportement** : Ralentit le joueur
- **Couleur** : Bleu
- **Destructible** : Non

### Fence (Barrière)
- **Comportement** : Collision solide
- **Couleur** : Marron clair
- **Destructible** : Oui (configurable)

## Checklist de test

### Tests de base
- [ ] Le Player a un Rigidbody2D et un Collider2D
- [ ] Le Player a le tag "Player"
- [ ] Les obstacles ont un Collider2D avec isTrigger = false
- [ ] Collision avec Wall : Player bloqué
- [ ] Messages de debug dans la Console

### Tests avancés
- [ ] Collision avec différents types d'obstacles
- [ ] Destruction d'obstacles destructibles
- [ ] Effet visuel lors des dégâts (flash rouge)
- [ ] Gestion des sons de collision (si configurés)

### Tests de performance
- [ ] Création/destruction d'obstacles en cours de jeu
- [ ] Multiple collisions simultanées
- [ ] Navigation fluide autour des obstacles

## Problèmes courants et solutions

### Le Player traverse les obstacles
**Causes possibles :**
- Pas de Collider2D sur l'obstacle
- isTrigger = true sur l'obstacle
- Pas de Rigidbody2D sur le Player
- Vitesse trop élevée du Player

**Solutions :**
- Vérifier les Colliders dans l'Inspector
- Réduire la vitesse du Player
- Activer "Continuous" collision detection sur le Rigidbody2D

### Pas de messages de debug
**Causes possibles :**
- Tag "Player" incorrect
- Script PlayerCollisionDebugger non attaché
- Console fermée

**Solutions :**
- Vérifier le tag du Player
- Ouvrir Window → General → Console
- Vérifier "Enable Debug Logs" dans l'Inspector

### Obstacles invisibles
**Causes possibles :**
- Pas de SpriteRenderer
- Sprite non assigné
- Couleur transparente

**Solutions :**
- Le script WallObstacle crée automatiquement un SpriteRenderer
- Redémarrer la scène si nécessaire

## Prochaines étapes

1. **Tester la collision de base** avec un mur
2. **Ajouter des obstacles variés** (rochers, arbres)
3. **Implémenter les effets spéciaux** (eau qui ralentit)
4. **Ajouter des sons** et effets visuels
5. **Intégrer avec le système de génération de map**

## Notes pour l'équipe

- **Aminata** : Testez d'abord la méthode 1 (test manuel)
- **Nelson** : Utilisez ObstacleTestManager pour créer rapidement des environnements de test
- **Mehdi** : Le PlayerCollisionDebugger vous donnera toutes les infos techniques

Commitez vos tests et partagez vos retours sur Discord !
