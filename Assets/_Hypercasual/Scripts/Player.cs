using System;
using System.Collections;
using UnityEngine;

namespace Com.SamuelHOARAU.Hypercasual
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameObject hand = default;
        [SerializeField] private GameObject body = default;
        [SerializeField] private Sword sword = default;

        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 5f;

        [SerializeField] private float swipeThresholdDistance = 100f;
        [SerializeField] private float swipeTimeThreshold = 0.1f;

        [SerializeField] private float attackDuration = 0.5f;

        [SerializeField] private float radiusX = 1f;
        [SerializeField] private float radiusY = 1.2f;

        public static Player Instance { get; private set; }

        private Vector3 startPosition;
        private Vector3 endPosition;
        private Vector3 worldPosition;
        private Vector3 center;

        private Quaternion targetRotation;
        private Quaternion initialRotation;

        private float distance;
        private float currentDuration;

        private event Action DoAction;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

        // Start is called before the first frame update
        public void Start()
        {
            center = hand.transform.position;
            worldPosition = hand.transform.position;
            SetModeMove();

            BPMManager.Instance.OnBPM += OnBPM_MovePlayer;
            sword.OnHit += OnHit_HitTarget;
        }

        private void OnDestroy()
        {
            sword.OnHit -= OnHit_HitTarget;

            if (Instance == this)
                Instance = null;
        }

        private void OnBPM_MovePlayer()
        {
            //throw new NotImplementedException();
        }

        private void OnHit_HitTarget(bool hit)
        {
            if (hit)
            {

            }
            else
            {

            }

            throw new NotImplementedException();
        }

        private void SetModeVoid()
        {
            DoAction = DoActionVoid;
        }

        private void DoActionVoid()
        {

        }

        private void SetModeMove()
        {
            DoAction = DoActionMove;
        }

        private void DoActionMove()
        {
            MoveHand();
            RotateHand();
        }

        // Update is called once per frame
        public void Update()
        {
            DoAction?.Invoke();
        }

        private void MoveHand()
        {
            Vector3 offset;
            Vector3 mousePosition;

            float distanceX;
            float distanceY;
            float angle;

            if (Input.GetMouseButton(0))
            {
                mousePosition = Input.mousePosition;
                mousePosition.z = Mathf.Abs(Camera.main.transform.position.z - hand.transform.position.z);

                worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            }

            offset = worldPosition - center;
            distanceX = offset.x / radiusX;
            distanceY = offset.y / radiusY;

            if (distanceX * distanceX + distanceY * distanceY > 1f)
            {
                angle = Mathf.Atan2(offset.y, offset.x);

                worldPosition = center + new Vector3(Mathf.Cos(angle) * radiusX,
                                                     Mathf.Sin(angle) * radiusY,
                                                     0f);
            }

            hand.transform.position = Vector3.Lerp(hand.transform.position, new Vector3(worldPosition.x, worldPosition.y, hand.transform.position.z), Time.deltaTime * moveSpeed);
        }

        private void RotateHand()
        {
            Vector3 offset = hand.transform.position - center;
            Vector3 ellipseForward = Vector3.forward;
            Vector3 radialDirection;
            Vector3 targetDirection;

            float distanceX = offset.x / radiusX;
            float distanceY = offset.y / radiusY;
            float distanceSquared = distanceX * distanceX + distanceY * distanceY;
            float angle;
            float distanceToCenterRatio;


            if (distanceSquared < 0.01f)
            {
                hand.transform.up = Vector3.Slerp(hand.transform.up, ellipseForward, Time.deltaTime * rotationSpeed);
                return;
            }

            angle = Mathf.Atan2(offset.y / radiusY, offset.x / radiusX);

            radialDirection = new Vector3(Mathf.Cos(angle) * radiusX,
                                          Mathf.Sin(angle) * radiusY,
                                          0f).normalized;

            distanceToCenterRatio = Mathf.Clamp01(Mathf.Sqrt(distanceSquared));
            targetDirection = Vector3.Slerp(ellipseForward, radialDirection, distanceToCenterRatio);

            hand.transform.up = Vector3.Slerp(hand.transform.up, targetDirection, Time.deltaTime * rotationSpeed);
        }

    }
}