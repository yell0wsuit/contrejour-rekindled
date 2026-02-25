using Microsoft.Xna.Framework;

namespace Mokus2D.Sensors
{
    public sealed class Accelerometer
    {
        public AccelerometerReading CurrentValue { get; private set; } = new AccelerometerReading(Vector3.Zero);

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }

    public struct AccelerometerReading(Vector3 acceleration)
    {
        public Vector3 Acceleration { get; } = acceleration;
    }
}
