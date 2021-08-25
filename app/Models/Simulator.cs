
using System;

namespace transport_sim_app.Models
{
    public class Simulator
    {
        private readonly Random _rnd = new Random();
        public bool AllFinished { get; set; }

        public float TrackDistance { get; set; }

        void SimulateRepairing(Transport _transport)
        {
            bool repaired = DateTime.Now.TimeOfDay - _transport.WheelPuncturedAt > _transport.RepairTime;
            if (repaired) _transport.WheelPuncturedAt = null;
        }

        void SimulatePuncturing(Transport _transport)
        {
            if (_rnd.NextDouble() <= _transport.WheelPunctureProbability)
                _transport.WheelPuncturedAt = DateTime.Now.TimeOfDay;
        }

        void SimulateMoving(Transport _transport)
        {
            _transport.DistanceTraveled += _transport.Speed;
        }
        public void Simulate(Transport _transport)
        {
            if (_transport.Finished) return;
            if (_transport.DistanceTraveled >= TrackDistance)
            {
                _transport.FinishedAt = DateTime.Now.TimeOfDay;
                return;
            }
            AllFinished = false;
            if (!_transport.WheelPunctured)
            {
                SimulateMoving(_transport);
                SimulatePuncturing(_transport);
            }
            else
                SimulateRepairing(_transport);
        }
    }
}