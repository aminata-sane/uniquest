using System.Collections.Generic;
using UnityEngine;
using UniQuest.Characters;
using UniQuest.Inventory;

namespace UniQuest.Core
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game State")]
        public GameState currentState = GameState.Exploration;
        
        [Header("Player Team")]
        public List<Character> playerTeam = new List<Character>();
        public Character currentActiveCharacter;
        
        [Header("Components")]
        public InventoryManager inventoryManager;
        
        [Header("Save/Load")]
        public string saveFileName = "uniquest_save.json";

        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<GameManager>();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeGame();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void InitializeGame()
        {
            // Initialiser les composants nécessaires
            if (inventoryManager == null)
            {
                inventoryManager = GetComponent<InventoryManager>();
                if (inventoryManager == null)
                {
                    inventoryManager = gameObject.AddComponent<InventoryManager>();
                }
            }

            Debug.Log("UniQuest Game Manager initialisé!");
        }

        public void ChangeGameState(GameState newState)
        {
            GameState previousState = currentState;
            currentState = newState;
            
            Debug.Log($"Changement d'état: {previousState} → {newState}");
            
            // Gérer les transitions d'état
            OnGameStateChanged(previousState, newState);
        }

        private void OnGameStateChanged(GameState previous, GameState current)
        {
            switch (current)
            {
                case GameState.Exploration:
                    // Activer la caméra d'exploration, UI de map, etc.
                    break;
                    
                case GameState.Combat:
                    // Activer la caméra de combat, UI de combat, etc.
                    break;
                    
                case GameState.Menu:
                    // Ouvrir les menus
                    break;
                    
                case GameState.Dialogue:
                    // Activer le système de dialogue
                    break;
            }
        }

        public void AddCharacterToTeam(Character character)
        {
            if (!playerTeam.Contains(character))
            {
                playerTeam.Add(character);
                Debug.Log($"{character.characterName} rejoint l'équipe!");
                
                if (currentActiveCharacter == null)
                {
                    SetActiveCharacter(character);
                }
            }
        }

        public void SetActiveCharacter(Character character)
        {
            if (playerTeam.Contains(character) && character.IsAlive())
            {
                currentActiveCharacter = character;
                Debug.Log($"{character.characterName} est maintenant le personnage actif");
            }
        }

        public List<Character> GetAliveTeamMembers()
        {
            return playerTeam.FindAll(c => c.IsAlive());
        }

        public bool IsGameOver()
        {
            return GetAliveTeamMembers().Count == 0;
        }

        public void SaveGame()
        {
            GameSaveData saveData = new GameSaveData();
            
            // Sauvegarder les données importantes
            saveData.currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            saveData.playerPosition = currentActiveCharacter != null ? currentActiveCharacter.transform.position : Vector3.zero;
            
            // Sauvegarder l'équipe
            foreach (Character character in playerTeam)
            {
                CharacterSaveData charData = new CharacterSaveData
                {
                    name = character.characterName,
                    level = character.level,
                    currentHP = character.currentHP,
                    maxHP = character.maxHP,
                    currentMP = character.currentMP,
                    maxMP = character.maxMP,
                    attack = character.attack,
                    defense = character.defense,
                    speed = character.speed,
                    precision = character.precision,
                    experience = character.experience,
                    experienceToNextLevel = character.experienceToNextLevel
                };
                saveData.teamData.Add(charData);
            }

            string jsonData = JsonUtility.ToJson(saveData, true);
            System.IO.File.WriteAllText(Application.persistentDataPath + "/" + saveFileName, jsonData);
            
            Debug.Log("Partie sauvegardée!");
        }

        public void LoadGame()
        {
            string filePath = Application.persistentDataPath + "/" + saveFileName;
            
            if (System.IO.File.Exists(filePath))
            {
                string jsonData = System.IO.File.ReadAllText(filePath);
                GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(jsonData);
                
                // Charger les données
                // TODO: Implémenter le chargement complet
                
                Debug.Log("Partie chargée!");
            }
            else
            {
                Debug.Log("Aucune sauvegarde trouvée.");
            }
        }
    }

    public enum GameState
    {
        Exploration,
        Combat,
        Menu,
        Dialogue,
        Inventory
    }

    [System.Serializable]
    public class GameSaveData
    {
        public string currentLevel;
        public Vector3 playerPosition;
        public List<CharacterSaveData> teamData = new List<CharacterSaveData>();
    }

    [System.Serializable]
    public class CharacterSaveData
    {
        public string name;
        public int level;
        public int currentHP;
        public int maxHP;
        public int currentMP;
        public int maxMP;
        public int attack;
        public int defense;
        public int speed;
        public int precision;
        public int experience;
        public int experienceToNextLevel;
    }
}
