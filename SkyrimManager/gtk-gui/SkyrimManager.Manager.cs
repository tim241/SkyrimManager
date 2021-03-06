
// This file has been generated by the GUI designer. Do not modify.
namespace SkyrimManager
{
	public partial class Manager
	{
		private global::Gtk.Fixed @fixed;

		private global::Gtk.ComboBox GameOption;

		private global::Gtk.Button install;

		private global::Gtk.ProgressBar progressbar1;

		private global::Gtk.ScrolledWindow modlist;

		private global::Gtk.Fixed fixed1;

		private global::Gtk.Button ReadConfButton;

		private global::Gtk.Button launchButton;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget SkyrimManager.Manager
			this.Name = "SkyrimManager.Manager";
			this.Title = global::Mono.Unix.Catalog.GetString("SkyrimManager");
			this.WindowPosition = ((global::Gtk.WindowPosition)(1));
			this.Resizable = false;
			this.AllowShrink = true;
			// Container child SkyrimManager.Manager.Gtk.Container+ContainerChild
			this.@fixed = new global::Gtk.Fixed();
			this.@fixed.Name = "fixed";
			this.@fixed.HasWindow = false;
			// Container child fixed.Gtk.Fixed+FixedChild
			this.GameOption = global::Gtk.ComboBox.NewText();
			this.GameOption.AppendText(global::Mono.Unix.Catalog.GetString("Skyrim"));
			this.GameOption.Name = "GameOption";
			this.GameOption.Active = 0;
			this.@fixed.Add(this.GameOption);
			global::Gtk.Fixed.FixedChild w1 = ((global::Gtk.Fixed.FixedChild)(this.@fixed[this.GameOption]));
			w1.X = 3;
			w1.Y = 3;
			// Container child fixed.Gtk.Fixed+FixedChild
			this.install = new global::Gtk.Button();
			this.install.CanFocus = true;
			this.install.Name = "install";
			this.install.UseUnderline = true;
			this.install.Label = global::Mono.Unix.Catalog.GetString("Install mod");
			this.@fixed.Add(this.install);
			global::Gtk.Fixed.FixedChild w2 = ((global::Gtk.Fixed.FixedChild)(this.@fixed[this.install]));
			w2.X = 2;
			w2.Y = 37;
			// Container child fixed.Gtk.Fixed+FixedChild
			this.progressbar1 = new global::Gtk.ProgressBar();
			this.progressbar1.WidthRequest = 800;
			this.progressbar1.HeightRequest = 20;
			this.progressbar1.Name = "progressbar1";
			this.@fixed.Add(this.progressbar1);
			global::Gtk.Fixed.FixedChild w3 = ((global::Gtk.Fixed.FixedChild)(this.@fixed[this.progressbar1]));
			w3.X = 3;
			w3.Y = 528;
			// Container child fixed.Gtk.Fixed+FixedChild
			this.modlist = new global::Gtk.ScrolledWindow();
			this.modlist.WidthRequest = 500;
			this.modlist.HeightRequest = 440;
			this.modlist.CanFocus = true;
			this.modlist.Name = "modlist";
			this.modlist.ShadowType = ((global::Gtk.ShadowType)(1));
			this.modlist.BorderWidth = ((uint)(30));
			// Container child modlist.Gtk.Container+ContainerChild
			global::Gtk.Viewport w4 = new global::Gtk.Viewport();
			w4.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child GtkViewport.Gtk.Container+ContainerChild
			this.fixed1 = new global::Gtk.Fixed();
			this.fixed1.Name = "fixed1";
			this.fixed1.HasWindow = false;
			w4.Add(this.fixed1);
			this.modlist.Add(w4);
			this.@fixed.Add(this.modlist);
			global::Gtk.Fixed.FixedChild w7 = ((global::Gtk.Fixed.FixedChild)(this.@fixed[this.modlist]));
			w7.X = 7;
			w7.Y = 86;
			// Container child fixed.Gtk.Fixed+FixedChild
			this.ReadConfButton = new global::Gtk.Button();
			this.ReadConfButton.CanFocus = true;
			this.ReadConfButton.Name = "ReadConfButton";
			this.ReadConfButton.UseUnderline = true;
			this.ReadConfButton.Label = global::Mono.Unix.Catalog.GetString("Refresh");
			this.@fixed.Add(this.ReadConfButton);
			global::Gtk.Fixed.FixedChild w8 = ((global::Gtk.Fixed.FixedChild)(this.@fixed[this.ReadConfButton]));
			w8.X = 420;
			w8.Y = 82;
			// Container child fixed.Gtk.Fixed+FixedChild
			this.launchButton = new global::Gtk.Button();
			this.launchButton.CanFocus = true;
			this.launchButton.Name = "launchButton";
			this.launchButton.UseUnderline = true;
			this.launchButton.Label = global::Mono.Unix.Catalog.GetString("Launch");
			this.@fixed.Add(this.launchButton);
			global::Gtk.Fixed.FixedChild w9 = ((global::Gtk.Fixed.FixedChild)(this.@fixed[this.launchButton]));
			w9.X = 749;
			w9.Y = 3;
			this.Add(this.@fixed);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.DefaultWidth = 810;
			this.DefaultHeight = 553;
			this.Show();
			this.@fixed.Removed += new global::Gtk.RemovedHandler(this.OnFixedRemoved);
			this.GameOption.Changed += new global::System.EventHandler(this.OnGameOptionChanged);
			this.install.Clicked += new global::System.EventHandler(this.OnInstallClicked);
			this.ReadConfButton.Clicked += new global::System.EventHandler(this.OnReadConfButtonClicked);
			this.launchButton.Clicked += new global::System.EventHandler(this.OnLaunchButtonClicked);
		}
	}
}
