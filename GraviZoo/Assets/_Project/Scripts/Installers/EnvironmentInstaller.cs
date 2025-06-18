using UnityEngine;
using Zenject;

namespace GraviZoo
{
    public class EnvironmentInstaller : MonoInstaller
    {
        [SerializeField] private ActionBarModel _actionBar;

        public override void InstallBindings()
        {
            BindTopPanel();
        }      
        private void BindTopPanel()
        {
            Container
                .Bind<ActionBarModel>()
                .FromInstance(_actionBar)
                .AsSingle();
        }
    }
}