using System.Windows;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using static Filo.FileSystem;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Diagnostics;


namespace Filo
{
    public partial class MainWindow : Window
    {
        private List<string> navigationHistory = new List<string>();
        private int currentHistoryIndex = -1;
        private string currentPath;
        private ObservableCollection<FileSystem.FileSystemItem> fileItems;

        public MainWindow() 
        {
            InitializeComponent();
            fileItems = new ObservableCollection<FileSystem.FileSystemItem>();
            InitializeUI();
        }

        // not a single clue what this is, need it for mouse button support
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.XButton1 == MouseButtonState.Pressed)
            {
                if (ToolbarBackButton.IsEnabled)
                {
                    ToolbarBackButton_Click(this, new RoutedEventArgs());
                    e.Handled = true;
                }
            }

            else if (e.XButton2 == MouseButtonState.Pressed)
            {
                if (ToolbarForwardButton.IsEnabled)
                {
                    ToolbarForwardButton_Click(this, new RoutedEventArgs());
                    e.Handled = true;
                }
            }
        }

        private void InitializeUI()
        {
            FileList.ItemsSource = fileItems;


            LoadTreeView(FolderTreeView);
        }

        private void LoadTreeView(TreeView treeView)
        {
            treeView.Items.Clear();

            var thisPc = new TreeViewItem { Header = "This PC", Tag = null };
            thisPc.Items.Add(CreateTreeItem("Desktop", Environment.SpecialFolder.Desktop));
            thisPc.Items.Add(CreateTreeItem("Documents", Environment.SpecialFolder.MyDocuments));
            thisPc.Items.Add(CreateTreeItem("Downloads", Environment.SpecialFolder.UserProfile + "\\Downloads"));
            thisPc.Items.Add(CreateTreeItem("Pictures", Environment.SpecialFolder.MyPictures));
            thisPc.Items.Add(CreateTreeItem("Music", Environment.SpecialFolder.MyMusic));
            thisPc.Items.Add(CreateTreeItem("Videos", Environment.SpecialFolder.MyVideos));
            thisPc.IsExpanded = true;
            treeView.Items.Add(thisPc);

            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    var driveItem = new TreeViewItem
                    {
                        Header = $"{drive.Name} ({drive.VolumeLabel})",
                        Tag = drive.RootDirectory.FullName
                    };
                    driveItem.Items.Add(null);
                    driveItem.Expanded += TreeViewItem_Expanded;
                    treeView.Items.Add(driveItem);
                }
            }
        }

        private void LoadDirectoryContents(string path)
        {
            fileItems.Clear();

            try
            {
                var dirInfo = new DirectoryInfo(path);

                foreach (var dir in dirInfo.GetDirectories())
                {
                    if (!dir.Attributes.HasFlag(FileAttributes.Hidden))
                    {
                        fileItems.Add(FileSystemItem.FromDirectory(dir));
                    }
                }

                foreach (var file in dirInfo.GetFiles())
                {
                    if (!file.Attributes.HasFlag(FileAttributes.Hidden))
                    {
                        fileItems.Add(FileSystemItem.FromFile(file));
                    }
                }

                UpdateStatusBar();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("no access.", "error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"error loading: {ex.Message}", "err", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateStatusBar()
        {
            MainStatusBarItemsText.Text = $"{fileItems.Count} items";

            long totalSize = fileItems.Where(f => !f.IsDirectory).Sum(f => f.GetSize());
            MainStatusBarSizeText.Text = Helpers.FormatFileSize(totalSize);
        }

        private TreeViewItem CreateTreeItem(string name, Environment.SpecialFolder folder)
        {
            return CreateTreeItem(name, Environment.GetFolderPath(folder));
        }

        private TreeViewItem CreateTreeItem(string name, string path)
        {
            var item = new TreeViewItem { Header = name, Tag = path };
            item.Items.Add(null);
            item.Expanded += TreeViewItem_Expanded;
            return item;
        }

        private void NavigateToPath(string path)
        {
            if (!Directory.Exists(path))
            {
                MessageBox.Show("dir does not exist.", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // add history if new navigation
            if (currentPath != null && currentPath != path)
            {
                // remove any forward history if new location
                if (currentHistoryIndex < navigationHistory.Count - 1)
                {
                    navigationHistory.RemoveRange(currentHistoryIndex + 1,
                        navigationHistory.Count - currentHistoryIndex - 1);
                }

                navigationHistory.Add(path);
                currentHistoryIndex = navigationHistory.Count - 1;
            }
            else if (currentPath == null)
            {
                // first
                navigationHistory.Add(path);
                currentHistoryIndex = 0;
            }

            currentPath = path;
            var pathBox = FindName("ToolbarPathTextBox") as TextBox;
            if (pathBox != null) pathBox.Text = path;

            LoadDirectoryContents(path);
            UpdateNavigationButtons();
        }

        private void UpdateNavigationButtons()
        {
            ToolbarBackButton.IsEnabled = currentHistoryIndex > 0;
            ToolbarForwardButton.IsEnabled = currentHistoryIndex < navigationHistory.Count - 1;
            ToolbarDirUpButton.IsEnabled = Directory.GetParent(currentPath) != null;
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListView;
            var item = listView?.SelectedItem as FileSystemItem;

            if (item != null)
            {
                if (item.IsDirectory)
                {
                    NavigateToPath(item.FullPath);
                }
                else
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo(item.FullPath) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"cant open file: {ex.Message}", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = e.NewValue as TreeViewItem;
            var path = item?.Tag as string;
            if (!string.IsNullOrEmpty(path))
            {
                NavigateToPath(path);
            }
        }

        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            var item = sender as TreeViewItem;
            if (item?.Items.Count == 1 && item.Items[0] == null)
            {
                item.Items.Clear();
                var path = item.Tag as string;
                if (!string.IsNullOrEmpty(path))
                {
                    try
                    {
                        foreach (var dir in Directory.GetDirectories(path))
                        {
                            var dirInfo = new DirectoryInfo(dir);
                            if (!dirInfo.Attributes.HasFlag(FileAttributes.Hidden))
                            {
                                var subItem = CreateTreeItem(dirInfo.Name, dir);
                                item.Items.Add(subItem);
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        private void ToolbarBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentHistoryIndex > 0)
            {
                currentHistoryIndex--;
                currentPath = navigationHistory[currentHistoryIndex];
                var pathBox = FindName("ToolbarPathTextBox") as TextBox;
                if (pathBox != null) pathBox.Text = currentPath;
                LoadDirectoryContents(currentPath);
                UpdateNavigationButtons();
            }
        }

        private void ToolbarForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentHistoryIndex < navigationHistory.Count - 1)
            {
                currentHistoryIndex++;
                currentPath = navigationHistory[currentHistoryIndex];
                var pathBox = FindName("ToolbarPathTextBox") as TextBox;
                if (pathBox != null) pathBox.Text = currentPath;
                LoadDirectoryContents(currentPath);
                UpdateNavigationButtons();
            }
        }

        private void ToolbarDirUpButton_Click(object sender, RoutedEventArgs e)
        {
            var parent = Directory.GetParent(currentPath);
            if (parent != null)
            {
                NavigateToPath(parent.FullName);
            }
        }

        private void ToolbarNewDirButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Helpers.InputDialog("enter folder name:", "new folder");
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.ResponseText))
            {
                try
                {
                    var newPath = Path.Combine(currentPath, dialog.ResponseText);
                    Directory.CreateDirectory(newPath);
                    LoadDirectoryContents(currentPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"cant create folder: {ex.Message}", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ToolbarDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var item = FileList?.SelectedItem as FileSystemItem;

            if (item != null)
            {
                var result = MessageBox.Show($"delete '{item.Name}'?", "confirm",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        if (item.IsDirectory)
                            Directory.Delete(item.FullPath, true);
                        else
                            File.Delete(item.FullPath);

                        LoadDirectoryContents(currentPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"cant delete: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ToolbarPathTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var pathBox = sender as TextBox;
                if (pathBox != null)
                {
                    NavigateToPath(pathBox.Text);
                }
            }
        }

        private void ToolbarPathGoButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPath(ToolbarPathTextBox.Text);
        }
    }

}
