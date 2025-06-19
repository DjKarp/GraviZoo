using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;


namespace GraviZoo
{
    public class EntryPoint : MonoBehaviour
    {
        private const string BootstrapSceneName = "Bootstrap";

        private GamePresenter _gamePresenter;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(GamePresenter gamePresenter, SignalBus signalBus)
        {
            _gamePresenter = gamePresenter;
            _signalBus = signalBus;

            _signalBus.Subscribe<RestartSceneSignal>(Restart);
        }

        private void Start()
        {
            _gamePresenter.Init();
        }

        private void Restart(RestartSceneSignal restartSceneSignal)
        {
            StartCoroutine(RestartScene());
        }

        private IEnumerator RestartScene()
        {
            yield return new WaitForSeconds(2.0f);

            SceneManager.LoadScene(BootstrapSceneName);
        }
    }
}
