using UnityEngine;
using System.Collections;
using System.IO;

public class FileIOUtil
{

	public static void WriteStringToFile (string path, string fileName, string content, bool appendFile)
	{
		StreamWriter sw = new StreamWriter(path + fileName, appendFile);
		sw.WriteLine(content);
		sw.Close();
	}

	public static string ReadStringFromFile (string fileName)
	{
		if (!File.Exists(fileName))
		{
			return "";	
		}

		StreamReader sr = new StreamReader(fileName);
		string fileContents = sr.ReadToEnd();
		sr.Close();
		return fileContents;
	}

	public static string[] GetStringArrayFromFile (string path, string fileName, char delim)
	{
		return GetStringArrayFromFile(path + fileName, delim);
	}

	public static string[] GetStringArrayFromFile (string fullPath, char delim)
	{
		if (!File.Exists(fullPath))
		{
			return new string[1] {""};	
		}

		StreamReader sr = new StreamReader(fullPath);
		string fileContents = sr.ReadToEnd();
		sr.Close();
		string[] result = fileContents.Split(delim);
		return result;
	}

	public static void WriteLineToFile (string[] strings, string PATH, string fileName, bool append)
	{
		string line = "";
		foreach (string s in strings)
		{
			line += s;
		}

		StreamWriter sw = new StreamWriter(PATH + fileName, append);

		sw.WriteLine(line);
		sw.Close();
	}

	public static string[] ReadLineFromFile (string PATH, string fileName)
	{
		StreamReader sr = new StreamReader(PATH + fileName);
		string output = sr.ReadLine();
		sr.Close();

		return output.Split('|');
	}


	public static string SerializeColor(Color c)
	{
		return string.Format("{0} {1} {2} {3}", c.r, c.g, c.b, c.a);
	}

	public static string SerializeVector4(Vector4 c)
	{
		return string.Format("{0} {1} {2} {3}", c.x, c.y, c.z, c.w);
	}

	public static string SerializeVector3(Vector3 c)
	{
		return string.Format("{0} {1} {2}", c.x, c.y, c.z);
	}

	public static string SerializeVector2(Vector2 c)
	{
		return string.Format("{0} {1}", c.x, c.y);
	}

	public static Vector3 DeserializeVector3(string v)
	{
		char[] sep = { ' ' };
		string[] s = v.Split(sep);
		Vector3 c = new Vector3();
		c.x = float.Parse(s[0]);
		c.y = float.Parse(s[1]);
		c.z = float.Parse(s[2]);
		return c;
	}

	public static Vector4 DeserializeVector4(string v)
	{
		char[] sep = { ' ' };
		string[] s = v.Split(sep);
		Vector4 c = new Vector4();
		c.x = float.Parse(s[0]);
		c.y = float.Parse(s[1]);
		c.z = float.Parse(s[2]);
		c.w = float.Parse(s[3]);
		return c;
	}

	public static Vector2 DeserializeVector2(string v)
	{
		char[] sep = { ' ' };
		string[] s = v.Split(sep);
		Vector2 c = new Vector2();
		c.x = float.Parse(s[0]);
		c.y = float.Parse(s[1]);
		return c;
	}

	public static Color DeserializeColor(string v)
	{
		char[] sep = { ' ' };
		string[] s = v.Split(sep);
		Color c = new Color();
		c.r = float.Parse(s[0]);
		c.g = float.Parse(s[1]);
		c.b = float.Parse(s[2]);
		c.a = float.Parse(s[3]);
		return c;
	}

}

