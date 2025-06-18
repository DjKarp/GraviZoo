using UnityEngine;
using Zenject;


namespace GraviZoo
{
    public class EntryPoint : MonoBehaviour
    {
        private GamePresenter _gamePresenter;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(GamePresenter gamePresenter, SignalBus signalBus)
        {
            _gamePresenter = gamePresenter;
            _signalBus = signalBus;
        }

        private void Start()
        {
            _gamePresenter.Init();
        }
    }
}
