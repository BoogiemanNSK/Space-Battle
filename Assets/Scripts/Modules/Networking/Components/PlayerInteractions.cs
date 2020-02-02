using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public class BuyActionTag : IEcsIgnoreInFilter {}
    public class DestroyActionTag : IEcsIgnoreInFilter{}
    public class MoveActionTag : IEcsIgnoreInFilter {};

    public class AttackActionTag : IEcsIgnoreInFilter{}
    public class HealActionTag : IEcsIgnoreInFilter{}
}