using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace RMA_Troubleshooting
{
    public partial class BitConverter : Form
    {
        public BitConverter()
        {
            InitializeComponent();
            mac();
        }

        public void mac()
        {
            PhysicalAddress newAddress = PhysicalAddress.Parse("00-11-22-33-44-55");
            /*if (PhysicalAddress.None.Equals(newAddress))
                return null;*/

            //return newAddress;
            //MessageBox.Show(newAddress.ToString());
            /*byte[] b = System.BitConverter.GetBytes(001122334455);
            MessageBox.Show(b.ToString());*/
            //十進位制轉二進位制  
            MessageBox.Show("十進位制166的二進位制表示: " + Convert.ToString(166, 2));
            //十進位制轉八進位制  
            MessageBox.Show("十進位制166的八進位制表示: " + Convert.ToString(166, 8));
            //十進位制轉十六進位制  
            MessageBox.Show("十進位制166的十六進位制表示: " + Convert.ToString(166, 16));

            //二進位制轉十進位制  
            MessageBox.Show("二進位制 111101 的十進位制表示: " + Convert.ToInt32("111101", 2));
            //八進位制轉十進位制  
            MessageBox.Show("八進位制 44 的十進位制表示: " + Convert.ToInt32("44", 8));
            //十六進位制轉十進位制  
            MessageBox.Show("十六進位制 CC的十進位制表示: " + Convert.ToInt32("CC", 16));

        }
    }
}
