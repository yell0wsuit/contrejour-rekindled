using System;
using System.Collections.Generic;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

using Microsoft.Xna.Framework;

using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class TeleportBodyClip : BodyClip
    {
        public TeleportBodyClip Sibling
        {
            get => sibling; set => sibling = value;
        }

        public EventSender UseEvent => useEvent;

        public TeleportBodyClip(LevelBuilderBase _builder, object _body, Node _clip, Hashtable _config)
            : base(_builder, _body, null, _config)
        {
            useEvent = new EventSender();
            ContreJourGame contreJourGame = (ContreJourGame)_builder.Game;
            limitSpeed = config.GetBool("limitSpeed");
            string text = config.GetString("color");
            if (text == null)
            {
                text = "0";
            }
            string text2;
            if (text != "0")
            {
                text2 = "McTeleportPartBlue.png";
            }
            else
            {
                text2 = contreJourGame.BlackSide ? "McTeleportPartBlue.png" : "McTeleportPart.png";
            }
            portal = new Portal(contreJourGame, _clip.Position, text2);
            if (contreJourGame.BonusChapter)
            {
                portal.Color = ContreJourConstants.GreenLightColor;
            }
            portalPosition = _clip.Position;
            _builder.RemoveChild(_clip);
            _builder.Add(portal, -2);
            portal.TargetScale = 1f;
            portal.SpeedValue = Maths.RandRangeMinMax(CocosUtil.iPadValue(20f), CocosUtil.iPadValue(35f));
            portal.ScaleStep = 0.2f;
            TeleportPortal teleportPortal = new(portal);
            _builder.Add(teleportPortal, -2);
            sibling = contreJourGame.GetTeleport(text);
            if (sibling != null)
            {
                sibling.Sibling = this;
            }
            else
            {
                contreJourGame.RegisterTeleportColor(this, text);
            }
            teleportables = new List<BodyClip>();
        }

        public void Use()
        {
            useEvent.SendEvent();
            portal.TargetScale = 0.2f;
            _ = builder.Game.Updater.CallAfterSelectorDelay(new Action(RestoreScale), 0.1f);
            _ = builder.Game.Updater.CallAfterSelectorDelay(new Action(SetMaxScale), 0.033333335f);
        }

        private void SetMaxScale()
        {
            portal.TargetScale = 1.2f;
        }

        public void UpdateTeleportTime()
        {
            teleportTime = builder.Game.TotalTime;
        }

        public override void OnCollisionStartPoint(Body body2, Contact point)
        {
            if (point.FixtureA.IsSensor && point.FixtureB.IsSensor)
            {
                return;
            }
            BodyClip bodyClip = (BodyClip)body2.UserData;
            if (teleportables.Contains(bodyClip))
            {
                return;
            }
            ITeleportable teleportable = bodyClip as ITeleportable;
            if (teleportable != null && teleportable.CanTeleport())
            {
                teleportables.Add(bodyClip);
                teleportable.Teleport(this);
                Mokus2DGame.SoundManager.PlaySound("teleport", 0.5f, 0f, 0f);
                teleportable.SnotEnabled = false;
                float num = body2.LinearVelocity.Length() / builder.EngineConfig.SizeMultiplier;
                float num2 = (num > 200f) ? (20f / num) : 0.1f;
                num2 = Maths.max(num2, 0.01f);
                teleportable.SetScaleTime(0f, num2);
                portal.TargetScale = 0.2f;
                _ = Schedule(delegate
                {
                    MoveHero(bodyClip);
                }, num2);
                teleporting = true;
                useEvent.SendEvent();
            }
        }

        public override void OnCollisionEndPoint(Body body2, Contact point)
        {
            BodyClip bodyClip = (BodyClip)body2.UserData;
            if (teleportables.Contains(bodyClip))
            {
                _ = teleportables.Remove(bodyClip);
            }
        }

        private void TeleportFromSibling(BodyClip teleportable)
        {
            teleportables.Add(teleportable);
        }

        private void MoveHero(BodyClip bodyClip)
        {
            ITeleportable teleportable = bodyClip as ITeleportable;
            sibling.TeleportFromSibling(bodyClip);
            bodyClip.Clip.Scale = 0f;
            bodyClip.Clip.StopAllActions();
            bodyClip.Body.SetTransform(sibling.Body.Position, bodyClip.Body.Rotation);
            teleportable.AfterTeleport();
            teleportable.SnotEnabled = true;
            portal.TargetScale = 1.2f;
            _ = builder.Game.Updater.CallAfterSelectorDelay(new Action(RestoreScale), 0.1f);
            teleportable.ForceClipPosition();
            ScaleHero(bodyClip);
            UpdateTeleportTime();
            sibling.UpdateTeleportTime();
            sibling.Use();
            if (limitSpeed)
            {
                Vector2 vector = FarseerUtil.clampLength(bodyClip.Body.LinearVelocity, 23.333334f);
                bodyClip.Body.LinearVelocity = vector;
            }
            teleporting = false;
        }

        public void ScaleHero(BodyClip bodyClip)
        {
            ITeleportable teleportable = bodyClip as ITeleportable;
            float num = Maths.min(0.1f, 0.1f / bodyClip.Body.LinearVelocity.Length() * 10f);
            teleportable.SetScaleTime(1f, num);
        }

        private void RestoreScale()
        {
            portal.TargetScale = 1f;
        }

        private const float MAX_TELEPORT_SPEED = 23.333334f;

        private const float MAX_SCALE = 1.2f;

        private const float MIN_SCALE = 0.2f;

        private const float TELEPORT_TIME = 0.1f;

        protected Portal portal;

        protected TeleportBodyClip sibling;

        protected float teleportTime;

        protected Vector2 portalPosition;

        protected bool teleporting;

        protected bool limitSpeed;

        protected List<BodyClip> teleportables;

        protected EventSender useEvent;
    }
}
