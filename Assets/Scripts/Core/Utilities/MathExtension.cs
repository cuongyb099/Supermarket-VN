namespace Core.Utilities
{
    public static class MathExtension
    {
        public static float NormalizeAngle(this float angle)
        {
            angle %= 360;
            if (angle < 0) angle += 360;
            return angle;
        }
    }
}
