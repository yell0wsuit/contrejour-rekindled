using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class FadeBackground : BackgroundBase
    {
        public FadeBackground(Node _node, Hashtable _config, ContreJourGame _game)
            : base(_node, _config, _game)
        {
            sprite = (Sprite)_node;
            if (!game.CanShowIntro)
            {
                sprite.Opacity = 0;
                sprite.Visible = false;
                return;
            }
            opacity = 1f;
        }

        public override void Update(float time)
        {
            base.Update(time);
            game.LightPower = 1f - sprite.OpacityFloat;
            if (sprite.Visible)
            {
                opacity -= time * 2f / 60f;
                sprite.OpacityFloat = opacity;
                if (sprite.OpacityFloat <= 0f)
                {
                    sprite.Visible = false;
                }
            }
        }

        protected float opacity;

        protected Sprite sprite;
    }
}
