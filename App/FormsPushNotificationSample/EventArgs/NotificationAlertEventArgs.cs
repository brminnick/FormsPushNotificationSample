using System;
namespace FormsPushNotificationSample
{
	public class NotificationAlertEventArgs : EventArgs
	{
		readonly string _titleText, _messageText, _cancelText, _acceptText;

		public NotificationAlertEventArgs(string titleText, string messageText, string cancelText, string acceptText)
		{
			_titleText = titleText;
			_messageText = messageText;
			_cancelText = cancelText;
			_acceptText = acceptText;
		}

		public NotificationAlertEventArgs(string titleText, string messageText, string cancelText)
		{
			_titleText = titleText;
			_messageText = messageText;
			_cancelText = cancelText;
		}

		public string MessageText => _messageText;

		public string CancelText => _cancelText;

		public string AcceptText => _acceptText;

		public string TitleText => _titleText;
	}
}
