namespace EcsRx.Pools.Identifiers
{
    public class SequentialIdentityGenerator : IIdentityGenerator
    {
        private int _lastIdentifier = 0;

        public int GenerateId()
        {
            if(_lastIdentifier >= int.MaxValue)
            {  _lastIdentifier = 0; }

            return ++_lastIdentifier;
        }
    }
}