namespace cryptolib.Models.Strategy.LevelsStrategy
{
    public class Level
    {
        public float Price;
        public LevelPurpose Purpose;
        public int levelStrange;

        public Level(float _price, LevelPurpose _purpose, int _strangelevel)
        {
            Price = _price;
            Purpose = _purpose;
            levelStrange = _strangelevel;
        }
    }
}