using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ThoughtWorks;
using ThoughtWorks.QRCode;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;
using System.IO;

namespace QRCodeMaker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();//文件打开对话框 
            openFileDialog.Filter = "JPEG|*.jpeg;*.jpg|位图文件|*.bmp|所有文件|*.*";//只能打开我们设置的这几类文件 
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                logoTextBox.Text = fileName;
                Bitmap newBmp = new Bitmap(fileName);
                Bitmap bmp = new Bitmap(newBmp);
                ORCodeImage = bmp;
                logoImg = bmp;
            }      
        }

        private object logoImg;//用于存放logo图片 
        private Image ORCodeImage;
  

        /// <summary> 
        /// 获取二维码生成图片        
        /// </summary> 
  
        public Bitmap getcode(string writeStr)
        {
            QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
            qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;//设置二维码编码格式 
            qRCodeEncoder.QRCodeScale = 8;//设置编码测量度             
            qRCodeEncoder.QRCodeVersion = 7;//设置编码版本   
            qRCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;//设置错误校验 
            Bitmap image = qRCodeEncoder.Encode(writeStr); 
            return image;
        }

        /// <summary>         /// 获取logo         
        /// </summary> 
        /// <returns></returns>         
        /// 
        public Bitmap getlogo()         
        { 
            Bitmap newBmp = new Bitmap("logo.jpg");//获取图片对象             
            Bitmap bmp = new Bitmap(newBmp, 30, 30);//缩放             
            return bmp;         
        }  

        /// <summary>
        /// /// 二维码保存         
        /// </summary> 
        /// <param name="sender"></param>         
        /// <param name="e"></param> 
        private void Save(string sFileName)         
        { 
            SaveFileDialog saveFile = new SaveFileDialog();//创建保存对话框 
            saveFile.Filter = "JPEG|*.jpeg;*.jpg|位图文件|*.bmp|所有文件|*.*";//设置保存的图片格式  
            if (ORCodeImage != null)
            {
                if (saveFile.ShowDialog() == DialogResult.OK)                 
                { 
                    string sFilePathName = saveFile.FileName;
                    Image img = ORCodeImage;                    
                    img.Save(sFilePathName);                 
                }             
            }             
            else             
            { 
                MessageBox.Show("请先生成二维码！");             
            }         
        }  

        private void makebutton_Click(object sender, EventArgs e)
        {
            string strText = textBox.Text; // "www.baidu.com/";

            decimal minValue = minNumericUpDown.Value;
            decimal maxValue = maxNumericUpDown.Value;
            if (minValue > maxValue)
            {
                MessageBox.Show("序号范围非法，请重新输入");
                return;
            }

            if (savePathTextBox.Text.Trim() == "")
            {
                MessageBox.Show("请设置保存位置");
                return;
            }

            for (; minValue <= maxValue; minValue++ )
            {
                 if (strText.Trim() != "") //验证输入的生成内容是否为空             
                 {
                    if (!strText.EndsWith("/"))
                        strText += "/";
                    Bitmap bCode = getcode(strText + minValue); //获取二维码图片                 
                    if (logoImg == null)
                    {
                        ORCodeImage = bCode;
                    }
                    else
                    {
                        Bitmap bLogo = logoImg as Bitmap; //获取logo图片对象 
                        bLogo = new Bitmap(bLogo, 80, 80); //改变图片的大小这里我们设置为30                     
                        int Y = bCode.Height;
                        int X = bCode.Width;
                        Point point = new Point(X / 2 - 40, Y / 2 - 40);//logo图片绘制到二维码上，这里将简单计算一下logo所在的坐标 
                        Graphics g = Graphics.FromImage(bCode);//创建一个画布                     
                        g.DrawImage(bLogo, point);//将logo图片绘制到二维码图片上                     
                        ORCodeImage = bCode;
                    }

                    System.Drawing.Imaging.ImageFormat[] formats = { System.Drawing.Imaging.ImageFormat.Jpeg, System.Drawing.Imaging.ImageFormat.Bmp, System.Drawing.Imaging.ImageFormat.Png };
                    string filePathName = savePathTextBox.Text + "\\" + minValue.ToString() + "." + comboBox1.Text;
                    if (!Directory.Exists(savePathTextBox.Text))
                        Directory.CreateDirectory(savePathTextBox.Text);
                    ORCodeImage.Save(filePathName, formats[comboBox1.SelectedIndex]);
                 }
                 else
                 {
                     MessageBox.Show("输入生成内容！");
                 }       
            }

            MessageBox.Show("二维码生成完成");
            System.Diagnostics.Process.Start("explorer.exe", savePathTextBox.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog saveFile = new FolderBrowserDialog();//创建保存对话框 
            if (saveFile.ShowDialog() == DialogResult.OK)
                savePathTextBox.Text = saveFile.SelectedPath;
            //saveFile.Filter>      
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }
    }
}
