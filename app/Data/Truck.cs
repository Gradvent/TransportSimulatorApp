namespace transport_sim_app.Data
{
    internal class Truck : Transport
    {
        public Truck()
        {
            Type = nameof(Truck);
        }

        public float CargoWeight { get; set; }
    }
}
