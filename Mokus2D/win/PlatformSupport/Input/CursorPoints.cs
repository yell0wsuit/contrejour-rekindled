using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mokus2D.win.PlatformSupport.Input
{
    public static class CursorPoints
    {
        public static List<CursorPoint> GetCursorPoints()
        {
            points.Clear();
            FillPoints(points);
            return points;
        }

        private static void FillPoints(List<CursorPoint> cursorPoints)
        {
            foreach (TouchLocation touchLocation in TouchPanel.GetState())
            {
                cursorPoints.Add(new CursorPoint(touchLocation.Position, touchLocation.Id));
            }

            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                cursorPoints.Add(new CursorPoint(new Vector2(mouse.X, mouse.Y), MouseTouchId));
            }
        }

        private const int MouseTouchId = 1000000;

        private static readonly List<CursorPoint> points = new(64);
    }
}
