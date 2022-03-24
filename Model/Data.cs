namespace Model
{
    [Serializable]
    public class Data
    {
        public List<UserModel> UserModelList { get; set; } = new List<UserModel>();
        public List<MessageModel> MessageModelList { get; set; } = new List<MessageModel>();
    }
}
