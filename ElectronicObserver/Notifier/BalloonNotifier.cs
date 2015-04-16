using System.Windows.Forms;

namespace ElectronicObserver.Notifier
{
    public class BalloonNotifier
    {
        private static readonly NotifyIcon notifyIcon = new NotifyIcon
            {
                Icon = Resource.ResourceManager.Instance.AppIcon,
                Visible = true
            };

        private static NotifyIcon NotifyIcon
        {
            get { return BalloonNotifier.notifyIcon; }
        } 


        public NotifierDialogData DialogData { get; set; }

        public BalloonNotifier(NotifierDialogData data)
        {
            DialogData = data.Clone();
        }
        
        public void Show()
        {
            if (BalloonNotifier.NotifyIcon == null)
                return;

            notifyIcon.ShowBalloonTip(1000, DialogData.Title, DialogData.Message, ToolTipIcon.None);
        }
    }
}
