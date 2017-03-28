namespace Persistity.Transformers
{
    public interface ITransformer
    {
        object TransformTo(object original);
        object TransformFrom(object converted);
    }
}