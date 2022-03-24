namespace Model
{
    [Serializable]
    public class MessageModel
    {
        public Guid PropUserGuid { get; set; }
        public DateTime PropCreationDateTime { get; set; }
        public string PropMessage { get; set; } = string.Empty;
    }
}
