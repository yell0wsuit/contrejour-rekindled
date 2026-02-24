using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

using Mokus2D.Extensions;

namespace Default.Namespace
{
    public static class PlasticineConstants
    {
        public static void ApplyStaticBodiesFilter(Body body)
        {
            foreach (Fixture fixture in body.FixtureList)
            {
                ApplyStaticBodiesFilter(fixture);
            }
        }

        public static void ApplyStaticBodiesFilter(Fixture fixture)
        {
            fixture.CollisionCategories = Category.Cat3;
        }

        public static void ApplyActiveBodiesFilter(Body body)
        {
            foreach (Fixture fixture in body.FixtureList)
            {
                ApplyActiveBodiesFilter(fixture);
            }
        }

        public static void ApplyActiveBodiesFilter(Fixture fixture)
        {
            fixture.CollisionCategories = Category.Cat2;
            fixture.CollidesWith = Category.Cat1 | Category.Cat4 | Category.Cat5 | Category.Cat6 | Category.Cat7 | Category.Cat8 | Category.Cat9 | Category.Cat10 | Category.Cat11 | Category.Cat12 | Category.Cat13 | Category.Cat14 | Category.Cat15 | Category.Cat16 | Category.Cat17 | Category.Cat18 | Category.Cat19 | Category.Cat20 | Category.Cat21 | Category.Cat22 | Category.Cat23 | Category.Cat24 | Category.Cat25 | Category.Cat26 | Category.Cat27 | Category.Cat28 | Category.Cat29 | Category.Cat30 | Category.Cat31;
        }

        public const float GRASS_OFFSET = 0.51666665f;

        public const float WIDE_BORDER_OFFSET = -0.41666666f;

        public const int PLASTICINE_BEZIER_POINTS = 3;

        public const float OUT_DIFF = 0.06666667f;

        public const float TOP_OFFSET = 0.5833333f;

        public const float SURFACE_OFFSET = 0.5833333f;

        public const float GROUND_OUT_OFFSET = 0.16666667f;

        public const float INNER_DRAG_OFFSET = -0.33333334f;

        public const float MAX_ORTO_OFFSET = 0.4f;

        public const float MAX_DRAG_OFFSET = 1.3333334f;

        public const float START_DRAG_TIME = 0.1f;

        public const float PLASTICINE_DENSITY = 0.3f;

        public const float SURFACE_WIDTH = 2.3333333f;

        public const float THICKESS = 0.8333333f;

        public const float PLASTICINE_PART_WIDTH = 0.6f;

        public const float PLASTICINE_WIDTH = 0.6f;

        public static readonly LightColor BLUE = new(new Color(0, 0, 0, 255), ContreJourConstants.BLUE_LIGHT_COLOR.ChangeAlpha(byte.MaxValue));

        public static readonly LightColor Green = new(new Color(0, 0, 0, 255), ContreJourConstants.GreenLightColor.ChangeAlpha(byte.MaxValue));

        public static readonly LightColor LAST_LIGHT = new(new Color(0, 0, 0, 255), new Color(10, 10, 10, 255));

        public static readonly LightColor WHITE = new(new Color(0, 0, 0, 255), ContreJourConstants.WHITE_LIGHT_COLOR.ChangeAlpha(byte.MaxValue));

        public static readonly Color WHITE_GROUND_COLOR = new(204, 204, 204, 255);

        public static readonly LightColor BLACK_LIGHT = new(WHITE_GROUND_COLOR, new Color(162, 162, 162, 255));

        public static readonly Color WHITE_GROUND_OUT_COLOR = WHITE_GROUND_COLOR.ChangeAlpha(0);

        public static readonly Color BLACK_BORDER_COLOR = ContreJourConstants.BLUE_LIGHT_COLOR.ChangeAlpha(byte.MaxValue);

        public static readonly Color BLACK_BORDER_OUT_COLOR = BLACK_BORDER_COLOR.ChangeAlpha(0);
    }
}
