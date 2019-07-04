using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

class SharpDir
{
	// This project was only possible with help and code from VladPVS.
	// https://github.com/VladPVS/FastSearchLibrary
	static void printUsage()
	{
		Console.WriteLine("\n[-] Usage: \n\t <PATH|.|C:\\|\\\\Computer\\Share\\> <FILE|payload.exe|passwords*|*.kdbx>\n");
		System.Environment.Exit(1);
	}
	static void Main(string[] args)
	{
		if (args == null || args.Length != 2)
		{
			printUsage();
		}

		string path = args[0];
		string pattern = args[1];

		try
		{
			List<FileInfo> files = GetFilesFast(path, pattern);

			Console.WriteLine();
			foreach (FileInfo File in files)
			{
				Console.WriteLine("{0}", File.FullName);
			}
		}
		catch (Exception)
		{
		}
	}

	public static List<FileInfo> GetFiles(string folder, string pattern)
	{
		DirectoryInfo dirInfo = null;
		DirectoryInfo[] directories = null;
		try
		{
			dirInfo = new DirectoryInfo(folder);
			directories = dirInfo.GetDirectories();

			if (directories.Length == 0)
				return new List<FileInfo>(dirInfo.GetFiles(pattern));
		}
		catch (Exception)
		{
			return new List<FileInfo>();
		}

		List<FileInfo> result = new List<FileInfo>();

		foreach (var d in directories)
		{
			result.AddRange(GetFiles(d.FullName, pattern));
		}

		try
		{
			result.AddRange(dirInfo.GetFiles(pattern));
		}
		catch (Exception)
		{
		}

		return result;
	}

	public static List<DirectoryInfo> GetStartDirectories(string folder, ConcurrentBag<FileInfo> files, string pattern)
	{
		DirectoryInfo dirInfo = null;
		DirectoryInfo[] directories = null;
		try
		{
			dirInfo = new DirectoryInfo(folder);
			directories = dirInfo.GetDirectories();

			foreach (var f in dirInfo.GetFiles(pattern))
			{
				files.Add(f);
			}

			if (directories.Length > 1)
				return new List<DirectoryInfo>(directories);

			if (directories.Length == 0)
				return new List<DirectoryInfo>();

		}
		catch (Exception)
		{
			return new List<DirectoryInfo>();
		}

		return GetStartDirectories(directories[0].FullName, files, pattern);
	}

	public static List<FileInfo> GetFilesFast(string folder, string pattern)
	{
		ConcurrentBag<FileInfo> files = new ConcurrentBag<FileInfo>();

		try
		{
			List<DirectoryInfo> startDirs = GetStartDirectories(folder, files, pattern);

			startDirs.AsParallel().ForAll((d) =>
			{
				GetStartDirectories(d.FullName, files, pattern).AsParallel().ForAll((dir) =>
				{
					GetFiles(dir.FullName, pattern).ForEach((f) => files.Add(f));
				});
			});
		}
		catch (Exception)
		{
		}
		return files.ToList();
	}
}