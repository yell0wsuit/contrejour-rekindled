namespace ContreJourMono.ContreJour.Game.Eyes
{
    public class EyeAnimation(string background, string eyeBall = null, bool lockY = false, bool lockX = false)
    {
        public string Background
        {
            get
            {
                return background;
            }
        }

        public string EyeBall
        {
            get
            {
                return eyeBall;
            }
        }

        public bool LockX
        {
            get
            {
                return lockX;
            }
        }

        public bool LockY
        {
            get
            {
                return lockY;
            }
        }

        public bool ReplaceBackground
        {
            get
            {
                return background != null;
            }
        }

        public bool ReplaceEye
        {
            get
            {
                return eyeBall != null;
            }
        }
    }
}
