namespace Persistity.Processors
{
    public interface IProcessor
    {
        byte[] Process(byte[] data);
    }
}