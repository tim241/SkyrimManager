using System;
namespace SkyrimManager
{
	public partial class ModSelect : Gtk.Dialog
	{
		internal static string mName 	= null;
		internal static string fn 		= null;
		public ModSelect()
		{
			this.Build();
		}

		protected void OnButtonCancelClicked(object sender, EventArgs e)
		{
			this.Destroy();
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			fn = ModChooser.Filename;
			if (fn.EndsWith(".zip") || fn.EndsWith(".7z"))
			{
				ModName mN = new ModName();
				mN.Show();
				this.Hide();
			}
				
		}
	}
}
