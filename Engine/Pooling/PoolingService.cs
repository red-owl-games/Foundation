namespace RedOwl.Engine
{
    public class PoolingService
    {
        public PoolingService()
        {
            //Log.Always("Init Pooling Service!");
        }
    }
    
    public partial class Game
    {
        public static PoolingService PoolingService => Find<PoolingService>();

        public static void BindPoolingService() => Bind(new PoolingService());
    }
}