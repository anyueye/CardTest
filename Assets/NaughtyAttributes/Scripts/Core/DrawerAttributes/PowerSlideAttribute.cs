using System;
using Unity.Mathematics;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class PowerSlideAttribute : DrawerAttribute
    {
        public float Base { get; private set; }
        public float MinPower { get; private set; }
        public float MaxPower { get; private set; }
        public float BaseRcpLn { get; private set; }

        public PowerSlideAttribute(float minPower, float maxPower, float baseValue)
        {
            this.MinPower = minPower;
            this.MaxPower = maxPower;
            this.Base = baseValue;
            this.BaseRcpLn = math.log(baseValue);
        }

        public PowerSlideAttribute(int minPower, int maxPower, float baseValue)
        {
            this.MaxPower = minPower;
            this.MaxPower = maxPower;
            this.Base = baseValue;
            this.BaseRcpLn = math.rcp(math.log(baseValue));
        }
    }
}
