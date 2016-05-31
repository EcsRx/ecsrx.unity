using Assets.Examples.RandomReactions.Systems;
using Zenject;

namespace Assets.Examples.RandomReactions.Installers
{
    public class ReactionsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ColorChangingSystem>().ToSelf().AsSingle();
            Container.Bind<CubeSetupSystem>().ToSelf().AsSingle();
            Container.Bind<CubeColourChangerSystem>().ToSelf().AsSingle();
        }
    }
}