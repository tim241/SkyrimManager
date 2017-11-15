using Gtk;
using Mono.Unix;
using System;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Collections.Generic;

namespace SkyrimManager
{
	public partial class Manager : Gtk.Window
	{
		internal static string ModDir = MainClass.ConfigDir + "/mods/";

		internal static void Error(string error, string errmsg = null, int exitcode = 1) { MainClass.Error(error, errmsg, exitcode); }

		internal static bool ExecFailed = false;

		public Manager() :
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			StartInit();
		}
		internal void StartInit()
		{
			ReadModConfig();
			if (File.Exists(MainClass.SkyrimDir + "/skse_loader.exe"))
				GameOption.AppendText("SKSE");
		}
		internal void ReadModConfig()
		{
			// Remove all existing checkbuttons
			foreach (Widget w in fixed1.Children)
				w.Destroy();
			// Re-read the config and add the checkbuttons
			string ModConfig = ModDir + "mods.xml";
			if (File.Exists(ModConfig))
			{
				XmlDocument XmlConfig = new XmlDocument();
				XmlConfig.Load(ModConfig);
				XmlElement xmlel = XmlConfig.DocumentElement;
				XmlNodeList xmlList = xmlel.SelectNodes("/SkyrimManager/mod");


				int cblocation = 5;
				foreach (XmlNode xmlNode in xmlList)
				{
					CheckButton cb = new CheckButton();
					cb.Label = xmlNode["id"].InnerText;
					cb.Name = xmlNode["id"].InnerText;
					if (xmlNode["enabled"].InnerText == "1")
						cb.Active = true;
					else
						cb.Active = false;
					fixed1.Put(cb, 5, cblocation);
					cblocation = cblocation + 20;
					cb.Show();
				}
			}
		}
		public static void InstallMod(string file, string name)
		{
			try {
				if (Directory.Exists(ModDir + name)) {
					new MessageDialog(new Window(WindowType.Popup), DialogFlags.Modal,
							  MessageType.Error, ButtonsType.Ok, "Mod has been installed already!").Run();
				}
				else {
					Directory.CreateDirectory(ModDir + name);
					if (file.EndsWith(".zip"))
						Exec("unzip", @"'" + file + @"' -d '" + ModDir + name + @"'", true, true);
					else if (file.EndsWith(".7z"))
						Exec("7z", @"x -o'" + ModDir + name + @"' '" + file + @"' -y", true, true);
					if (!ExecFailed)
					{
						AddModToList(name, ModDir + name);
						SymLinkMod(ModDir + name);
					}
					else
						Directory.Delete(ModDir + name);
				}
			}
			catch (Exception e) {
				Error("Failed to install mod", e.ToString());
			}
		}
		internal static void SymLinkMod(string modDir)
		{
			// Fix this
			string SourcePath = modDir;
			string DestinationPath = MainClass.SkyrimDir + "/Data/";
			foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
							SearchOption.AllDirectories))
				Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));
			foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
							SearchOption.AllDirectories))
				File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
		}
		internal static void Exec(string command, string parameters, bool WaitForExit = true, bool ErrorOnPerror = false)
		{
			Process cmd = new Process();
			cmd.StartInfo.FileName 					= command;
			cmd.StartInfo.Arguments 				= parameters;
			cmd.StartInfo.UseShellExecute 			= false;
			cmd.StartInfo.RedirectStandardOutput	= true;
			cmd.StartInfo.RedirectStandardError 	= true;
			cmd.Start();
			if (WaitForExit)
				cmd.WaitForExit();
			if (ErrorOnPerror)
				if (cmd.ExitCode != 0)
				{
					ExecFailed = true;
					Console.WriteLine(cmd.StandardOutput.ReadToEnd() + Environment.NewLine
				                      + "Error: " + cmd.StandardError.ReadToEnd() );
					new MessageDialog(new Window(WindowType.Popup), DialogFlags.Modal,
									  MessageType.Error, ButtonsType.Ok, "Mod failed to install!").Show();
				}
		}
		internal static void AddModToList(string name, string path)
		{
			try
			{
				string ModConfig = ModDir + "mods.xml";
				if (!File.Exists(ModConfig))
				{
					// Make new xml and add the mod name + path
					XmlWriterSettings modListsettings = new XmlWriterSettings();
					modListsettings.Indent = true;
					modListsettings.NewLineOnAttributes = true;
					XmlWriter writer = XmlWriter.Create(ModConfig, modListsettings);
					writer.WriteStartDocument();
					writer.WriteStartElement("SkyrimManager");
					writer.WriteStartElement("mod");
					writer.WriteStartElement("enabled"); writer.WriteString("1"); writer.WriteEndElement();
					writer.WriteStartElement("id"); writer.WriteString(name); writer.WriteEndElement();
					writer.WriteStartElement("path"); writer.WriteString(path); writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteEndDocument();
					writer.Flush(); writer.Close();
				}
				else
				{
					// TODO Make this shorter 

					XmlDocument XmlConfig = new XmlDocument();
					XmlConfig.Load(ModConfig);

					XmlWriterSettings modListsettings = new XmlWriterSettings();
					modListsettings.Indent = true;
					modListsettings.NewLineOnAttributes = true;
					XmlWriter writer = XmlWriter.Create(ModConfig, modListsettings);
					// Load the xml items and add the new mod name + path to the list
					// then write the items to the xml
					XmlElement xmlel 	= XmlConfig.DocumentElement;
					XmlNodeList xmlList = xmlel.SelectNodes("/SkyrimManager/mod");

					writer.WriteStartDocument();
					writer.WriteStartElement("SkyrimManager");

					foreach (XmlNode xmlNode in xmlList)
					{
						writer.WriteStartElement("mod");
						writer.WriteStartElement("enabled"); writer.WriteString(xmlNode["enabled"].InnerText); writer.WriteEndElement();
						writer.WriteStartElement("id"); writer.WriteString(xmlNode["id"].InnerText); writer.WriteEndElement();
						writer.WriteStartElement("path"); writer.WriteString(xmlNode["path"].InnerText); writer.WriteEndElement();
						writer.WriteEndElement();
					}
					writer.WriteStartElement("mod");
					writer.WriteStartElement("enabled"); writer.WriteString("1"); writer.WriteEndElement();
					writer.WriteStartElement("id"); writer.WriteString(name); writer.WriteEndElement();
					writer.WriteStartElement("path"); writer.WriteString(path); writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteEndDocument();
					// Write config
					writer.Flush(); writer.Close();
				}
			}
			catch (Exception e)
			{
				Error("Writing mod config failed!", e.ToString());
			}
		}
		protected void OnDeleteEvent(object o, Gtk.DeleteEventArgs args)
		{
			Environment.Exit(0);
		}

		protected void OnGameOptionChanged(object sender, EventArgs e)
		{
			// Change profile here
		}

		protected void OnLaunchButtonClicked(object sender, EventArgs e)
		{
			string game = null;
			if (GameOption.ActiveText == "Skyrim")
				game = "SkyrimLauncher.exe";
			else if (GameOption.ActiveText == "SKSE")
				game = "skse_loader.exe";
			Process skyrim 							= new Process();
			skyrim.StartInfo.FileName 				= "wine";
			skyrim.StartInfo.UseShellExecute 		= true;
			//skyrim.StartInfo.RedirectStandardError 	= true;
			//skyrim.StartInfo.RedirectStandardOutput = true;
			skyrim.StartInfo.Arguments 				= @"'" + MainClass.SkyrimDir + "/" + game + @"'";
			skyrim.Start();
		}

		protected void OnInstallClicked(object sender, EventArgs e)
		{
			ModSelect MS = new ModSelect();
			MS.Show();
		}

		protected void OnReadConfButtonClicked(object sender, EventArgs e)
		{
			// Re-read the config
			ReadModConfig();
		}

		protected void OnFixedRemoved(object o, RemovedArgs args)
		{
			Environment.Exit(0);
		}
	}
}
