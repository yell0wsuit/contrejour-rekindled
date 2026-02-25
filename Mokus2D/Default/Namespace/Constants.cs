using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

namespace Default.Namespace
{
    public static class Constants
    {
        public static bool IsTrial
        {
            get
            {
                if (!isTrialGet)
                {
                    isTrialGet = true;
                    isTrial = Guide.IsTrialMode;
                }
                return isTrial;
            }
        }

        public static int LevelsToPlay => !isTrial ? 20 : 10;

        public const string CONFIG = "config";

        public const string TYPE = "type";

        public const string POSITION = "position";

        public const float PRECISION = 1E-10f;

        public const int BonusChapter = 5;

        public const int BlueChapter = 1;

        public const int WhiteChapter = 3;

        public const int RoseChapter = 4;

        public const int LEVELS_IN_CHAPTER = 20;

        public const int TrialLevelsInChapter = 10;

        public const int K_GAME_AUTOROTATION_NONE = 0;

        public const int K_GAME_AUTOROTATION_CC_DIRECTOR = 1;

        public const int K_GAME_AUTOROTATION_UI_VIEW_CONTROLLER = 2;

        private static bool isTrial;

        private static bool isTrialGet;

        public static bool IS_IPAD = false;

        public static bool IS_RETINA = true;

        public static readonly int ChaptersCount = IsTrial ? 2 : 6;

        public static readonly int NormalChaptersCount = IsTrial ? 2 : 5;

        public static class config
        {
            public const int velocityIterations = 16;

            public const int positionIterations = 20;

            public const float sizeMultiplier = 0.033333335f;

            public const float density = 0.3f;

            public const float restitution = 0f;

            public const float friction = 1f;

            public static readonly Vector2 gravity = new(0f, 10f);
        }
    }
}
