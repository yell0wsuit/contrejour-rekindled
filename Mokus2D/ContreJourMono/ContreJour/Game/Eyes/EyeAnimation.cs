namespace Mokus2D.ContreJourMono.ContreJour.Game.Eyes
{
    public class EyeAnimation(string background, string eyeBall = null, bool lockY = false, bool lockX = false)
    {
        public string Background => background;

        public string EyeBall => eyeBall;

        public bool LockX => lockX;

        public bool LockY => lockY;

        public bool ReplaceBackground => background != null;

        public bool ReplaceEye => eyeBall != null;
    }
}
