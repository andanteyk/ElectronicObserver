using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserverUpdater.Utillity
{
	public static class ZipFileExtensions
	{
		/// <summary>
		/// Extract zip file to directory.
		/// this method doesn't generate root directory of zip file.
		/// </summary>
		/// <param name="source">Zip file</param>
		/// <param name="destinationDirectoryName">Destination Direcctory path</param>
		/// <param name="progress">Progress receiver</param>
		/// <param name="overwrite">overwrite?</param>
		public static void ExtractToDirectory(this ZipArchive source, string destinationDirectoryName, IProgress<ZipProgress> progress, bool overwrite)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			if (destinationDirectoryName == null)
				throw new ArgumentNullException(nameof(destinationDirectoryName));
			
			// Rely on Directory.CreateDirectory for validation of destinationDirectoryName.
			// Note that this will give us a good DirectoryInfo even if destinationDirectoryName exists:
			DirectoryInfo di = Directory.CreateDirectory(destinationDirectoryName);
			string destinationDirectoryFullPath = di.FullName;

			int count = 0;
			var root = source.Entries[0]?.FullName;
			foreach (ZipArchiveEntry entry in source.Entries)
			{
				count++;
				//Remove zip file Root directory from path.
				var newName = entry.FullName;//.Substring(root.Length); // Man What The Fuck?
				string fileDestinationPath = Path.Combine(destinationDirectoryFullPath, newName);

				if (!fileDestinationPath.StartsWith(destinationDirectoryFullPath, StringComparison.OrdinalIgnoreCase))
					throw new IOException("File is extracting to outside of the folder specified.");

				//Report extracting progress.
				var zipProgress = new ZipProgress(source.Entries.Count, count, entry.FullName);
				progress.Report(zipProgress);

				if (Path.GetFileName(fileDestinationPath).Length == 0)
				{
					// If it is a directory:
					if (entry.Length != 0)
						throw new IOException("Directory entry with data.");

					Directory.CreateDirectory(fileDestinationPath);
				}
				else
				{
					// If it is a file:
					// Create containing directory:
					//Check if entry is root directory
					if (!(fileDestinationPath == destinationDirectoryName))
					{
						Directory.CreateDirectory(Path.GetDirectoryName(fileDestinationPath));
						entry.ExtractToFile(fileDestinationPath, overwrite: overwrite);
					}
				}
			}
		}
	}
}
