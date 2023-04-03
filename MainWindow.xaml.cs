using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace RETASK
{
    public partial class MainWindow : Window, IComponentConnector
    {
        public MainWindow()
        {
            InitializeComponent();
            Mon.Fill =  white;
            Tue.Fill =  white;
            Wed.Fill =  white;
            Thu.Fill =  white;
            Fri.Fill =  white;
            Sat.Fill =  white;
            Sun.Fill =  white;
            WorkWithTasks task = new WorkWithTasks();
            task.updateTasks( HomeContainer);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += (sender, e) =>
            {
                task.updateTasks(HomeContainer);
            };
            timer.Interval = TimeSpan.FromHours(1);
            timer.Start();
        }
        Brush
            orange = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFE66F24"),
            darkorange = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFC75E1C"),
            blue = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF424D88"),
            black = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF000000"),
            gray = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF3E3E3E"),
            lightgray = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFABADB3"),
            ultralightgray = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFFFFF"),
            white = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFFFFF"),
            transparent = (SolidColorBrush)new BrushConverter().ConvertFrom("#00FFFFFF"),
            hoverblue = (SolidColorBrush)new BrushConverter().ConvertFrom("#7eb4ea");

        private void HideContainers()
        {
             HomeContainer.Visibility = Visibility.Hidden;
             AddContainer.Visibility = Visibility.Hidden;
             StatsContainer.Visibility = Visibility.Hidden;
             DeleteContainer.Visibility = Visibility.Hidden;
             HomeContainerScroll.Visibility = Visibility.Hidden;
             DeleteContainerScroll.Visibility = Visibility.Hidden;
             TodaySwitchIcon.Visibility = Visibility.Hidden;
             TodaySwitchButton.Visibility = Visibility.Hidden;
             HomeButton.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFE66F24");
             AddButton.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFE66F24");
             StatsButton.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFE66F24");
             DeleteButton.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFE66F24");
        }

        public void DeleteContainer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WorkWithTasks task = new WorkWithTasks();
            Canvas clickedObject = e.Source as Canvas;
            WrapPanel wrapPanel = clickedObject.Parent as WrapPanel;
            int index = wrapPanel.Children.IndexOf(clickedObject);
             task.DeleteTask(clickedObject, index);
        }

        private void TodaySwitch(object sender, MouseButtonEventArgs e)
        {
            WorkWithTasks task = new WorkWithTasks();
            if (TodaySwitchIcon.PrimaryColor == orange)
            {
                 TodaySwitchIcon.PrimaryColor =  ultralightgray;
                 task.LoadTasks( HomeContainer, "Delete");
            }
            else
            {
                 TodaySwitchIcon.PrimaryColor =  orange;
                 task.updateTasks( HomeContainer);
            }
        }

        private void sidebutton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WorkWithTasks task = new WorkWithTasks();
            Rectangle btn = (Rectangle)sender;
            string name = btn.Name;

            switch (name)
            {
                case "HomeButton":
                    HideContainers();
                    task.updateTasks(HomeContainer);
                    TodaySwitchIcon.PrimaryColor = orange;
                    HomeContainer.Visibility = Visibility.Visible;
                    HomeContainerScroll.Visibility = Visibility.Visible;
                    TodaySwitchIcon.Visibility = Visibility.Visible;
                    TodaySwitchButton.Visibility = Visibility.Visible;
                    HomeButton.Fill = blue;
                    break;
                case "AddButton":
                    HideContainers();
                    AddContainer.Visibility = Visibility.Visible;
                    AddButton.Fill = blue;
                    Description.Text = "";
                    break;
                case "DeleteButton":
                    HideContainers();
                    DeleteContainer.Visibility = Visibility.Visible;
                    DeleteContainerScroll.Visibility = Visibility.Visible;
                    DeleteButton.Fill = blue;
                    task.LoadTasks(DeleteContainer, "Delete");
                    break;
            }
        }

        private void sidebutton_MouseEnter(object sender, EventArgs e)
        {
            Rectangle btn = (Rectangle)sender;
            string name = btn.Name;
            switch (name)
            {
                case "HomeButton":
                    if (HomeContainer?.Visibility == Visibility.Hidden) btn.Fill = darkorange;
                    break;
                case "AddButton":
                    if (AddContainer?.Visibility == Visibility.Hidden) btn.Fill = darkorange;
                    break;
                case "StatsButton":
                    if (StatsContainer?.Visibility == Visibility.Hidden) btn.Fill = darkorange;
                    break;
                case "DeleteButton":
                    if (DeleteContainer?.Visibility == Visibility.Hidden) btn.Fill = darkorange;
                    break;
            }

        }
        private void sidebutton_MouseLeave(object sender, EventArgs e)
        {
            Rectangle btn = (Rectangle)sender;
            string name = btn.Name;
            switch (name)
            {
                case "HomeButton":
                    if (HomeContainer?.Visibility == Visibility.Hidden) btn.Fill = orange;
                    break;
                case "AddButton":
                    if (AddContainer?.Visibility == Visibility.Hidden) btn.Fill = orange;
                    break;
                case "StatsButton":
                    if (StatsContainer?.Visibility == Visibility.Hidden) btn.Fill = orange;
                    break;
                case "DeleteButton":
                    if (DeleteContainer?.Visibility == Visibility.Hidden) btn.Fill = orange;
                    break;
            }
        }

                string selectedcolor = "#FF219040";
        private Dictionary<string, bool> days = new Dictionary<string, bool>
        {
            { "Mon", false },
            { "Tue", false },
            { "Wed", false },
            { "Thu", false },
            { "Fri", false },
            { "Sat", false },
            { "Sun", false }
        };

        private void SaveTaskButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WorkWithTasks task = new WorkWithTasks();
            task.AddTask( HomeContainer,  Description.Text,  days,  selectedcolor);
             HideContainers();
             HomeContainer.Visibility = Visibility.Visible;
             HomeContainerScroll.Visibility = Visibility.Visible;
             HomeButton.Fill =  blue;
        }

        private void colorbutton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
            defaultInterpolatedStringHandler.AppendFormatted<Color>(((SolidColorBrush)((Rectangle)sender).Fill).Color);
             selectedcolor = defaultInterpolatedStringHandler.ToStringAndClear();
        }

        private void DoneButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Rectangle)sender).Fill =  transparent;
        }

        private void AddContainer_MouseDown(object sender, MouseButtonEventArgs e)
        {
             TaskExample.Background = (SolidColorBrush)new BrushConverter().ConvertFrom( selectedcolor);
             NameExample.Content =  Description.Text;
             DayExample.Content = "";
            foreach (KeyValuePair<string, bool> day in  days)
            {
                bool value = day.Value;
                if (value)
                {
                    Label dayExample =  DayExample;
                    object content = dayExample.Content;
                    dayExample.Content = ((content != null) ? content.ToString() : null) + " " + day.Key;
                }
            }
        }

        private void EN_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TEXT1.Content = "Add Tasks";
            TEXT2.Content = "Achieve your goals systematically";
            TEXT3.Content = "Name your task";
            TEXT4.Content = "Weekly frequency";
            TEXT5.Content = "Mon";
            TEXT6.Content = "Tue";
            TEXT7.Content = "Wen";
            TEXT8.Content = "Thu";
            TEXT9.Content = "Fri";
            TEXT10.Content = "Sat";
            TEXT11.Content = "Sun";
            TEXT12.Content = "Task color";
            Settings.Visibility = Visibility.Hidden;
        }

        private void RU_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TEXT1.Content = "Добавить задачу";
            TEXT2.Content = "Достигайте своих целей систематично";
            TEXT3.Content = "Назовите свою задачу";
            TEXT4.Content = "Еженедельная периодичность";
            TEXT5.Content = "Пн";
            TEXT6.Content = "Вт";
            TEXT7.Content = "Ср";
            TEXT8.Content = "Чт";
            TEXT9.Content = "Пт";
            TEXT10.Content = "Сб";
            TEXT11.Content = "Вс";
            TEXT12.Content = "Цвет задачи";
            Settings.Visibility = Visibility.Hidden;
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bool flag = e.ChangedButton == MouseButton.Left;
            if (flag)
            {
                base.DragMove();
            }
        }

        private void SettingsButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bool flag =  Settings.Visibility == Visibility.Hidden;
            if (flag)
            {
                 Settings.Visibility = Visibility.Visible;
            }
            else
            {
                 Settings.Visibility = Visibility.Hidden;
            }
        }

        private void Description_TextChanged(object sender, TextChangedEventArgs e)
        {
             NameExample.Content =  Description.Text;
        }

        private void DoneButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Rectangle)sender).Fill =  hoverblue;
        }

        private void day_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle btn = (Rectangle)sender;
            bool flag = btn.Fill == white;
            btn.Fill = ultralightgray;
            string name = btn.Name;
            if (days.ContainsKey(name))
            {
                days[name] = !days[name];
            }
        }

        private void buttons_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Rectangle)sender).Stroke =  hoverblue;
            ((Rectangle)sender).StrokeThickness = 2.5;
        }

        private void buttons_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Rectangle)sender).Stroke =  lightgray;
            ((Rectangle)sender).StrokeThickness = 1.0;
        }

        private void CloseButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            base.Close();
        }

        private void MinimizeButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            base.WindowState = WindowState.Minimized;
        }

        private void windowbutton_MouseEnter(object sender, EventArgs e)
        {
            Rectangle btn = (Rectangle)sender;
            string name = btn.Name;
            string a = name;
            if (!(a == "CloseButton"))
            {
                if (a == "MinimizeButton")
                {
                     MinimizeRectangle1.Fill =  black;
                     MinimizeRectangle1.Height = 2.2;
                }
            }
            else
            {
                 CloseRectangle1.Fill =  black;
                 CloseRectangle2.Fill =  black;
                 CloseRectangle1.Height = 2.2;
                 CloseRectangle2.Height = 2.2;
            }
        }

        private void windowbutton_MouseLeave(object sender, EventArgs e)
        {
            Rectangle btn = (Rectangle)sender;
            string name = btn.Name;
            string a = name;
            if (!(a == "CloseButton"))
            {
                if (a == "MinimizeButton")
                {
                     MinimizeRectangle1.Fill =  gray;
                     MinimizeRectangle1.Height = 1.5;
                }
            }
            else
            {
                 CloseRectangle1.Fill =  gray;
                 CloseRectangle2.Fill =  gray;
                 CloseRectangle1.Height = 1.5;
                 CloseRectangle2.Height = 1.5;
            }
        }
    }
}