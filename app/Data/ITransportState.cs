namespace transport_sim_app.Data
{
    public interface ITransportState
    {
        string Id { get; set; }
        string Type { get; set; }
        string Name { get; set; }
        float Speed { get; set; }
        float WheelPunctureProbability { get; set; }
        float RepairTimeSeconds { get; set; }
    }
}