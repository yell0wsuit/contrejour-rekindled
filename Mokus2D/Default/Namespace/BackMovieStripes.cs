namespace Default.Namespace
{
    public class BackMovieStripes : MovieStripesView
    {
        public EventSender BackEvent => backEvent;

        public BackMovieStripes()
            : base(false, false)
        {
            backEvent = new EventSender();
            CocosUtil.iPad(36, 50);
            clickableLayer = new ClickableLayer(0);
            AddChild(clickableLayer, 5);
            ShowBack();
        }

        public void ShowBack()
        {
        }

        private void OnBackClick()
        {
            backEvent.SendEvent();
            backEvent.Enabled = false;
        }

        private const float FADE_TIME = 2f;

        protected EventSender backEvent;

        protected ClickableLayer clickableLayer;
    }
}
