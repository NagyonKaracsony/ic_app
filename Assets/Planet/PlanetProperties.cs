namespace Assets
{
    /// <summary>Size category type of the planet.</summary>
    public enum SizeCategoryUnit { ExtremeSmall, Small, Medium, Large, ExtremeLarge }
    /// <summary>Gravity category type of the planet.</summary>
    public enum GravityCategoryUnit { ExtremeLow, Low, Medium, High, ExtremeHigh }
    /// <summary>Temperature category type of the planet.</summary>
    public enum TemperatureCategoryUnit { ExtremeCold, Cold, Temperate, Hot, ExtremeHot }
    /// <summary>Gravity category of the planet.</summary>
    public enum PressureCategoryUnit { None, ExtremeLow, Low, Medium, High, ExtremeHigh }
    /// <summary>Holds information on the properties of the planet.</summary>
    public class PlanetProperties
    {
        public SizeCategoryUnit SizeCategory;
        public GravityCategoryUnit GravityCategory;
        public TemperatureCategoryUnit TemperatureCategory;
        public PressureCategoryUnit PressureCategory;
        /// <summary> The Mass of the planet, in kilograms.</summary>
        public double Mass;
        /// <summary> The radius of the planet, in kilometers.</summary>
        public double Radius;
        /// <summary> The surface gravity of the planet, in m/s².</summary>
        public double SurfaceGravity;
        /// <summary> The surface gravity of the planet, in kelvin degrees.</summary>
        public double Temperature;
        /// <summary> The atmospheric Pressure of the planet, in atmospheres. </summary>
        public double AtmosphericPressure;
        public PlanetProperties()
        {

        }
    }
}