namespace Model
{
    [Serializable]
    public class UserModel
    {
        public Guid Guid { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}
