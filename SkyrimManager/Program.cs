using Gtk;
using System;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace SkyrimManager
{
	class MainClass
	{
		/* TODO
		 * Make main mod manager
		 	* Make 'overlay' mod system
		 	* Make mod list and save it to a xml file
		 	* Make NMM mods work
		 		* xml 	scripts
		 		* cs 	scripts
		  	* Add SKSE install button 
		  	* Add launch buttons 	/
		  		* Skyrim 			/
		  		* SKSE 				/
		  	* Add wine log file
		 * Add more error handlers 
		 * Add a log file
		 */

		internal static string ConfigFile;
		internal static string ConfigDir = null;
		internal static string SkyrimDir;
		internal static string SkyrimLocalAppData;

		public static void Main(string[] args)
		{
			StartInit();
			Application.Init();
			CheckRequirements();
			if (File.Exists(ConfigFile))
			{
				CheckConf();
				new Manager().Show();
			}
			else
			{
				GetInfo();
				new Setup().Show();
			}
			Application.Run();
		}
		// Check things before starting the application
		internal static void StartInit()
		{
			if (Environment.GetEnvironmentVariable("HOME") != null)
			{
				ConfigFile 	= Environment.GetEnvironmentVariable("HOME") + "/.config/smm/config.xml";
				ConfigDir 	= Environment.GetEnvironmentVariable("HOME") + "/.config/smm";
			}
			else
				Error("Missing HOME variable");
		}
		// Write error to the console and exit with exitcode
		internal static void Error(string error, string errmsg = null, int exitcode = 1)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("Error: ");
			Console.ResetColor();
			if (string.IsNullOrEmpty(errmsg))
				errmsg = error;
			Console.Write(errmsg + Environment.NewLine);
			new MessageDialog(new Window(WindowType.Popup), DialogFlags.Modal, 
			                  MessageType.Error, ButtonsType.Close, error).Run();
			Environment.Exit(exitcode);
		}
		internal static void Rexec(string program)
		{
			Process check = new Process();
			check.StartInfo.FileName 					= "which";
			check.StartInfo.Arguments 					= program;
			check.StartInfo.UseShellExecute 			= false;
			check.StartInfo.RedirectStandardError 		= true;
			check.StartInfo.RedirectStandardOutput 		= true;
			check.Start();
			check.WaitForExit();
			if (check.ExitCode != 0)
				Error(program + " is not installed!");
			else
				Console.WriteLine(program + " is installed");
		}
		//
		internal static void CheckRequirements()
		{
			string[] requirements = { "wine", "winepath",
										"zip", "7z" };
			foreach(string program in requirements)
				Rexec(program);
		}
		//
		internal static void CheckConf()
		{
			if (!IsConfValid())
				Error("Config is invalid!");
		}
		//
		internal static void GetInfo()
		{
			string wineSkyrimLocalAppData = null;
			try {
				Process wine = new Process();
				wine.StartInfo.FileName = "wine";
				wine.StartInfo.Arguments = "cmd.exe /c 'echo %localappdata%'";
				wine.StartInfo.UseShellExecute 			= false;
				wine.StartInfo.RedirectStandardOutput 	= true;
				wine.StartInfo.RedirectStandardError 	= true;
				wine.Start();
				wine.WaitForExit();
				if (wine.ExitCode != 0)
					Error("wine command failed!");
				wineSkyrimLocalAppData = wine.StandardOutput.ReadLine().Replace(@"\", "/") + "/Skyrim";
			}
			catch (Exception){
				Error("wine command failed!");
			}
			try {
				Process GetInf = new Process();
				GetInf.StartInfo.FileName = "winepath";
				GetInf.StartInfo.Arguments = @"'" + wineSkyrimLocalAppData + @"'";
				GetInf.StartInfo.UseShellExecute 		= false;
				GetInf.StartInfo.RedirectStandardOutput = true;
				GetInf.Start();
				GetInf.WaitForExit();
				if (GetInf.ExitCode != 0)
					Error("winepath command failed!");
				SkyrimLocalAppData = GetInf.StandardOutput.ReadLine();
			}
			catch (Exception) {
				Error("winepath command failed!");
			}
			if (!Directory.Exists(SkyrimLocalAppData))
				Error("Directory doesn't exist, make sure Skyrim has been launched at least once!");
		}
		// Check if the config file is valid
		internal static bool IsConfValid()
		{
			try {
				XmlDocument xmlconfig = new XmlDocument();
				xmlconfig.Load(ConfigFile);
				SkyrimDir 			= xmlconfig.DocumentElement.GetAttribute("path");
				SkyrimLocalAppData  = xmlconfig.DocumentElement.GetAttribute("wineSLocalAppdata");
				if (!string.IsNullOrEmpty(SkyrimDir) && !string.IsNullOrEmpty(SkyrimLocalAppData))
					return true;
				else
					return false;
			}
			catch (Exception) {
				return false;
			}
		}
		// Write XML config file
		internal static void WriteConf(string skyrimDir)
		{
			try {
				Directory.CreateDirectory(Environment.GetEnvironmentVariable("HOME") + "/.config");
				Directory.CreateDirectory(Environment.GetEnvironmentVariable("HOME") + "/.config/smm");
				Directory.CreateDirectory(Environment.GetEnvironmentVariable("HOME") + "/.config/smm/mods");
			}
			catch (Exception) {
				Error("Failed to create directory!");
			}
			try {
				XmlWriterSettings xmlsettings 	= new XmlWriterSettings();
				xmlsettings.Indent 				= true;
				xmlsettings.NewLineOnAttributes = true;
				XmlWriter writer 				= XmlWriter.Create(ConfigFile, xmlsettings);
				writer.WriteStartDocument();
				writer.WriteStartElement("Skyrim");
				writer.WriteAttributeString("path", skyrimDir);
				writer.WriteAttributeString("wineSLocalAppdata", SkyrimLocalAppData);
				writer.WriteEndElement();
				writer.WriteEndDocument();
				writer.Flush(); writer.Close();
				Console.WriteLine("Config saved");
			}
			catch (Exception) {
				Error("Failed to create config!");
			}
		}
	}
}
