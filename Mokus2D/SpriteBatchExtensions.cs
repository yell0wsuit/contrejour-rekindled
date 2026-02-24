using System;
using System.Globalization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public static class SpriteBatchExtensions
{
    public static Vector2 DrawInt32(this SpriteBatch spriteBatch, SpriteFont spriteFont, int value, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, int layerDepth)
    {
        if (spriteBatch == null)
        {
            throw new ArgumentNullException("spriteBatch");
        }
        if (spriteFont == null)
        {
            throw new ArgumentNullException("spriteFont");
        }
        Vector2 vector = position;
        if (value == -2147483648)
        {
            vector.X += spriteFont.MeasureString(minValue).X;
            spriteBatch.DrawString(spriteFont, minValue, position, color, rotation, origin, scale, effects, layerDepth);
            position = vector;
        }
        else
        {
            if (value < 0)
            {
                vector.X += spriteFont.MeasureString("-").X;
                spriteBatch.DrawString(spriteFont, "-", position, color);
                value = -value;
                position = vector;
            }
            int num = 0;
            do
            {
                int num2 = value % 10;
                value /= 10;
                charBuffer[num] = digits[num2];
                xposBuffer[num] = spriteFont.MeasureString(digits[num2]).X;
                num++;
            }
            while (value > 0);
            for (int i = num - 1; i >= 0; i--)
            {
                vector.X += xposBuffer[i];
                spriteBatch.DrawString(spriteFont, charBuffer[i], position, color, rotation, origin, scale, effects, layerDepth);
                position = vector;
            }
        }
        return position;
    }

    private static string[] digits = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"];

    private static string[] charBuffer = new string[10];

    private static float[] xposBuffer = new float[10];

    private static readonly string minValue = int.MinValue.ToString(CultureInfo.InvariantCulture);
}
