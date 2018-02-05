namespace BindingsRx.Convertors
{
    public class TextToFloatConvertor : IConvertor<string, float>
    {
        public string From(float value)
        { return value.ToString(); }

        public float From(string value)
        {
            float output;
            float.TryParse(value, out output);
            return output;
        }
    }
}