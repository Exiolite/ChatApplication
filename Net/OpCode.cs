namespace Net
{
    public enum OpCode
    {
        ConnectToServer = 0,
        ConnectToClient = 1,
        Disconnect = 2,
        DisconnectFromServer = 3,
        DisconnectFromClient = 4,
        SendMessage = 5,
        SynchronizeData = 6
    }
}
