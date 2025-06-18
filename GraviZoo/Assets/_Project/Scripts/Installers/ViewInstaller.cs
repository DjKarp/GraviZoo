using UnityEngine;
using Zenject;

namespace GraviZoo
{
    public class ViewInstaller : MonoInstaller
    {
        [SerializeField] private ReloadTilesButton _reloadTilesButton;    
        public override void InstallBindings()
        {
            BindGameplayButtonsInstallers();
        }

        private void BindGameplayButtonsInstallers()
        {
            Container
                .Bind<ReloadTilesButton>()
                .FromInstance(_reloadTilesButton)
                .AsSingle();
        }
    }
}