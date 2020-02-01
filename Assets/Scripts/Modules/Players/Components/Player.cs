using System.Collections.Generic;
using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public class Player
    {
        public string Name;
        public int HP;
        public int Power;
        public int Location;
    }

    public class UserPlayer : IEcsIgnoreInFilter {}

    public class PlayersUpdateTag : IEcsIgnoreInFilter{}
}