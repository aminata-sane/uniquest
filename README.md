# UniQuest - RPG en C# / Unity 🎮

Jeu de rôle tour par tour développé par **Aminata**, **Nelson** et **Mehdi**.
*Le RPG où chaque ligne de code est un pas vers l'épopée.*

## 🚀 Quick Start

### Cloner le projet
```bash
git clone https://github.com/aminata-sane/uniquest.git
cd uniquest
```

### Ouvrir dans Unity
1. Lancer Unity Hub
2. **Add** → **Add project from disk**
3. Sélectionner le dossier `uniquest`
4. Double-cliquer pour ouvrir le projet

## 🌿 Workflow Git (Branches)

### Créer sa branche de développement
```bash
# Aminata - Map et déplacements
git checkout -b aminata/step2-map-movement

# Nelson - PNJ et événements  
git checkout -b nelson/step2-npcs-events

# Mehdi - Sauvegarde et transitions
git checkout -b mehdi/step2-save-transitions
```

### Développer et commit
```bash
git add .
git commit -m "✨ Description des changements"
git push origin nom-de-votre-branche
```

### Fusionner dans main (quand la feature est finie)
```bash
git checkout main
git pull origin main
git merge votre-branche
git push origin main
```

## 📊 Structure du Projet

```
Assets/
├── Scripts/
│   ├── Characters/          # Character.cs, Player.cs (Aminata)
│   ├── Combat/             # Attack.cs, système de combat
│   ├── Map/                # MapManager.cs, CameraFollow.cs (Aminata)
│   ├── UI/                 # Interfaces utilisateur (Mehdi)
│   ├── Inventory/          # InventoryManager.cs (Mehdi)
│   └── GameManager.cs      # Manager principal
├── Sprites/                # Images et textures
└── Scenes/                 # Scènes Unity
```

## 🎯 Répartition des Tâches

### Étape 2 (En cours)
- **Aminata** : Map principale + déplacement joueur
- **Nelson** : PNJ + coffres + événements aléatoires
- **Mehdi** : Sauvegarde position + transitions exploration/combat

### Documentation
- **Diagramme de classes** : `Documentations/Diagramme-Classes.md`
- **Répartition complète** : `Documentations/Répartions.md`

## 🔧 Classes Principales

- **`Character`** : Classe abstraite pour tous les personnages
- **`Player`** : Contrôleur du joueur avec mouvement WASD
- **`Attack`** : Système d'attaques (ScriptableObjects)
- **`GameManager`** : Manager principal (Singleton)
- **`InventoryManager`** : Gestion complète de l'inventaire

## 🎮 Comment Jouer (Version Test)

1. Ouvrir la scène `GameScene` 
2. Appuyer sur **Play** ▶️
3. Utiliser **WASD** ou **flèches directionnelles** pour bouger
4. Vérifier la console Unity pour les messages de debug

---

**Bon développement ! 🚀**
