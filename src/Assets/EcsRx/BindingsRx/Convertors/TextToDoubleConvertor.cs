namespace BindingsRx.Convertors
{
    public class TextToDoubleConvertor : IConvertor<string, double>
    {
        public string From(double value)
        { return value.ToString(); }

        public double From(string value)
        {
            double output;
            double.TryParse(value, out output);
            return output;
        }
    }
}