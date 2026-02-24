using System;

using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

namespace Default.Namespace
{
    public class ContactListener
    {
        public ContactListener(World world)
        {
            ContactManager contactManager = world.ContactManager;
            contactManager.BeginContact = (BeginContactDelegate)Delegate.Combine(contactManager.BeginContact, new BeginContactDelegate(BeginContact));
            ContactManager contactManager2 = world.ContactManager;
            contactManager2.EndContact = (EndContactDelegate)Delegate.Combine(contactManager2.EndContact, new EndContactDelegate(EndContact));
            ContactManager contactManager3 = world.ContactManager;
            contactManager3.PreSolve = (PreSolveDelegate)Delegate.Combine(contactManager3.PreSolve, new PreSolveDelegate(PreSolve));
            ContactManager contactManager4 = world.ContactManager;
            contactManager4.PostSolve = (PostSolveDelegate)Delegate.Combine(contactManager4.PostSolve, new PostSolveDelegate(PostSolve));
        }

        public bool BeginContact(Contact contact)
        {
            ProcessContact(contact, delegate (BodyClip bodyClip, Body body)
            {
                bodyClip.OnCollisionStartPoint(body, contact);
            });
            return true;
        }

        private void ProcessContact(Contact contact, Action<BodyClip, Body> action)
        {
            BodyClip bodyClip = (BodyClip)contact.FixtureA.Body.UserData;
            BodyClip bodyClip2 = (BodyClip)contact.FixtureB.Body.UserData;
            Body body = contact.FixtureA.Body;
            Body body2 = contact.FixtureB.Body;
            if (bodyClip != null)
            {
                action.Invoke(bodyClip, body2);
            }
            if (bodyClip2 != null)
            {
                action.Invoke(bodyClip2, body);
            }
        }

        public void EndContact(Contact contact)
        {
            ProcessContact(contact, delegate (BodyClip bodyClip, Body body)
            {
                bodyClip.OnCollisionEndPoint(body, contact);
            });
        }

        public void PreSolve(Contact contact, ref Manifold manifold)
        {
            if (contact.IsTouching)
            {
                ProcessContact(contact, delegate (BodyClip bodyClip, Body body)
                {
                    bodyClip.OnCollisionPoint(body, contact);
                });
            }
        }

        public void PostSolve(Contact contact, ContactVelocityConstraint impulse)
        {
            if (contact.IsTouching)
            {
                BodyClip bodyClip = (BodyClip)contact.FixtureA.Body.UserData;
                BodyClip bodyClip2 = (BodyClip)contact.FixtureB.Body.UserData;
                if (bodyClip != null)
                {
                    bodyClip.PostSolvePointImpulse(contact.FixtureB.Body, contact, impulse);
                }
                if (bodyClip2 != null)
                {
                    bodyClip2.PostSolvePointImpulse(contact.FixtureA.Body, contact, impulse);
                }
            }
        }
    }
}
