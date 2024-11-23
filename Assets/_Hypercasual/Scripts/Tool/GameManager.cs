using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.SamuelHOARAU.Hypercasual
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private EnemySpawner spawner;
        [SerializeField] private string menuScene;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(StopGame());
        }

        private IEnumerator StopGame()
        {
            yield return new WaitForSeconds(25);

            spawner.StopSpawner();

            yield return new WaitForSeconds(5);

            SceneManager.LoadScene(menuScene);
        }
    }
}


