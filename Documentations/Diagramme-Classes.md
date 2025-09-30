# Diagramme de Classes - UniQuest RPG

## Architecture Générale

```mermaid
classDiagram
    class Character {
        <<abstract>>
        +string characterName
        +int level
        +int currentHP
        +int maxHP
        +int currentMP
        +int maxMP
        +int attack
        +int defense
        +int speed
        +int precision
        +int experience
        +int experienceToNextLevel
        +CharacterType type
        +List~Attack~ availableAttacks
        
        +InitializeCharacter() void
        +TakeDamage(int damage) void
        +RestoreMP(int amount) void
        +GainExperience(int exp) void
        +CheckLevelUp() void
        +LevelUp() void
        +CanUseAttack(Attack attack) bool
        +IsAlive() bool
        +OnCharacterKO() void
    }

    class Player {
        +Vector3 position
        +float moveSpeed
        +bool canMove
        
        +Move(Vector2 direction) void
        +OnTriggerEnter(Collider other) void
    }

    class Enemy {
        +AILevel intelligence
        +float detectionRange
        +List~AttackStrategy~ strategies
        
        +ChooseAction() AttackAction
        +ExecuteAI() void
        +OnDefeat() void
    }

    class Warrior {
        +int rage
        +List~WarriorSkill~ warriorSkills
        
        +UseRage() void
        +LearnWarriorSkill() void
    }

    class Mage {
        +int magicPower
        +List~Spell~ spells
        
        +CastSpell(Spell spell) void
        +LearnSpell() void
    }

    class Attack {
        +string attackName
        +string description
        +AttackType type
        +int baseDamage
        +int mpCost
        +int accuracy
        +int criticalChance
        +float criticalMultiplier
        +bool canMiss
        +bool isHealingMove
        +int healAmount
        
        +ExecuteAttack(Character attacker, Character target) AttackResult
        +CheckHit(Character attacker) bool
        +CheckCritical() bool
        +CalculateDamage(Character attacker) int
    }

    class AttackResult {
        +string attackName
        +string attacker
        +string target
        +int damage
        +int healing
        +bool missed
        +bool isCritical
        +string message
    }

    class Item {
        +string itemName
        +string description
        +ItemType type
        +Sprite icon
        +int healingValue
        +int manaRestoreValue
        +int attackBoost
        +int defenseBoost
        +int speedBoost
        +bool consumable
        +bool usableInCombat
        +bool usableOutOfCombat
        
        +UseItem(Character target) bool
    }

    class InventorySlot {
        +Item item
        +int quantity
        
        +InventorySlot(Item item, int quantity)
    }

    class InventoryManager {
        +int maxSlots
        +List~InventorySlot~ inventory
        
        +AddItem(Item item, int quantity) bool
        +UseItem(Item item, Character target) bool
        +HasItem(Item item) bool
        +GetItemQuantity(Item item) int
    }

    class GameManager {
        +GameState currentState
        +List~Character~ playerTeam
        +Character currentActiveCharacter
        +InventoryManager inventoryManager
        +string saveFileName
        
        +ChangeGameState(GameState newState) void
        +AddCharacterToTeam(Character character) void
        +SetActiveCharacter(Character character) void
        +GetAliveTeamMembers() List~Character~
        +IsGameOver() bool
        +SaveGame() void
        +LoadGame() void
    }

    class CombatManager {
        +Character currentAttacker
        +Character currentTarget
        +List~Character~ turnOrder
        +CombatState combatState
        
        +StartCombat() void
        +ProcessTurn() void
        +EndCombat() void
        +CalculateTurnOrder() void
    }

    %% Relations d'héritage
    Character <|-- Player
    Character <|-- Enemy
    Player <|-- Warrior
    Player <|-- Mage

    %% Relations de composition
    Character "*" --o "1" GameManager : playerTeam
    Attack "*" --o "1" Character : availableAttacks
    Item "*" --o "1" InventorySlot : item
    InventorySlot "*" --o "1" InventoryManager : inventory
    InventoryManager "1" --o "1" GameManager : inventoryManager

    %% Relations d'association
    Attack ..> AttackResult : creates
    Character --> Attack : uses
    Character --> Item : uses
    GameManager --> CombatManager : manages

    %% Énumérations
    class CharacterType {
        <<enumeration>>
        Warrior
        Mage
        Archer
        Healer
    }

    class AttackType {
        <<enumeration>>
        Physical
        Magic
        Healing
        Special
    }

    class ItemType {
        <<enumeration>>
        Consumable
        Key
        Boost
        Special
    }

    class GameState {
        <<enumeration>>
        Exploration
        Combat
        Menu
        Dialogue
        Inventory
    }

    Character --> CharacterType
    Attack --> AttackType
    Item --> ItemType
    GameManager --> GameState
```

## Description des Classes Principales

### 🧙‍♂️ **Character (Classe Abstraite)**
- **Rôle** : Classe de base pour tous les personnages (joueurs et ennemis)
- **Responsabilités** : 
  - Gestion des statistiques (PV, PM, attaque, défense, etc.)
  - Système de niveau et d'expérience
  - Gestion des attaques disponibles
  - Mécaniques de combat de base

### ⚔️ **Attack (ScriptableObject)**
- **Rôle** : Définit les attaques disponibles dans le jeu
- **Responsabilités** :
  - Calcul des dégâts et de la précision
  - Gestion des coups critiques
  - Exécution des attaques avec résultats détaillés

### 💰 **Item & InventoryManager**
- **Rôle** : Gestion complète du système d'inventaire
- **Responsabilités** :
  - Stockage et organisation des objets
  - Utilisation des objets (potions, clés, boosts)
  - Vérification de la disponibilité des objets

### 🎮 **GameManager (Singleton)**
- **Rôle** : Manager principal du jeu
- **Responsabilités** :
  - Gestion de l'état du jeu (exploration, combat, menus)
  - Gestion de l'équipe du joueur
  - Sauvegarde et chargement des données

## Avantages de cette Architecture

### ✅ **Modularité**
- Chaque classe a une responsabilité claire
- Facile d'ajouter de nouveaux types de personnages ou d'attaques

### ✅ **Extensibilité**
- Héritage pour les différents types de personnages
- ScriptableObjects pour créer facilement des attaques et objets

### ✅ **Réutilisabilité**
- Les classes de base peuvent être étendues
- Les systèmes sont indépendants les uns des autres

### ✅ **Testabilité**
- Chaque composant peut être testé individuellement
- Séparation claire des responsabilités

## Prochaines Étapes d'Implémentation

1. **Étape 2** : Aminata → Map & Déplacement (Player movement)
2. **Étape 3** : Tous → Système de combat (CombatManager)
3. **Étape 4** : Spécialisations des personnages (Warrior, Mage, etc.)
