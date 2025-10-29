using UnityEngine;

public static class GameConfig
{
    // Thông số tiểu hành tinh
    public static class AsteroidConfig
    {
        public const float MIN_SPEED = 1f;
        public const float MAX_SPEED = 3f;
        public const int BASE_HEALTH = 1;
        public const float MIN_DIRECTION_X = -1f;
        public const float MAX_DIRECTION_X = 0f;
        public const float MIN_DIRECTION_Y = -1f;
        public const float MAX_DIRECTION_Y = 1f;
    }

    // Thông số sao
    public static class StarConfig
    {
        public const float MIN_MOVE_SPEED = 0.5f;
        public const float MAX_MOVE_SPEED = 3f;
        public const float MIN_TARGET_SPEED = 0.1f;
        public const float MAX_TARGET_SPEED = 1f;
        public const int SCORE_VALUE = 1;
        
        // Giới hạn vị trí spawn
        public const float MIN_SPAWN_X = -9f;
        public const float MAX_SPAWN_X = 9f;
        public const float MIN_SPAWN_Y = -5f;
        public const float MAX_SPAWN_Y = 5f;
    }

    // Thông số level
    public static class LevelConfig
    {
        // Giá trị cơ bản cho level 1
        public const float BASE_SPAWN_INTERVAL = 3.0f;        // Spawn chậm hơn ở level 1
        public const float BASE_SPEED_MULTIPLIER = 1.0f;
        public const float BASE_STAR_SPAWN_CHANCE = 0.25f;    // Giảm tỉ lệ xuất hiện sao (25%)
        public const int BASE_SCORE_GOAL = 5;               // Giảm số sao cần thu thập

        // Hệ số thay đổi cho mỗi level
        public const float SPAWN_INTERVAL_DECREASE_PER_LEVEL = 0.3f;  // Giảm 0.3s mỗi level
        public const float SPEED_INCREASE_PER_LEVEL = 0.15f;         // Tăng 15% tốc độ mỗi level
        public const float STAR_CHANCE_DECREASE_PER_LEVEL = 0.03f;   // Giảm 3% tỉ lệ sao mỗi level
        public const int SCORE_GOAL_INCREASE_PER_LEVEL = 5;         // Tăng 5 điểm mỗi level
    }

    // Thông số World
    public static class WorldConfig
    {
        public const float DESTROY_POSITION_X = -11f; // Vị trí vật thể bị hủy
        public const float BASE_WORLD_SPEED = 1f;
        public const float PAUSED_TIME_SCALE = 0f;
        public const float NORMAL_TIME_SCALE = 1f;
    }

    // Thông số UI
    public static class UIConfig
    {
        public const string SCORE_FORMAT = "Score: {0} / {1}";
        public const string LEVEL_FORMAT = "Level {0}";
        public const string WIN_TEXT = "YOU WIN!";
        public const string LOSE_TEXT = "GAME OVER";
    }
}