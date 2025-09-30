Répartition des tâches — UniQuest (C# + Unity)

 🎮 Voici une répartition claire, étape-par-étape, avec ce que toi, Nelson et Mehdi devez faire concrètement dans Unity/C#. J’ai inclus les livrables, critères d’acceptation, intégrations et exemples de code utiles à implémenter tout de suite.

# Répartition des Tâches - Projet UniQuest (RPG en C# / Unity)

## Membres de l'équipe
- **Aminata**
- **Nelson**
- **Mehdi**

---

## Étape 1 : Mise en place du projet ✅
### Tâches communes (TERMINÉ)
- ✅ Installer Unity et configurer le projet en C#.
- ✅ Créer un repository GitHub pour le travail collaboratif.
- ✅ Définir l'architecture logicielle et les conventions de code.
- ✅ Concevoir un premier diagramme de classes (Personnage, Attaque, Objet, Inventaire).

### 📁 **Structure du projet créée :**
```
Assets/
├── Scripts/
│   ├── Characters/          # Classes des personnages
│   ├── Combat/             # Système de combat
│   ├── Map/                # Gestion de la carte
│   ├── UI/                 # Interfaces utilisateur
│   ├── Inventory/          # Système d'inventaire
│   └── GameManager.cs      # Manager principal
├── Sprites/                # Images et textures
└── Scenes/                 # Scènes Unity
```

### 🔧 **Classes de base implémentées :**
- **`Character.cs`** : Classe abstraite pour tous les personnages
- **`Attack.cs`** : Système d'attaques avec ScriptableObjects
- **`InventoryManager.cs`** : Gestion complète de l'inventaire
- **`GameManager.cs`** : Manager principal avec sauvegarde/chargement

### 🎯 **Prêt pour la suite :**
- Nelson et Mehdi peuvent maintenant cloner le repo : `git clone https://github.com/aminata-sane/uniquest.git`
- Ouvrir le projet dans Unity Hub
- Commencer le développement selon la répartition ci-dessous

---

## Étape 2 : Map et Déplacements
- **Aminata** :
  - Créer la **map principale** (tileset, zones explorables).
  - Implémenter le **déplacement du joueur** (touches fléchées).
  - Gestion des collisions de base (ne pas traverser les murs/obstacles).

- **Nelson** :
  - Placer des **PNJ** et des **coffres** sur la carte.
  - Déclencher les événements aléatoires (rencontres ennemies).
  - Ajouter une mini-map ou un système de repères visuels.

- **Mehdi** :
  - Créer un système de **sauvegarde/chargement** de la position sur la carte.
  - Mettre en place la transition entre **exploration** et **combat**.

---

## Étape 3 : Système de Combat
- **Aminata** :
  - Développer la **boucle de combat tour par tour**.
  - Implémenter les actions : **Attaque / Magie / Objet**.
  - Gestion de la **barre de mana (PM)**.

- **Nelson** :
  - Développer l’**IA ennemie** (niveau simple → avancé).
  - Gérer les types d’attaques, coups critiques et esquives.
  - Implémenter la **gestion du Game Over**.

- **Mehdi** :
  - Mettre en place l’**interface de combat** (PV, PM, stats visibles).
  - Ajouter l’animation des attaques et effets visuels (dégâts, buffs).
  - Gestion du remplacement d’un personnage KO.

---

## Étape 4 : Équipe et Inventaire
- **Aminata** :
  - Développer la **classe Personnage** avec héritage (guerrier, mage, etc.).
  - Système de **gain d’expérience et montée de niveau**.
  - Débloquer de nouvelles attaques selon les niveaux.

- **Nelson** :
  - Créer le **menu Équipe** (affichage stats, PV/PM, compétences).
  - Gérer la **composition de l’équipe** (ajout/retrait).
  - Assurer la persistance des données de l’équipe (sauvegarde).

- **Mehdi** :
  - Développer le **système d’inventaire** (potions, clés, boosts).
  - Gérer l’utilisation des objets en combat et hors combat.
  - Créer une interface pour l’inventaire.

---

## Étape 5 : Quêtes et Progression
- **Aminata** :
  - Système de **quêtes principales et secondaires**.
  - Dialogue avec PNJ et gestion d’objectifs.
  
- **Nelson** :
  - Gestion des **récompenses** (objets, expérience).
  - Création de **scénarios simples** (enchaînement d’événements).
  
- **Mehdi** :
  - Intégration des quêtes dans le menu du joueur.
  - Suivi de progression des quêtes.

---

## Étape 6 : Tests et Finalisation
- **Aminata** :
  - Écriture de **tests unitaires** pour le système de combat.
  
- **Nelson** :
  - Tests des **systèmes d’événements et de sauvegarde**.
  
- **Mehdi** :
  - Tests des **interfaces graphiques et inventaire**.

---

## Étape 7 : Soutenance
- Préparer une **démonstration fluide** du jeu.
- Répartir la présentation :  
  - [Ton prénom] → Explication de la **map et des combats**.  
  - Nelson → Présentation de l’**IA et des systèmes de quêtes**.  
  - Mehdi → Démo de l’**inventaire et interfaces**.  

---

## Organisation et Collaboration
- Utilisation de **GitHub** pour le versioning.
- Réunions rapides en équipe (daily/hebdo) pour suivre l’avancement.
- Documentation du code et des choix techniques dans un **Wiki** du projet.
