using System;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Visual.Data;

namespace Mokus2D.Visual
{
    public class Label : LabelBase
    {
        public string TextString
        {
            get
            {
                return text.ToString();
            }
            set
            {
                text.Clear();
                text.Append(value);
            }
        }

        public Label(SpriteFont font, string textString)
            : this(font)
        {
            Append(textString);
        }

        public Label(SpriteFont font)
            : base(font)
        {
            IgnoreMissingCharacters = true;
            text = new StringBuilder();
            textDirty = true;
        }

        public Label()
            : base()
        {
            IgnoreMissingCharacters = true;
            text = new StringBuilder();
            textDirty = true;
        }

        public override Vector2 Size
        {
            get
            {
                TryRefreshText();
                return base.Size;
            }
        }

        public Label Append(char value, int repeatCount)
        {
            text.Append(value, repeatCount);
            textDirty = true;
            return this;
        }

        public Label Append(char[] value, int startIndex, int charCount)
        {
            text.Append(value, startIndex, charCount);
            textDirty = true;
            return this;
        }

        public Label Append(string value)
        {
            text.Append(value);
            textDirty = true;
            return this;
        }

        public Label AppendLine()
        {
            text.AppendLine();
            textDirty = true;
            return this;
        }

        public Label AppendLine(string value)
        {
            text.AppendLine(value);
            textDirty = true;
            return this;
        }

        public Label Append(string value, int startIndex, int count)
        {
            text.Append(value, startIndex, count);
            textDirty = true;
            return this;
        }

        public Label Insert(int index, string value, int count)
        {
            text.Insert(index, value, count);
            textDirty = true;
            return this;
        }

        public Label Remove(int startIndex, int length)
        {
            text.Remove(startIndex, length);
            textDirty = true;
            return this;
        }

        public Label Append(bool value)
        {
            text.Append(value);
            textDirty = true;
            return this;
        }

        public Label Append(sbyte value)
        {
            text.Append(value);
            textDirty = true;
            return this;
        }

        public Label Append(byte value)
        {
            text.Append(value);
            textDirty = true;
            return this;
        }

        public Label Append(char value)
        {
            text.Append(value);
            textDirty = true;
            return this;
        }

        public Label Append(short value)
        {
            text.Append(value);
            textDirty = true;
            return this;
        }

        public Label Append(int value)
        {
            text.Append(value);
            textDirty = true;
            return this;
        }

        public Label Append(long value)
        {
            text.Append(value);
            textDirty = true;
            return this;
        }

        public Label Append(float value)
        {
            text.Append(value);
            textDirty = true;
            return this;
        }

        public Label Append(double value)
        {
            text.Append(value);
            textDirty = true;
            return this;
        }

        public Label Append(ushort value)
        {
            text.Append(value);
            textDirty = true;
            return this;
        }

        public Label Append(uint value)
        {
            text.Append(value);
            textDirty = true;
            return this;
        }

        public Label Append(ulong value)
        {
            text.Append(value);
            textDirty = true;
            return this;
        }

        public Label Append(object value)
        {
            text.Append(value);
            textDirty = true;
            return this;
        }

        public Label Append(char[] value)
        {
            text.Append(value);
            textDirty = true;
            return this;
        }

        public Label Insert(int index, string value)
        {
            text.Insert(index, value);
            textDirty = true;
            return this;
        }

        public Label Insert(int index, char[] value)
        {
            text.Insert(index, value);
            textDirty = true;
            return this;
        }

        public Label Insert(int index, char[] value, int startIndex, int charCount)
        {
            text.Insert(index, value, startIndex, charCount);
            textDirty = true;
            return this;
        }

        public Label AppendFormat(string format, params object[] args)
        {
            text.AppendFormat(format, args);
            textDirty = true;
            return this;
        }

        public Label AppendFormat(IFormatProvider provider, string format, params object[] args)
        {
            text.AppendFormat(provider, format, args);
            textDirty = true;
            return this;
        }

        public Label Replace(string oldValue, string newValue)
        {
            text.Replace(oldValue, newValue);
            textDirty = true;
            return this;
        }

        public Label Replace(string oldValue, string newValue, int startIndex, int count)
        {
            text.Replace(oldValue, newValue, startIndex, count);
            textDirty = true;
            return this;
        }

        public Label Replace(char oldChar, char newChar)
        {
            text.Replace(oldChar, newChar);
            textDirty = true;
            return this;
        }

        public Label Replace(char oldChar, char newChar, int startIndex, int count)
        {
            text.Replace(oldChar, newChar, startIndex, count);
            textDirty = true;
            return this;
        }

        public int Length
        {
            get
            {
                return text.Length;
            }
            set
            {
                text.Length = value;
            }
        }

        public int Capacity
        {
            get
            {
                return text.Capacity;
            }
            set
            {
                text.Capacity = value;
            }
        }

        public char this[int index]
        {
            get
            {
                return text[index];
            }
            set
            {
                text[index] = value;
            }
        }

        public int EnsureCapacity(int capacity)
        {
            return text.EnsureCapacity(capacity);
        }

        public Label Clear()
        {
            text.Clear();
            textDirty = true;
            return this;
        }

        public Label AppendText(object value)
        {
            text.Append(value);
            textDirty = true;
            return this;
        }

        protected virtual void RefreshText()
        {
            Size = MeasureString(text);
        }

        protected Vector2 MeasureString(StringBuilder textValue)
        {
            Vector2 vector;
            try
            {
                vector = Font.MeasureString(textValue);
            }
            catch (Exception)
            {
                if (!IgnoreMissingCharacters)
                {
                    throw;
                }
                vector = Vector2.Zero;
            }
            return vector;
        }

        protected void TryRefreshText()
        {
            if (textDirty)
            {
                RefreshText();
                textDirty = false;
            }
        }

        protected override void DrawSprite(VisualState state, Color color)
        {
            TryRefreshText();
            DrawText(state, color);
        }

        protected virtual void DrawText(VisualState state, Color color)
        {
            if (text.Length > 0)
            {
                DrawLine(state, color, text, Vector2.Zero);
            }
        }

        protected void DrawLine(VisualState state, Color color, StringBuilder line, Vector2 position)
        {
            try
            {
                batch.DrawString(Font, line, position, color, 0f, AnchorInPixels, state.SpritesScaleFactor, SpriteEffects.None, 0f);
            }
            catch (Exception)
            {
                if (!IgnoreMissingCharacters)
                {
                    throw;
                }
            }
        }

        public bool IgnoreMissingCharacters;

        protected StringBuilder text;

        protected bool textDirty;
    }
}
