using System;

using Microsoft.Xna.Framework;

using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace.Windows
{
    public class TrialAchievementWindow : PopUpWindow
    {
        public TrialAchievementWindow()
        {
            AddChild(new LayerColor(Color.Red));
            Mokus2DGame.KeysController.AddBackKeyListener(new Action(OnBack), -1);
        }

        private void OnBack()
        {
            Mokus2DGame.KeysController.StopPropagation();
            Mokus2DGame.KeysController.RemoveBackKeyListener(new Action(OnBack));
            Open = false;
        }
    }
}
