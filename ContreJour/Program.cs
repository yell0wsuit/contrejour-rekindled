using System;

namespace ContreJour
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using (Launch launch = new())
            {
                launch.Run();
            }
        }
    }
}
