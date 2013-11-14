using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;


namespace SerebiiCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Serebii Crawler";

            Console.WriteLine("Serebii Crawler\nv0.2\n\n");
            Console.Write("Press any key to start!");
            Console.ReadLine();
            Console.WriteLine();

            using (var client = new WebClient())
            {
                string check = "<td class=\"foo\">Effort Values Earned</td>";
                string output = "";
                int maxValue = 718;

                for (int k = 1; k <= maxValue; k++)
                {
                    string dCurrent = "";
                    string value = k.ToString();
                    
                    if (k < 10){ value = "00" + k; }
                    else if (k < 100){ value = "0" + k; }

                    try
                    {
                        if (Convert.ToInt32(value) <= 151)
                        {
                            bool dExists = Directory.Exists("(1 - 151) Kanto");
                            if (!dExists) { Directory.CreateDirectory("(1 - 151) Kanto"); }
                            dCurrent = "(1 - 151) Kanto\\";
                        }
                        else if (Convert.ToInt32(value) <= 251 && Convert.ToInt32(value) >= 152)
                        {
                            bool dExists = Directory.Exists("(152 - 251) Johto");
                            if (!dExists) { Directory.CreateDirectory("(152 - 251) Johto"); }
                            dCurrent = "(152 - 251) Johto\\";
                        }
                        else if (Convert.ToInt32(value) <= 386 && Convert.ToInt32(value) >= 252)
                        {
                            bool dExists = Directory.Exists("(252 - 386) Hoenn");
                            if (!dExists) { Directory.CreateDirectory("(252 - 386) Hoenn"); }
                            dCurrent = "(252 - 386) Hoenn\\";
                        }
                        else if (Convert.ToInt32(value) <= 493 && Convert.ToInt32(value) >= 387)
                        {
                            bool dExists = Directory.Exists("(387 - 493) Sinnoh");
                            if (!dExists) { Directory.CreateDirectory("(387 - 493) Sinnoh"); }
                            dCurrent = "(387 - 493) Sinnoh\\";
                        }
                        if (Convert.ToInt32(value) <= 649 && Convert.ToInt32(value) >= 494)
                        {
                            bool dExists = Directory.Exists("(494 - 649) Unova");
                            if (!dExists) { Directory.CreateDirectory("(494 - 649) Unova"); }
                            dCurrent = "(494 - 649) Unova\\";
                        }
                        if (Convert.ToInt32(value) <= 718 && Convert.ToInt32(value) >= 650)
                        {
                            bool dExists = Directory.Exists("(650 - 718) Kalos");
                            if (!dExists) { Directory.CreateDirectory("(650 - 718) Unova"); }
                            dCurrent = "(650 - 718) Unova\\";
                        }
                    }
                    catch (Exception fail) { Console.WriteLine(fail.Message); } 

                    try
                    {
                        //pull image
                        client.DownloadFile("http://www.serebii.net/xy/pokemon/" + value + ".png", dCurrent + value + ".png");

                        string content = client.DownloadString("http://www.serebii.net/pokedex-xy/" + value + ".shtml");
                        content.Trim();

                        string name = string.Empty;

                        string[] result = content.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                        for (int l = 0; l < result.Length; l++)
                        {
                            if (result[l].Contains("<title>"))
                            {
                                string[] nameData = result[l].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                name = nameData[0].Replace("<title>", "").Trim();
                            }
                        }

                        for (int i = 0; i < result.Length; i++)
                        {
                            if (result[i].Contains(check))
                            {
                                output = result[i + 6].Replace("<td class=\"fooinfo\">", "").Replace("</td>", "").Replace("<br /></td><td class=\"fooinfo\">", "").Replace("<br />", " ").Trim();
                                //Console.WriteLine(output);
                                if (!File.Exists(dCurrent + "values.txt"))
                                {
                                    File.Create(dCurrent + "values.txt").Close();
                                }

                                using (StreamWriter w = File.AppendText(dCurrent + "values.txt"))
                                {
                                    w.WriteLine(value + " " + name + " / Effort Values Earned: " + output);
                                }

                                double percent = ((double)k / (double)maxValue) * 100;
                                RenderConsoleProgress((int)Math.Ceiling(percent), '▓', ConsoleColor.Green, k.ToString() + "/" + maxValue + " Completed (" + (int)Math.Ceiling(percent) + "%)");

                                break;
                            }
                        }
                    }
                    catch (Exception fail) { Console.WriteLine(fail.Message); }
                }
            }
        }

        //other stuff
        public static void OverwriteConsoleMessage(string message)
        {
            Console.CursorLeft = 0;
            int maxCharacterWidth = Console.WindowWidth - 1;
            if (message.Length > maxCharacterWidth)
            {
                message = message.Substring(0, maxCharacterWidth - 3) + "...";
            }
            message = message + new string(' ', maxCharacterWidth - message.Length);
            Console.Write(message);
        }

        public static void RenderConsoleProgress(int percentage)
        {
            RenderConsoleProgress(percentage, '\u2590', Console.ForegroundColor, "");
        }

        public static void RenderConsoleProgress(int percentage, char progressBarCharacter, ConsoleColor color, string message)
        {
            Console.CursorVisible = false;
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.CursorLeft = 0;
            int width = Console.WindowWidth - 1;
            int newWidth = (int)((width * percentage) / 100d);
            string progBar = new string(progressBarCharacter, newWidth) +
                  new string(' ', width - newWidth);
            Console.Write(progBar);
            if (string.IsNullOrEmpty(message)) message = "";
            Console.CursorTop++;
            OverwriteConsoleMessage(message);
            Console.CursorTop--;
            Console.ForegroundColor = originalColor;
            Console.CursorVisible = true;
        }
    }
}
