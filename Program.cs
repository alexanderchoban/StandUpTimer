using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace StandUpTimer
{
    public class Program
    {
        static public IConfigurationRoot Configuration { get; set; }
        static System.Threading.Timer Timer;
        static bool Running;
        static int MsForTimer = 1800000;

        public static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.Title = "Stand Up Timer";


            while (true)
            {
                Spin spinner = new Spin();

                Console.Clear();
                Console.Write("Press Enter to start timer, press Esc to exit. " + spinner.GetChar());

                bool waitingForKey = true;
                while (waitingForKey)
                {
                    System.Threading.Thread.Sleep(250);
                    Console.Write("\b \b");
                    Console.Write(spinner.GetChar());

                    if (Console.KeyAvailable)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                waitingForKey = false;
                                break;
                            case ConsoleKey.Escape:
                                Console.CursorVisible = true;
                                return;
                        }
                    }
                }

                Console.Clear();

                System.Threading.TimerCallback cb = new System.Threading.TimerCallback(Done);
                DateTime StartedAt = DateTime.Now;
                Timer = new System.Threading.Timer(Done, Running, MsForTimer, 1);
                Running = true;
                bool escKey = false;

                Console.Write("Stand up timer: ");
                string display = "";
                while (Running)
                {
                    int timediff = Convert.ToInt32(Convert.ToDouble(MsForTimer) / 1000 - (DateTime.Now - StartedAt).TotalSeconds);
                    int minutes = timediff / 60;
                    int seconds = timediff % 60;
                    string newdisplay = $"{minutes.ToString().PadLeft(2, '0')}:{seconds.ToString().PadLeft(2, '0')}";

                    if (newdisplay != display)
                    {
                        for (int j = 0; j < display.Length; j++)
                        {
                            Console.Write("\b \b");
                        }

                        display = newdisplay;
                        Console.Write(display);
                    }

                    if ((Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
                    {
                        Done(Running);
                        escKey = true;
                    }

                    System.Threading.Thread.Sleep(500);
                }

                if (!escKey)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Spin warnSpin = new Spin("warning");
                    Console.Clear();
                    Console.Write("Stand up" + warnSpin.GetChar());

                    while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter))
                    {
                        Console.Write("\b \b");
                        Console.Write(warnSpin.GetChar());
                        Console.Beep();

                        System.Threading.Thread.Sleep(500);
                    }

                    Console.ResetColor();
                }
            }
        }

        public static void Done(object obj)
        {
            Running = false;
            Timer.Dispose();
        }
    }

    public class Spin
    {
        private char[] spin;
        private int curspin;

        public Spin()
        {
            curspin = 0;
            spin = new char[] { '|', '/', '-', '\\' };
        }

        public Spin(string type)
        {
            curspin = 0;

            if (type == "warning")
                spin = new char[] { '!', ' ' };
            else
                spin = new char[] { '|', '/', '-', '\\' };
        }

        public char GetChar()
        {
            curspin++;
            if (curspin == spin.Length)
                curspin = 0;

            return spin[curspin];
        }

        public void Reset()
        {
            curspin = 0;
        }
    }
}