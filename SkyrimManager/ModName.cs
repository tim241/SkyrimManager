using System;
namespace SkyrimManager
{
	public partial class ModName : Gtk.Dialog
	{
		public ModName()
		{
			this.Build();
			try
			{
				int index = ModSelect.fn.Split('/').Length;
				textview4.Buffer.Text = ModSelect.fn.Split('.')[0].Split('/')[index - 1];
			}
			catch (Exception e)
			{
				Console.WriteLine("Error: " + e.ToString());
			}
		}

		protected void OnButtonCancelClicked(object sender, EventArgs e)
		{
			this.Destroy();
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			this.Hide();
			string mName = textview4.Buffer.Text;
			Manager.InstallMod(ModSelect.fn, mName);
			this.Destroy();
		}
	}
}
