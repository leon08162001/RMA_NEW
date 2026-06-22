using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace RMA_Troubleshooting
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(MD5Encrypt("cipherbond"));
        }

        public string MD5Encrypt(string pToEncrypt)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArr = Encoding.Default.GetBytes(pToEncrypt);
            des.Key = Encoding.ASCII.GetBytes("8lvbe4kE");
            des.IV = Encoding.ASCII.GetBytes("8lvbe4kE");
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArr, 0, inputByteArr.Length);
            cs.FlushFinalBlock();
            StringBuilder sb = new StringBuilder();
            byte[] array = ms.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                byte b = array[i];
                sb.AppendFormat("{0:X2}", b);
            }
            sb.ToString();
            return sb.ToString();
        }
    }
}
