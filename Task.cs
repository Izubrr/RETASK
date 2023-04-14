using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using FontAwesome6;
using FontAwesome6.Svg;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RETASK
{
    internal class WorkWithTasks
    {

        static List<Dictionary<string, object>> ReadJson()
        {
            File.AppendAllText("tasks.json", "");
            string text = File.ReadAllText("tasks.json");
            List<Dictionary<string, object>> tasks = new List<Dictionary<string, object>>();
            string json = File.ReadAllText("tasks.json");
            tasks = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
            return tasks;
        }


        public void updateTasks(WrapPanel wrapPanel)
        {
            var tasks = ReadJson();
            if (!File.Exists("date.txt")) File.WriteAllText("date.txt", DateTime.Today.ToString());
            else
            {
                string content = File.ReadAllText("date.txt");
                DateTime today = Convert.ToDateTime(content);
                if (DateTime.Today > today)
                {
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        Dictionary<string, object> task = tasks[i];
                        task["complete"] = false;
                        string updatedjson = JsonConvert.SerializeObject(tasks);
                        File.WriteAllText("tasks.json", updatedjson);
                    }
                    File.WriteAllText("date.txt", DateTime.Today.ToString());
                    LoadTasks(wrapPanel, "Create");
                }
                else if (DateTime.Today == today) LoadTasks(wrapPanel, "Create");
            }
        }

        public void AddTask(WrapPanel wrapPanel, string name, Dictionary<string, bool> days, string selectedcolor)
        {
            var tasks = ReadJson();
            TaskUICreate(wrapPanel, name, days, selectedcolor, "Create");
            Dictionary<string, object> taskData = new Dictionary<string, object>
            {
                { "name", name },
                { "days", days },
                { "selectedcolor", selectedcolor },
                { "complete", false }
            };
            tasks.Add(taskData);
            string json = JsonConvert.SerializeObject(tasks);
            File.WriteAllText("tasks.json", json);
        }
        public void DoneTask(object sender, MouseButtonEventArgs e)
        {
            var tasks = ReadJson();
            Canvas canvas = (sender as Rectangle).Parent as Canvas;
            canvas.Visibility = Visibility.Collapsed;
            Canvas clickedObject = canvas;
            WrapPanel wrapPanel = clickedObject.Parent as WrapPanel;
            int index = wrapPanel.Children.IndexOf(clickedObject);
            Dictionary<string, object> task = tasks[index];
            task["complete"] = true;
            string updatedjson = JsonConvert.SerializeObject(tasks);
            File.WriteAllText("tasks.json", updatedjson);
        }

        public void DeleteTask(Canvas clickedObject, int index)
        {
            string json = File.ReadAllText("tasks.json");
            List<Dictionary<string, object>> items = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
            if (items.Count > 0) items.RemoveAt(index);
            string updatedJson = JsonConvert.SerializeObject(items);
            File.WriteAllText("tasks.json", updatedJson);
            WrapPanel wrapPanel = clickedObject.Parent as WrapPanel;
            wrapPanel.Children.Remove(clickedObject);
        }

        private void DoneButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Rectangle)sender).Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#00FFFFFF");
        }

        private void DoneButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Rectangle)sender).Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#7eb4ea");
        }

        private void TaskUICreate(WrapPanel wrapPanel, string name, Dictionary<string, bool> days, string selectedcolor, string operation)
        {
            var tasks = ReadJson();

            Rectangle donerectangle = new Rectangle();
            donerectangle.Height = 33.0;
            donerectangle.Width = 33.0;
            Canvas.SetLeft(donerectangle, 248.0);
            Canvas.SetTop(donerectangle, 3.0);
            donerectangle.RadiusX = 360.0;
            donerectangle.RadiusY = 360.0;
            donerectangle.Fill = Brushes.Transparent;
            donerectangle.MouseDown += DoneTask;
            donerectangle.MouseEnter += DoneButton_MouseEnter;
            donerectangle.MouseLeave += DoneButton_MouseLeave;
            SvgAwesome svgAwesome = new SvgAwesome();
            svgAwesome.Icon = EFontAwesomeIcon.Solid_CircleCheck;
            svgAwesome.PrimaryColor = Brushes.WhiteSmoke;
            svgAwesome.IsHitTestVisible = false;
            svgAwesome.Height = 29.0;
            svgAwesome.Width = 29.0;
            Canvas.SetRight(svgAwesome, 5.0);
            Canvas.SetTop(svgAwesome, 5.0);
            svgAwesome.HorizontalAlignment = HorizontalAlignment.Left;
            svgAwesome.VerticalAlignment = VerticalAlignment.Center;
            Label label = new Label();
            label.Content = name;
            Canvas.SetLeft(label, 10.0);
            Canvas.SetTop(label, 67.0);
            label.FontSize = 18.0;
            label.Foreground = Brushes.White;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.Width = 264.0;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.IsHitTestVisible = false;
            Label label2 = new Label();
            foreach (KeyValuePair<string, bool> day in days)
            {
                bool value = day.Value;
                if (value)
                {
                    Label label3 = label2;
                    object content = label3.Content;
                    label3.Content = ((content != null) ? content.ToString() : null) + " " + day.Key;
                }
            }
            Canvas.SetLeft(label2, 10.0);
            Canvas.SetTop(label2, 132.0);
            label2.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFE4E4E4");
            label2.HorizontalAlignment = HorizontalAlignment.Left;
            label2.VerticalAlignment = VerticalAlignment.Center;
            label2.Width = 264.0;
            label2.IsHitTestVisible = false;
            Canvas canvas = new Canvas();
            canvas.Name = "T1";
            canvas.Height = 168.0;
            canvas.Width = 284.0;
            canvas.HorizontalAlignment = HorizontalAlignment.Left;
            canvas.VerticalAlignment = VerticalAlignment.Top;
            canvas.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(selectedcolor);
            canvas.Margin = new Thickness(32.0, 10.0, 0.0, 0.0);
            if (operation == "Create")
            {
                canvas.Children.Add(donerectangle);
                canvas.Children.Add(svgAwesome);
            }
            canvas.Children.Add(label);
            canvas.Children.Add(label2);
            foreach (Canvas x in new List<Canvas>{ canvas })
            {
                wrapPanel.Children.Add(x);
            }
        }
        public void LoadTasks(WrapPanel wrapPanel, string operation)
        {
            var tasks = ReadJson();
            wrapPanel.Children.Clear();
            string todayStr = DateTime.Today.DayOfWeek.ToString().Substring(0, 3);
            if (tasks.Count > 0)
            {
                for (int i = 0; i < tasks.Count; i++)
                {
                    string selectedcolor = (string)tasks[i]["selectedcolor"];
                    string name = (string)tasks[i]["name"];
                    JObject days = tasks[i]["days"] as JObject;
                    Dictionary<string, bool> days_object = days.ToObject<Dictionary<string, bool>>();
                    TaskUICreate(wrapPanel, name, days_object, selectedcolor, operation);
                    if (operation == "Create")
                    {
                        bool complete = (bool)tasks[i]["complete"];
                        if (complete) wrapPanel.Children[i].Visibility = Visibility.Collapsed;
                    }
                }         
            }
        }
    }
}
