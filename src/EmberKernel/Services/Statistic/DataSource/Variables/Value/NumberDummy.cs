namespace EmberKernel.Services.Statistic.DataSource.Variables.Value
{
    public class NumberDummy
    {
        public static readonly NumberDummy Dummy = new NumberDummy();
#pragma warning disable IDE0060 // Remove unused parameter
        public static bool operator +(NumberDummy _, double __)
        {
            return true;
        }
        public static bool operator +(NumberDummy _, decimal __)
        {
            return true;
        }
#pragma warning restore IDE0060 // Remove unused parameter
    }
}
