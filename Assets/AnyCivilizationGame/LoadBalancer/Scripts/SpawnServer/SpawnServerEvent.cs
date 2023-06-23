public enum SpawnServerEvent : byte
{
    Ready = 0x01,
    ConnectToGameServer = 0x2,
    CloseRoom = 0x3,
    RoomInfoEvent = 0x4,
    DateMessageResponseEvent = 0x5,
    DateMessageRequestEvent = 0x6,
}