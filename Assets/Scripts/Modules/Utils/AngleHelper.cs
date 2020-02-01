namespace Modules.Utils
{
    public static class AngleHelper
    {
        public static bool IsAngleInRange(float angle, float min, float max)
        {
            angle = Normalize(angle);            
            min = Normalize(min);
            max = Normalize(max);

            if (min < max)
            {
                return min < angle && angle < max;
            }
            return min < angle || angle < max;
        }

        public static float GetAnglePosition(float angle, float min, float max)
        {
            if (!IsAngleInRange(angle, min, max))
                return 0;

            angle = Normalize(angle);
            min = Normalize(min);
            max = Normalize(max);

            if (min > max)
            {
                max += 360;                
            }

            if (angle < min)
            {
                angle += 360;
            }

            return (angle - min) / (max - min);
        }

        public static float Normalize(float angle)
        {
            angle %= 360;
            if (angle < 0)
                angle += 360;
            return angle;
        }
    }
}
