namespace transport_sim_app.Models
{
    public interface ISimulationScope
    {
        bool AllFinished { get; set; }
        float TrackDistance { get; set; }
    }
}