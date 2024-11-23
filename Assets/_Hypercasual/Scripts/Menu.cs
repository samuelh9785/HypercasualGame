using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Com.SamuelHOARAU.Hypercasual
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private Button BPMButton60 = default;
        [SerializeField] private Button BPMButton80 = default;

        [SerializeField] private string scene60 = default;
        [SerializeField] private string scene80 = default;

        // Start is called before the first frame update
        void Start()
        {
            BPMButton60.onClick.AddListener(() => StartScene(scene60));
            BPMButton80.onClick.AddListener(() => StartScene(scene80));
        }

        private void StartScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}