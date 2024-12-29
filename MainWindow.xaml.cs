using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ToDoListWPF
{
    public partial class MainWindow : Window
    {
        private int taskCount = 0;
        private const int MaxTasks = 8;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            AddTask();
        }
        private void AddTask()
        {
            if (taskCount >= MaxTasks)
            {
                MessageBox.Show("You can not add more than 8 tasks.", "Limit Reached", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (taskCount == 0)
            {
                TaskHeaderText.Text = "Today's Tasks";
            }
            var taskPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 5, 0, 5)
            };
            var removeButton = new Button
            {

                Width = 20,
                Height = 20,
                Content = "-",
                FontSize = 22,
                Background = Brushes.Transparent,
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                Margin = new Thickness(3, -12, 8, 0),
            };
            removeButton.Click += (s, e) => {
                TaskListPanel.Children.Remove(taskPanel);
                taskCount--;
                if (taskCount == 0)
                {
                    TaskHeaderText.Text = "No tasks yet";
                }
            };
            var taskCheckBox = new CheckBox
            {
                Width = 20,
                Height = 20,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
            };
            var taskTextBox = new TextBox
            {
                VerticalAlignment = VerticalAlignment.Center,
                Width = 200,
                FontSize = 16,
                Foreground = Brushes.White,
                Background = Brushes.Transparent,
                Text = "New Task"
            };
            taskTextBox.GotFocus += TaskTextBox_GotFocus;
            taskTextBox.LostFocus += TaskTextBox_LostFocus;

            taskPanel.Children.Add(removeButton);
            taskPanel.Children.Add(taskTextBox);
            taskPanel.Children.Add(taskCheckBox);
            TaskListPanel.Children.Insert(TaskListPanel.Children.Count - 1, taskPanel);

            taskCount++;
        }
        private void TaskTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "New Task")
            {
                textBox.Text = "";
            }
        }
        private void TaskTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "New Task";
            }
        }
        private void AddTaskButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddTaskButton.Background = new SolidColorBrush(Color.FromRgb(0, 128, 0));
            AddTaskButton.Effect = new System.Windows.Media.Effects.DropShadowEffect
            {
                Color = Colors.Black,
                Direction = 320,
                ShadowDepth = 4,
                Opacity = 0.5
            };
        }
        private void AddTaskButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AddTaskButton.Background = Brushes.Green;
            AddTaskButton.Effect = null;
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddTask();
            }
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearAllTasks();
        }
        private void ClearAllTasks()
        {
            var childrenToRemove = TaskListPanel.Children
                .OfType<StackPanel>()
                .Where(child => child.Children
                    .OfType<Button>()
                    .Any(btn => btn.Content != null && btn.Content.ToString() == "-"))
                .ToList();

            foreach (var child in childrenToRemove)
            {
                TaskListPanel.Children.Remove(child);
            }

            taskCount = 0;
            TaskHeaderText.Text = "No tasks yet";
        }
    }
}