using Model;
using System;
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
        public HttpClient HttpClient { get; set; }
        public const string ServerUrl = "https://localhost:44396/api/";


        public RelayCommand CMDConnect { get; set; }
        public RelayCommand CMDSendMessage { get; set; }

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

        #region PropStartDateTimeToSort
        private DateTime _startDateTime = DateTime.Now;

        public DateTime PropStartDateTimeToSort
        {
            get { return _startDateTime; }
            set
            { 
                _startDateTime = value;
                NotifyPropertyChanged(nameof(PropStartDateTimeToSort));
            }
        }

        #endregion

        #region PropEndDateTimeToSort
        private DateTime _endDateTime = DateTime.Now;

        public DateTime PropEndDateTimeToSort
        {
            get { return _endDateTime; }
            set
            {
                _endDateTime = value;
                NotifyPropertyChanged(nameof(PropEndDateTimeToSort));
            }
        }

        #endregion

        #region PropSortInDateTimeRangeFlag
        private bool _sortInDateTimeRangeFlag;

        public bool PropSortInDateTimeRangeFlag
        {
            get { return _sortInDateTimeRangeFlag; }
            set 
            { 
                _sortInDateTimeRangeFlag = value;
                NotifyPropertyChanged(nameof(PropSortInDateTimeRangeFlag));
                TrySortMessagesInRange();
            }
        }

        #endregion


        public MainViewModel()
        {
            HttpClient = new HttpClient();

            CMDConnect = new RelayCommand(o => ReceiveMessageData(), b => !string.IsNullOrEmpty(PropUsername));
            CMDSendMessage = new RelayCommand(o => SendMessage(), b => !string.IsNullOrEmpty(PropMessage));
        }

        private async void ReceiveMessageData()
        {
            var jsonMessageList = await HttpClient.GetStringAsync($"{ServerUrl}message");
            var receivedMessageList = JsonSerializer.Deserialize<List<Message>>(jsonMessageList);
            PropMessageList = new ObservableCollection<Message>(receivedMessageList);
        }

        private async void SendMessage()
        {
            var message = new Message()
            {
                PropUsername = PropUsername,
                PropMessage = PropMessage,
                PropCreationDateTime = DateTime.Now
            };
            var jsonMessage = JsonSerializer.Serialize(message);

            await HttpClient.PostAsJsonAsync($"{ServerUrl}message", jsonMessage);

            ReceiveMessageData();

            PropMessage = string.Empty;
        }

        private void TrySortMessagesInRange()
        {
            if (PropSortInDateTimeRangeFlag)
            {
                var dateRangedMessageCollection = new ObservableCollection<Message>();
                foreach (var message in PropMessageList)
                {
                    if (message.PropCreationDateTime >= PropStartDateTimeToSort &&
                        message.PropCreationDateTime <= PropEndDateTimeToSort)
                    {
                        dateRangedMessageCollection.Add(message);
                    }
                }
                PropMessageList = dateRangedMessageCollection;
            }
            else
                ReceiveMessageData();
        }
    }
}
