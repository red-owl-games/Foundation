namespace RedOwl.Engine
{
    public class LocationMessage : MessageBase<LocationRef> {}
    
    public class LocationChannel : ChannelBase<LocationMessage, LocationRef> {}
}