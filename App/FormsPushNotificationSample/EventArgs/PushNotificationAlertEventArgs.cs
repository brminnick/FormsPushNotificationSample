using System;
namespace FormsPushNotificationSample
{
	public class PushNotificationAlertEventArgs : EventArgs
	{
		readonly string _titleText, _messageText, _cancelText, _acceptText;

		public PushNotificationAlertEventArgs(string titleText, string messageText, string cancelText, string acceptText)
		{
			_titleText = titleText;
			_messageText = messageText;
			_cancelText = cancelText;
			_acceptText = acceptText;
		}

		public PushNotificationAlertEventArgs(string titleText, string messageText, string cancelText)
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
