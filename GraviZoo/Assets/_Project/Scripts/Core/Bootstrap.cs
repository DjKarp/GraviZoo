using UnityEngine.SceneManagement;
using UnityEngine;

namespace GraviZoo
{
    public class Bootstrap : MonoBehaviour
    {
        private const string GameplaySceneName = "Gameplay";

        private void Start()
        {
            SceneManager.LoadScene(GameplaySceneName);
        }

    }
}
