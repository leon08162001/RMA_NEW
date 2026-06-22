using System;
using System.Windows.Forms;

namespace RMA_Troubleshooting
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Insert_Repair_Price(args));
            Application.Run(new CipherMenu(args));
            //Application.Run(new Form1());
            //Application.Run(new BitConverter());

        }
    }
}
