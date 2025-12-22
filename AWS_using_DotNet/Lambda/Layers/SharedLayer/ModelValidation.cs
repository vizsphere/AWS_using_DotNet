namespace SharedLayer
{
    public class ModelValidation
    {
        public static bool IsNameValid(string name)
        {
            return name.Length < 2 ? false : true;
        }
    }
}
