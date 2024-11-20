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

        public static Player Instance { get; private set; }

        private Vector3 startPosition;
        private Vector3 endPosition;
        private Vector3 worldPosition;

        private Quaternion targetRotation;
        private Quaternion initialRotation;

        private float distance;
        private float currentDuration;

        private Coroutine checkSwipe;

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

            CheckSwipeInput();
        }

        private void DoActionMove()
        {
            MoveHand();
            RotateHand();

            CheckSwipeInput();
        }

        private void SetModeAttack()
        {
            DoAction = DoActionAttack;

            StopCoroutine(checkSwipe);
            checkSwipe = null;

            currentDuration = 0;
            initialRotation = hand.transform.rotation;

            Quaternion rotationX = new Quaternion(1,0, 0, 0);

            targetRotation = initialRotation * rotationX;
        }

        private void DoActionAttack()
        {
            MoveHand();

            float ratio = currentDuration / attackDuration;

            hand.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, ratio);

            if (currentDuration < attackDuration)
                currentDuration += Time.deltaTime;
            else
                SetModeMove();
        }

        // Update is called once per frame
        public void Update()
        {
            DoAction?.Invoke();
        }

        private void MoveHand()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = Mathf.Abs(Camera.main.transform.position.z - hand.transform.position.z);

                worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            }

            hand.transform.position = Vector3.Lerp(hand.transform.position, new Vector3(worldPosition.x, worldPosition.y, hand.transform.position.z), Time.deltaTime * moveSpeed);
        }

        private void RotateHand()
        {
            Vector3 handScreenPosition = Camera.main.WorldToScreenPoint(hand.transform.position);

            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, handScreenPosition.z);
            Vector3 directionToCenter = handScreenPosition - screenCenter;

            Vector3 targetScreenPosition = screenCenter + directionToCenter.normalized * Mathf.Max(Screen.width, Screen.height);
            Vector3 targetWorldPosition = Camera.main.ScreenToWorldPoint(targetScreenPosition);
            Vector3 direction = (targetWorldPosition - hand.transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);

            // Interpolation fluide vers la rotation cible
            hand.transform.rotation = Quaternion.Slerp(hand.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            //hand.transform.up = direction;
        }

        private IEnumerator CheckSwipe()
        {
            while (true)
            {
                startPosition = Input.mousePosition;
                startPosition.z = 0;

                yield return new WaitForSeconds(swipeTimeThreshold);

                endPosition = Input.mousePosition;
                endPosition.z = 0;

                distance = Vector3.Distance(endPosition, startPosition);

                if (distance > swipeThresholdDistance)
                {
                    SetModeAttack();
                }
            }
        }

        private void CheckSwipeInput()
        {
            if (Input.GetMouseButton(0))
            {
                if (checkSwipe == null) checkSwipe = StartCoroutine(CheckSwipe());
            }
            else
            {
                if (checkSwipe != null) StopCoroutine(checkSwipe);
                checkSwipe = null;
            }
        }
    }
}