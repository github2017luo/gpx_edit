using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace gpx_edit
{
	class Program
	{
		// brief: delete duplicate <trk>-entries from a gpx-file
		// brief: only the last occurance of the entry is important

		static void Main(string[] args)
		{
			if (args.Count() == 0)
			{
				System.Console.WriteLine("usage: gpx_edit.exe <gpx-file>");
				return;
			}

			String strXmlPathName = args[0];
			if (Path.GetExtension(strXmlPathName) != ".gpx")
			{
				System.Console.WriteLine("invalid gpx-file!");
				return;
			}

			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(strXmlPathName);

			XmlNodeList nodes = xmlDoc.SelectNodes("//*[local-name()='gpx']/*[local-name()='trk']");

			List<string> listNodeNames = new List<string>();
			List<string> listNodeNamesToDelete = new List<string>();
			foreach (XmlNode node in nodes)
			{
				if (listNodeNames.Contains(node.FirstChild.InnerText))
				{
					listNodeNamesToDelete.Add(node.FirstChild.InnerText);
				}
				else
				{
					listNodeNames.Add(node.FirstChild.InnerText);
				}
			}

			// delete first occurance(s) in node-list
			foreach (XmlNode node in nodes)
			{
				if (listNodeNamesToDelete.Contains(node.FirstChild.InnerText))
				{
					System.Console.WriteLine("Deleting " + node.FirstChild.InnerText + "...");

					node.ParentNode.RemoveChild(node);
					listNodeNamesToDelete.Remove(node.FirstChild.InnerText);
				}
			}
			System.Console.WriteLine("Done!");

			String strXmlPathNameOUT = "";

			string[] strXmlPathParts = strXmlPathName.Split('\\');
			for (int i=0; i<strXmlPathParts.Count() - 1; i++)
			{
				strXmlPathNameOUT += strXmlPathParts[i];
				strXmlPathNameOUT += @"\";
			}
			strXmlPathNameOUT += Path.GetFileNameWithoutExtension(strXmlPathName) + "_out.gpx";

			xmlDoc.Save(strXmlPathNameOUT);

			System.Console.ReadKey();
		}
	}
}
