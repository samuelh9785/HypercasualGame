using System;
using UnityEngine;

namespace Com.SamuelHOARAU.Hypercasual
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private GameObject body = default;
        [SerializeField] private float distance = 100f;
        [SerializeField] private float speedAfterHit = 20f;

        [SerializeField] private GameObject armorUp = default;
        [SerializeField] private GameObject armorDown = default;
        [SerializeField] private GameObject armorLeft = default;
        [SerializeField] private GameObject armorRight = default;

        private Vector3 startPosition;
        private Vector3 targetPosition;
        private Vector3 direction;

        private float lerpTime = 0f;
        private float lerpDuration = 0.5f;

        private float spawnTime = 0f;
        private float spawnDuration = 0.5f;

        private float despawnTime = 0f;
        private float despawnDuration = 1f;

        private event Action DoAction;
        
        [HideInInspector] public EnemyArmor armor;

        // Start is called before the first frame update
        void Start()
        {
            Plane plane = new Plane(Vector3.right, Vector3.zero);
            Vector3 playerPosOnPlane = plane.ClosestPointOnPlane(Player.Instance.body.transform.position);
            Vector3 enemyPosOnPlane = plane.ClosestPointOnPlane(transform.position);

            direction = ((Player.Instance.body.transform.position + new Vector3(0, 1.5f, 0)) - enemyPosOnPlane).normalized;
            body.transform.localScale = Vector3.zero;
        }

        public void Init(EnemyArmor armorParameter)
        {
            armorUp.SetActive(armorParameter.armorUp);
            armorDown.SetActive(armorParameter.armorBottom);
            armorLeft.SetActive(armorParameter.armorLeft);
            armorRight.SetActive(armorParameter.armorRight);

            SetModeSpawn();
        }

        private void SetModeVoid()
        {
            DoAction = DoActionVoid;
        }

        private void DoActionVoid()
        {

        }

        private void SetModeHit()
        {
            DoAction = DoActionHit;
        }

        private void DoActionHit()
        {
            if (despawnTime < despawnDuration)
            {
                body.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, despawnTime / despawnDuration);
                despawnTime += Time.deltaTime;
            }
            else
            {
                Destroy(body.gameObject);
            }
        }

        private void SetModeSpawn()
        {
            DoAction = DoActionSpawn;
        }

        private void DoActionSpawn()
        {
            if(spawnTime < spawnDuration)
            {
                body.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, spawnTime / spawnDuration);
                spawnTime += Time.deltaTime;
            }
            else
            {
                BPMManager.Instance.OnBPM += OnBPM_StartMove;

                SetModeVoid();
            }
        }

        private void SetModeMove()
        {
            DoAction = DoActionMove;

            lerpDuration = BPMManager.Instance.BeatInterval / 2;
            lerpTime = 0f;

            startPosition = body.transform.position;
            targetPosition = body.transform.position + direction * distance;
        }

        private void DoActionMove()
        {
            lerpTime += Time.deltaTime / lerpDuration;
            body.transform.position = Vector3.Lerp(startPosition, targetPosition, lerpTime);

            // Si le déplacement est terminé
            if (lerpTime >= 1f)
                SetModeVoid();
        }

        public void IsHit(Vector3 swordPosition)
        {
            SetModeHit();
            BPMManager.Instance.OnBPM -= OnBPM_StartMove;

            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;

            armorUp.GetComponent<Collider>().enabled = false;
            armorDown.GetComponent<Collider>().enabled = false;
            armorLeft.GetComponent<Collider>().enabled = false;
            armorRight.GetComponent<Collider>().enabled = false;

            Vector3 direction = (transform.position - swordPosition).normalized;
            GetComponent<Rigidbody>().velocity = direction * speedAfterHit;
        }

        private void OnBPM_StartMove() => SetModeMove();

        // Update is called once per frame
        void Update()
        {
            DoAction?.Invoke();
        }
    }
}