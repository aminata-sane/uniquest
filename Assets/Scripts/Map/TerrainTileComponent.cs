using UnityEngine;

namespace UniQuest.Map
{
    public class TerrainTileComponent : MonoBehaviour
    {
        [Header("Terrain Data")]
        public TerrainType terrainType;
        public float movementSpeedMultiplier = 1f;
        public bool isWalkable = true;
        public int damagePerSecond = 0;

        private TerrainTile terrainData;
        private float damageTimer = 0f;

        public void Initialize(TerrainTile data)
        {
            terrainData = data;
            terrainType = data.type;
            movementSpeedMultiplier = data.movementSpeedMultiplier;
            isWalkable = data.isWalkable;
            damagePerSecond = data.damagePerSecond;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                HandlePlayerOnTerrain(other.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log($"Player entre sur terrain: {terrainType}");
                
                // Jouer son de terrain si configuré
                if (terrainData?.walkSound != null)
                {
                    AudioSource.PlayClipAtPoint(terrainData.walkSound, transform.position, 0.5f);
                }
            }
        }

        private void HandlePlayerOnTerrain(GameObject player)
        {
            // Gestion des dégâts par seconde (lave, etc.)
            if (damagePerSecond > 0)
            {
                damageTimer += Time.deltaTime;
                if (damageTimer >= 1f)
                {
                    ApplyTerrainDamage(player);
                    damageTimer = 0f;
                }
            }

            // Appliquer modificateur de vitesse
            var playerScript = player.GetComponent<UniQuest.Characters.Player>();
            if (playerScript != null)
            {
                // On ajoutera cette méthode au Player plus tard
                // playerScript.SetTerrainSpeedMultiplier(movementSpeedMultiplier);
            }
        }

        private void ApplyTerrainDamage(GameObject player)
        {
            var character = player.GetComponent<UniQuest.Characters.Character>();
            if (character != null)
            {
                character.TakeDamage(damagePerSecond);
                Debug.Log($"💥 {terrainType} inflige {damagePerSecond} dégâts au joueur!");
                
                // Effet visuel de dégâts
                CreateDamageEffect();
            }
        }

        private void CreateDamageEffect()
        {
            // Effet visuel simple - flash rouge
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                StartCoroutine(FlashDamage(sr));
            }
        }

        private System.Collections.IEnumerator FlashDamage(SpriteRenderer sr)
        {
            Color originalColor = sr.color;
            sr.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            sr.color = originalColor;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log($"Player quitte terrain: {terrainType}");
                damageTimer = 0f; // Reset timer
            }
        }
    }
}
