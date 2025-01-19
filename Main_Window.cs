using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;



namespace M_MiSideLanguageEditor_C_
{

    public partial class Main_Window : Form
    {
        //变量==================================================
        public const string WindowTitle = "米塔语言编辑器";
        public const string Version = "Beta1.1.0.0";
        public static string MisidePath = "Unknown";
        public static string LangPath = "Unknown";
        public static int EggTemp = 0;


        //Main_Window===========================================

        public Main_Window()
        {
            InitializeComponent();
            this.Text = WindowTitle + Version;
            MessageBox.Show("此版本为测试版本，很多功能不齐全，请谨慎使用!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            textBox_DLocation12.Enabled = false;

            listBox_DLocation12.SelectedIndex = 0; // 设置第一个项为选中项

        }
        //Functions============================================

        //扫描文件夹
        public void ScanFolders(string folderPath)
        {
            List<string> subFolders = new List<string>();
            try
            {
                DirectoryInfo directory = new DirectoryInfo(folderPath);
                DirectoryInfo[] subDirectories = directory.GetDirectories();

                foreach (DirectoryInfo subDirectory in subDirectories)
                {
                    subFolders.Add(subDirectory.Name);
                }

                listBox_LanguageList.Items.Clear();
                listBox_LanguageList.Items.AddRange(subFolders.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错: " + ex.Message);
            }
        }

        //读取文件(全读取)
        static string ReadTxtFile(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string content = reader.ReadToEnd();
                    Console.WriteLine(content);
                    return content;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("读取文件时出错: " + e.Message);
                return "";
            }
        }

        //修改一行
        static bool WriteLineFile(string filePath, int targetLineNumber, string newLineContent)
        {
            try
            {
                List<string> lines = new List<string>();

                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    int lineCount = 1;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (lineCount == targetLineNumber)
                        {
                            lines.Add(newLineContent);
                        }
                        else
                        {
                            lines.Add(line);
                        }

                        lineCount++;
                    }
                }

                File.WriteAllLines(filePath, lines);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("修改文件时出错: " + e.Message);
                return false;

            }
        }

        //读取一行
        static void ReadLineFile(string filePath, int targetLineNumber)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                int currentLineNumber = 1;
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    if (currentLineNumber == targetLineNumber)
                    {
                        Console.WriteLine(line);
                        break;
                    }

