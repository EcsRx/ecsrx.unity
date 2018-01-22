using System;
using UnityEngine;
using System.Collections;
using Zenject;

namespace Zenject.SpaceFighter
{
    // We prefer to use ScriptableObjectInstaller for installers that contain game settings
    // There's no reason why you couldn't use a MonoInstaller here instead, however
    // using ScriptableObjectInstaller has advantages here that make it nice for settings:
    //
    // 1) You can change these values at runtime and have those changes persist across play
    //    sessions.  If it was a MonoInstaller then any changes would be lost when you hit stop
    // 2) You can easily create multiple ScriptableObject instances of this installer to test
    //    different customizations to settings.  For example, you might have different instances
    //    for each difficulty mode of your game, such as "Easy", "Hard", etc.
    // 3) If your settings are associated with a game object composition root, then using
    //    ScriptableObjectInstaller can be easier since there will only ever be one definitive
    //    instance for each setting.  Otherwise, you'd have to change the settings for each game
    //    object composition root separately at runtime
    //
    // Uncomment if you want to add alternative game settings
    //[CreateAssetMenu(menuName = "Space Fighter/Game Settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public EnemySpawner.Settings EnemySpawner;
        public GameRestartHandler.Settings GameRestartHandler;
        public GameInstaller.Settings GameInstaller;
        public PlayerSettings Player;
        public EnemySettings Enemy;

        [Serializable]
        public class PlayerSettings
        {
            public PlayerMoveHandler.Settings PlayerMoveHandler;
            public PlayerShootHandler.Settings PlayerShootHandler;
            public PlayerDamageHandler.Settings PlayerCollisionHandler;
            public PlayerHealthWatcher.Settings PlayerHealthWatcher;
        }

        [Serializable]
        public class EnemySettings
        {
            public EnemyTunables DefaultSettings;
            public EnemyStateIdle.Settings EnemyStateIdle;
            public EnemyRotationHandler.Settings EnemyRotationHandler;
            public EnemyStateFollow.Settings EnemyStateFollow;
            public EnemyStateAttack.Settings EnemyStateAttack;
            public EnemyDeathHandler.Settings EnemyHealthWatcher;
            public EnemyCommonSettings EnemyCommonSettings;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(EnemySpawner);
            Container.BindInstance(GameRestartHandler);
            Container.BindInstance(GameInstaller);

            Container.BindInstance(Player.PlayerMoveHandler);
            Container.BindInstance(Player.PlayerShootHandler);
            Container.BindInstance(Player.PlayerCollisionHandler);
            Container.BindInstance(Player.PlayerHealthWatcher);

            Container.BindInstance(Enemy.EnemyStateIdle);
            Container.BindInstance(Enemy.EnemyRotationHandler);
            Container.BindInstance(Enemy.EnemyStateFollow);
            Container.BindInstance(Enemy.EnemyStateAttack);
            Container.BindInstance(Enemy.EnemyHealthWatcher);
            Container.BindInstance(Enemy.EnemyCommonSettings);
        }
    }
}

