using UnityEngine;

namespace UniQuest.Map
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Camera Follow Settings")]
        public Transform target; // Le joueur à suivre
        public float followSpeed = 5f;
        public Vector3 offset = new Vector3(0, 0, -10); // Décalage de la caméra
        
        [Header("Camera Bounds (Optional)")]
        public bool useBounds = false;
        public float minX, maxX, minY, maxY;
        
        private void LateUpdate()
        {
            if (target == null) return;
            
            // Position cible de la caméra
            Vector3 targetPosition = target.position + offset;
            
            // Si on utilise des limites pour la caméra
            if (useBounds)
            {
                targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
                targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
            }
            
            // Mouvement fluide de la caméra
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
        
        // Méthode pour définir les limites de la caméra
        public void SetBounds(float minX, float maxX, float minY, float maxY)
        {
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
            useBounds = true;
        }
    }
}
