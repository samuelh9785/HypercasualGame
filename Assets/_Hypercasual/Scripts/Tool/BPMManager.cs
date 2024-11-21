using System;
using UnityEngine;

namespace Com.SamuelHOARAU.Hypercasual
{
    public class BPMManager : MonoBehaviour
    {
        [SerializeField] private int bpm = 120;

        private float _beatInterval;

        private float nextBeatTime;
        private bool isRunning = false;

        public float BeatInterval => _beatInterval;

        public static BPMManager Instance { get; private set; }

        public event Action OnBPM;

        public void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this;

            Application.targetFrameRate = 60;
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

            
            if (Time.time >= nextBeatTime)
            {
                TriggerBPM();
                nextBeatTime += _beatInterval;
            }
        }

        private void UpdateBeatInterval()
        {
            _beatInterval = 60f / bpm;
        }

        public void StartBPM()
        {
            isRunning = true;
            nextBeatTime = Time.time + _beatInterval;
        }

        public void StopBPM()
        {
            isRunning = false;
        }

        public void SetBPM(int newBPM)
        {
            bpm = Mathf.Max(1, newBPM);
            UpdateBeatInterval();
        }

        private void TriggerBPM()
        {
            OnBPM?.Invoke();
        }
    }
}