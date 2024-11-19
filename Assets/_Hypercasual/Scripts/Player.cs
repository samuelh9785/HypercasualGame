using System;
using System.Collections;
using UnityEngine;

namespace Com.SamuelHOARAU.Hypercasual
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameObject hand = default;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 5f;

        [SerializeField] private float swipeThresholdDistance = 100f;
        [SerializeField] private float swipeTimeThreshold = 0.1f;

        private Vector3 startPosition;
        private Vector3 endPosition;
        private Vector3 worldPosition;

        private float distance;

        private Coroutine checkSwipe;

        private event Action DoAction;

        // Start is called before the first frame update
        public void Start()
        {
            worldPosition = hand.transform.position;
            SetModeMove();
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

            if (Input.GetMouseButtonDown(0))
            {
                if (checkSwipe != null) StopCoroutine(checkSwipe);
                checkSwipe = StartCoroutine(CheckSwipe());
            }

            if (Input.GetMouseButtonUp(1))
            {
                StopCoroutine(checkSwipe);
            }
        }

        private void SetModeAttack()
        {
            DoAction = DoActionAttack;
        }

        private void DoActionAttack()
        {

        }

        // Update is called once per frame
        public void Update()
        {
            DoAction();
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

                if(distance > swipeThresholdDistance) 
                {
                    Debug.Log("Swipe done");
                }
            }     
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

            hand.transform.up = direction;
        }
    }
}