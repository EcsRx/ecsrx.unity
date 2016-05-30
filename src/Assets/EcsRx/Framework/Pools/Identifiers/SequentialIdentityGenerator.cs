namespace EcsRx.Pools.Identifiers
{
    public class SequentialIdentityGenerator : IIdentifyGenerator
    {
        private int _lastIdentifier = 0;

        public int GenerateId()
        { return ++_lastIdentifier; }
    }
}