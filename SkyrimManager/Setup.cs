using System;
using System.IO;
using Gtk;
namespace SkyrimManager
{
	public partial class Setup : Gtk.Dialog
	{
		public Setup()
		{
			this.Build();
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			string SkyrimDir = SkyrimDirSelecter.Filename;
			if (File.Exists(SkyrimDir + "/SkyrimLauncher.exe") && File.Exists(SkyrimDir + "/TESV.exe"))
			{
				MainClass.WriteConf(SkyrimDir);
				this.Destroy();
				MainClass.Main(new string[] { "--nocheck" });
			}
		}

		protected void OnButtonCancelClicked(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}
	}
}
