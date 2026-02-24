using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Mokus2D;
using Mokus2D.Input;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class CocosUtil
    {
        public static int CornerOffset
        {
            get
            {
                return iPad(36, 50);
            }
        }

        public static float RetinaWp7(float value)
        {
            return value / 2f;
        }

        public static float Wp7Retina(float value)
        {
            return value * 2f;
        }

        public static int Wp7Retina(int value)
        {
            return value * 2;
        }

        public static Vector2 Vector2Retina(float x, float y)
        {
            return Vector2Retina(new Vector2(x, y));
        }

        public static Vector2 Retina2Vector(Vector2 point)
        {
            return point / 2f;
        }

        public static Vector2 Vector2Retina(Vector2 point)
        {
            return point * 2f;
        }

        public static T lite<T>(T liteValue, T value)
        {
            return !Constants.IsTrial ? value : liteValue;
        }

        public static T iPad<T>(T ipadValue, T iPhoneValue)
        {
            return Constants.IS_IPAD ? ipadValue : iPhoneValue;
        }

        public static T iPad<T>(ref T ipadValue, ref T iPhoneValue)
        {
            return Constants.IS_IPAD ? ipadValue : iPhoneValue;
        }

        public static bool isArmV7()
        {
            return true;
        }

        public static CGSize CGSizeMult(CGSize size, float mult)
        {
            return new CGSize(size.Width * mult, size.Height * mult);
        }

        public static Vector2 sizeToPoint(CGSize size)
        {
            return new Vector2(size.Width, size.Height);
        }

        public static Vector2 toRetina(Vector2 value)
        {
            return Constants.IS_RETINA ? value * 1f : value;
        }

        public static Vector2 fromRetina(Vector2 value)
        {
            return Constants.IS_RETINA ? value / 1f : value;
        }

        public static Vector2 toIPhone(Vector2 value)
        {
            return !Constants.IS_IPAD ? value * 1f : value;
        }

        public static CGSize sizeToIPhone(CGSize value)
        {
            return !Constants.IS_IPAD ? CGSizeMult(value, 1f) : value;
        }

        public static float iPadValue(float ipadValue)
        {
            return Constants.IS_IPAD ? ipadValue : ipadValue / 1f;
        }

        public static float iPhoneValue(float ipadValue)
        {
            return Constants.IS_IPAD ? ipadValue : ipadValue * 1f;
        }

        public static float iPad(float ipadValue, float iphoneValue)
        {
            return Constants.IS_IPAD ? ipadValue : iphoneValue;
        }

        public static float retinaValue(float value)
        {
            return Constants.IS_RETINA ? value * 1f : value;
        }

        public static float r(float value)
        {
            return Constants.IS_IPAD || Constants.IS_RETINA ? value : value / 1f;
        }

        public static CGSize cgs2(CGSize size)
        {
            return Constants.IS_IPAD || Constants.IS_RETINA ? size : new CGSize(size.Width / 1f, size.Height / 1f);
        }

        public static CGSize cgsIPad(float width, float height)
        {
            return Constants.IS_IPAD ? new CGSize(width, height) : new CGSize(width / 1f, height / 1f);
        }

        public static Vector2 ccp2Point(Vector2 point)
        {
            return Constants.IS_IPAD || Constants.IS_RETINA ? point : point / 1f;
        }

        public static Vector3 ccp2Point(Vector3 point)
        {
            return Constants.IS_IPAD || Constants.IS_RETINA ? point : point / 1f;
        }

        public static Vector2 ccp2(float x, float y)
        {
            return Constants.IS_IPAD || Constants.IS_RETINA ? new Vector2(x, y) : new Vector2(x / 1f, y / 1f);
        }

        public static Vector2 ccpInt(Vector2 point)
        {
            return new Vector2((int)point.X, (int)point.Y);
        }

        public static Vector2 ccpIPad(float x, float y)
        {
            return Constants.IS_IPAD ? new Vector2(x, y) : new Vector2(x / 1f, y / 1f);
        }

        public static Vector2 toIPad(Vector2 ipadValue)
        {
            return ccpIPad(ipadValue.X, ipadValue.Y);
        }

        public static Color ccc3FromInt(int color)
        {
            int num = color % 256;
            color >>= 8;
            int num2 = color % 256;
            color >>= 8;
            return new Color(color % 256, num2, num);
        }

        public static Color ccc4ToCcc3(Color color)
        {
            return new Color(color.R, color.G, color.B);
        }

        public static Color ccc3ToCcc4(Color color, ushort opacity)
        {
            return new Color(color.R, color.G, color.B, opacity);
        }

        public static Color ccc4ChangeAlpha(Color color, ushort opacity)
        {
            return new Color(color.R, color.G, color.B, opacity);
        }

        public static Color ccc4Mix(Color color1, Color color2, float color1Coeff)
        {
            return Color.Lerp(color1, color2, 1f - color1Coeff);
        }

        public static Color ccc3Mix(Color color1, Color color2, float color1Coeff)
        {
            return Color.Lerp(color1, color2, 1f - color1Coeff);
        }

        public static Color ccc3Mult(Color color, float mult)
        {
            return new Color(color.R * mult, color.G * mult, color.B * mult);
        }

        public static Vector2 ScreenCenter()
        {
            return ScreenPosition(new Vector2(0.5f, 0.5f));
        }

        public static Vector2 ScreenPosition(Vector2 anchor)
        {
            CGSize cgsize = Mokus2DGame.ScreenSize;
            return new Vector2(cgsize.Width * anchor.X, cgsize.Height * anchor.Y);
        }

        public static Vector2 TouchPointInNode(Touch touch, Node node)
        {
            return node.GlobalToLocal(touch.Position, true);
        }

        public static Color StringToColor(string stringP)
        {
            List<int> list = new();
            return new Color(list[3], list[2], list[1], list[0]);
        }

        public static void ReplaceNodeWithCleanup(Node node, Node otherNode)
        {
            Node parent = node.Parent;
            node.RemoveFromParent();
            parent.AddChild(otherNode, node.Layer);
        }

        private const float TEXTURE_SCALE_FACTOR = 1f;

        private const int RETINA_COORDS_MULTIPLIER = 2;

        private static bool armChecked;

        private static bool _isArmV7;
    }
}
