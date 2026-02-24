using System;
using System.Collections.Generic;
using System.Linq;

using Default.Namespace;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Mokus2D.Util.MathUtils;
using Mokus2D.Visual.Data;

namespace Mokus2D.Input
{
    public class KeysController : IUpdatable
    {
        public void StopPropagation()
        {
            stoped = true;
        }

        public void Update(float time)
        {
            inUpdate = true;
            ButtonState back = GamePad.GetState(PlayerIndex.One).Buttons.Back;
            bool backPressed = back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape);
            if (backPressed && !isBackPressed)
            {
                foreach (ActionPriority actionPriority in backKeysListeners)
                {
                    actionPriority.Action.Invoke();
                    if (stoped)
                    {
                        break;
                    }
                }
            }
            isBackPressed = backPressed;
            stoped = false;
            inUpdate = false;
            for (int i = 0; i < toRemove.Count; i++)
            {
                _ = backKeysListeners.Remove(toRemove[i]);
            }
            toRemove.Clear();
        }

        public bool ContainsListener(Action action)
        {
            ActionPriority actionPriority = FindItem(action);
            return backKeysListeners.Contains(actionPriority) && !toRemove.Contains(actionPriority);
        }

        private ActionPriority FindItem(Action action)
        {
            return Enumerable.FirstOrDefault(backKeysListeners, item => item.Action == action);
        }

        public void RemoveBackKeyListener(Action action)
        {
            if (inUpdate)
            {
                toRemove.Add(FindItem(action));
                return;
            }
            _ = backKeysListeners.Remove(FindItem(action));
        }

        public void AddBackKeyListener(Action action, int priority = 0)
        {
            backKeysListeners.Add(new ActionPriority(action, 0));
        }

        private static readonly Comparison<ActionPriority> comparison = (first, second) => Comparisons.IntComparizon(first.Priority, second.Priority);

        private readonly SortedList<ActionPriority> backKeysListeners = new(64, comparison);

        private readonly List<ActionPriority> toRemove = new(64);

        private bool isBackPressed;

        private bool inUpdate;

        private bool stoped;
    }
}
