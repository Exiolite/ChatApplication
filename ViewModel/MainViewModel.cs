using Model;
using Net;

namespace ViewModel
{
    public class MainViewModel : ViewModel
    {
        public RelayCommand CMDConnectToServer { get; set; }
        public RelayCommand CMDSendMessage { get; set; }


        private Client _client;


        #region PropMessageModelCollection
        private Data _data = new Data();

        public Data PropData
        {
            get { return _data; }
            set { _data = value; NotifyPropertyChanged(nameof(PropData)); }
        }

        #endregion

        #region PropUsername
        private string _username = string.Empty;

        public string PropUsername
        {
            get { return _username; }
            set { _username = value; NotifyPropertyChanged(nameof(PropUsername)); }
        }

        #endregion

        #region PropMessage
        private string _message = string.Empty;

        public string PropMessage
        {
            get { return _message; }
            set { _message = value; NotifyPropertyChanged(nameof(PropMessage)); }
        }

        #endregion


        public MainViewModel()
        {
            _client = new Client();

            CMDConnectToServer = new RelayCommand(o => _client.Send(OpCode.ConnectToServer, PropUsername), o => !string.IsNullOrEmpty(PropUsername));
            CMDSendMessage = new RelayCommand(o => _client.Send(OpCode.SendMessage, PropMessage), o => !string.IsNullOrEmpty(PropMessage));
        }
    }
}