                    currentLineNumber++;
                }
            }
        }

        //删除文件
        static void DeleteFile(string filePath)
        {
            try
            {
                // 尝试删除文件
                File.Delete(filePath);
                Console.WriteLine("文件已成功删除！");
            }
            catch (Exception ex)
            {
                Console.WriteLine("删除文件时发生错误: " + ex.Message);
            }
        }

        //创建并写入
        static void CreateAndWriteToFile(string filePath, string content)
        {
            try
            {
                // 使用 StreamWriter 创建并打开文件，如果文件不存在则创建，如果存在则覆盖
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    writer.Write(content);  // 写入内容
                }
                Console.WriteLine("文件创建并写入成功！");
            }
            catch (Exception ex)
            {
                Console.WriteLine("创建文件或写入内容时发生错误: " + ex.Message);
            }
        }

        //取随机数
        static int GetRandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max + 1);
        }
        //Main=================================================

        //主窗口加载
        private void Main_Window_Load(object sender, EventArgs e)
        { 

        }

        //关于程序
        private void 关于程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("米塔语言编辑器(MiSide Language Editor)\n\nHuaZi-华子 版权所有 Copyright ©  2024-2025 保留所有权利 盗版必究 此软件完全免费\n\n此软件使用C# WinForm开发", "关于程序", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        //浏览...
        private void button_GamePath_Browser_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                MisidePath = folderBrowserDialog.SelectedPath;
                textBox_GamePath.Text = MisidePath;
                listBox_LanguageList.Items.Clear();

                ScanFolders(MisidePath + "\\Data\\Languages");
            }

        }

        //语言列表索引更改
        private void listBox_LanguageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine(MisidePath + "\\Data\\Languages\\" + listBox_LanguageList.SelectedItem.ToString() + "\\AlternativeName.txt");
            LangPath = MisidePath + "\\Data\\Languages\\" + listBox_LanguageList.SelectedItem.ToString();

            //可用Group框
            button_GamePath_Browser.Enabled = false;
            groupBox_Ach.Enabled = true;
            groupBox_Cloth.Enabled = true;
            groupBox_Croe.Enabled = true;
            groupBox_LanguageSetting.Enabled = true;
            groupBox_Location.Enabled = true;
            groupBox_Talk.Enabled = true;

            




            //游戏内显示
            textBox_LS_DisplayName.Text = ReadTxtFile(LangPath + "\\AlternativeName.txt");
            textBox_LS_DisplayName.Enabled=true;

            //成就

            listBox_Ach.Items.Clear ();
            string Ach_filePath = LangPath+ "\\Achievements.txt";  

            using (StreamReader reader = new StreamReader(Ach_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_Ach.Items.Add(line);
                }
            }
            textBox_Ach_Set.Text = "Unknown";
            textBox_Ach_Set.Enabled = false;

            //服装

            listBox_Cloth.Items.Clear();
            string Cloth_filePath = LangPath + "\\Clothes.txt";

            using (StreamReader reader = new StreamReader(Cloth_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_Cloth.Items.Add(line);
                }
            }
            textBox_Cloth_Set.Text = "Unknown";
            textBox_Cloth_Set.Enabled = false;

            //核心

            listBox_Croe.Items.Clear();
            string Croe_filePath = LangPath + "\\CoreSoft.txt";

            using (StreamReader reader = new StreamReader(Croe_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_Croe.Items.Add(line);
                }
            }
            textBox_Croe_Set.Text = "Unknown";
            textBox_Croe_Set.Enabled = false;

            //Location
            for (int i = 0; i < 1; i++)
            {
                //Location1

                listBox_Location1.Items.Clear();
                string Location1_filePath = LangPath + "\\Location 1.txt";

                using (StreamReader reader = new StreamReader(Location1_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_Location1.Items.Add(line);
                    }
                }
                textBox_Location1.Text = "Unknown";
                textBox_Location1.Enabled = false;


                //Location2

                listBox_Location2.Items.Clear();
                string Location2_filePath = LangPath + "\\Location 2.txt";

                using (StreamReader reader = new StreamReader(Location2_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_Location2.Items.Add(line);
                    }
                }
                textBox_Location2.Text = "Unknown";
                textBox_Location2.Enabled = false;


                //Location3

                listBox_Location3.Items.Clear();
                string Location3_filePath = LangPath + "\\Location 3.txt";

                using (StreamReader reader = new StreamReader(Location3_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_Location3.Items.Add(line);
                    }
                }
                textBox_Location3.Text = "Unknown";
                textBox_Location3.Enabled = false;

                //Location4

                listBox_Location4.Items.Clear();
                string Location4_filePath = LangPath + "\\Location 4.txt";

                using (StreamReader reader = new StreamReader(Location4_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_Location4.Items.Add(line);
                    }
                }
                textBox_Location4.Text = "Unknown";
                textBox_Location4.Enabled = false;



                //Location6

                listBox_Location6.Items.Clear();
                string Location6_filePath = LangPath + "\\Location 6.txt";

                using (StreamReader reader = new StreamReader(Location6_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_Location6.Items.Add(line);
                    }
                }
                textBox_Location6.Text = "Unknown";
                textBox_Location6.Enabled = false;

                //Location7

                listBox_Location7.Items.Clear();
                string Location7_filePath = LangPath + "\\Location 7.txt";

                using (StreamReader reader = new StreamReader(Location7_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_Location7.Items.Add(line);
                    }
                }
                textBox_Location7.Text = "Unknown";
                textBox_Location7.Enabled = false;

                //Location8

                listBox_Location8.Items.Clear();
                string Location8_filePath = LangPath + "\\Location 8.txt";

                using (StreamReader reader = new StreamReader(Location8_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_Location8.Items.Add(line);
                    }
                }
                textBox_Location8.Text = "Unknown";
                textBox_Location8.Enabled = false;

                //Location9

                listBox_Location9.Items.Clear();
                string Location9_filePath = LangPath + "\\Location 9.txt";

                using (StreamReader reader = new StreamReader(Location9_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_Location9.Items.Add(line);
                    }
                }
                textBox_Location9.Text = "Unknown";
                textBox_Location9.Enabled = false;

                //Location10

                listBox_Location10.Items.Clear();
                string Location10_filePath = LangPath + "\\Location 10.txt";

                using (StreamReader reader = new StreamReader(Location10_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_Location10.Items.Add(line);
                    }
                }
                textBox_Location10.Text = "Unknown";
                textBox_Location10.Enabled = false;

                //Location11

                listBox_Location11.Items.Clear();
                string Location11_filePath = LangPath + "\\Location 11.txt";

                using (StreamReader reader = new StreamReader(Location11_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_Location11.Items.Add(line);
                    }
                }
                textBox_Location11.Text = "Unknown";
                textBox_Location11.Enabled = false;

                //Location12

                listBox_Location12.Items.Clear();
                string Location12_filePath = LangPath + "\\Location 12.txt";

                using (StreamReader reader = new StreamReader(Location12_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_Location12.Items.Add(line);
                    }
                }
                textBox_Location12.Text = "Unknown";
                textBox_Location12.Enabled = false;

                //Location13

                listBox_Location13.Items.Clear();
                string Location13_filePath = LangPath + "\\Location 13.txt";

                using (StreamReader reader = new StreamReader(Location13_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_Location13.Items.Add(line);
                    }
                }
                textBox_Location13.Text = "Unknown";
                textBox_Location13.Enabled = false;

                //Location14

                listBox_Location14.Items.Clear();
                string Location14_filePath = LangPath + "\\Location 14.txt";

                using (StreamReader reader = new StreamReader(Location14_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_Location14.Items.Add(line);
                    }
                }
                textBox_Location14.Text = "Unknown";
                textBox_Location14.Enabled = false;

                //Location19

                listBox_Location19.Items.Clear();
                string Location19_filePath = LangPath + "\\Location 19.txt";

                using (StreamReader reader = new StreamReader(Location19_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_Location19.Items.Add(line);
                    }
                }
                textBox_Location19.Text = "Unknown";
                textBox_Location19.Enabled = false;

            }



            //LocationDialogue Location
            for (int i = 0; i < 1; i++)
            {
                //DLocation1
                listBox_DLocation1.Items.Clear();
                string DLocation1_filePath = LangPath + "\\LocationDialogue Location1.txt";

                using (StreamReader reader = new StreamReader(DLocation1_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_DLocation1.Items.Add(line);
                    }
                }
                textBox_DLocation1.Text = "Unknown";
                textBox_DLocation1.Enabled = false;

                //DLocation2
                listBox_DLocation2.Items.Clear();
                string DLocation2_filePath = LangPath + "\\LocationDialogue Location2.txt";

                using (StreamReader reader = new StreamReader(DLocation2_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_DLocation2.Items.Add(line);
                    }
                }
                textBox_DLocation2.Text = "Unknown";
                textBox_DLocation2.Enabled = false;

                //DLocation3
                listBox_DLocation3.Items.Clear();
                string DLocation3_filePath = LangPath + "\\LocationDialogue Location3.txt";

                using (StreamReader reader = new StreamReader(DLocation3_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_DLocation3.Items.Add(line);
                    }
                }
                textBox_DLocation3.Text = "Unknown";
                textBox_DLocation3.Enabled = false;

                //DLocation4
                listBox_DLocation4.Items.Clear();
                string DLocation4_filePath = LangPath + "\\LocationDialogue Location4.txt";

                using (StreamReader reader = new StreamReader(DLocation4_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_DLocation4.Items.Add(line);
                    }
                }
                textBox_DLocation4.Text = "Unknown";
                textBox_DLocation4.Enabled = false;

                //DLocation5
                listBox_DLocation5.Items.Clear();
                string DLocation5_filePath = LangPath + "\\LocationDialogue Location5.txt";

                using (StreamReader reader = new StreamReader(DLocation5_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_DLocation5.Items.Add(line);
                    }
                }
                textBox_DLocation5.Text = "Unknown";
                textBox_DLocation5.Enabled = false;

                //DLocation6
                listBox_DLocation6.Items.Clear();
                string DLocation6_filePath = LangPath + "\\LocationDialogue Location6.txt";

                using (StreamReader reader = new StreamReader(DLocation6_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_DLocation6.Items.Add(line);
                    }
                }
                textBox_DLocation6.Text = "Unknown";
                textBox_DLocation6.Enabled = false;

                //DLocation7
                listBox_DLocation7.Items.Clear();
                string DLocation7_filePath = LangPath + "\\LocationDialogue Location7.txt";

                using (StreamReader reader = new StreamReader(DLocation7_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_DLocation7.Items.Add(line);
                    }
                }
                textBox_DLocation7.Text = "Unknown";
                textBox_DLocation7.Enabled = false;

                //DLocation8
                listBox_DLocation8.Items.Clear();
                string DLocation8_filePath = LangPath + "\\LocationDialogue Location8.txt";

                using (StreamReader reader = new StreamReader(DLocation8_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_DLocation8.Items.Add(line);
                    }
                }
                textBox_DLocation8.Text = "Unknown";
                textBox_DLocation8.Enabled = false;

                //DLocation9
                listBox_DLocation9.Items.Clear();
                string DLocation9_filePath = LangPath + "\\LocationDialogue Location9.txt";

                using (StreamReader reader = new StreamReader(DLocation9_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_DLocation9.Items.Add(line);
                    }
                }
                textBox_DLocation9.Text = "Unknown";
                textBox_DLocation9.Enabled = false;

                //DLocation10
                listBox_DLocation10.Items.Clear();
                string DLocation10_filePath = LangPath + "\\LocationDialogue Location10.txt";

                using (StreamReader reader = new StreamReader(DLocation10_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_DLocation10.Items.Add(line);
                    }
                }
                textBox_DLocation10.Text = "Unknown";
                textBox_DLocation10.Enabled = false;

                //DLocation11
                listBox_DLocation11.Items.Clear();
                string DLocation11_filePath = LangPath + "\\LocationDialogue Location11.txt";

                using (StreamReader reader = new StreamReader(DLocation11_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_DLocation11.Items.Add(line);
                    }
                }
                textBox_DLocation11.Text = "Unknown";
                textBox_DLocation11.Enabled = false;

                //DLocation12
                listBox_DLocation12.Items.Clear();
                string DLocation12_filePath = LangPath + "\\LocationDialogue Location12.txt";

                using (StreamReader reader = new StreamReader(DLocation12_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_DLocation12.Items.Add(line);
                    }
                }
                listBox_DLocation12.SelectedIndex= 0;
                textBox_DLocation12.Text = "Unknown";
                textBox_DLocation12.Enabled = false;

                //DLocation13
                listBox_DLocation13.Items.Clear();
                string DLocation13_filePath = LangPath + "\\LocationDialogue Location13.txt";

                using (StreamReader reader = new StreamReader(DLocation13_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_DLocation13.Items.Add(line);
                    }
                }
                textBox_DLocation13.Text = "Unknown";
                textBox_DLocation13.Enabled = false;

                //DLocation14
                listBox_DLocation14.Items.Clear();
                string DLocation14_filePath = LangPath + "\\LocationDialogue Location14.txt";

                using (StreamReader reader = new StreamReader(DLocation14_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_DLocation14.Items.Add(line);
                    }
                }
                textBox_DLocation14.Text = "Unknown";
                textBox_DLocation14.Enabled = false;

                //DLocation15
                listBox_DLocation15.Items.Clear();
                string DLocation15_filePath = LangPath + "\\LocationDialogue Location15.txt";

                using (StreamReader reader = new StreamReader(DLocation15_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_DLocation15.Items.Add(line);
                    }
                }
                textBox_DLocation15.Text = "Unknown";
                textBox_DLocation15.Enabled = false;

                //DLocation17
                listBox_DLocation17.Items.Clear();
                string DLocation17_filePath = LangPath + "\\LocationDialogue Location17.txt";

                using (StreamReader reader = new StreamReader(DLocation17_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_DLocation17.Items.Add(line);
                    }
                }
                textBox_DLocation17.Text = "Unknown";
                textBox_DLocation17.Enabled = false;

                //DLocation18
                listBox_DLocation18.Items.Clear();
                string DLocation18_filePath = LangPath + "\\LocationDialogue Location18.txt";

                using (StreamReader reader = new StreamReader(DLocation18_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_DLocation18.Items.Add(line);
                    }
                }
                textBox_DLocation18.Text = "Unknown";
                textBox_DLocation18.Enabled = false;

                //DLocation19
                listBox_DLocation19.Items.Clear();
                string DLocation19_filePath = LangPath + "\\LocationDialogue Location19.txt";

                using (StreamReader reader = new StreamReader(DLocation19_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_DLocation19.Items.Add(line);
                    }
                }
                textBox_DLocation19.Text = "Unknown";
                textBox_DLocation19.Enabled = false;

                //DLocation20
                listBox_DLocation20.Items.Clear();
                string DLocation20_filePath = LangPath + "\\LocationDialogue Location20.txt";

                using (StreamReader reader = new StreamReader(DLocation20_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox_DLocation20.Items.Add(line);
                    }
                }
                textBox_DLocation20.Text = "Unknown";
                textBox_DLocation20.Enabled = false;



            }


        }

        //成就索引更改
        private void listBox_Ach_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Ach_Set.Text=listBox_Ach.SelectedItem.ToString();
            textBox_Ach_Set.Enabled = true;

        }

        //游戏内显示文本被更改
        private void textBox_LS_DisplayName_Leave(object sender, EventArgs e)
        {
            DeleteFile(LangPath + "\\AlternativeName.txt");
            CreateAndWriteToFile(LangPath + "\\AlternativeName.txt", textBox_LS_DisplayName.Text);

        }

        //成就设置
        private void textBox_Ach_Set_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\Achievements.txt", listBox_Ach.SelectedIndex + 1, textBox_Ach_Set.Text);

            //刷新成就列表
            listBox_Ach.Items.Clear();

            string filePath = LangPath + "\\Achievements.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_Ach.Items.Add(line);
                }
            }



        }

        //服装索引更改
        private void listBox_Cloth_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            textBox_Cloth_Set.Text = listBox_Cloth.SelectedItem.ToString();
            textBox_Cloth_Set.Enabled = true;
            if(textBox_Cloth_Set.Text=="")
            {
                textBox_Cloth_Set.Enabled = false;
                textBox_Cloth_Set.Text = "[换行符不可更改]";
            }
        }

        //服装更改
        private void textBox_Cloth_Set_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\Clothes.txt", listBox_Cloth.SelectedIndex + 1, textBox_Cloth_Set.Text);

            //刷新服装列表
            listBox_Cloth.Items.Clear();

            string filePath = LangPath + "\\Clothes.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_Cloth.Items.Add(line);
                }
            }

        }

        //核心索引更改
        private void listBox_Croe_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Croe_Set.Text = listBox_Croe.SelectedItem.ToString();
            textBox_Croe_Set.Enabled = true;
            if (textBox_Croe_Set.Text == "")
            {
                textBox_Croe_Set.Enabled = false;
                textBox_Croe_Set.Text = "[换行符不可更改]";
            }
        }

        //核心更改
        private void textBox_Croe_Set_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\CoreSoft.txt", listBox_Croe.SelectedIndex + 1, textBox_Croe_Set.Text);

            //刷新核心列表
            listBox_Croe.Items.Clear();

            string filePath = LangPath + "\\CoreSoft.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_Croe.Items.Add(line);
                }
            }
        }
        

        
        
        
        //Location1索引更改
        private void listBox_Location1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Location1.Text = listBox_Location1.SelectedItem.ToString();
            textBox_Location1.Enabled = true;
            if (textBox_Location1.Text == "")
            {
                textBox_Location1.Enabled = false;
                textBox_Location1.Text = "[不可修改换行符]";
            }
            if (textBox_Location1.Text.Length >= 2)
            {
                if (textBox_Location1.Text.Substring(0, 2) == "//")
                {
                    textBox_Location1.Enabled = false;
                    textBox_Location1.Text = "[不可修改注释]";
                }
            }
        }

        //Location2索引更改
        private void listBox_Location2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Debug");
            textBox_Location2.Text = listBox_Location2.SelectedItem.ToString();
            textBox_Location2.Enabled = true;
            if (textBox_Location2.Text == "")
            {
                textBox_Location2.Enabled = false;
                textBox_Location2.Text = "[不可修改换行符]";
            }
            if (textBox_Location2.Text.Length >= 2)
            {
                if (textBox_Location2.Text.Substring(0, 2) == "//")
                {
                    textBox_Location2.Enabled = false;
                    textBox_Location2.Text = "[不可修改注释]";
                }
            }
        }

        //Location3索引更改
        private void listBox_Location3_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Location3.Text = listBox_Location3.SelectedItem.ToString();
            textBox_Location3.Enabled = true;
            if (textBox_Location3.Text == "")
            {
                textBox_Location3.Enabled = false;
                textBox_Location3.Text = "[不可修改换行符]";
            }
            if (textBox_Location3.Text.Length >= 2)
            {
                if (textBox_Location3.Text.Substring(0, 2) == "//")
                {
                    textBox_Location3.Enabled = false;
                    textBox_Location3.Text = "[不可修改注释]";
                }
            }
        }

        //Location4索引更改
        private void listBox_Location4_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Location4.Text = listBox_Location4.SelectedItem.ToString();
            textBox_Location4.Enabled = true;
            if (textBox_Location4.Text == "")
            {
                textBox_Location4.Enabled = false;
                textBox_Location4.Text = "[不可修改换行符]";
            }
            if (textBox_Location4.Text.Length >= 2)
            {
                if (textBox_Location4.Text.Substring(0, 2) == "//")
                {
                    textBox_Location4.Enabled = false;
                    textBox_Location4.Text = "[不可修改注释]";
                }
            }
        }

        //Location6索引更改
        private void listBox_Location6_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Location6.Text = listBox_Location6.SelectedItem.ToString();
            textBox_Location6.Enabled = true;
            if (textBox_Location6.Text == "")
            {
                textBox_Location6.Enabled = false;
                textBox_Location6.Text = "[不可修改换行符]";
            }
            if (textBox_Location6.Text.Length >= 2)
            {
                if (textBox_Location6.Text.Substring(0, 2) == "//")
                {
                    textBox_Location6.Enabled = false;
                    textBox_Location6.Text = "[不可修改注释]";
                }
            }
        }

        //Location7索引更改
        private void listBox_Location7_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Location7.Text = listBox_Location7.SelectedItem.ToString();
            textBox_Location7.Enabled = true;
            if (textBox_Location7.Text == "")
            {
                textBox_Location7.Enabled = false;
                textBox_Location7.Text = "[不可修改换行符]";
            }
            if (textBox_Location7.Text.Length >= 2)
            {
                if (textBox_Location7.Text.Substring(0, 2) == "//")
                {
                    textBox_Location7.Enabled = false;
                    textBox_Location7.Text = "[不可修改注释]";
                }
            }
        }

        //Location8索引更改
        private void listBox_Location8_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Location8.Text = listBox_Location8.SelectedItem.ToString();
            textBox_Location8.Enabled = true;
            if (textBox_Location8.Text == "")
            {
                textBox_Location8.Enabled = false;
                textBox_Location8.Text = "[不可修改换行符]";
            }
            if (textBox_Location8.Text.Length >= 2)
            {
                if (textBox_Location8.Text.Substring(0, 2) == "//")
                {
                    textBox_Location8.Enabled = false;
                    textBox_Location8.Text = "[不可修改注释]";
                }
            }
        }

        //Location9索引更改
        private void listBox_Location9_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Location9.Text = listBox_Location9.SelectedItem.ToString();
            textBox_Location9.Enabled = true;
            if (textBox_Location9.Text == "")
            {
                textBox_Location9.Enabled = false;
                textBox_Location9.Text = "[不可修改换行符]";
            }
            if (textBox_Location9.Text.Length >= 2)
            {
                if (textBox_Location9.Text.Substring(0, 2) == "//")
                {
                    textBox_Location9.Enabled = false;
                    textBox_Location9.Text = "[不可修改注释]";
                }
            }
        }

        //Location10索引更改
        private void listBox_Location10_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Location10.Text = listBox_Location10.SelectedItem.ToString();
            textBox_Location10.Enabled = true;
            if (textBox_Location10.Text == "")
            {
                textBox_Location10.Enabled = false;
                textBox_Location10.Text = "[不可修改换行符]";
            }
            if (textBox_Location10.Text.Length >= 2)
            {
                if (textBox_Location10.Text.Substring(0, 2) == "//")
                {
                    textBox_Location10.Enabled = false;
                    textBox_Location10.Text = "[不可修改注释]";
                }
            }
        }

        //Location11索引更改
        private void listBox_Location11_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Location11.Text = listBox_Location11.SelectedItem.ToString();
            textBox_Location11.Enabled = true;
            if (textBox_Location11.Text == "")
            {
                textBox_Location11.Enabled = false;
                textBox_Location11.Text = "[不可修改换行符]";
            }
            if (textBox_Location11.Text.Length >= 2)
            {
                if (textBox_Location11.Text.Substring(0, 2) == "//")
                {
                    textBox_Location11.Enabled = false;
                    textBox_Location11.Text = "[不可修改注释]";
                }
            }
        }

        //Location12索引更改
        private void listBox_Location12_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Location12.Text = listBox_Location12.SelectedItem.ToString();
            textBox_Location12.Enabled = true;
            if (textBox_Location12.Text == "")
            {
                textBox_Location12.Enabled = false;
                textBox_Location12.Text = "[不可修改换行符]";
            }
            if (textBox_Location12.Text.Length >= 2)
            {
                if (textBox_Location12.Text.Substring(0, 2) == "//")
                {
                    textBox_Location12.Enabled = false;
                    textBox_Location12.Text = "[不可修改注释]";
                }
            }
        }

        //Location13索引更改
        private void listBox_Location13_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Location13.Text = listBox_Location13.SelectedItem.ToString();
            textBox_Location13.Enabled = true;
            if (textBox_Location13.Text == "")
            {
                textBox_Location13.Enabled = false;
                textBox_Location13.Text = "[不可修改换行符]";
            }
            if (textBox_Location13.Text.Length >= 2)
            {
                if (textBox_Location13.Text.Substring(0, 2) == "//")
                {
                    textBox_Location13.Enabled = false;
                    textBox_Location13.Text = "[不可修改注释]";
                }
            }
        }

        //Location14索引更改
        private void listBox_Location14_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Location14.Text = listBox_Location14.SelectedItem.ToString();
            textBox_Location14.Enabled = true;
            if (textBox_Location14.Text == "")
            {
                textBox_Location14.Enabled = false;
                textBox_Location14.Text = "[不可修改换行符]";
            }
            if (textBox_Location14.Text.Length >= 2)
            {
                if (textBox_Location14.Text.Substring(0, 2) == "//")
                {
                    textBox_Location14.Enabled = false;
                    textBox_Location14.Text = "[不可修改注释]";
                }
            }
        }

        //Location19索引更改
        private void listBox_Location19_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Location19.Text = listBox_Location19.SelectedItem.ToString();
            textBox_Location19.Enabled = true;
            if (textBox_Location19.Text == "")
            {
                textBox_Location19.Enabled = false;
                textBox_Location19.Text = "[不可修改换行符]";
            }
            if (textBox_Location19.Text.Length >= 2)
            {
                if (textBox_Location19.Text.Substring(0, 2) == "//")
                {
                    textBox_Location19.Enabled = false;
                    textBox_Location19.Text = "[不可修改注释]";
                }
            }
        }


        //
        //Location更改====================================================================================================================
        //


        //Location1更改
        private void textBox_Location1_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\Location 1.txt", listBox_Location1.SelectedIndex + 1, textBox_Location1.Text);

            //刷新Location1列表
            listBox_Location1.Items.Clear();

            string filePath = LangPath + "\\Location 1.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_Location1.Items.Add(line);
                }
            }
        }

        //Location2更改
        private void textBox_Location2_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\Location 2.txt", listBox_Location2.SelectedIndex + 1, textBox_Location2.Text);

            //刷新Location2列表
            listBox_Location2.Items.Clear();

            string filePath = LangPath + "\\Location 2.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_Location2.Items.Add(line);
                }
            }
        }

        //Location3更改
        private void textBox_Location3_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\Location 3.txt", listBox_Location3.SelectedIndex + 1, textBox_Location3.Text);

            //刷新Location3列表
            listBox_Location3.Items.Clear();

            string filePath = LangPath + "\\Location 3.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_Location3.Items.Add(line);
                }
            }
        }

        //Location4更改
        private void textBox_Location4_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\Location 4.txt", listBox_Location4.SelectedIndex + 1, textBox_Location4.Text);

            //刷新Location4列表
            listBox_Location4.Items.Clear();

            string filePath = LangPath + "\\Location 4.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_Location4.Items.Add(line);
                }
            }
        }

        //Location6更改
        private void textBox_Location6_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\Location 6.txt", listBox_Location6.SelectedIndex + 1, textBox_Location6.Text);

            //刷新Location6列表
            listBox_Location6.Items.Clear();

            string filePath = LangPath + "\\Location 6.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_Location6.Items.Add(line);
                }
            }
        }

        //Location7更改
        private void textBox_Location7_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\Location 7.txt", listBox_Location7.SelectedIndex + 1, textBox_Location7.Text);

            //刷新Location7列表
            listBox_Location7.Items.Clear();

            string filePath = LangPath + "\\Location 7.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_Location7.Items.Add(line);
                }
            }
        }

        //Location8更改
        private void textBox_Location8_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\Location 8.txt", listBox_Location8.SelectedIndex + 1, textBox_Location8.Text);

            //刷新Location8列表
            listBox_Location8.Items.Clear();

            string filePath = LangPath + "\\Location 8.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_Location8.Items.Add(line);
                }
            }
        }

        //Location9更改
        private void textBox_Location9_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\Location 9.txt", listBox_Location9.SelectedIndex + 1, textBox_Location9.Text);

            //刷新Location9列表
            listBox_Location9.Items.Clear();

            string filePath = LangPath + "\\Location 9.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_Location9.Items.Add(line);
                }
            }
        }

        //Location10更改
        private void textBox_Location10_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\Location 10.txt", listBox_Location10.SelectedIndex + 1, textBox_Location10.Text);

            //刷新Location10列表
            listBox_Location10.Items.Clear();

            string filePath = LangPath + "\\Location 10.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_Location10.Items.Add(line);
                }
            }
        }

        //Location11更改
        private void textBox_Location11_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\Location 11.txt", listBox_Location11.SelectedIndex + 1, textBox_Location11.Text);

            //刷新Location11列表
            listBox_Location11.Items.Clear();

            string filePath = LangPath + "\\Location 11.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_Location11.Items.Add(line);
                }
            }
        }

        //Location12更改
        private void textBox_Location12_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\Location 12.txt", listBox_Location12.SelectedIndex + 1, textBox_Location12.Text);

            //刷新Location12列表
            listBox_Location12.Items.Clear();

            string filePath = LangPath + "\\Location 12.txt";
            
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_Location12.Items.Add(line);
                }
            }
        }
        
        //Location13更改
        private void textBox_Location13_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\Location 13.txt", listBox_Location13.SelectedIndex + 1, textBox_Location13.Text);

            //刷新Location13列表
            listBox_Location13.Items.Clear();

            string filePath = LangPath + "\\Location 13.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_Location13.Items.Add(line);
                }
            }
        }

        //Location14更改
        private void textBox_Location14_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\Location 14.txt", listBox_Location14.SelectedIndex + 1, textBox_Location14.Text);

            //刷新Location14列表
            listBox_Location14.Items.Clear();

            string filePath = LangPath + "\\Location 14.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_Location14.Items.Add(line);
                }
            }
        }

        //Location19更改
        private void textBox_Location19_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\Location 19.txt", listBox_Location19.SelectedIndex + 1, textBox_Location19.Text);

            //刷新Location19列表
            listBox_Location19.Items.Clear();

            string filePath = LangPath + "\\Location 19.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_Location19.Items.Add(line);
                }
            }
        }

        private void 最终协议ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1.许可授予：本软件许可协议（以下简称 “本协议”）是您（个人或单一实体，以下简称 “被许可方”）与 Huazi&Xiaowang（以下简称 “许可方”）之间关于使用《米塔语言编辑器(MiSide Language Editor)》软件（以下简称 “本软件”）的法律协议。许可方授予被许可方一项非排他性、不可转让的有限许可，仅允许被许可方在符合本协议条款的前提下，在个人设备上使用本软件。\r\n\r\n2.使用限制：被许可方不得对本软件进行反向工程、反编译、拆卸或以其他方式试图获取本软件的源代码；不得将本软件用于任何商业目的，包括但不限于利用本软件修改游戏台词后进行盈利活动；不得将本软件分发给任何第三方，除非事先获得许可方的书面同意。\r\n\r\n3.知识产权：本软件及其相关文档的所有知识产权，包括但不限于版权、商标权、专利权等，均归许可方所有。被许可方在使用本软件过程中，不得侵犯许可方的任何知识产权。\r\n\r\n4.责任限制：在法律允许的最大范围内，许可方对因使用或无法使用本软件而导致的任何直接、间接、偶然、特殊、惩罚性或后果性损害（包括但不限于数据丢失、业务中断、利润损失等）不承担任何责任，无论该损害是基于合同、侵权（包括疏忽）还是其他法律理论。\r\n\r\n5.协议终止：如果被许可方违反本协议的任何条款，许可方有权立即终止本协议，且被许可方必须立即停止使用本软件，并销毁本软件的所有副本。\r\n\r\n6.法律适用与争议解决：本协议的签订、履行、解释及争议解决均适用中国法律。如双方在本协议履行过程中发生争议，应首先通过友好协商解决；协商不成的，任何一方均有权向有管辖权的人民法院提起诉讼。\r\n\r\n7.其他条款：本协议构成双方就本软件使用的完整协议，取代双方之前所有关于本软件的口头或书面协议。本协议的任何条款若被认定为无效或不可执行，不影响其他条款的有效性和可执行性。\r\n\r\n", "最终许可协议", MessageBoxButtons.OK, MessageBoxIcon.Information);

     

        }

       
        private void 更新日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Beta1.0.0.0更新日志:\n  -更新了你所看到的所有东西\n\nBeta1.1.0.0更新日志:/n  -移除彩蛋/n  -更新最终许可协议(EULA)", "更新日志", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        //
        //DLocation索引更改=======================================================================================
        // 

        //DLocation1索引更改
        private void listBox_DLocation1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_DLocation1.Text = listBox_DLocation1.SelectedItem.ToString();
            textBox_DLocation1.Enabled = true;
            if (textBox_DLocation1.Text == "")
            {
                textBox_DLocation1.Enabled = false;
                textBox_DLocation1.Text = "[不可修改换行符]";
            }
            if (textBox_DLocation1.Text.Length >= 2)
            {
                if (textBox_DLocation1.Text.Substring(0, 2) == "//")
                {
                    textBox_DLocation1.Enabled = false;
                    textBox_DLocation1.Text = "[不可修改注释]";
                }
            }
        }

        //DLocation2索引更改
        private void listBox_DLocation2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_DLocation2.Text = listBox_DLocation2.SelectedItem.ToString();
            textBox_DLocation2.Enabled = true;
            if (textBox_DLocation2.Text == "")
            {
                textBox_DLocation2.Enabled = false;
                textBox_DLocation2.Text = "[不可修改换行符]";
            }
            if (textBox_DLocation2.Text.Length >= 2)
            {
                if (textBox_DLocation2.Text.Substring(0, 2) == "//")
                {
                    textBox_DLocation2.Enabled = false;
                    textBox_DLocation2.Text = "[不可修改注释]";
                }
            }
        }

        //DLocation3索引更改
        private void listBox_DLocation3_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_DLocation3.Text = listBox_DLocation3.SelectedItem.ToString();
            textBox_DLocation3.Enabled = true;
            if (textBox_DLocation3.Text == "")
            {
                textBox_DLocation3.Enabled = false;
                textBox_DLocation3.Text = "[不可修改换行符]";
            }
            if (textBox_DLocation3.Text.Length >= 2)
            {
                if (textBox_DLocation3.Text.Substring(0, 2) == "//")
                {
                    textBox_DLocation3.Enabled = false;
                    textBox_DLocation3.Text = "[不可修改注释]";
                }
            }
        }

        //DLocation4索引更改
        private void listBox_DLocation4_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_DLocation4.Text = listBox_DLocation4.SelectedItem.ToString();
            textBox_DLocation4.Enabled = true;
            if (textBox_DLocation4.Text == "")
            {
                textBox_DLocation4.Enabled = false;
                textBox_DLocation4.Text = "[不可修改换行符]";
            }
            if (textBox_DLocation4.Text.Length >= 2)
            {
                if (textBox_DLocation4.Text.Substring(0, 2) == "//")
                {
                    textBox_DLocation4.Enabled = false;
                    textBox_DLocation4.Text = "[不可修改注释]";
                }
            }
        }

        //DLocation5索引更改
        private void listBox_DLocation5_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_DLocation5.Text = listBox_DLocation5.SelectedItem.ToString();
            textBox_DLocation5.Enabled = true;
            if (textBox_DLocation5.Text == "")
            {
                textBox_DLocation5.Enabled = false;
                textBox_DLocation5.Text = "[不可修改换行符]";
            }
            if (textBox_DLocation5.Text.Length >= 2)
            {
                if (textBox_DLocation5.Text.Substring(0, 2) == "//")
                {
                    textBox_DLocation5.Enabled = false;
                    textBox_DLocation5.Text = "[不可修改注释]";
                }
            }
        }

        //DLocation6索引更改
        private void listBox_DLocation6_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_DLocation6.Text = listBox_DLocation6.SelectedItem.ToString();
            textBox_DLocation6.Enabled = true;
            if (textBox_DLocation6.Text == "")
            {
                textBox_DLocation6.Enabled = false;
                textBox_DLocation6.Text = "[不可修改换行符]";
            }
            if (textBox_DLocation6.Text.Length >= 2)
            {
                if (textBox_DLocation6.Text.Substring(0, 2) == "//")
                {
                    textBox_DLocation6.Enabled = false;
                    textBox_DLocation6.Text = "[不可修改注释]";
                }
            }
        }

        //DLocation7索引更改
        private void listBox_DLocation7_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_DLocation7.Text = listBox_DLocation7.SelectedItem.ToString();
            textBox_DLocation7.Enabled = true;
            if (textBox_DLocation7.Text == "")
            {
                textBox_DLocation7.Enabled = false;
                textBox_DLocation7.Text = "[不可修改换行符]";
            }
            if (textBox_DLocation7.Text.Length >= 2)
            {
                if (textBox_DLocation7.Text.Substring(0, 2) == "//")
                {
                    textBox_DLocation7.Enabled = false;
                    textBox_DLocation7.Text = "[不可修改注释]";
                }
            }
        }

        //DLocation8索引更改
        private void listBox_DLocation8_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_DLocation8.Text = listBox_DLocation8.SelectedItem.ToString();
            textBox_DLocation8.Enabled = true;
            if (textBox_DLocation8.Text == "")
            {
                textBox_DLocation8.Enabled = false;
                textBox_DLocation8.Text = "[不可修改换行符]";
            }
            if (textBox_DLocation8.Text.Length >= 2)
            {
                if (textBox_DLocation8.Text.Substring(0, 2) == "//")
                {
                    textBox_DLocation8.Enabled = false;
                    textBox_DLocation8.Text = "[不可修改注释]";
                }
            }
        }

        //DLocation9索引更改
        private void listBox_DLocation9_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_DLocation9.Text = listBox_DLocation9.SelectedItem.ToString();
            textBox_DLocation9.Enabled = true;
            if (textBox_DLocation9.Text == "")
            {
                textBox_DLocation9.Enabled = false;
                textBox_DLocation9.Text = "[不可修改换行符]";
            }
            if (textBox_DLocation9.Text.Length >= 2)
            {
                if (textBox_DLocation9.Text.Substring(0, 2) == "//")
                {
                    textBox_DLocation9.Enabled = false;
                    textBox_DLocation9.Text = "[不可修改注释]";
                }
            }
        }

        //DLocation10索引更改
        private void listBox_DLocation10_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_DLocation10.Text = listBox_DLocation10.SelectedItem.ToString();
            textBox_DLocation10.Enabled = true;
            if (textBox_DLocation10.Text == "")
            {
                textBox_DLocation10.Enabled = false;
                textBox_DLocation10.Text = "[不可修改换行符]";
            }
            if (textBox_DLocation10.Text.Length >= 2)
            {
                if (textBox_DLocation10.Text.Substring(0, 2) == "//")
                {
                    textBox_DLocation10.Enabled = false;
                    textBox_DLocation10.Text = "[不可修改注释]";
                }
            }
        }

        //DLocation11索引更改
        private void listBox_DLocation11_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_DLocation11.Text = listBox_DLocation11.SelectedItem.ToString();
            textBox_DLocation11.Enabled = true;
            if (textBox_DLocation11.Text == "")
            {
                textBox_DLocation11.Enabled = false;
                textBox_DLocation11.Text = "[不可修改换行符]";
            }
            if (textBox_DLocation11.Text.Length >= 2)
            {
                if (textBox_DLocation11.Text.Substring(0, 2) == "//")
                {
                    textBox_DLocation11.Enabled = false;
                    textBox_DLocation11.Text = "[不可修改注释]";
                }
            }
        }

        //DLocation12索引更改
        private void listBox_DLocation12_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_DLocation12.Text = listBox_DLocation12.SelectedItem.ToString();
            textBox_DLocation12.Enabled = true;
            if (textBox_DLocation12.Text == "")
            {
                textBox_DLocation12.Enabled = false;
                textBox_DLocation12.Text = "[不可修改换行符]";
            }
            if (textBox_DLocation12.Text.Length >= 2)
            {
                if (textBox_DLocation12.Text.Substring(0, 2) == "//")
                {
                    textBox_DLocation12.Enabled = false;
                    textBox_DLocation12.Text = "[不可修改注释]";
                }
            }



        }

        //DLocation13索引更改
        private void listBox_DLocation13_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_DLocation13.Text = listBox_DLocation13.SelectedItem.ToString();
            textBox_DLocation13.Enabled = true;
            if (textBox_DLocation13.Text == "")
            {
                textBox_DLocation13.Enabled = false;
                textBox_DLocation13.Text = "[不可修改换行符]";
            }
            if (textBox_DLocation13.Text.Length >= 2)
            {
                if (textBox_DLocation13.Text.Substring(0, 2) == "//")
                {
                    textBox_DLocation13.Enabled = false;
                    textBox_DLocation13.Text = "[不可修改注释]";
                }
            }
        }

        //DLocation14索引更改
        private void listBox_DLocation14_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_DLocation14.Text = listBox_DLocation14.SelectedItem.ToString();
            textBox_DLocation14.Enabled = true;
            if (textBox_DLocation14.Text == "")
            {
                textBox_DLocation14.Enabled = false;
                textBox_DLocation14.Text = "[不可修改换行符]";
            }
            if (textBox_DLocation14.Text.Length >= 2)
            {
                if (textBox_DLocation14.Text.Substring(0, 2) == "//")
                {
                    textBox_DLocation14.Enabled = false;
                    textBox_DLocation14.Text = "[不可修改注释]";
                }
            }
        }

        //DLocation15索引更改
        private void listBox_DLocation15_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_DLocation15.Text = listBox_DLocation15.SelectedItem.ToString();
            textBox_DLocation15.Enabled = true;
            if (textBox_DLocation15.Text == "")
            {
                textBox_DLocation15.Enabled = false;
                textBox_DLocation15.Text = "[不可修改换行符]";
            }
            if (textBox_DLocation15.Text.Length >= 2)
            {
                if (textBox_DLocation15.Text.Substring(0, 2) == "//")
                {
                    textBox_DLocation15.Enabled = false;
                    textBox_DLocation15.Text = "[不可修改注释]";
                }
            }
        }

        //DLocation17索引更改
        private void listBox_DLocation17_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_DLocation17.Text = listBox_DLocation17.SelectedItem.ToString();
            textBox_DLocation17.Enabled = true;
            if (textBox_DLocation17.Text == "")
            {
                textBox_DLocation17.Enabled = false;
                textBox_DLocation17.Text = "[不可修改换行符]";
            }
            if (textBox_DLocation17.Text.Length >= 2)
            {
                if (textBox_DLocation17.Text.Substring(0, 2) == "//")
                {
                    textBox_DLocation17.Enabled = false;
                    textBox_DLocation17.Text = "[不可修改注释]";
                }
            }
        }

        //DLocation18索引更改
        private void listBox_DLocation18_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_DLocation18.Text = listBox_DLocation18.SelectedItem.ToString();
            textBox_DLocation18.Enabled = true;
            if (textBox_DLocation18.Text == "")
            {
                textBox_DLocation18.Enabled = false;
                textBox_DLocation18.Text = "[不可修改换行符]";
            }
            if (textBox_DLocation18.Text.Length >= 2)
            {
                if (textBox_DLocation18.Text.Substring(0, 2) == "//")
                {
                    textBox_DLocation18.Enabled = false;
                    textBox_DLocation18.Text = "[不可修改注释]";
                }
            }
        }

        //DLocation19索引更改
        private void listBox_DLocation19_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_DLocation19.Text = listBox_DLocation19.SelectedItem.ToString();
            textBox_DLocation19.Enabled = true;
            if (textBox_DLocation19.Text == "")
            {
                textBox_DLocation19.Enabled = false;
                textBox_DLocation19.Text = "[不可修改换行符]";
            }
            if (textBox_DLocation19.Text.Length >= 2)
            {
                if (textBox_DLocation19.Text.Substring(0, 2) == "//")
                {
                    textBox_DLocation19.Enabled = false;
                    textBox_DLocation19.Text = "[不可修改注释]";
                }
            }
        }

        //DLocation20索引更改
        private void listBox_DLocation20_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_DLocation20.Text = listBox_DLocation20.SelectedItem.ToString();
            textBox_DLocation20.Enabled = true;
            if (textBox_DLocation20.Text == "")
            {
                textBox_DLocation20.Enabled = false;
                textBox_DLocation20.Text = "[不可修改换行符]";
            }
            if (textBox_DLocation20.Text.Length >= 2)
            {
                if (textBox_DLocation20.Text.Substring(0, 2) == "//")
                {
                    textBox_DLocation20.Enabled = false;
                    textBox_DLocation20.Text = "[不可修改注释]";
                }
            }
        }

        //
        //DLocation更改=================================================================================='
        //

        //DLocation1更改
        private void textBox_DLocation1_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\LocationDialogue Location1.txt", listBox_DLocation1.SelectedIndex + 1, textBox_DLocation1.Text);

            //刷新DLocation1列表
            listBox_DLocation1.Items.Clear();

            string filePath = LangPath + "\\LocationDialogue Location1.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_DLocation1.Items.Add(line);
                }
            }
        }

        //DLocation2更改
        private void textBox_DLocation2_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\LocationDialogue Location2.txt", listBox_DLocation2.SelectedIndex + 2, textBox_DLocation2.Text);

            //刷新DLocation2列表
            listBox_DLocation2.Items.Clear();

            string filePath = LangPath + "\\LocationDialogue Location2.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_DLocation2.Items.Add(line);
                }
            }
        }

        //DLocation3更改
        private void textBox_DLocation3_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\LocationDialogue Location3.txt", listBox_DLocation3.SelectedIndex + 3, textBox_DLocation3.Text);

            //刷新DLocation3列表
            listBox_DLocation3.Items.Clear();

            string filePath = LangPath + "\\LocationDialogue Location3.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_DLocation3.Items.Add(line);
                }
            }
        }

        //DLocation4更改
        private void textBox_DLocation4_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\LocationDialogue Location4.txt", listBox_DLocation4.SelectedIndex + 4, textBox_DLocation4.Text);

            //刷新DLocation4列表
            listBox_DLocation4.Items.Clear();

            string filePath = LangPath + "\\LocationDialogue Location4.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_DLocation4.Items.Add(line);
                }
            }
        }

        //DLocation5更改
        private void textBox_DLocation5_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\LocationDialogue Location5.txt", listBox_DLocation5.SelectedIndex + 5, textBox_DLocation5.Text);

            //刷新DLocation5列表
            listBox_DLocation5.Items.Clear();

            string filePath = LangPath + "\\LocationDialogue Location5.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_DLocation5.Items.Add(line);
                }
            }
        }

        //DLocation6更改
        private void textBox_DLocation6_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\LocationDialogue Location6.txt", listBox_DLocation6.SelectedIndex + 6, textBox_DLocation6.Text);

            //刷新DLocation6列表
            listBox_DLocation6.Items.Clear();

            string filePath = LangPath + "\\LocationDialogue Location6.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_DLocation6.Items.Add(line);
                }
            }
        }

        //DLocation7更改
        private void textBox_DLocation7_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\LocationDialogue Location7.txt", listBox_DLocation7.SelectedIndex + 7, textBox_DLocation7.Text);

            //刷新DLocation7列表
            listBox_DLocation7.Items.Clear();

            string filePath = LangPath + "\\LocationDialogue Location7.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_DLocation7.Items.Add(line);
                }
            }
        }

        //DLocation8更改
        private void textBox_DLocation8_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\LocationDialogue Location8.txt", listBox_DLocation8.SelectedIndex + 8, textBox_DLocation8.Text);

            //刷新DLocation8列表
            listBox_DLocation8.Items.Clear();
            //第2025行！！！！
            string filePath = LangPath + "\\LocationDialogue Location8.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_DLocation8.Items.Add(line);
                }
            }
        }

        //DLocation9更改
        private void textBox_DLocation9_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\LocationDialogue Location9.txt", listBox_DLocation9.SelectedIndex + 9, textBox_DLocation9.Text);

            //刷新DLocation9列表
            listBox_DLocation9.Items.Clear();

            string filePath = LangPath + "\\LocationDialogue Location9.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_DLocation9.Items.Add(line);
                }
            }
        }

        //DLocation10更改
        private void textBox_DLocation10_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\LocationDialogue Location10.txt", listBox_DLocation10.SelectedIndex + 10, textBox_DLocation10.Text);

            //刷新DLocation10列表
            listBox_DLocation10.Items.Clear();

            string filePath = LangPath + "\\LocationDialogue Location10.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_DLocation10.Items.Add(line);
                }
            }
        }

        //DLocation11更改
        private void textBox_DLocation11_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\LocationDialogue Location11.txt", listBox_DLocation11.SelectedIndex + 11, textBox_DLocation11.Text);

            //刷新DLocation11列表
            listBox_DLocation11.Items.Clear();

            string filePath = LangPath + "\\LocationDialogue Location11.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_DLocation11.Items.Add(line);
                }
            }
        }

        //DLocation12更改
        private void textBox_DLocation12_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\LocationDialogue Location12.txt", listBox_DLocation12.SelectedIndex + 12, textBox_DLocation12.Text);

            //刷新DLocation12列表
            listBox_DLocation12.Items.Clear();

            string filePath = LangPath + "\\LocationDialogue Location12.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_DLocation12.Items.Add(line);
                }
            }
        }

        //DLocation13更改
        private void textBox_DLocation13_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\LocationDialogue Location13.txt", listBox_DLocation13.SelectedIndex + 13, textBox_DLocation13.Text);

            //刷新DLocation13列表
            listBox_DLocation13.Items.Clear();

            string filePath = LangPath + "\\LocationDialogue Location13.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_DLocation13.Items.Add(line);
                }
            }
        }

        //DLocation14更改
        private void textBox_DLocation14_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\LocationDialogue Location14.txt", listBox_DLocation14.SelectedIndex + 14, textBox_DLocation14.Text);

            //刷新DLocation14列表
            listBox_DLocation14.Items.Clear();

            string filePath = LangPath + "\\LocationDialogue Location14.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_DLocation14.Items.Add(line);
                }
            }
        }

        //DLocation15更改
        private void textBox_DLocation15_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\LocationDialogue Location15.txt", listBox_DLocation15.SelectedIndex + 15, textBox_DLocation15.Text);

            //刷新DLocation15列表
            listBox_DLocation15.Items.Clear();

            string filePath = LangPath + "\\LocationDialogue Location15.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_DLocation15.Items.Add(line);
                }
            }
        }

        //DLocation17更改
        private void textBox_DLocation17_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\LocationDialogue Location17.txt", listBox_DLocation17.SelectedIndex + 17, textBox_DLocation17.Text);

            //刷新DLocation17列表
            listBox_DLocation17.Items.Clear();

            string filePath = LangPath + "\\LocationDialogue Location17.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_DLocation17.Items.Add(line);
                }
            }
        }

        //DLocation18更改
        private void textBox_DLocation18_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\LocationDialogue Location18.txt", listBox_DLocation18.SelectedIndex + 18, textBox_DLocation18.Text);

            //刷新DLocation18列表
            listBox_DLocation18.Items.Clear();

            string filePath = LangPath + "\\LocationDialogue Location18.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_DLocation18.Items.Add(line);
                }
            }
        }

        //DLocation19更改
        private void textBox_DLocation19_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\LocationDialogue Location19.txt", listBox_DLocation19.SelectedIndex + 19, textBox_DLocation19.Text);

            //刷新DLocation19列表
            listBox_DLocation19.Items.Clear();

            string filePath = LangPath + "\\LocationDialogue Location19.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_DLocation19.Items.Add(line);
                }
            }
        }

        //DLocation20更改
        private void textBox_DLocation20_Leave(object sender, EventArgs e)
        {
            WriteLineFile(LangPath + "\\LocationDialogue Location20.txt", listBox_DLocation20.SelectedIndex + 20, textBox_DLocation20.Text);

            //刷新DLocation20列表
            listBox_DLocation20.Items.Clear();

            string filePath = LangPath + "\\LocationDialogue Location20.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox_DLocation20.Items.Add(line);
                }
            }
        }











    }
}
