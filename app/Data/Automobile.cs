namespace transport_sim_app.Data
{
    internal class Automobile : Transport
    {
        public int PersonCount { get; set; }
        public Automobile()
        {
            Type = nameof(Automobile);
        }
    }
}