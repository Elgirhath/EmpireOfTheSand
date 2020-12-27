namespace Ai
{
    public class MacroState
    {
        public class StateClass
        {
            public int UnitCount { get; set; }
            public int CastleCount { get; set; }
            public int WaterStorageCount { get; set; }
            public int SandStorageCount { get; set; }
        }

        public float Reward { get; set; }
        public bool GameEnded { get; set; }
        public StateClass State { get; set; }
    }
}