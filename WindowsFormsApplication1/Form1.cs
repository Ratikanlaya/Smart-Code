using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        SerialPort serial1 = null;
        SerialPort serial2 = null;
        private bool isConnect = false;
        private string[] data;
        private string[] infor;
        private int dataSum;
        private int inforSum;
        private int dataAverage;
        private int inforAverage;
        private Stopwatch stopWatch = new Stopwatch();
        private string raw = string.Empty;
        private string raw2 = string.Empty;
       
        public Form1()
        {
            InitializeComponent();
            serialRecieveTextBox1.Visible = false;
            serialRecieveTextBox2.Visible = false;

            try
            {
                comportComboBox1.Items.Clear();
                comportComboBox2.Items.Clear();

                foreach (String ser in SerialPort.GetPortNames())
                {
                    comportComboBox1.Items.Add(ser);
                    

                }
                comportComboBox1.SelectedIndex = 0;

                foreach (String ser2 in SerialPort.GetPortNames())
                {
                    comportComboBox2.Items.Add(ser2);
                   

                }
                comportComboBox2.SelectedIndex = 0;

            }
            catch (Exception)
            {
                MessageBox.Show("1");
            }
        }

        public struct Clock
        {
            public int sec;
            public int min;
            public int hour;
            //public int millisec;
        }

        Clock total;

        private void connectButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (isConnect == false)
                {
                    serial1 = new SerialPort(comportComboBox1.SelectedItem.ToString(), 115200, Parity.None, 8, StopBits.One);
                    serial2 = new SerialPort(comportComboBox2.SelectedItem.ToString(), 115200, Parity.None, 8, StopBits.One);

                    if (serial1 != null && serial2 != null)
                    {
                        serial1.Open();
                        serial2.Open();
                        isConnect = true;
                        connectButton.Text = "Disconnect";
                       // comportInfoLabel.Visible = true;
                        comportComboBox1.Enabled = false;
                        comportComboBox2.Enabled = false;
                        resetButton.Enabled = false;
                        saveButton.Enabled = true;
                       
                        serial1.DataReceived += new SerialDataReceivedEventHandler(serial1_DataReceived);
                        serial2.DataReceived += new SerialDataReceivedEventHandler(serial2_DataReceived);
                        serialRecieveTextBox1.Visible = false;
                        serialRecieveTextBox2.Visible = false;
                        timer1.Start();

                    }
                }
                else
                {
                    if (isConnect)
                    {
                        serial1.Close();
                        serial2.Close();
                        isConnect = false;
                        connectButton.Text = "Connect";
                        
                   
                        comportComboBox1.Enabled = true;
                        comportComboBox2.Enabled = true;
                        resetButton.Enabled = true;
                        saveButton.Enabled = true;

                        
                       // serial.BaudRate = int.Parse(baudRateComboBox.Text);
                        serialRecieveTextBox1.Visible = false;
                        serialRecieveTextBox2.Visible = false;
                        timer1.Stop();

                    }

                }

            }
            catch (Exception)
            {
                MessageBox.Show("2");
            }
        }

        private delegate void updateAnalogTextBox();
        private void updateTextBox()
        {
            toeTextBox1.Text = data[0];
            mth1TextBox1.Text = data[5];
            mth3TextBox1.Text = data[4];
            mth5TextBox1.Text = data[3];
            arch1TextBox1.Text = data[1];
            arch2TextBox1.Text = data[2];
            arch3TextBox1.Text = data[7];
            heelTextBox1.Text = data[6];
            averageTextBox1.Text = Convert.ToString(dataAverage);
            
            serialRecieveTextBox1.AppendText(raw);


        }

        private delegate void updateAnalogTextBox2();
        private void updateTextBox2()
        {
            toeTextBox2.Text = infor[0];
            mth1TextBox2.Text = infor[5];
            mth3TextBox2.Text = infor[4];
            mth5TextBox2.Text = infor[3];
            arch1TextBox2.Text = infor[1];
            arch2TextBox2.Text = infor[2];
            arch3TextBox2.Text = infor[7];
            heelTextBox2.Text = infor[6];
            averageTextBox2.Text = Convert.ToString(inforAverage);

            serialRecieveTextBox2.AppendText(raw2);


        }

        void serial1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
          
            try
            {
                string d = serial1.ReadLine();
               
                int intBegin = d.IndexOf("#");
                int intEnd = d.IndexOf("*");

                    int length = intEnd - (intBegin + 1);
                    char[] ch = { ',' };
                    data = d.Substring(intBegin + 1, length).Split(ch);
                    dataSum = Convert.ToInt16(data[0]) + Convert.ToInt16(data[1]) + Convert.ToInt16(data[2]) + Convert.ToInt16(data[3]) + Convert.ToInt16(data[4]) + Convert.ToInt16(data[5]) + Convert.ToInt16(data[6]) + Convert.ToInt16(data[7]);

                    dataAverage = dataSum;


                raw = d;
           
            }
            catch (Exception)
            {
                MessageBox.Show("3");
            }

            this.BeginInvoke(new updateAnalogTextBox(updateTextBox));
            this.BeginInvoke(new updatePixPanel1(update_pixPanel1));
            this.BeginInvoke(new updatePixPanel2(update_pixPanel2));
            this.BeginInvoke(new updatePixPanel3(update_pixPanel3));
            this.BeginInvoke(new updatePixPanel4(update_pixPanel4));
            this.BeginInvoke(new updatePixPanel5(update_pixPanel5));
            this.BeginInvoke(new updatePixPanel6(update_pixPanel6));
            this.BeginInvoke(new updatePixPanel7(update_pixPanel7));
            this.BeginInvoke(new updatePixPanel8(update_pixPanel8));

      
        }

        void serial2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
            try
            {
                
                string t = serial2.ReadLine();

           
                int intBegin2 = t.IndexOf("a");
                int intEnd2 = t.IndexOf("b");
                

                    int length2 = intEnd2 - (intBegin2 + 1);
                    char[] ch2 = { '-' };
                    infor = t.Substring(intBegin2 + 1, length2).Split(ch2);
                    inforSum = Convert.ToInt16(infor[0]) + Convert.ToInt16(infor[1]) + Convert.ToInt16(infor[2]) + Convert.ToInt16(infor[3]) + Convert.ToInt16(infor[4]) + Convert.ToInt16(infor[5]) + Convert.ToInt16(infor[6]) + Convert.ToInt16(infor[7]);
                    inforAverage = inforSum;
               


                raw2 = t;
                
            }
            catch (Exception)
            {
                MessageBox.Show("4");
            }

            this.BeginInvoke(new updateAnalogTextBox2(updateTextBox2));
         
            this.BeginInvoke(new updatePixPanel9(update_pixPanel9));
            this.BeginInvoke(new updatePixPanel10(update_pixPanel10));
            this.BeginInvoke(new updatePixPanel11(update_pixPanel11));
            this.BeginInvoke(new updatePixPanel12(update_pixPanel12));
            this.BeginInvoke(new updatePixPanel13(update_pixPanel13));
            this.BeginInvoke(new updatePixPanel14(update_pixPanel14));
            this.BeginInvoke(new updatePixPanel15(update_pixPanel15));
            this.BeginInvoke(new updatePixPanel16(update_pixPanel16));


        }



        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serial1 != null && serial2 != null)
            {
                if (serial1.IsOpen && serial2.IsOpen)
                {
                    serial1.Close();
                    serial2.Close();
                }
            }
        }

       private void panel1_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            SolidBrush sbr = new SolidBrush(Color.White);
            Rectangle rec = new Rectangle(0, 0, panel1.Width, panel1.Height);

            g.FillRectangle(sbr, rec);

            Pen myPen1 = new Pen(Color.LightGray, 2);
            myPen1.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            Pen myPen2 = new Pen(Color.Black, 4);

            SolidBrush sbr2 = new SolidBrush(Color.FromArgb(0, 35, 75));
            Point[] points = new Point[] {
                                new Point (160,640),
                                new Point (110,620),                               
                                new Point (86,540),
                                new Point (50,260),
                                new Point (58,160),
                                new Point (64,140),
                                new Point (70,120),
                                new Point (78,100),
                                new Point (84,80),
                                new Point (96,60),
                                new Point (120,40),
                                new Point (140,30),
                                new Point (180,20),
                                new Point (200,20),
                                new Point (220,30),
                                new Point (230,40),
                                new Point (240,60),
                                new Point (250,80),
                                new Point (270,160),
                                new Point (270,200),
                                new Point (260,280),
                                new Point (242,360),
                                new Point (240,560),
                                new Point (238,580),
                                new Point (234,600),
                                new Point (220,620),
                                new Point (200,630),
                                new Point (160,640)

                             
                                };

            g.DrawLines(myPen1, points);
            g.DrawCurve(myPen2, points);
            g.FillClosedCurve(sbr2, points);

            //this.BackColor = Color.White;

            Pen myPen3 = new Pen(Color.LightGray, 2);
            myPen1.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            Pen myPen4 = new Pen(Color.Black, 4);
            SolidBrush sbr3 = new SolidBrush(Color.FromArgb(0, 35, 75));
            Point[] points2 = new Point[] {
                                new Point (440,640),
                                new Point (490,620),                               
                                new Point (514,540),
                                new Point (550,260),
                                new Point (542,160),
                                new Point (536,140),
                                new Point (530,120),
                                new Point (522,100),
                                new Point (516,80),
                                new Point (504,60),
                                new Point (480,40),
                                new Point (460,30),
                                new Point (420,20),
                                new Point (400,20),
                                new Point (380,30),
                                new Point (370,40),
                                new Point (360,60),
                                new Point (350,80),
                                new Point (330,160),
                                new Point (330,200),
                                new Point (340,280),
                                new Point (358,360),
                                new Point (360,560),
                                new Point (362,580),
                                new Point (366,600),
                                new Point (380,620),
                                new Point (400,630),
                                new Point (440,640)                              
                                
                                };

            g.DrawLines(myPen3, points2);
            g.DrawCurve(myPen4, points2);
            g.FillClosedCurve(sbr3, points2);

            //this.BackColor = Color.White;
        }

       private void pnColor_Paint(object sender, PaintEventArgs e)
       {

           Graphics myGraphics = pnColor.CreateGraphics();
           Rectangle rec = new Rectangle(0, 0, 30, 15);
           SolidBrush sb = new SolidBrush(Color.White);

           sb.Color = Color.FromArgb(255, 0, 0); myGraphics.FillRectangle(sb, rec);
           sb.Color = Color.FromArgb(255, 145, 0); rec.Y = 15; myGraphics.FillRectangle(sb, rec);
           sb.Color = Color.FromArgb(255, 200, 0); rec.Y = 30; myGraphics.FillRectangle(sb, rec);
           sb.Color = Color.FromArgb(255, 255, 0); rec.Y = 45; myGraphics.FillRectangle(sb, rec);
           sb.Color = Color.FromArgb(200, 255, 100); rec.Y = 60; myGraphics.FillRectangle(sb, rec);
           sb.Color = Color.FromArgb(50, 255, 150); rec.Y = 75; myGraphics.FillRectangle(sb, rec);
           sb.Color = Color.FromArgb(50, 205, 205); rec.Y = 90; myGraphics.FillRectangle(sb, rec);
           sb.Color = Color.FromArgb(0, 100, 205); rec.Y = 105; myGraphics.FillRectangle(sb, rec);
           sb.Color = Color.FromArgb(0, 50, 150); rec.Y = 120; myGraphics.FillRectangle(sb, rec);
           sb.Color = Color.FromArgb(0, 35, 75); rec.Y = 135; myGraphics.FillRectangle(sb, rec);
           //sb.Color = Color.FromArgb(255, 255, 255); rec.Y = 150; myGraphics.FillRectangle(sb, rec);
       }
        private delegate void updatePixPanel1();
        private void update_pixPanel1()
        {
            if (Convert.ToDouble(data[0]) <= 50)
            {
                Graphics c = pixPanel1.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);

            }
            else if (50 < Convert.ToDouble(data[0]) && Convert.ToDouble(data[0]) <= 100)
            {
                Graphics c = pixPanel1.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 10, 10, 40, 40);

            }
            else if (100 < Convert.ToDouble(data[0]) && Convert.ToDouble(data[0]) <= 200)
            {
                Graphics c = pixPanel1.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 6, 6, 48, 48);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 14, 14, 32, 32);

            }
            else if (200 < Convert.ToDouble(data[0]) && Convert.ToDouble(data[0]) <= 500)
            {
                Graphics c = pixPanel1.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 18, 18, 24, 24);

            }
            else if (500 < Convert.ToDouble(data[0]) && Convert.ToDouble(data[0]) <= 700)
            {
                Graphics c = pixPanel1.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 16, 16, 28, 28);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 22, 22, 16, 16);

            }
            else if (700 < Convert.ToDouble(data[0]) && Convert.ToDouble(data[0]) <= 900)
            {
                Graphics c = pixPanel1.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 16, 16, 28, 28);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 22, 22, 16, 16);

            }
            else if (900 < Convert.ToDouble(data[0]) && Convert.ToDouble(data[0]) <= 1100)
            {
                Graphics c = pixPanel1.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 14, 14, 32, 32);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 18, 18, 24, 24);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 22, 22, 16, 16);

            }
            else if (1100 < Convert.ToDouble(data[0]) && Convert.ToDouble(data[0]) <= 1300)
            {
                Graphics c = pixPanel1.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 8, 8, 44, 44);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 12, 12, 36, 36);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 16, 16, 28, 28);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 20, 20, 20, 20);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 24, 24, 12, 12);

            }
            else if (1300 < Convert.ToDouble(data[0]) && Convert.ToDouble(data[0]) <= 1400)
            {
                Graphics c = pixPanel1.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);

            }
            else if (Convert.ToDouble(data[0]) > 1400)
            {
                Graphics c = pixPanel1.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);
                SolidBrush Brush10 = new SolidBrush(Color.FromArgb(255, 0, 0));     // Red
                c.FillEllipse(Brush10, 26, 26, 8, 8);
            }

        }

        private delegate void updatePixPanel2();
        private void update_pixPanel2()
        {
            if (Convert.ToDouble(data[3]) <= 50)
            {
                Graphics c = pixPanel2.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);

            }
            else if (50 < Convert.ToDouble(data[3]) && Convert.ToDouble(data[3]) <= 100)
            {
                Graphics c = pixPanel2.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 10, 10, 40, 40);

            }
            else if (100 < Convert.ToDouble(data[3]) && Convert.ToDouble(data[3]) <= 200)
            {
                Graphics c = pixPanel2.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 6, 6, 48, 48);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 14, 14, 32, 32);

            }
            else if (200 < Convert.ToDouble(data[3]) && Convert.ToDouble(data[3]) <= 500)
            {
                Graphics c = pixPanel2.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 18, 18, 24, 24);

            }
            else if (500 < Convert.ToDouble(data[3]) && Convert.ToDouble(data[3]) <= 700)
            {
                Graphics c = pixPanel2.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 16, 16, 28, 28);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 22, 22, 16, 16);

            }
            else if (700 < Convert.ToDouble(data[3]) && Convert.ToDouble(data[3]) <= 900)
            {
                Graphics c = pixPanel2.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 16, 16, 28, 28);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 22, 22, 16, 16);

            }
            else if (900 < Convert.ToDouble(data[3]) && Convert.ToDouble(data[3]) <= 1100)
            {
                Graphics c = pixPanel2.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 14, 14, 32, 32);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 18, 18, 24, 24);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 22, 22, 16, 16);

            }
            else if (1100 < Convert.ToDouble(data[3]) && Convert.ToDouble(data[3]) <= 1300)
            {
                Graphics c = pixPanel2.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 8, 8, 44, 44);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 12, 12, 36, 36);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 16, 16, 28, 28);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 20, 20, 20, 20);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 24, 24, 12, 12);

            }
            else if (1300 < Convert.ToDouble(data[3]) && Convert.ToDouble(data[3]) <= 1400)
            {
                Graphics c = pixPanel2.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);

            }
            else if (Convert.ToDouble(data[3]) > 1400)
            {
                Graphics c = pixPanel2.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);
                SolidBrush Brush10 = new SolidBrush(Color.FromArgb(255, 0, 0));     // Red
                c.FillEllipse(Brush10, 26, 26, 8, 8);
            }

        }

        private delegate void updatePixPanel3();
        private void update_pixPanel3()
        {
            if (Convert.ToDouble(data[4]) <= 50)
            {
                Graphics c = pixPanel3.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);

            }
            else if (50 < Convert.ToDouble(data[4]) && Convert.ToDouble(data[4]) <= 100)
            {
                Graphics c = pixPanel3.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 10, 10, 40, 40);

            }
            else if (100 < Convert.ToDouble(data[4]) && Convert.ToDouble(data[4]) <= 200)
            {
                Graphics c = pixPanel3.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 6, 6, 48, 48);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 14, 14, 32, 32);

            }
            else if (200 < Convert.ToDouble(data[4]) && Convert.ToDouble(data[4]) <= 500)
            {
                Graphics c = pixPanel3.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 18, 18, 24, 24);

            }
            else if (500 < Convert.ToDouble(data[4]) && Convert.ToDouble(data[4]) <= 700)
            {
                Graphics c = pixPanel3.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 16, 16, 28, 28);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 22, 22, 16, 16);

            }
            else if (700 < Convert.ToDouble(data[4]) && Convert.ToDouble(data[4]) <= 900)
            {
                Graphics c = pixPanel3.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 16, 16, 28, 28);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 22, 22, 16, 16);

            }
            else if (900 < Convert.ToDouble(data[4]) && Convert.ToDouble(data[4]) <= 1100)
            {
                Graphics c = pixPanel3.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 14, 14, 32, 32);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 18, 18, 24, 24);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 22, 22, 16, 16);

            }
            else if (1100 < Convert.ToDouble(data[4]) && Convert.ToDouble(data[4]) <= 1300)
            {
                Graphics c = pixPanel3.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 8, 8, 44, 44);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 12, 12, 36, 36);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 16, 16, 28, 28);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 20, 20, 20, 20);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 24, 24, 12, 12);

            }
            else if (1300 < Convert.ToDouble(data[4]) && Convert.ToDouble(data[4]) <= 1400)
            {
                Graphics c = pixPanel3.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);

            }
            else if (Convert.ToDouble(data[4]) > 1400)
            {
                Graphics c = pixPanel3.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);
                SolidBrush Brush10 = new SolidBrush(Color.FromArgb(255, 0, 0));     // Red
                c.FillEllipse(Brush10, 26, 26, 8, 8);
            }

        }

        private delegate void updatePixPanel4();
        private void update_pixPanel4()
        {
            if (Convert.ToDouble(data[5]) <= 150)
            {
                Graphics c = pixPanel4.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);

            }
            else if (150 < Convert.ToDouble(data[5]) && Convert.ToDouble(data[5]) <= 200)
            {
                Graphics c = pixPanel4.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 10, 10, 40, 40);

            }
            else if (200 < Convert.ToDouble(data[5]) && Convert.ToDouble(data[5]) <= 300)
            {
                Graphics c = pixPanel4.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 6, 6, 48, 48);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 14, 14, 32, 32);

            }
            else if (300 < Convert.ToDouble(data[5]) && Convert.ToDouble(data[5]) <= 500)
            {
                Graphics c = pixPanel4.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 18, 18, 24, 24);

            }
            else if (500 < Convert.ToDouble(data[5]) && Convert.ToDouble(data[5]) <= 700)
            {
                Graphics c = pixPanel4.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 16, 16, 28, 28);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 22, 22, 16, 16);

            }
            else if (700 < Convert.ToDouble(data[5]) && Convert.ToDouble(data[5]) <= 900)
            {
                Graphics c = pixPanel4.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 16, 16, 28, 28);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 22, 22, 16, 16);

            }
            else if (900 < Convert.ToDouble(data[5]) && Convert.ToDouble(data[5]) <= 1100)
            {
                Graphics c = pixPanel4.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 14, 14, 32, 32);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 18, 18, 24, 24);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 22, 22, 16, 16);

            }
            else if (1100 < Convert.ToDouble(data[5]) && Convert.ToDouble(data[5]) <= 1300)
            {
                Graphics c = pixPanel4.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 8, 8, 44, 44);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 12, 12, 36, 36);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 16, 16, 28, 28);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 20, 20, 20, 20);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 24, 24, 12, 12);

            }
            else if (1300 < Convert.ToDouble(data[5]) && Convert.ToDouble(data[5]) <= 1400)
            {
                Graphics c = pixPanel4.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);

            }
            else if (Convert.ToDouble(data[5]) > 1400)
            {
                Graphics c = pixPanel4.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);
                SolidBrush Brush10 = new SolidBrush(Color.FromArgb(255, 0, 0));     // Red
                c.FillEllipse(Brush10, 26, 26, 8, 8);
            }

        }

        private delegate void updatePixPanel5();
        private void update_pixPanel5()
        {
            if (Convert.ToDouble(data[1]) <= 50)
            {
                Graphics c = pixPanel5.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);

            }
            else if (50 < Convert.ToDouble(data[1]) && Convert.ToDouble(data[1]) <= 100)
            {
                Graphics c = pixPanel5.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 10, 10, 40, 40);

            }
            else if (100 < Convert.ToDouble(data[1]) && Convert.ToDouble(data[1]) <= 200)
            {
                Graphics c = pixPanel5.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 18, 18, 24, 24);

            }
            else if (200 < Convert.ToDouble(data[1]) && Convert.ToDouble(data[1]) <= 500)
            {
                Graphics c = pixPanel5.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 18, 18, 24, 24);

            }
            else if (500 < Convert.ToDouble(data[1]) && Convert.ToDouble(data[1]) <= 700)
            {
                Graphics c = pixPanel5.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 16, 16, 28, 28);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 22, 22, 16, 16);

            }
            else if (700 < Convert.ToDouble(data[1]) && Convert.ToDouble(data[1]) <= 900)
            {
                Graphics c = pixPanel5.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 16, 16, 28, 28);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 22, 22, 16, 16);

            }
            else if (900 < Convert.ToDouble(data[1]) && Convert.ToDouble(data[1]) <= 1100)
            {
                Graphics c = pixPanel5.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 14, 14, 32, 32);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 18, 18, 24, 24);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 22, 22, 16, 16);

            }
            else if (1100 < Convert.ToDouble(data[1]) && Convert.ToDouble(data[1]) <= 1300)
            {
                Graphics c = pixPanel5.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 8, 8, 44, 44);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 12, 12, 36, 36);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 16, 16, 28, 28);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 20, 20, 20, 20);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 24, 24, 12, 12);

            }
            else if (1300 < Convert.ToDouble(data[1]) && Convert.ToDouble(data[1]) <= 1400)
            {
                Graphics c = pixPanel5.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);

            }
            else if (Convert.ToDouble(data[1]) > 1400)
            {
                Graphics c = pixPanel5.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);
                SolidBrush Brush10 = new SolidBrush(Color.FromArgb(255, 0, 0));     // Red
                c.FillEllipse(Brush10, 26, 26, 8, 8);
            }

        }

        private delegate void updatePixPanel6();
        private void update_pixPanel6()
        {
            if (Convert.ToDouble(data[2]) <= 50)
            {
                Graphics c = pixPanel6.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);

            }
            else if (50 < Convert.ToDouble(data[2]) && Convert.ToDouble(data[2]) <= 100)
            {
                Graphics c = pixPanel6.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 10, 10, 40, 40);

            }
            else if (100 < Convert.ToDouble(data[2]) && Convert.ToDouble(data[2]) <= 200)
            {
                Graphics c = pixPanel6.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 6, 6, 48, 48);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 14, 14, 32, 32);

            }
            else if (200 < Convert.ToDouble(data[2]) && Convert.ToDouble(data[2]) <= 500)
            {
                Graphics c = pixPanel6.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 18, 18, 24, 24);

            }
            else if (500 < Convert.ToDouble(data[2]) && Convert.ToDouble(data[2]) <= 700)
            {
                Graphics c = pixPanel6.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 16, 16, 28, 28);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 22, 22, 16, 16);

            }
            else if (700 < Convert.ToDouble(data[2]) && Convert.ToDouble(data[2]) <= 900)
            {
                Graphics c = pixPanel6.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 16, 16, 28, 28);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 22, 22, 16, 16);

            }
            else if (900 < Convert.ToDouble(data[2]) && Convert.ToDouble(data[2]) <= 1100)
            {
                Graphics c = pixPanel6.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 14, 14, 32, 32);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 18, 18, 24, 24);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 22, 22, 16, 16);

            }
            else if (1100 < Convert.ToDouble(data[2]) && Convert.ToDouble(data[2]) <= 1300)
            {
                Graphics c = pixPanel6.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 8, 8, 44, 44);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 12, 12, 36, 36);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 16, 16, 28, 28);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 20, 20, 20, 20);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 24, 24, 12, 12);

            }
            else if (1300 < Convert.ToDouble(data[2]) && Convert.ToDouble(data[2]) <= 1400)
            {
                Graphics c = pixPanel6.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);

            }
            else if (Convert.ToDouble(data[2]) > 1400)
            {
                Graphics c = pixPanel6.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);
                SolidBrush Brush10 = new SolidBrush(Color.FromArgb(255, 0, 0));     // Red
                c.FillEllipse(Brush10, 26, 26, 8, 8);
            }

        }

        private delegate void updatePixPanel7();
        private void update_pixPanel7()
        {
            if (Convert.ToDouble(data[7]) <= 50)
            {
                Graphics c = pixPanel7.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);

            }
            else if (50 < Convert.ToDouble(data[7]) && Convert.ToDouble(data[7]) <= 100)
            {
                Graphics c = pixPanel7.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 10, 10, 40, 40);

            }
            else if (100 < Convert.ToDouble(data[7]) && Convert.ToDouble(data[7]) <= 200)
            {
                Graphics c = pixPanel7.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 6, 6, 48, 48);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 14, 14, 32, 32);

            }
            else if (200 < Convert.ToDouble(data[7]) && Convert.ToDouble(data[7]) <= 500)
            {
                Graphics c = pixPanel7.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 18, 18, 24, 24);

            }
            else if (500 < Convert.ToDouble(data[7]) && Convert.ToDouble(data[7]) <= 700)
            {
                Graphics c = pixPanel7.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 16, 16, 28, 28);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 22, 22, 16, 16);

            }
            else if (700 < Convert.ToDouble(data[7]) && Convert.ToDouble(data[7]) <= 900)
            {
                Graphics c = pixPanel7.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 16, 16, 28, 28);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 22, 22, 16, 16);

            }
            else if (900 < Convert.ToDouble(data[7]) && Convert.ToDouble(data[7]) <= 1100)
            {
                Graphics c = pixPanel7.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 14, 14, 32, 32);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 18, 18, 24, 24);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 22, 22, 16, 16);

            }
            else if (1100 < Convert.ToDouble(data[7]) && Convert.ToDouble(data[7]) <= 1300)
            {
                Graphics c = pixPanel7.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 8, 8, 44, 44);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 12, 12, 36, 36);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 16, 16, 28, 28);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 20, 20, 20, 20);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 24, 24, 12, 12);

            }
            else if (1300 < Convert.ToDouble(data[7]) && Convert.ToDouble(data[7]) <= 1400)
            {
                Graphics c = pixPanel7.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);

            }
            else if (Convert.ToDouble(data[7]) > 1400)
            {
                Graphics c = pixPanel7.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);
                SolidBrush Brush10 = new SolidBrush(Color.FromArgb(255, 0, 0));     // Red
                c.FillEllipse(Brush10, 26, 26, 8, 8);
            }

        }


        private delegate void updatePixPanel8();
        private void update_pixPanel8()
        {
            if (Convert.ToDouble(data[6]) <= 50)
            {
                Graphics c = pixPanel8.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);

            }
            else if (50 < Convert.ToDouble(data[6]) && Convert.ToDouble(data[6]) <= 100)
            {
                Graphics c = pixPanel8.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 10, 10, 40, 40);

            }
            else if (100 < Convert.ToDouble(data[6]) && Convert.ToDouble(data[6]) <= 200)
            {
                Graphics c = pixPanel8.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 6, 6, 48, 48);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 14, 14, 32, 32);

            }
            else if (200 < Convert.ToDouble(data[6]) && Convert.ToDouble(data[6]) <= 500)
            {
                Graphics c = pixPanel8.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 18, 18, 24, 24);

            }
            else if (500 < Convert.ToDouble(data[6]) && Convert.ToDouble(data[6]) <= 700)
            {
                Graphics c = pixPanel8.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 16, 16, 28, 28);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 22, 22, 16, 16);

            }
            else if (700 < Convert.ToDouble(data[6]) && Convert.ToDouble(data[6]) <= 900)
            {
                Graphics c = pixPanel8.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 16, 16, 28, 28);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 22, 22, 16, 16);

            }
            else if (900 < Convert.ToDouble(data[6]) && Convert.ToDouble(data[6]) <= 1100)
            {
                Graphics c = pixPanel8.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 14, 14, 32, 32);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 18, 18, 24, 24);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 22, 22, 16, 16);

            }
            else if (1100 < Convert.ToDouble(data[6]) && Convert.ToDouble(data[6]) <= 1300)
            {
                Graphics c = pixPanel8.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 8, 8, 44, 44);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 12, 12, 36, 36);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 16, 16, 28, 28);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 20, 20, 20, 20);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 24, 24, 12, 12);

            }
            else if (1300 < Convert.ToDouble(data[6]) && Convert.ToDouble(data[6]) <= 1400)
            {
                Graphics c = pixPanel8.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);

            }
            else if (Convert.ToDouble(data[6]) > 1400)
            {
                Graphics c = pixPanel8.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);
                SolidBrush Brush10 = new SolidBrush(Color.FromArgb(255, 0, 0));     // Red
                c.FillEllipse(Brush10, 26, 26, 8, 8);
            }

        }


        private delegate void updatePixPanel9();
        private void update_pixPanel9()
        {
            if (Convert.ToDouble(infor[0]) <= 50)
            {
                Graphics c = pixPanel9.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);

            }
            else if (50 < Convert.ToDouble(infor[0]) && Convert.ToDouble(infor[0]) <= 100)
            {
                Graphics c = pixPanel9.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 10, 10, 40, 40);

            }
            else if (100 < Convert.ToDouble(infor[0]) && Convert.ToDouble(infor[0]) <= 200)
            {
                Graphics c = pixPanel9.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 6, 6, 48, 48);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 14, 14, 32, 32);

            }
            else if (200 < Convert.ToDouble(infor[0]) && Convert.ToDouble(infor[0]) <= 500)
            {
                Graphics c = pixPanel9.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 18, 18, 24, 24);

            }
            else if (500 < Convert.ToDouble(infor[0]) && Convert.ToDouble(infor[0]) <= 700)
            {
                Graphics c = pixPanel9.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 16, 16, 28, 28);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 22, 22, 16, 16);

            }
            else if (700 < Convert.ToDouble(infor[0]) && Convert.ToDouble(infor[0]) <= 900)
            {
                Graphics c = pixPanel9.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 16, 16, 28, 28);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 22, 22, 16, 16);

            }
            else if (900 < Convert.ToDouble(infor[0]) && Convert.ToDouble(infor[0]) <= 1100)
            {
                Graphics c = pixPanel9.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 14, 14, 32, 32);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 18, 18, 24, 24);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 22, 22, 16, 16);

            }
            else if (1100 < Convert.ToDouble(infor[0]) && Convert.ToDouble(infor[0]) <= 1300)
            {
                Graphics c = pixPanel9.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 8, 8, 44, 44);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 12, 12, 36, 36);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 16, 16, 28, 28);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 20, 20, 20, 20);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 24, 24, 12, 12);

            }
            else if (1300 < Convert.ToDouble(infor[0]) && Convert.ToDouble(infor[0]) <= 1400)
            {
                Graphics c = pixPanel9.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);

            }
            else if (Convert.ToDouble(infor[0]) > 1400)
            {
                Graphics c = pixPanel9.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);
                SolidBrush Brush10 = new SolidBrush(Color.FromArgb(255, 0, 0));     // Red
                c.FillEllipse(Brush10, 26, 26, 8, 8);
            }

        }

        private delegate void updatePixPanel10();
        private void update_pixPanel10()
        {
            if (Convert.ToDouble(infor[5]) <= 50)
            {
                Graphics c = pixPanel10.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);

            }
            else if (50 < Convert.ToDouble(infor[5]) && Convert.ToDouble(infor[5]) <= 100)
            {
                Graphics c = pixPanel10.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 10, 10, 40, 40);

            }
            else if (100 < Convert.ToDouble(infor[5]) && Convert.ToDouble(infor[5]) <= 200)
            {
                Graphics c = pixPanel10.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 6, 6, 48, 48);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 14, 14, 32, 32);

            }
            else if (200 < Convert.ToDouble(infor[5]) && Convert.ToDouble(infor[5]) <= 500)
            {
                Graphics c = pixPanel10.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 18, 18, 24, 24);

            }
            else if (500 < Convert.ToDouble(infor[5]) && Convert.ToDouble(infor[5]) <= 700)
            {
                Graphics c = pixPanel10.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 16, 16, 28, 28);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 22, 22, 16, 16);

            }
            else if (700 < Convert.ToDouble(infor[5]) && Convert.ToDouble(infor[5]) <= 900)
            {
                Graphics c = pixPanel10.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 16, 16, 28, 28);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 22, 22, 16, 16);

            }
            else if (900 < Convert.ToDouble(infor[5]) && Convert.ToDouble(infor[5]) <= 1100)
            {
                Graphics c = pixPanel10.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 14, 14, 32, 32);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 18, 18, 24, 24);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 22, 22, 16, 16);

            }
            else if (1100 < Convert.ToDouble(infor[5]) && Convert.ToDouble(infor[5]) <= 1300)
            {
                Graphics c = pixPanel10.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 8, 8, 44, 44);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 12, 12, 36, 36);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 16, 16, 28, 28);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 20, 20, 20, 20);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 24, 24, 12, 12);

            }
            else if (1300 < Convert.ToDouble(infor[5]) && Convert.ToDouble(infor[5]) <= 1400)
            {
                Graphics c = pixPanel10.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);

            }
            else if (Convert.ToDouble(infor[5]) > 1400)
            {
                Graphics c = pixPanel10.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);
                SolidBrush Brush10 = new SolidBrush(Color.FromArgb(255, 0, 0));     // Red
                c.FillEllipse(Brush10, 26, 26, 8, 8);
            }

        }

        private delegate void updatePixPanel11();
        private void update_pixPanel11()
        {
            if (Convert.ToDouble(infor[4]) <= 50)
            {
                Graphics c = pixPanel11.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);

            }
            else if (50 < Convert.ToDouble(infor[4]) && Convert.ToDouble(infor[4]) <= 100)
            {
                Graphics c = pixPanel11.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 10, 10, 40, 40);

            }
            else if (100 < Convert.ToDouble(infor[4]) && Convert.ToDouble(infor[4]) <= 200)
            {
                Graphics c = pixPanel11.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 6, 6, 48, 48);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 14, 14, 32, 32);

            }
            else if (200 < Convert.ToDouble(infor[4]) && Convert.ToDouble(infor[4]) <= 500)
            {
                Graphics c = pixPanel11.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 18, 18, 24, 24);

            }
            else if (500 < Convert.ToDouble(infor[4]) && Convert.ToDouble(infor[4]) <= 700)
            {
                Graphics c = pixPanel11.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 16, 16, 28, 28);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 22, 22, 16, 16);

            }
            else if (700 < Convert.ToDouble(infor[4]) && Convert.ToDouble(infor[4]) <= 900)
            {
                Graphics c = pixPanel11.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 16, 16, 28, 28);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 22, 22, 16, 16);

            }
            else if (900 < Convert.ToDouble(infor[4]) && Convert.ToDouble(infor[4]) <= 1100)
            {
                Graphics c = pixPanel11.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 14, 14, 32, 32);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 18, 18, 24, 24);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 22, 22, 16, 16);

            }
            else if (1100 < Convert.ToDouble(infor[4]) && Convert.ToDouble(infor[4]) <= 1300)
            {
                Graphics c = pixPanel11.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 8, 8, 44, 44);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 12, 12, 36, 36);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 16, 16, 28, 28);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 20, 20, 20, 20);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 24, 24, 12, 12);

            }
            else if (1300 < Convert.ToDouble(infor[4]) && Convert.ToDouble(infor[4]) <= 1400)
            {
                Graphics c = pixPanel11.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);

            }
            else if (Convert.ToDouble(infor[4]) > 1400)
            {
                Graphics c = pixPanel11.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);
                SolidBrush Brush10 = new SolidBrush(Color.FromArgb(255, 0, 0));     // Red
                c.FillEllipse(Brush10, 26, 26, 8, 8);
            }

        }

        private delegate void updatePixPanel12();
        private void update_pixPanel12()
        {
            if (Convert.ToDouble(infor[3]) <= 50)
            {
                Graphics c = pixPanel12.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);

            }
            else if (50 < Convert.ToDouble(infor[3]) && Convert.ToDouble(infor[3]) <= 100)
            {
                Graphics c = pixPanel12.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 10, 10, 40, 40);

            }
            else if (100 < Convert.ToDouble(infor[3]) && Convert.ToDouble(infor[3]) <= 200)
            {
                Graphics c = pixPanel12.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 6, 6, 48, 48);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 14, 14, 32, 32);

            }
            else if (200 < Convert.ToDouble(infor[3]) && Convert.ToDouble(infor[3]) <= 500)
            {
                Graphics c = pixPanel12.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 18, 18, 24, 24);

            }
            else if (500 < Convert.ToDouble(infor[3]) && Convert.ToDouble(infor[3]) <= 700)
            {
                Graphics c = pixPanel12.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 16, 16, 28, 28);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 22, 22, 16, 16);

            }
            else if (700 < Convert.ToDouble(infor[3]) && Convert.ToDouble(infor[3]) <= 900)
            {
                Graphics c = pixPanel12.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 16, 16, 28, 28);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 22, 22, 16, 16);

            }
            else if (900 < Convert.ToDouble(infor[3]) && Convert.ToDouble(infor[3]) <= 1100)
            {
                Graphics c = pixPanel12.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 14, 14, 32, 32);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 18, 18, 24, 24);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 22, 22, 16, 16);

            }
            else if (1100 < Convert.ToDouble(infor[3]) && Convert.ToDouble(infor[3]) <= 1300)
            {
                Graphics c = pixPanel12.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 8, 8, 44, 44);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 12, 12, 36, 36);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 16, 16, 28, 28);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 20, 20, 20, 20);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 24, 24, 12, 12);

            }
            else if (1300 < Convert.ToDouble(infor[3]) && Convert.ToDouble(infor[3]) <= 1400)
            {
                Graphics c = pixPanel12.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);

            }
            else if (Convert.ToDouble(infor[3]) > 1400)
            {
                Graphics c = pixPanel12.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);
                SolidBrush Brush10 = new SolidBrush(Color.FromArgb(255, 0, 0));     // Red
                c.FillEllipse(Brush10, 26, 26, 8, 8);
            }

        }

        private delegate void updatePixPanel13();
        private void update_pixPanel13()
        {
            if (Convert.ToDouble(infor[1]) <= 50)
            {
                Graphics c = pixPanel13.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);

            }
            else if (50 < Convert.ToDouble(infor[1]) && Convert.ToDouble(infor[1]) <= 100)
            {
                Graphics c = pixPanel13.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 10, 10, 40, 40);

            }
            else if (100 < Convert.ToDouble(infor[1]) && Convert.ToDouble(infor[1]) <= 200)
            {
                Graphics c = pixPanel13.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 18, 18, 24, 24);

            }
            else if (200 < Convert.ToDouble(infor[1]) && Convert.ToDouble(infor[1]) <= 500)
            {
                Graphics c = pixPanel13.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 18, 18, 24, 24);

            }
            else if (500 < Convert.ToDouble(infor[1]) && Convert.ToDouble(infor[1]) <= 700)
            {
                Graphics c = pixPanel13.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 16, 16, 28, 28);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 22, 22, 16, 16);

            }
            else if (700 < Convert.ToDouble(infor[1]) && Convert.ToDouble(infor[1]) <= 900)
            {
                Graphics c = pixPanel13.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 16, 16, 28, 28);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 22, 22, 16, 16);

            }
            else if (900 < Convert.ToDouble(infor[1]) && Convert.ToDouble(infor[1]) <= 1100)
            {
                Graphics c = pixPanel13.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 14, 14, 32, 32);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 18, 18, 24, 24);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 22, 22, 16, 16);

            }
            else if (1100 < Convert.ToDouble(infor[1]) && Convert.ToDouble(infor[1]) <= 1300)
            {
                Graphics c = pixPanel13.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 8, 8, 44, 44);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 12, 12, 36, 36);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 16, 16, 28, 28);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 20, 20, 20, 20);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 24, 24, 12, 12);

            }
            else if (1300 < Convert.ToDouble(infor[1]) && Convert.ToDouble(infor[1]) <= 1400)
            {
                Graphics c = pixPanel13.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);

            }
            else if (Convert.ToDouble(infor[1]) > 1400)
            {
                Graphics c = pixPanel13.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);
                SolidBrush Brush10 = new SolidBrush(Color.FromArgb(255, 0, 0));     // Red
                c.FillEllipse(Brush10, 26, 26, 8, 8);
            }

        }

        private delegate void updatePixPanel14();
        private void update_pixPanel14()
        {
            if (Convert.ToDouble(infor[2]) <= 50)
            {
                Graphics c = pixPanel14.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);

            }
            else if (50 < Convert.ToDouble(infor[2]) && Convert.ToDouble(infor[2]) <= 100)
            {
                Graphics c = pixPanel14.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 10, 10, 40, 40);

            }
            else if (100 < Convert.ToDouble(infor[2]) && Convert.ToDouble(infor[2]) <= 200)
            {
                Graphics c = pixPanel14.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 6, 6, 48, 48);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 14, 14, 32, 32);

            }
            else if (200 < Convert.ToDouble(infor[2]) && Convert.ToDouble(infor[2]) <= 500)
            {
                Graphics c = pixPanel14.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 18, 18, 24, 24);

            }
            else if (500 < Convert.ToDouble(infor[2]) && Convert.ToDouble(infor[2]) <= 700)
            {
                Graphics c = pixPanel14.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 16, 16, 28, 28);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 22, 22, 16, 16);

            }
            else if (700 < Convert.ToDouble(infor[2]) && Convert.ToDouble(infor[2]) <= 900)
            {
                Graphics c = pixPanel14.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 16, 16, 28, 28);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 22, 22, 16, 16);

            }
            else if (900 < Convert.ToDouble(infor[2]) && Convert.ToDouble(infor[2]) <= 1100)
            {
                Graphics c = pixPanel14.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 14, 14, 32, 32);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 18, 18, 24, 24);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 22, 22, 16, 16);

            }
            else if (1100 < Convert.ToDouble(infor[2]) && Convert.ToDouble(infor[2]) <= 1300)
            {
                Graphics c = pixPanel14.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 8, 8, 44, 44);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 12, 12, 36, 36);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 16, 16, 28, 28);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 20, 20, 20, 20);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 24, 24, 12, 12);

            }
            else if (1300 < Convert.ToDouble(infor[2]) && Convert.ToDouble(infor[2]) <= 1400)
            {
                Graphics c = pixPanel14.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);

            }
            else if (Convert.ToDouble(infor[2]) > 1400)
            {
                Graphics c = pixPanel14.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);
                SolidBrush Brush10 = new SolidBrush(Color.FromArgb(255, 0, 0));     // Red
                c.FillEllipse(Brush10, 26, 26, 8, 8);
            }

        }

        private delegate void updatePixPanel15();
        private void update_pixPanel15()
        {
            if (Convert.ToDouble(infor[6]) <= 50)
            {
                Graphics c = pixPanel15.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);

            }
            else if (50 < Convert.ToDouble(infor[6]) && Convert.ToDouble(infor[6]) <= 100)
            {
                Graphics c = pixPanel15.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 10, 10, 40, 40);

            }
            else if (100 < Convert.ToDouble(infor[6]) && Convert.ToDouble(infor[6]) <= 200)
            {
                Graphics c = pixPanel15.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 6, 6, 48, 48);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 14, 14, 32, 32);

            }
            else if (200 < Convert.ToDouble(infor[6]) && Convert.ToDouble(infor[6]) <= 500)
            {
                Graphics c = pixPanel15.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 18, 18, 24, 24);

            }
            else if (500 < Convert.ToDouble(infor[6]) && Convert.ToDouble(infor[6]) <= 700)
            {
                Graphics c = pixPanel15.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 16, 16, 28, 28);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 22, 22, 16, 16);

            }
            else if (700 < Convert.ToDouble(infor[6]) && Convert.ToDouble(infor[6]) <= 900)
            {
                Graphics c = pixPanel15.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 16, 16, 28, 28);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 22, 22, 16, 16);

            }
            else if (900 < Convert.ToDouble(infor[6]) && Convert.ToDouble(infor[6]) <= 1100)
            {
                Graphics c = pixPanel15.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 14, 14, 32, 32);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 18, 18, 24, 24);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 22, 22, 16, 16);

            }
            else if (1100 < Convert.ToDouble(infor[6]) && Convert.ToDouble(infor[6]) <= 1300)
            {
                Graphics c = pixPanel15.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 8, 8, 44, 44);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 12, 12, 36, 36);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 16, 16, 28, 28);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 20, 20, 20, 20);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 24, 24, 12, 12);

            }
            else if (1300 < Convert.ToDouble(infor[6]) && Convert.ToDouble(infor[6]) <= 1400)
            {
                Graphics c = pixPanel15.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);

            }
            else if (Convert.ToDouble(infor[6]) > 1400)
            {
                Graphics c = pixPanel15.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);
                SolidBrush Brush10 = new SolidBrush(Color.FromArgb(255, 0, 0));     // Red
                c.FillEllipse(Brush10, 26, 26, 8, 8);
            }

        }


        private delegate void updatePixPanel16();
        private void update_pixPanel16()
        {
            if (Convert.ToDouble(infor[7]) <= 50)
            {
                Graphics c = pixPanel16.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);

            }
            else if (50 < Convert.ToDouble(infor[7]) && Convert.ToDouble(infor[7]) <= 100)
            {
                Graphics c = pixPanel16.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 10, 10, 40, 40);

            }
            else if (100 < Convert.ToDouble(infor[7]) && Convert.ToDouble(infor[7]) <= 200)
            {
                Graphics c = pixPanel16.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 6, 6, 48, 48);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 14, 14, 32, 32);

            }
            else if (200 < Convert.ToDouble(infor[7]) && Convert.ToDouble(infor[7]) <= 500)
            {
                Graphics c = pixPanel16.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 18, 18, 24, 24);

            }
            else if (500 < Convert.ToDouble(infor[7]) && Convert.ToDouble(infor[7]) <= 700)
            {
                Graphics c = pixPanel16.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 4, 4, 52, 52);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 10, 10, 40, 40);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 16, 16, 28, 28);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 22, 22, 16, 16);

            }
            else if (700 < Convert.ToDouble(infor[7]) && Convert.ToDouble(infor[7]) <= 900)
            {
                Graphics c = pixPanel16.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 16, 16, 28, 28);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 22, 22, 16, 16);

            }
            else if (900 < Convert.ToDouble(infor[7]) && Convert.ToDouble(infor[7]) <= 1100)
            {
                Graphics c = pixPanel16.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 6, 6, 48, 48);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 10, 10, 40, 40);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 14, 14, 32, 32);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 18, 18, 24, 24);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 22, 22, 16, 16);

            }
            else if (1100 < Convert.ToDouble(infor[7]) && Convert.ToDouble(infor[7]) <= 1300)
            {
                Graphics c = pixPanel16.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 8, 8, 44, 44);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 12, 12, 36, 36);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 16, 16, 28, 28);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 20, 20, 20, 20);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 24, 24, 12, 12);

            }
            else if (1300 < Convert.ToDouble(infor[7]) && Convert.ToDouble(infor[7]) <= 1400)
            {
                Graphics c = pixPanel16.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);

            }
            else if (Convert.ToDouble(infor[7]) > 1400)
            {
                Graphics c = pixPanel16.CreateGraphics();
                SolidBrush Brush1 = new SolidBrush(Color.FromArgb(0, 35, 75));  // Blue
                c.FillEllipse(Brush1, 0, 0, 60, 60);
                SolidBrush Brush2 = new SolidBrush(Color.FromArgb(0, 50, 150));
                c.FillEllipse(Brush2, 2, 2, 56, 56);
                SolidBrush Brush3 = new SolidBrush(Color.FromArgb(0, 100, 205));
                c.FillEllipse(Brush3, 4, 4, 52, 52);
                SolidBrush Brush4 = new SolidBrush(Color.FromArgb(50, 205, 205));
                c.FillEllipse(Brush4, 6, 6, 48, 48);
                SolidBrush Brush5 = new SolidBrush(Color.FromArgb(50, 255, 150));
                c.FillEllipse(Brush5, 8, 8, 44, 44);
                SolidBrush Brush6 = new SolidBrush(Color.FromArgb(200, 255, 100));
                c.FillEllipse(Brush6, 12, 12, 36, 36);
                SolidBrush Brush7 = new SolidBrush(Color.FromArgb(255, 255, 0));
                c.FillEllipse(Brush7, 16, 16, 28, 28);
                SolidBrush Brush8 = new SolidBrush(Color.FromArgb(255, 200, 0));
                c.FillEllipse(Brush8, 20, 20, 20, 20);
                SolidBrush Brush9 = new SolidBrush(Color.FromArgb(255, 145, 0));
                c.FillEllipse(Brush9, 24, 24, 12, 12);
                SolidBrush Brush10 = new SolidBrush(Color.FromArgb(255, 0, 0));     // Red
                c.FillEllipse(Brush10, 26, 26, 8, 8);
            }

        }
 
        private void pixPanel1_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics myGraphics1 = pixPanel1.CreateGraphics();
            SolidBrush sbr = new SolidBrush(Color.FromArgb(0, 35, 75));
            Rectangle rec = new Rectangle(0, 0, pixPanel1.Width, pixPanel1.Height);
            myGraphics1.FillRectangle(sbr, rec);
        }

        private void pixPanel2_Paint_1(object sender, PaintEventArgs e)
        {

            Graphics myGraphics2 = pixPanel2.CreateGraphics();
            SolidBrush sbr = new SolidBrush(Color.FromArgb(0, 35, 75));
            Rectangle rec = new Rectangle(0, 0, pixPanel2.Width, pixPanel2.Height);
            myGraphics2.FillRectangle(sbr, rec);
        }

        private void pixPanel3_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics myGraphics3 = pixPanel3.CreateGraphics();
            SolidBrush sbr = new SolidBrush(Color.FromArgb(0, 35, 75));
            Rectangle rec = new Rectangle(0, 0, pixPanel3.Width, pixPanel3.Height);
            myGraphics3.FillRectangle(sbr, rec);
        }

         private void pixPanel4_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics myGraphics4 = pixPanel4.CreateGraphics();
            SolidBrush sbr = new SolidBrush(Color.FromArgb(0, 35, 75));
            Rectangle rec = new Rectangle(0, 0, pixPanel4.Width, pixPanel4.Height);
            myGraphics4.FillRectangle(sbr, rec);
        }

        private void pixPanel5_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics myGraphics5 = pixPanel5.CreateGraphics();
            SolidBrush sbr = new SolidBrush(Color.FromArgb(0, 35, 75));
            Rectangle rec = new Rectangle(0, 0, pixPanel5.Width, pixPanel5.Height);
            myGraphics5.FillRectangle(sbr, rec);
        }

        private void pixPanel6_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics myGraphics6 = pixPanel6.CreateGraphics();
            SolidBrush sbr = new SolidBrush(Color.FromArgb(0, 35, 75));
            Rectangle rec = new Rectangle(0, 0, pixPanel6.Width, pixPanel6.Height);
            myGraphics6.FillRectangle(sbr, rec);
        }

        private void pixPanel7_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics myGraphics7 = pixPanel7.CreateGraphics();
            SolidBrush sbr = new SolidBrush(Color.FromArgb(0, 35, 75));
            Rectangle rec = new Rectangle(0, 0, pixPanel7.Width, pixPanel7.Height);
            myGraphics7.FillRectangle(sbr, rec);
        }

        private void pixPanel8_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics myGraphics8 = pixPanel8.CreateGraphics();
            SolidBrush sbr = new SolidBrush(Color.FromArgb(0, 35, 75));
            Rectangle rec = new Rectangle(0, 0, pixPanel8.Width, pixPanel8.Height);
            myGraphics8.FillRectangle(sbr, rec);
        }
       
        private void pixPanel9_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics myGraphics9 = pixPanel9.CreateGraphics();
            SolidBrush sbr = new SolidBrush(Color.FromArgb(0, 35, 75));
            Rectangle rec = new Rectangle(0, 0, pixPanel9.Width, pixPanel9.Height);
            myGraphics9.FillRectangle(sbr, rec);
        }

        private void pixPanel10_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics myGraphics10 = pixPanel10.CreateGraphics();
            SolidBrush sbr = new SolidBrush(Color.FromArgb(0, 35, 75));
            Rectangle rec = new Rectangle(0, 0, pixPanel10.Width, pixPanel10.Height);
            myGraphics10.FillRectangle(sbr, rec);
        }

        private void pixPanel11_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics myGraphics11 = pixPanel11.CreateGraphics();
            SolidBrush sbr = new SolidBrush(Color.FromArgb(0, 35, 75));
            Rectangle rec = new Rectangle(0, 0, pixPanel11.Width, pixPanel11.Height);
            myGraphics11.FillRectangle(sbr, rec);
        }

        private void pixPanel12_Paint(object sender, PaintEventArgs e)
        {
            Graphics myGraphics12 = pixPanel12.CreateGraphics();
            SolidBrush sbr = new SolidBrush(Color.FromArgb(0, 35, 75));
            Rectangle rec = new Rectangle(0, 0, pixPanel12.Width, pixPanel12.Height);
            myGraphics12.FillRectangle(sbr, rec);
        }

        private void pixPanel13_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics myGraphics13 = pixPanel13.CreateGraphics();
            SolidBrush sbr = new SolidBrush(Color.FromArgb(0, 35, 75));
            Rectangle rec = new Rectangle(0, 0, pixPanel13.Width, pixPanel13.Height);
            myGraphics13.FillRectangle(sbr, rec);
        }

        private void pixPanel14_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics myGraphics14 = pixPanel14.CreateGraphics();
            SolidBrush sbr = new SolidBrush(Color.FromArgb(0, 35, 75));
            Rectangle rec = new Rectangle(0, 0, pixPanel14.Width, pixPanel14.Height);
            myGraphics14.FillRectangle(sbr, rec);
        }

        private void pixPanel15_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics myGraphics15 = pixPanel15.CreateGraphics();
            SolidBrush sbr = new SolidBrush(Color.FromArgb(0, 35, 75));
            Rectangle rec = new Rectangle(0, 0, pixPanel15.Width, pixPanel15.Height);
            myGraphics15.FillRectangle(sbr, rec);
        }

        private void pixPanel16_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics myGraphics16 = pixPanel16.CreateGraphics();
            SolidBrush sbr = new SolidBrush(Color.FromArgb(0, 35, 75));
            Rectangle rec = new Rectangle(0, 0, pixPanel16.Width, pixPanel16.Height);
            myGraphics16.FillRectangle(sbr, rec);
        }


        private void resetButton_Click(object sender, EventArgs e)
        {
            toeTextBox1.Text = "0";
            mth1TextBox1.Text = "0";
            mth3TextBox1.Text = "0";
            mth5TextBox1.Text = "0";
            arch1TextBox1.Text = "0";
            arch2TextBox1.Text = "0";
            arch3TextBox1.Text = "0";
            heelTextBox1.Text = "0";


            toeTextBox2.Text = "0";
            mth1TextBox2.Text = "0";
            mth3TextBox2.Text = "0";
            mth5TextBox2.Text = "0";
            arch1TextBox2.Text = "0";
            arch2TextBox2.Text = "0";
            arch3TextBox2.Text = "0";
            heelTextBox2.Text = "0";


            countTimeTextBox.Text = "00 : 00 : 00";
            total.hour = 0;
            total.min = 0;
            total.sec = 0;
            //total.millisec = 0;

            comportComboBox1.Items.Clear();
            comportComboBox2.Items.Clear();

            foreach (String ser in SerialPort.GetPortNames())
            {
                comportComboBox1.Items.Add(ser);
                

            }
            comportComboBox1.SelectedIndex = 0;
            foreach (String ser2 in SerialPort.GetPortNames())
            {
                comportComboBox2.Items.Add(ser2);


            }
            comportComboBox2.SelectedIndex = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            countTimeTextBox.Text = String.Format("{0:00} : {1:00} : {2:00}", total.hour, total.min, total.sec);

            #region Total
            //if (total.millisec >= 9)
            //{
            //  total.millisec = 0;
            //total.sec++;
            if (total.sec >= 59)
            {
                total.sec = 0;
                total.min++;
                if (total.min == 60)
                {
                    total.min = 0;
                    total.hour++;
                    if (total.hour == 24)
                    {
                        total.sec = 0;
                        total.min = 0;
                        total.hour = 0;
                    }
                }
            }
            //}
            else total.sec++;
            #endregion
        }


        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.FileName = "";
                saveFileDialog1.DefaultExt = ".txt";
                saveFileDialog1.Filter = "Text Files(*.txt)|*.txt";
                saveFileDialog1.FileName = "";
                saveFileDialog1.DefaultExt = ".xls";
                saveFileDialog1.Filter = "Text Files(*.xls)|*.xls";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter sw = File.CreateText(saveFileDialog1.FileName);

                    foreach (string str in serialRecieveTextBox1.Lines)
                    {
                        sw.WriteLine(str);
                    }
                    /*
                    foreach (string str2 in averageTextBox1.Lines)
                    {
                        sw.WriteLine("Left: " + str2);
                    }
                    foreach (string str3 in averageTextBox2.Lines)
                    {
                        sw.WriteLine("Right: " + str3);
                    }
                    foreach (string str4 in countTimeTextBox.Lines)
                    {
                        sw.WriteLine("Time: " + str4);
                    }
                     */
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                saveFileDialog2.FileName = "";
                saveFileDialog2.DefaultExt = ".txt";
                saveFileDialog2.Filter = "Text Files(*.txt)|*.txt";
                saveFileDialog2.FileName = "";
                saveFileDialog2.DefaultExt = ".xls";
                saveFileDialog2.Filter = "Text Files(*.xls)|*.xls";

                if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter sw = File.CreateText(saveFileDialog2.FileName);

                    foreach (string str in serialRecieveTextBox2.Lines)
                    {
                        sw.WriteLine(str);
                    }
                    /*
                    foreach (string str2 in averageTextBox1.Lines)
                    {
                        sw.WriteLine("Left: " + str2);
                    }
                    foreach (string str3 in averageTextBox2.Lines)
                    {
                        sw.WriteLine("Right: " + str3);
                    }
                    foreach (string str4 in countTimeTextBox.Lines)
                    {
                        sw.WriteLine("Time: " + str4);
                    }
                     */
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toeTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        
        

       
      

        
       
       

        

        

        
    }
}
