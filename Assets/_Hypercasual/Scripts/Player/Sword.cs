using UnityEngine;

namespace Com.SamuelHOARAU.Hypercasual
{
    public delegate void OnHitHandler(bool hit);

    public class Sword : MonoBehaviour
    {
        [SerializeField] private Collider bladeCollider = default;

        public event OnHitHandler OnHit;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                if (other.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.IsHit(transform.position);
                    OnHit?.Invoke(true);
                }
            }

            if (other.CompareTag("Shield"))
            {
                Debug.Log("Fail");
                OnHit?.Invoke(false);
            }
        }

        public void EnableCollider()
        {
            bladeCollider.enabled = true;
        }

        public void DisableCollider()
        {
            bladeCollider.enabled = false;
        }
    }
}