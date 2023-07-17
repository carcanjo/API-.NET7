namespace SeuTempo.Core.MetodsExtensions
{
    public static class MetodsExtensions
    {
        public static string ToStingFormataData(this string valor)
            => valor.ToStingFormataData();

        public static string ToStingFormataCpf(this string valor)
            => Convert.ToInt64(valor).ToString(@"000\.000\.000\-00");
        public static string ToStingFormataCnpj(this string valor)
            => Convert.ToInt64(valor).ToString(@"00\.000\.000\/0000-00");
    }
}
