namespace Modules.CoreGame
{
    public enum WorldPointType
    {
        Planet = 0,
        Asteroid = 1,
        Station  = 2
    }
    
    public class WorldPoint
    {
        public WorldPointType PointType;
        public int PointID;
    }
}