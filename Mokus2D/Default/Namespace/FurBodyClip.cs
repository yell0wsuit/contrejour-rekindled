using System;
using System.Collections.Generic;

using FarseerPhysics.Dynamics.Contacts;

using Microsoft.Xna.Framework;

using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public abstract class FurBodyClip : ContreJourBodyClip
    {
        public FurBodyClip(LevelBuilderBase _builder, object _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            if (_clip == null)
            {
                _clip = new Node
                {
                    Scale = _config.GetVector("scale").X
                };
                _ = _builder.AddChild(_clip);
                clip = _clip;
            }
            grassStep = 6.2831855f / GrassCount();
            trampleAngle = 4f * grassStep;
            baseSprite = ClipFactory.CreateWithAnchor("McRotatorBase");
            baseSprite.Scale = Width() / baseSprite.Size.X;
            clip.AddChild(baseSprite);
            grassSystem = CreateFur();
            CreateGrass();
        }

        public FurCircle CreateFur()
        {
            FurCircle furCircle = new(GrassTexture(), GrassCount(), GrassRadius());
            clip.AddChild(furCircle);
            return furCircle;
        }

        public abstract string GrassTexture();

        public abstract int GrassCount();

        public abstract int GrassRadius();

        public abstract float Width();

        public virtual void SetBaseWidth(float newWidth)
        {
            baseSprite.Scale = newWidth / baseSprite.Size.X;
        }

        public override void Update(float time)
        {
            base.Update(time);
            float num = -Maths.Clamp(Body.AngularVelocity * 10f, -65f, 65f);
            UpdateContactAngles();
            foreach (RotatorGrass rotatorGrass in grass)
            {
                rotatorGrass.UpdateAngle(time, num);
            }
        }

        public void UpdateContactAngles()
        {
            foreach (RotatorGrass rotatorGrass in grass)
            {
                rotatorGrass.ContactAngle = 0f;
            }
            for (ContactEdge contactEdge = Body.ContactList; contactEdge != null; contactEdge = contactEdge.Next)
            {
                if (contactEdge.Contact.IsTouching && contactEdge.Other.UserData != null && !FarseerUtil.IsSensor(contactEdge.Contact))
                {
                    Vector2 worldPoint = FarseerUtil.GetWorldPoint(contactEdge.Contact);
                    float num = Maths.atan2Vec(Body.GetLocalPoint(worldPoint));
                    num = Maths.SimplifyAngleRadiansStartValue(num, 0f);
                    AddContactAngle(num);
                }
            }
        }

        public void AddContactAngleIndex(float angle, int index)
        {
            index = (int)Maths.fmodP(index, GrassCount());
            RotatorGrass rotatorGrass = grass[index];
            angle = Maths.SimplifyAngleRadiansStartValue(angle, rotatorGrass.InitialAngle - 3.1415927f);
            int num = Math.Sign(rotatorGrass.InitialAngle - angle);
            float num2 = trampleAngle - Maths.Abs(angle - rotatorGrass.InitialAngle);
            rotatorGrass.ContactAngle = num2 * 1.3f * num;
        }

        public void AddContactAngle(float angle)
        {
            for (int i = 0; i < 3; i++)
            {
                int num = (int)((angle - (i * grassStep)) / grassStep);
                AddContactAngleIndex(angle, num);
                int num2 = (int)((angle + ((i + 1) * grassStep)) / grassStep);
                AddContactAngleIndex(angle, num2);
            }
        }

        public void CreateGrass()
        {
            for (int i = 0; i < GrassCount(); i++)
            {
                Particle particle = grassSystem.Particles[i];
                RotatorGrass rotatorGrass = new(particle)
                {
                    InitialAngle = grassSystem.GetItemAngle(i)
                };
                grass.Add(rotatorGrass);
            }
        }

        protected List<RotatorGrass> grass = new();

        protected FurCircle grassSystem;

        protected float grassStep;

        protected float trampleAngle;

        protected Sprite baseSprite;
    }
}
