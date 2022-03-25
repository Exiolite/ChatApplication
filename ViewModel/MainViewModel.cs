using Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ViewModel
{
    public class MainViewModel : ViewModel
    {
        public RelayCommand CMDRefresh { get; set; }
        public RelayCommand CMDSendMessage { get; set; }


        public HttpClient HttpClient { get; set; }
        public const string ServerUrl = "https://localhost:44396/api/";


        #region PropUsername
        private string _username = string.Empty;

        public string PropUsername
        {
            get { return _username; }
            set { _username = value; NotifyPropertyChanged(nameof(PropUsername)); }
        }
        #endregion

        #region PropMessageCollection
        private ObservableCollection<Message> _messageList = new ObservableCollection<Message>();

        public ObservableCollection<Message> PropMessageList
        {
            get { return _messageList; }
            set { _messageList = value; NotifyPropertyChanged(nameof(PropMessageList)); }
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
            HttpClient = new HttpClient();

            CMDRefresh = new RelayCommand(o => SetPropMessageCollection());
            CMDSendMessage = new RelayCommand(o => SendMessage(), o => !string.IsNullOrEmpty(PropMessage));
        }

        private async void SetPropMessageCollection()
        {
            var jsonMessageList = await HttpClient.GetStringAsync($"{ServerUrl}message");
            var receivedMessageList = JsonSerializer.Deserialize<List<Message>>(jsonMessageList);
            PropMessageList.Clear();
            foreach (var message in receivedMessageList)
            {
                PropMessageList.Add(message);
            }
        }

        private async void SendMessage()
        {
            var message = new Message()
            {
                PropUsername = PropUsername,
                PropMessage = PropMessage,
                PropCreationDateTime = System.DateTime.Now
            };
            var jsonMessage = JsonSerializer.Serialize(message);

            HttpContent content = new StringContent(jsonMessage);
            await HttpClient.PostAsJsonAsync($"{ServerUrl}message", jsonMessage);
            SetPropMessageCollection();
        }
    }
}
