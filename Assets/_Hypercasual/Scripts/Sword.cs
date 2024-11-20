using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.SamuelHOARAU.Hypercasual
{
    public delegate void OnHitHandler(bool hit);

    public class Sword : MonoBehaviour
    {
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
            Debug.Log("Hit");
        }
    }
}