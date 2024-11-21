using System;
using UnityEngine;

namespace Com.SamuelHOARAU.Hypercasual
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyPattern enemyPattern = default;
        [SerializeField] private GameObject enemyPrefab = default;
        [SerializeField] private int BPMIntervalSpawn = 5;
        [SerializeField] private float spacing = 20f;

        private int currentBPM = 0;
        private event Action DoAction;
        

        // Start is called before the first frame update
        void Start()
        {
            SetModeSpawn();
        }

        // Update is called once per frame
        void Update()
        {
            DoAction();
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

            BPMManager.Instance.OnBPM += OnBPM_CheckSpawn;
        }

        private void DoActionSpawn()
        {

        }

        private void OnBPM_CheckSpawn()
        {
            currentBPM++;

            if(currentBPM >= BPMIntervalSpawn)
            {
                currentBPM = 0;
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            int indexPatern = UnityEngine.Random.Range(0, enemyPattern.ListPattern.Count);
            int nbEnemy = enemyPattern.ListPattern[indexPatern].listEnemyArmor.Count;
            float startOffset = -(nbEnemy - 1) * spacing / 2;
            float xOffset;

            Vector3 spawnPosition;
            EnemyArmor armor;

            for (int i = 0; i < nbEnemy; i++)
            {
                xOffset = startOffset + i * spacing;
                spawnPosition = transform.position + new Vector3(xOffset, 0, 0);
                armor = enemyPattern.ListPattern[indexPatern].listEnemyArmor[i];

                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity).GetComponentInChildren<Enemy>().Init(armor);
            }
        }
    }
}
