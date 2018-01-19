using System;

namespace Zenject.SpaceFighter
{
    public class PlayerDiedSignal : Signal<PlayerDiedSignal>
    {
    }

    public class EnemyKilledSignal : Signal<EnemyKilledSignal>
    {
    }
}
