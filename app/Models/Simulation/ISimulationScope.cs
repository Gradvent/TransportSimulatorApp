namespace transport_sim_app.Models.Simulation
{
    public interface ISimulationScope
    {
        bool AllFinished { get; }
        float TrackDistance { get; }
    }
}