namespace cryptolib.Models.Strategy.LevelsStrategy
{
    public struct Level
    {
        public float Price { get; init; }
        public LevelPurpose Purpose { get; init; }
        public int levelStrange { get; set; }

        public Level(float _price, LevelPurpose _purpose, int _strangelevel)
        {
            Price = _price;
            Purpose = _purpose;
            levelStrange = _strangelevel;
        }
    }
}