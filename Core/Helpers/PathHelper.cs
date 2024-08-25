using System.IO;
using System.Threading.Tasks;

namespace Core.Helpers
{
	public static class PathHelper
	{
		public static void EnsureDirExists(string dir)
		{
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
		}

		public static async Task WriteAllBytesAsync(string dest, byte[] data)
		{
			EnsureDirExists(Path.GetDirectoryName(dest));
			await File.WriteAllBytesAsync(dest, data).ConfigureAwait(false);
		}
	}
}
