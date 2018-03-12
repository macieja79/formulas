using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IWshRuntimeLibrary;

namespace Formulas
{
	public class FileTools
	{

		public static string GetPathFromLnkFile(string lnkFilePath)
		{
			WshShell shell = new WshShell(); 
            WshShortcut shortcut = (WshShortcut)shell.CreateShortcut(lnkFilePath);
			return shortcut.TargetPath;
		}
	
	}
}
