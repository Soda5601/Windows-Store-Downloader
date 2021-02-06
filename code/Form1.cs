﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Windows_Store_Downloader
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
            
        }
        
        private bool textBoxHasText = false;
        Form2 Form2 = new Form2();
        WriteToTemp WriteToTemp = new WriteToTemp();
        public static string postContent;
        private void AttributeInputReady(object sender, EventArgs e)
        {
            HasText();
            if (textBoxHasText == false)
            {
                attributeText.Text = "";
                attributeText.ForeColor = Color.Black;
            }
            else
            {
                attributeText.ForeColor = Color.Black;
            }

            
        }
        private void HasText()
        {

                if (attributeText.Text == "" || attributeText.Text == Language.lang_attributes[0] ||
                attributeText.Text == Language.lang_input || attributeText.Text == Language.lang_attributes[1] ||
                attributeText.Text == Language.lang_attributes[2] || attributeText.Text == Language.lang_attributes[3])
                {
                    textBoxHasText = false;
                }
                else
                {
                    textBoxHasText = true;
                }
            

        }
        private void AttributeInputDeselect(object sender, EventArgs e)
        {
            HasText();
                if (textBoxHasText == false)
            {
                attributeText.Text = SetAttributeText();
                attributeText.ForeColor = Color.Gray;
                textBoxHasText = false;
            }
            else
            {
                textBoxHasText = true;
            }
        }
        private string SetAttributeText() {
            return Language.lang_attributes[typeBox.SelectedIndex];
        }

        
        private void DownloadButton_Click(object sender, EventArgs e)
        {
            downloadButton.Enabled = false;
            Form2.complete = false;
            progressBar1.Value = 0;
            if (typeBox.SelectedIndex == -1 || routeBox.SelectedIndex == -1 || attributeText.Text == "")
            {
                MessageBox.Show(Language.lang_baddown,Language.lang_baddowninfo,MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                if (langText.Text == "")
                {
                    langText.Text = Thread.CurrentThread.CurrentCulture.Name; 
                }//提交语言
                postContent = "type=" + Http_Post.type[typeBox.SelectedIndex] + "&url=" + attributeText.Text + "&ring=" +
          Http_Post.ring[routeBox.SelectedIndex] + "&lang=" + langText.Text;

                Thread post = new Thread(Form2.Browse);
                post.SetApartmentState(ApartmentState.STA);
                post.Start();                
                while (Form2.complete == false)
                {
                    if(progressBar1.Value <= 99)
                    {
                        Random random = new Random(new Guid().GetHashCode());
                        Thread.Sleep(random.Next(67, 101));
                        progressBar1.PerformStep();
                    }
                }//伪装进度条
                progressBar1.Value = 100;
                downloadButton.Enabled = true;
                if (Form2.returnid == -1)
                {
                    MessageBox.Show(Language.lang_interr, Language.lang_interr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }//意外
                new Form2().ShowDialog();
                if (Form2.returnid == 1)
                {
                    try
                    {

                       
                        
                        
                    }
                    catch (InvalidComObjectException) { }
                    catch (Exception ex)
                    {
                        Language.InternalErrMsgBox(ex);
                    }
                }

            }
            
        }

        private void ChangeLanguage(object sender, EventArgs e)//更改语言
        {
            if (langBox.Text == "English")
            {
                English_Lang();
            } else if (langBox.Text == "中文（简体）") {
                Chinese_Lang();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

            RefreshForm();
            WriteToTemp.ReadFrom();
            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name == "zh-CN") {
                Chinese_Lang();
                langBox.SelectedIndex = 1;
            } else {
                English_Lang();
                langBox.SelectedIndex = 0;
            }

            User32.AnimateWindow(this.Handle, 200, User32.AW_BLEND | User32.AW_ACTIVE | User32.AW_VER_NEGATIVE);
            RefreshForm();
            this.Opacity = 0.9;
            

        }

        private void Chinese_Lang()
        {
            Language.Chinese_Lang();
            SetLang();
        }
        private void English_Lang()
        {
            Language.English_Lang();
            SetLang();
        }
        private void SetLang() {
            typeLinkText.Text = Language.lang_typelink;
            langPackText.Text = Language.lang_language;
            routeText.Text = Language.lang_route;
            downloadButton.Text = Language.lang_downbutton;
            this.Text = Language.lang_title;
            groupBox1.Text = Language.lang_downbutton;
            attributeText.Text = Language.lang_input;
            progressText.Text = Language.lang_prog;
        }//设置语言文本

        private void RefreshText(object sender, EventArgs e)//刷新文本
        {
            HasText();
            if (textBoxHasText == false)
            {
                attributeText.Text = SetAttributeText();
                attributeText.ForeColor = Color.Gray;
                textBoxHasText = false;
            }
            else
            {
                textBoxHasText = true;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)//淡出
        {
            this.Opacity = 1;
            User32.AnimateWindow(this.Handle, 300, User32.AW_BLEND | User32.AW_HIDE);
        }
        private void RefreshForm()//初始化窗口
        {
            Bitmap a = Properties.Resources.store;
            a.MakeTransparent(Color.FromArgb(0, 255, 0));//透明图片
            pictureBox1.BackgroundImage = a;
            if (System.Diagnostics.Debugger.IsAttached != true)
            {
                langText.Visible = false;
                langPackText.Visible = false;
                debugWebsite.Visible = false;
                debugWebBrowser.Visible = false;
            }//调试内容
            typeBox.SelectedIndex = 0;
            routeBox.SelectedIndex = 2;
            //初始化选择框

            attributeText.Text = SetAttributeText();
            attributeText.ForeColor = Color.Gray;
            textBoxHasText = false;
            //初始化文字
          
        }

        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
                

                Graphics g = e.Graphics;   //实例化Graphics 对象g
                Color FColor = Color.FromArgb(0xE8, 0xF1, 0xE7); //颜色1
                Color TColor = Color.FromArgb(0xCA, 0xC7, 0xC7);  //颜色2
                Brush b = new LinearGradientBrush(this.ClientRectangle, FColor, TColor, LinearGradientMode.BackwardDiagonal);  //实例化刷子，第一个参数指示上色区域，第二个和第三个参数分别渐变颜色的开始和结束，第四个参数表示颜色的方向。
                g.FillRectangle(b, this.ClientRectangle);  //进行上色 




        }

        private void debugWebBrowser_Click(object sender, EventArgs e)
        {
            new Form2().Show();
            //Form2.webBrowser1.Navigate(debugWebsite.Text);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(Form2.webBrowser1.Url);
        }

    }
    class User32
    {
        /// <summary>
        /// 窗体动画函数
        /// </summary>
        /// <param name="hwnd">指定产生动画的窗口的句柄</param>
        /// <param name="dwTime">指定动画持续的时间</param>
        /// <param name="dwFlags">指定动画类型，可以是一个或多个标志的组合。</param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        //下面是可用的常量，根据不同的动画效果声明自己需要的
        public const int AW_HOR_POSITIVE = 0x0001;//自左向右显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志
        public const int AW_HOR_NEGATIVE = 0x0002;//自右向左显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志
        public const int AW_VER_POSITIVE = 0x0004;//自顶向下显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志
        public const int AW_VER_NEGATIVE = 0x0008;//自下向上显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志该标志
        public const int AW_CENTER = 0x0010;//若使用了AW_HIDE标志，则使窗口向内重叠；否则向外扩展
        public const int AW_HIDE = 0x10000;//隐藏窗口
        public const int AW_ACTIVE = 0x20000;//激活窗口，在使用了AW_HIDE标志后不要使用这个标志
        public const int AW_SLIDE = 0x40000;//使用滑动类型动画效果，默认为滚动动画类型，当使用AW_CENTER标志时，这个标志就被忽略
        public const int AW_BLEND = 0x80000;//使用淡入淡出效果
    }//淡入淡出
    
}
