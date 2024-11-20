using System;
using UnityEngine;

namespace Com.SamuelHOARAU.Hypercasual
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float distance = 100f;

        private Vector3 startPosition;
        private Vector3 targetPosition;
        private Vector3 direction;

        private float lerpTime = 0f;
        private float lerpDuration = 0.5f;

        public event Action DoAction;

        // Start is called before the first frame update
        void Start()
        {
            Plane plane = new Plane(Vector3.right, Vector3.zero);
            Vector3 playerPosOnPlane = plane.ClosestPointOnPlane(Player.Instance.transform.position);

            direction = (playerPosOnPlane - transform.position).normalized;
    
            BPMManager.Instance.OnBPM += OnBPM_StartMove;

            SetModeVoid();
        }

        private void SetModeVoid()
        {
            DoAction = DoActionVoid;
        }

        private void DoActionVoid()
        {

        }

        private void SetModeSpawn()
        {
            DoAction = DoActionSpawn;
        }

        private void DoActionSpawn()
        {

        }

        private void SetModeMove()
        {
            DoAction = DoActionMove;

            lerpDuration = BPMManager.Instance.BeatInterval / 2;
            lerpTime = 0f;



            startPosition = transform.position;
            targetPosition = transform.position + direction * distance;

        }

        private void DoActionMove()
        {
            lerpTime += Time.deltaTime / lerpDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, lerpTime);

            // Si le déplacement est terminé
            if (lerpTime >= 1f)
                SetModeVoid();
        }

        private void OnBPM_StartMove() => SetModeMove();

        // Update is called once per frame
        void Update()
        {
            DoAction?.Invoke();
        }
    }
}