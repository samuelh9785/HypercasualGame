using System;
using UnityEngine;

namespace Com.SamuelHOARAU.Hypercasual
{
    public class BPMManager : MonoBehaviour
    {
        [SerializeField] private int bpm = 120; // Nombre de BPM (ajustable dans l'inspecteur)

        public float BeatInterval => _beatInterval;
        private float _beatInterval; // Intervalle entre chaque battement en secondes

        private float nextBeatTime; // Temps pour le prochain battement
        private bool isRunning = false; // Indique si le BPMManager est actif

        public static BPMManager Instance { get; private set; }

        public event Action OnBPM; // Événement déclenché à chaque battement

        public void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            OnBPM = null;

            if (Instance == this)
                Instance = null;
        }

        private void Start()
        {
            UpdateBeatInterval();
            StartBPM();
        }

        private void Update()
        {
            if (!isRunning) return;

            // Vérifie si le temps actuel dépasse le temps prévu pour le prochain battement
            if (Time.time >= nextBeatTime)
            {
                TriggerBPM();
                nextBeatTime += _beatInterval; // Planifie le prochain battement
            }
        }

        // Met à jour l'intervalle entre les battements en fonction des BPM
        private void UpdateBeatInterval()
        {
            _beatInterval = 60f / bpm;
        }

        // Démarre le BPMManager
        public void StartBPM()
        {
            isRunning = true;
            nextBeatTime = Time.time + _beatInterval; // Initialiser le prochain battement
        }

        // Arrête le BPMManager
        public void StopBPM()
        {
            isRunning = false;
        }

        // Définit les BPM et met à jour l'intervalle
        public void SetBPM(int newBPM)
        {
            bpm = Mathf.Max(1, newBPM); // Évite les BPM à 0 ou négatifs
            UpdateBeatInterval();
        }

        // Déclenche l'événement du battement
        private void TriggerBPM()
        {
            OnBPM?.Invoke(); // Appelle tous les abonnés de l'événement
        }
    }
}