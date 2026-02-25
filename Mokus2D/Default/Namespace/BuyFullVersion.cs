using System;

using Default.Namespace.Windows;

using Microsoft.Xna.Framework;

using Mokus2D.Effects.Actions;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class BuyFullVersion : PopUpWindow, IDisposable
    {
        public BuyFullVersion()
        {
            Sprite sprite = new("McBuyFullVersionBackground")
            {
                Position = new Vector2(0f, ScreenConstants.W7FromIPhoneSize.Y),
                Scale = 1f / ScreenConstants.Scales.fromIPhone2ByHeight
            };
            container.AddChild(sprite);
            buyButton = new TouchSprite("McBuyFullVersionButton");
            _ = new ButtonSound(buyButton);
            clickableLayer.AddChild(buyButton);
            buyButton.Position = new Vector2(592f, 263f) / ScreenConstants.Scales.fromIPhone2ByHeight;
            buyButton.Scale = sprite.Scale;
            buyButtonEffect = new ButtonSprite(buyButton)
            {
                TargetScale = 1.04f * buyButton.Scale,
                EffectTime = 0.05f
            };
            buyButton.ClickEvent.AddListenerSelector(new Action(OnBuyClick));
            buyButton.TouchBeganEvent.AddListenerSelector(new Action(OnBuyTouchBegan));
            buyButton.TouchEndEvent.AddListenerSelector(new Action(OnBuyTouchEnd));
            buyButton.TouchOutEvent.AddListenerSelector(new Action(OnBuyTouchEnd));
            if (ContreJourLabel.IsEnglish)
            {
                textSprite = new Sprite("McBuyFullVersionText");
            }
            else
            {
                textSprite = ContreJourLabel.CreateLabel(26f, "BUY_FULL_VERSION", true);
                textSprite.Scale = 0.66f;
            }
            textSprite.Position = new Vector2(1f, -6f);
            buyButton.AddChild(textSprite);
            NodeAction nodeAction = Actions.CreateNeonEffect(0.5f, 1f);
            textSprite.Run(nodeAction);
            buyButton.ClickEvent.AddListenerSelector(new Action(MarketUtils.NavigateToMarket));
            MultilineLabel multilineLabel = ContreJourLabel.CreateMultilineLabel(18f, "BUY_FULL_VERSION_TO_UNLOCK");
            container.AddChild(multilineLabel);
            multilineLabel.Anchor = new Vector2(0f, 1f);
            multilineLabel.LineSpacing += 1f;
            multilineLabel.Position = new Vector2(5f, 5f);
            multilineLabel.Color = ContreJourConstants.GREY_COLOR;
            LastParticles lastParticles = new()
            {
                ParticlesScale = new Range(1.9f, 1f)
            };
            container.AddChild(lastParticles, 11);
            lastParticles.CreateBetweenBounds(20);
            lastParticles.Angle = new Range(-80f, 30f);
        }

        private void OnBuyTouchBegan()
        {
            textSprite.StopAllActions();
            textSprite.Run(new FadeIn(0.2f));
        }

        private void OnBuyTouchEnd()
        {
            NodeAction nodeAction = Actions.CreateNeonEffect(0.5f, 1f);
            textSprite.Run(nodeAction);
        }

        private void OnBuyClick()
        {
            MarketUtils.NavigateToMarket();
        }

        public new void Dispose()
        {
        }

        private static readonly float VERTICAL_POSITION = CocosUtil.iPad(140, 120);

        private static readonly Vector2 UNIVERSAL_POSITION = CocosUtil.ccpIPad(410f, VERTICAL_POSITION);

        private static readonly Vector2 IPHONE_POSITION = CocosUtil.ccpIPad(310f, VERTICAL_POSITION);

        private ButtonSprite buyButtonEffect;

        private TouchSprite buyButton;

        private Node textSprite;
    }
}
