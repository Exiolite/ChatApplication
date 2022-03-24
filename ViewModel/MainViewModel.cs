using Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class MainViewModel : ViewModel
    {
        public RelayCommand CMDConnectToServer { get; set; }
        public RelayCommand CMDSendMessage { get; set; }


        private Client _client;


        #region PropUsername
        private string _username = string.Empty;

        public string PropUsername
        {
            get { return _username; }
            set { _username = value; NotifyPropertyChanged(nameof(PropUsername)); }
        }

        #endregion


        public MainViewModel()
        {
            _client = new Client();

            CMDConnectToServer = new RelayCommand(o => _client.ConnectToServer(PropUsername), o => !string.IsNullOrEmpty(PropUsername));
            CMDSendMessage = new RelayCommand(o => _client.SendMessage());
        }
    }
}
