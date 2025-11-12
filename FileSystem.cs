using System.Diagnostics;
using System.IO;
using System.Windows.Media;

namespace Filo
{
    public class FileSystem
    {
        public class FileSystemItem
        {
            public string Name { get; set; }
            public string FullPath { get; set; }
            public DateTime DateModified { get; set; }
            public string Type { get; set; }
            public string Size { get; set; }
            public bool IsDirectory { get; set; }
            public string Icon { get; set; }

            public static FileSystemItem FromDirectory(DirectoryInfo dirInfo)
            {
                return new FileSystemItem
                {
                    Name = dirInfo.Name,
                    FullPath = dirInfo.FullName,
                    DateModified = dirInfo.LastWriteTime,
                    Type = "File folder",
                    Size = "",
                    IsDirectory = true,
                    Icon = "📁"
                };
            }

            public static FileSystemItem FromFile(FileInfo fileInfo)
            {
                return new FileSystemItem
                {
                    Name = fileInfo.Name,
                    FullPath = fileInfo.FullName,
                    DateModified = fileInfo.LastWriteTime,
                    Type = Helpers.GetFileType(fileInfo.Extension),
                    Size = Helpers.FormatFileSize(fileInfo.Length),
                    IsDirectory = false,
                    Icon = Helpers.GetFileIcon(fileInfo.Extension)
                };
            }

            public void Open()
            {
                if (!IsDirectory && File.Exists(FullPath))
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo(FullPath) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException($"Cannot open file: {ex.Message}", ex);
                    }
                }
            }

            public void Delete()
            {
                try
                {
                    if (IsDirectory)
                    {
                        if (Directory.Exists(FullPath))
                            Directory.Delete(FullPath, true);
                    }
                    else
                    {
                        if (File.Exists(FullPath))
                            File.Delete(FullPath);
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Cannot delete: {ex.Message}", ex);
                }
            }

            public void Rename(string newName)
            {
                try
                {
                    string directory = Path.GetDirectoryName(FullPath);
                    string newPath = Path.Combine(directory, newName);

                    if (IsDirectory)
                    {
                        Directory.Move(FullPath, newPath);
                    }
                    else
                    {
                        File.Move(FullPath, newPath);
                    }

                    Name = newName;
                    FullPath = newPath;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Cannot rename: {ex.Message}", ex);
                }
            }

            public bool Exists()
            {
                return IsDirectory ? Directory.Exists(FullPath) : File.Exists(FullPath);
            }

            public long GetSize()
            {
                if (IsDirectory)
                    return GetDirectorySize(FullPath);

                if (File.Exists(FullPath))
                    return new FileInfo(FullPath).Length;

                return 0;
            }

            private static long GetDirectorySize(string path)
            {
                try
                {
                    var dirInfo = new DirectoryInfo(path);
                    long size = 0;

                    foreach (var file in dirInfo.GetFiles())
                        size += file.Length;

                    foreach (var dir in dirInfo.GetDirectories())
                        size += GetDirectorySize(dir.FullName);

                    return size;
                }
                catch
                {
                    return 0;
                }
            }


        }
    }
}
