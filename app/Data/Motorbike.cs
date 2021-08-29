namespace transport_sim_app.Data
{
    class Motorbike : Transport
    {
        public Motorbike()
        {
            Type = nameof(Motorbike);
        }
        public bool HasSidecar { get; set; }
    }
}