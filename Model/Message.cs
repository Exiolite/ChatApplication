namespace Model
{
    [Serializable]
    public class Message
    {
        public string PropUsername { get; set; } = string.Empty;
        public string PropCreationDateTime { get; set; } = string.Empty;
        public string PropMessage { get; set; } = string.Empty;
    }
}
