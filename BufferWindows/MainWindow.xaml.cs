using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HalconDotNet;
using System.Threading;

namespace BufferWindows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private HTuple BufferWindow;
        
        public MainWindow()
        {
            InitializeComponent();
            
        }

        //测试普通状况下的绘图时间
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Thread(new ThreadStart(() =>
            {
                HOperatorSet.ClearWindow(hWindow1.HalconWindow);
                DateTime d1 = DateTime.Now;
                HObject hobj;
                HTuple Row, Col;
                HOperatorSet.ReadImage(out hobj, @"C:\Users\Public\Documents\MVTec\HALCON-12.0\examples\images\autobahn\scene_00.png");
                HOperatorSet.GetImageSize(hobj, out Col, out Row);
                HOperatorSet.SetPart(hWindow1.HalconWindow, 0, 0, Col - 1, Row - 1);
                hobj.DispObj(hWindow1.HalconWindow);
                hobj.Dispose();
                Random ran = new Random();
                HOperatorSet.GenCircleContourXld(out hobj, 200, 200, 100, 0, 6.28318, "positive", 1);
                for (int i = 0; i < 5000; i++)
                {
                    hobj.DispObj(hWindow1.HalconWindow);
                    HOperatorSet.WriteString(hWindow1.HalconWindow, "测试数据");
                }
                hobj.Dispose();
                Log1.Dispatcher.BeginInvoke(new Action(() => {
                    Log1.Text = "耗时" + DateTime.Now.Subtract(d1).TotalMilliseconds + "ms";
                }));
            }))
            { IsBackground = true}.Start();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new Thread(new ThreadStart(() =>
            {
                HOperatorSet.ClearWindow(BufferWindow);
                DateTime d1 = DateTime.Now;
                HObject hobj;
                HTuple Row, Col;
                HOperatorSet.ReadImage(out hobj, @"C:\Users\Public\Documents\MVTec\HALCON-12.0\examples\images\autobahn\scene_00.png");
                HOperatorSet.GetImageSize(hobj, out Col, out Row);
                HOperatorSet.SetPart(BufferWindow, 0, 0, Row - 1, Col - 1);


                //HOperatorSet.SetWindowExtents(BufferWindow, row, column, width, height);
                //////HOperatorSet.CloseWindow(BufferWindow);
                //HTuple row = 0, column = 0, width, height;
                //////hWindow1.Dispatcher.Invoke(new Action(() => {
                //////    width = hWindow1.ActualWidth;
                //////    height = hWindow1.ActualHeight;
                //////}));

                HTuple row, column, width, height;
                HOperatorSet.GetWindowExtents(hWindow1.HalconWindow, out row, out column, out width, out height);
                HOperatorSet.SetWindowExtents(BufferWindow, row, column, width, height);


                HOperatorSet.DispObj(hobj, BufferWindow);
                hobj.Dispose();
                Random ran = new Random();
                HOperatorSet.GenCircleContourXld(out hobj, 200, 200, 100, 0, 6.28318, "positive", 1);
                for (int i = 0; i < 5000; i++)
                {
                    HOperatorSet.DispObj(hobj, BufferWindow);
                    HOperatorSet.WriteString(BufferWindow, "测试数据");
                }
                hobj.Dispose();

                
                HOperatorSet.DumpWindowImage(out hobj, BufferWindow);
                HOperatorSet.GetImageSize(hobj, out Col, out Row);
                HOperatorSet.SetPart(hWindow2.HalconWindow, 0, 0, Row - 1, Col - 1);
                HOperatorSet.DispObj(hobj, hWindow2.HalconWindow);

                Log2.Dispatcher.BeginInvoke(new Action(() => {
                    Log2.Text = "耗时" + DateTime.Now.Subtract(d1).TotalMilliseconds + "ms";
                }));
            }))
            { IsBackground = true }.Start();
        }


        private void Window_ContentRendered(object sender, EventArgs e)
        {
            HOperatorSet.OpenWindow(0,0,1920,1080,0,"buffer","",out BufferWindow);

        }
    }
}
