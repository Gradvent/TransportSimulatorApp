namespace transport_sim_app.Configuration
{
    public class SimulationOptions
    {
        public const string Simulation = nameof(Simulation);
        private const int DISTANCE_DEFAULT = 1000;

        public float Distance { get; set; } = DISTANCE_DEFAULT;
        public string SimulationSeed { get; set; }
    }
}