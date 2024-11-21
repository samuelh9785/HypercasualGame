using UnityEngine;
using UnityEngine.UI; // Nécessaire si vous utilisez un Text UI

namespace Com.SamuelHOARAU.Hypercasual
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private Text fpsText; // Lien vers un élément UI Text

        private float deltaTime = 0.0f;

        void Update()
        {
            // Calcul de la variation de temps entre les frames
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

            // Calcul des FPS
            float fps = 1.0f / deltaTime;

            // Affichage des FPS dans un texte UI
            if (fpsText != null)
            {
                fpsText.text = $"{Mathf.Ceil(fps).ToString("00")} FPS";
            }
        }
    }
}