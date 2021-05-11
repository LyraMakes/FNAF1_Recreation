namespace FNAF1_Recreation
{
    public static class Program
    {
        public static string Title = "Five\nNights\nAt\nFreddy's";
        public static string Version = "v a.001";
        public static string Copyright = "(c) 2014 Scott Cawthon";

        public static string Disclaimer =
@"             WARNING!

 This game contains flashing lights,
loud noises, and lots of jumpscares!";


        [System.STAThread]
        static void Main()
        {
            using Game1 game = new Game1();
            game.Run();
        }
    }
}
