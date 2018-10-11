using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CenturyLink_Coding_Challenge
{
	public partial class Login : Form
	{
		public Login()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			G.Login = textBox1.Text;
			G.Password = textBox2.Text;
			if (G.Login != "" && G.Login != "Username" && G.Password != "" && bPasswordEntered)
			{
				this.Hide();
			}
			else
			{
				MessageBox.Show("Login is incorrect, try again.");
			}

		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			var txtSender = (TextBox)sender;
			var cursorPosition = txtSender.SelectionStart;
			txtSender.Text = Regex.Replace(txtSender.Text, "[^0-9a-zA-Z-]", "");
			txtSender.SelectionStart = cursorPosition;
		}

		bool bPasswordEntered = false;
		private void textBox2_TextChanged(object sender, EventArgs e)
		{
			bPasswordEntered = true;
		}
	}
}
