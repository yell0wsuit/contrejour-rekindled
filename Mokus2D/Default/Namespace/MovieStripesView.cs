using Microsoft.Xna.Framework;

using Mokus2D.Effects.Actions;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class MovieStripesView : Node
    {
        public EventSender RestartEvent
        {
            get
            {
                return restartEvent;
            }
        }

        public EventSender MenuEvent
        {
            get
            {
                return menuEvent;
            }
        }

        public MovieStripesView(bool blackSide, bool fade)
        {
            restartEvent = new EventSender();
            menuEvent = new EventSender();
            CGSize cgsize = ScreenConstants.W7FromIPhoneSize;
            topSquare = new ColorSquare(BLACK_COLOR_3, new CGSize(cgsize.Width, CocosUtil.iPad(150f, STRIPES_HEIGHT_IPHONE)));
            bottomSquare = new ColorSquare(BLACK_COLOR_3, new CGSize(cgsize.Width, CocosUtil.iPad(150f, STRIPES_HEIGHT_IPHONE)));
            if (fade)
            {
                fadeSquare = new ColorSquare(ContreJourConstants.WHITE_COLOR_3, new CGSize(cgsize.Width, cgsize.Height));
                AddChild(fadeSquare);
                fadeSquare.OpacityFloat = 0f;
            }
            fadeOpacity = 0.7058824f;
            if (blackSide)
            {
                fadeSquare.Color = ContreJourConstants.BLUE_LIGHT_COLOR;
                fadeOpacity = 0.7058824f;
            }
            AddChild(topSquare);
            AddChild(bottomSquare);
            bottomSquare.Position = new Vector2(0f, -CocosUtil.iPad(150f, STRIPES_HEIGHT_IPHONE) - 4f);
            topSquare.Position = new Vector2(0f, cgsize.Height + 4f);
        }

        public void Show()
        {
            if (fadeSquare != null)
            {
                fadeSquare.Run(new FadeTo(FINISH_DURATION, fadeOpacity));
            }
            MoveTo moveTo = new(FINISH_DURATION, new Vector2(topSquare.Position.X, topSquare.Position.Y - CocosUtil.iPad(150f, STRIPES_HEIGHT_IPHONE)));
            MoveTo moveTo2 = new(FINISH_DURATION, new Vector2(bottomSquare.Position.X, bottomSquare.Position.Y + CocosUtil.iPad(150f, STRIPES_HEIGHT_IPHONE)));
            bottomSquare.Run(moveTo2);
            topSquare.Run(moveTo);
        }

        public const float STRIPES_HEIGHT = 150f;

        protected Node topSquare;

        protected Node bottomSquare;

        protected ColorSquare fadeSquare;

        protected EventSender restartEvent;

        protected EventSender menuEvent;

        protected float fadeOpacity;

        private Color BLACK_COLOR_3 = ContreJourConstants.BLACK_COLOR_3;

        protected float FINISH_DURATION = 0.8f;

        public static readonly float STRIPES_HEIGHT_IPHONE = CocosUtil.Wp7Retina(40);
    }
}
