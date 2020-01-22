using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TiaOpennessHelper.VWSymbolic;

namespace TiaPortalOpennessDemo.Views
{
    /// <summary>
    /// Interaction logic for OptionsRobotView.xaml
    /// </summary>
    public partial class OptionsRobotView : Window
    {
        public string SavePath { get; set; }
        public string PlcDBPath { get; set; }
        public object Current { get; set; }
        private List<RobotInfo> robsInfo;
        public List<List<RobotBase>> RobBase { get; set; }
        public List<List<RobotTecnologie>> RobTecnologies { get; set; }
        public List<List<RobotSafeRangeMonitoring>> RobSafeRangeMonitoring { get; set; }
        public List<List<RobotSafeOperation>> RobSafeOperations { get; set; }
        public bool Changes { get; set; }
        private RobotView rv;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="robsInfo"></param>
        public OptionsRobotView(object dataContext, List<RobotInfo> robsInfo)
        {
            Changes = false;
            this.robsInfo = robsInfo;
            DataContext = dataContext;
            InitializeComponent();
        }

        /// <summary>
        /// Initialise Robot View Window with Default Constructor
        /// </summary>
        private void InitRobotViewDefault()
        {
            rv = new RobotView(this)
            {
                SavePath = SavePath,
                Current = Current,
                PlcDBPath = PlcDBPath
            };
            rv.ShowDialog();
            Changes = rv.Changes;
        }

        /// <summary>
        /// Initialise Robot View Window with Robot from Excel Constructor
        /// </summary>
        private void InitRobotViewRobFromExcel(RobotInfo robInfo)
        {
            rv = new RobotView(this, robInfo)
            {
                SavePath = SavePath,
                Current = Current,
                PlcDBPath = PlcDBPath
            };
            rv.ShowDialog();
            Changes = rv.Changes;
        }

        /// <summary>
        /// Handles "BtnCreateNew_Click" event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCreateNew_Click(object sender, RoutedEventArgs e)
        {
            InitRobotViewDefault();
        }

        /// <summary>
        /// Add buttons to window
        /// </summary>
        private void AddButtons()
        {
            int margin = 97;
            for (int i = 0; i < robsInfo.Count; i++)
            {
                string name = robsInfo[i].Name.Insert(0, "r");

                LinearGradientBrush gradientBrush = new LinearGradientBrush
                {
                    StartPoint = new Point(0.5, 0),
                    EndPoint = new Point(0.5, 1),
                    MappingMode = BrushMappingMode.RelativeToBoundingBox
                };
                gradientBrush.GradientStops.Add(new GradientStop(Colors.White, 0.0));
                gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(220,220,220), 1.0));

                StackPanel sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                TextBlock tb = new TextBlock
                {
                    Text = robsInfo[i].Name + "  ",
                    TextAlignment = TextAlignment.Center,
                    FontSize = 14,
                    FontFamily = new FontFamily("Segoe UI Emoji")
                };
                Image image = new Image
                {
                    Source = new BitmapImage(new Uri(@"/TiaPortalOpennessDemo;component/Images/kuka_icon.ico", UriKind.Relative)),
                    Height = 15
                };
                sp.Children.Add(tb);
                sp.Children.Add(image);

                Button newBtn = new Button
                {
                    Content = robsInfo[i].Name,
                    Name = name,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 32,
                    Width = 307,
                    Margin = new Thickness(10, margin, 10, 10),
                    VerticalAlignment = VerticalAlignment.Top,
                    Background = gradientBrush
                };
                newBtn.Content = sp;

                var thisRobInfo = robsInfo[i];

                newBtn.Click += (s, e) => 
                {
                    InitRobotViewRobFromExcel(thisRobInfo);
                };

                buttons.Children.Add(newBtn);
                margin += 42;
            }
        }

        /// <summary>
        /// Handles "Window_Closed" event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            if(rv != null)
            {
                rv.Close();
            }
        }

        /// <summary>
        /// Handles "Window_Loaded" event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddButtons();
        }
    }
}
