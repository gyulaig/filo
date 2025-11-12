using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Filo
{
    internal class Helpers
    {
        public static class FileUtils
        {
            public static string GetFileIcon(string extension)
            {
                return extension.ToLower() switch
                {
                    // Images
                    ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" or ".tiff" or ".svg" or ".webp" or ".heic" or ".avif" => "🖼️",

                    // Documents
                    ".doc" or ".docx" or ".odt" or ".rtf" or ".txt" or ".md" or ".log" or ".tex" or ".pdf" => "📄",

                    // Spreadsheets
                    ".xls" or ".xlsx" or ".ods" or ".csv" or ".tsv" => "📊",

                    // Presentations
                    ".ppt" or ".pptx" or ".odp" or ".key" => "📽️",

                    // Archives
                    ".zip" or ".rar" or ".7z" or ".tar" or ".gz" or ".bz2" or ".xz" or ".iso" => "📦",

                    // Audio
                    ".mp3" or ".wav" or ".flac" or ".aac" or ".ogg" or ".wma" or ".m4a" or ".aiff" or ".mid" or ".midi" => "🎵",

                    // Video
                    ".mp4" or ".avi" or ".mkv" or ".mov" or ".wmv" or ".webm" or ".flv" or ".mpeg" or ".mpg" or ".3gp" => "🎬",

                    // Code & Scripts
                    ".cs" or ".cpp" or ".c" or ".h" or ".hpp" or ".py" or ".js" or ".ts" or ".jsx" or ".tsx" or ".html" or ".css" or
                    ".java" or ".php" or ".json" or ".xml" or ".sql" or ".sh" or ".rb" or ".go" or ".rs" or ".swift" or ".kt" or ".kts" or
                    ".lua" or ".pl" or ".r" or ".ipynb" or ".dart" => "💻",

                    // System / Config
                    ".exe" or ".msi" or ".bat" or ".cmd" or ".ini" or ".cfg" or ".conf" or ".yaml" or ".yml" or ".env" or ".dll" or ".sys" => "⚙️",

                    // Fonts
                    ".ttf" or ".otf" or ".woff" or ".woff2" or ".eot" => "🔤",

                    // Design / Creative
                    ".psd" or ".ai" or ".xd" or ".fig" or ".sketch" or ".afdesign" or ".afphoto" or ".blend" => "🎨",

                    // CAD / 3D
                    ".dwg" or ".dxf" or ".stl" or ".obj" or ".fbx" or ".3ds" or ".step" or ".iges" or ".glb" or ".gltf" => "📐",

                    // eBooks / Text
                    ".epub" or ".mobi" or ".azw3" => "📚",

                    // Internet / Data
                    ".torrent" or ".url" or ".html" or ".htm" => "🌐",
                    ".db" or ".sqlite" or ".sqlite3" or ".db3" => "🗄️",

                    // Science / Data
                    ".mat" or ".npz" or ".h5" or ".hdf5" or ".sav" or ".dta" => "🧪",

                    _ => "📄"
                };
            }

            public static string GetFileType(string extension)
            {
                return extension.ToLower() switch
                {
                    // Images
                    ".jpg" or ".jpeg" => "JPEG Image",
                    ".png" => "PNG Image",
                    ".gif" => "GIF Image",
                    ".bmp" => "Bitmap Image",
                    ".tiff" => "TIFF Image",
                    ".svg" => "Scalable Vector Graphic",
                    ".webp" => "WebP Image",
                    ".heic" => "HEIC Image",
                    ".avif" => "AVIF Image",

                    // Documents
                    ".doc" or ".docx" => "Microsoft Word Document",
                    ".odt" => "OpenDocument Text",
                    ".rtf" => "Rich Text Format",
                    ".txt" => "Text File",
                    ".md" => "Markdown File",
                    ".log" => "Log File",
                    ".tex" => "LaTeX Document",
                    ".pdf" => "PDF Document",

                    // Spreadsheets
                    ".xls" or ".xlsx" => "Excel Spreadsheet",
                    ".ods" => "OpenDocument Spreadsheet",
                    ".csv" => "Comma-Separated Values File",
                    ".tsv" => "Tab-Separated Values File",

                    // Presentations
                    ".ppt" or ".pptx" => "PowerPoint Presentation",
                    ".odp" => "OpenDocument Presentation",
                    ".key" => "Apple Keynote Presentation",

                    // Archives
                    ".zip" => "ZIP Archive",
                    ".rar" => "RAR Archive",
                    ".7z" => "7-Zip Archive",
                    ".tar" => "TAR Archive",
                    ".gz" => "GZIP Archive",
                    ".bz2" => "BZIP2 Archive",
                    ".xz" => "XZ Archive",
                    ".iso" => "Disk Image File",

                    // Audio
                    ".mp3" => "MP3 Audio",
                    ".wav" => "WAV Audio",
                    ".flac" => "FLAC Audio",
                    ".aac" => "AAC Audio",
                    ".ogg" => "Ogg Vorbis Audio",
                    ".wma" => "Windows Media Audio",
                    ".m4a" => "MPEG-4 Audio",
                    ".aiff" => "AIFF Audio",
                    ".mid" or ".midi" => "MIDI File",

                    // Video
                    ".mp4" => "MP4 Video",
                    ".avi" => "AVI Video",
                    ".mkv" => "Matroska Video",
                    ".mov" => "QuickTime Movie",
                    ".wmv" => "Windows Media Video",
                    ".webm" => "WebM Video",
                    ".flv" => "Flash Video",
                    ".mpeg" or ".mpg" => "MPEG Video",
                    ".3gp" => "3GPP Video",

                    // Programming
                    ".cs" => "C# Source File",
                    ".cpp" => "C++ Source File",
                    ".c" => "C Source File",
                    ".h" or ".hpp" => "C/C++ Header File",
                    ".py" => "Python Script",
                    ".js" => "JavaScript File",
                    ".ts" => "TypeScript File",
                    ".jsx" => "React JSX File",
                    ".tsx" => "React TypeScript File",
                    ".html" or ".htm" => "HTML Document",
                    ".css" => "Cascading Style Sheet",
                    ".java" => "Java Source File",
                    ".php" => "PHP Script",
                    ".json" => "JSON File",
                    ".xml" => "XML Document",
                    ".sql" => "SQL Database File",
                    ".sh" => "Shell Script",
                    ".rb" => "Ruby Script",
                    ".go" => "Go Source File",
                    ".rs" => "Rust Source File",
                    ".swift" => "Swift Source File",
                    ".kt" or ".kts" => "Kotlin Source File",
                    ".lua" => "Lua Script",
                    ".pl" => "Perl Script",
                    ".r" => "R Script",
                    ".ipynb" => "Jupyter Notebook",
                    ".dart" => "Dart Source File",

                    // System / Config
                    ".exe" => "Windows Executable",
                    ".msi" => "Windows Installer Package",
                    ".bat" or ".cmd" => "Batch Script",
                    ".ini" or ".cfg" or ".conf" or ".yaml" or ".yml" or ".env" => "Configuration File",
                    ".dll" => "Dynamic Link Library",
                    ".sys" => "System File",

                    // Fonts
                    ".ttf" => "TrueType Font",
                    ".otf" => "OpenType Font",
                    ".woff" => "Web Open Font Format",
                    ".woff2" => "Web Open Font Format 2",
                    ".eot" => "Embedded OpenType Font",

                    // Design
                    ".psd" => "Adobe Photoshop Document",
                    ".ai" => "Adobe Illustrator File",
                    ".xd" => "Adobe XD Project",
                    ".fig" => "Figma Design File",
                    ".sketch" => "Sketch Design File",
                    ".afdesign" => "Affinity Designer File",
                    ".afphoto" => "Affinity Photo File",
                    ".blend" => "Blender Project",

                    // 3D / CAD
                    ".dwg" => "AutoCAD Drawing",
                    ".dxf" => "AutoCAD Exchange File",
                    ".stl" => "STL 3D Model",
                    ".obj" => "Wavefront OBJ Model",
                    ".fbx" => "Autodesk FBX File",
                    ".3ds" => "3D Studio Model",
                    ".step" or ".stp" => "STEP 3D Model",
                    ".iges" or ".igs" => "IGES 3D Model",
                    ".glb" or ".gltf" => "GL Transmission Format",

                    // eBooks
                    ".epub" => "EPUB eBook",
                    ".mobi" => "Mobipocket eBook",
                    ".azw3" => "Amazon Kindle eBook",

                    // Web / Internet
                    ".torrent" => "BitTorrent File",
                    ".url" => "Internet Shortcut",
                    ".db" or ".sqlite" or ".sqlite3" or ".db3" => "SQLite Database",

                    // Scientific
                    ".mat" => "MATLAB Data File",
                    ".npz" => "NumPy Compressed Archive",
                    ".h5" or ".hdf5" => "HDF5 Data File",
                    ".sav" => "SPSS Data File",
                    ".dta" => "Stata Data File",

                    _ => $"{extension.ToUpper().TrimStart('.')} File"
                };
            }
        }

        public static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            return $"{len:0.#} {sizes[order]}";
        }

        // garbage AI code just to make it work
        public class InputDialog : Window
        {
            private TextBox textBox;
            public string ResponseText => textBox.Text;

            public InputDialog(string question, string title)
            {
                Title = title;
                Width = 400;
                Height = 150;
                WindowStartupLocation = WindowStartupLocation.CenterOwner;

                var grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                var stack = new StackPanel { Margin = new Thickness(10) };
                stack.Children.Add(new TextBlock { Text = question, Margin = new Thickness(0, 0, 0, 10) });
                textBox = new TextBox();
                textBox.KeyDown += (s, e) => { if (e.Key == Key.Enter) DialogResult = true; };
                stack.Children.Add(textBox);
                Grid.SetRow(stack, 0);
                grid.Children.Add(stack);

                var buttonPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(10)
                };

                var okBtn = new Button { Content = "OK", Width = 75, Margin = new Thickness(0, 0, 10, 0), IsDefault = true };
                okBtn.Click += (s, e) => DialogResult = true;
                buttonPanel.Children.Add(okBtn);

                var cancelBtn = new Button { Content = "Cancel", Width = 75, IsCancel = true };
                buttonPanel.Children.Add(cancelBtn);

                Grid.SetRow(buttonPanel, 1);
                grid.Children.Add(buttonPanel);

                Content = grid;
                textBox.Focus();
            }
        }
    }
}

