using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using System.IO.Ports;
using System.IO;



namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection webcam;
        private VideoCaptureDevice cam;
        int R;
        int G;
        int B;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cam = new VideoCaptureDevice(webcam[comboBox1.SelectedIndex].MonikerString);
            cam.NewFrame += cam_NewFrame;
            cam.DesiredFrameRate = 40;
            cam.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox2.DataSource = SerialPort.GetPortNames();
            webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo cihaz in webcam)
            {
                comboBox1.Items.Add(cihaz.Name);
            }
            cam = new VideoCaptureDevice();
        }
        private void cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap image = (Bitmap)eventArgs.Frame.Clone();
            Bitmap image2 = (Bitmap)eventArgs.Frame.Clone();
            image = new Mirror(false, true).Apply(image);
            pictureBox1.Image = image;

            byte r, g, b;
            r = Convert.ToByte(R);
            g = Convert.ToByte(G);
            b = Convert.ToByte(B);
            EuclideanColorFiltering filtre = new EuclideanColorFiltering();
            filtre.CenterColor = new RGB(r,g,b);
            filtre.Radius = 100;
            filtre.ApplyInPlace(image2);
            image2 = new Mirror(false, true).Apply(image2);
            BlobCounter bc = new BlobCounter();
            bc.FilterBlobs = true;
            bc.MinHeight = 25;
            bc.MinWidth = 25;
            bc.ProcessImage(image2);
            Rectangle[] rects = bc.GetObjectsRectangles();
            foreach (Rectangle rect in rects)
            {
                int x, y;
                y = image2.Size.Height / 3;
                x = image2.Size.Width / 3;
                if (rect.X < x && rect.Y < y)
                {
                    serialPort1.Write("1");
                }
                else if (rect.X < 2 * x && rect.Y < y)
                {
                    serialPort1.Write("2");
                }
                else if (rect.X < 3 * x && rect.Y < y)
                {
                    serialPort1.Write("3");
                }
                else if (rect.X < x && rect.Y < 2 * y)
                {
                    serialPort1.Write("4");
                }
                else if (rect.X < 2 * x && rect.Y < 2 * y)
                {
                    serialPort1.Write("5");
                }
                else if (rect.X < 3 * x && rect.Y < 2 * y)
                {
                    serialPort1.Write("6");
                }
                else if (rect.X < x && rect.Y < 3 * y)
                {
                    serialPort1.Write("7");
                }
                else if (rect.X < 2 * x && rect.Y < 3 * y)
                {
                    serialPort1.Write("8");
                }
                else if (rect.X < 3 * x && rect.Y < 3 * y)
                {
                    serialPort1.Write("9");
                }


            }

            pictureBox2.Image = image2;

        }

    

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            R = trackBar1.Value;
            textBox1.Text = trackBar1.Value.ToString();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            G = trackBar2.Value;
            textBox2.Text = trackBar2.Value.ToString();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            B = trackBar3.Value;
            textBox3.Text = trackBar3.Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.BaudRate = 9600;
            serialPort1.PortName = comboBox2.SelectedItem.ToString();
            serialPort1.Open();
        }
    }
    
}
