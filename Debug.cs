namespace FNAF1_Recreation
{
    static class Debug
    {
        public static void WriteLine(string s) => System.Diagnostics.Debug.WriteLine(s);

        public static void WriteLine(object obj) => WriteLine($"{obj}");
    }
}
