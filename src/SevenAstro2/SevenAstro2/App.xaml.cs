using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

namespace SevenAstro2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    internal partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            System.Threading.ThreadPool.SetMaxThreads(15000, 30000);
            System.Threading.ThreadPool.SetMinThreads(10000, 20000);

            Global.SetDateFormat();

            new System.Threading.Tasks.Task(() => GenerateCodeFromCSV(), System.Threading.Tasks.TaskCreationOptions.PreferFairness).Start();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
        }

        [Conditional("GEN_CODE")]
        void GenerateCodeFromCSV()
        {
            var source = @"C:\Users\Kaveh\Downloads\City_of_Iran.csv";
            var destination = @"C:\Users\Kaveh\Downloads\code.txt";

            using (var csvFile = new Microsoft.VisualBasic.FileIO.TextFieldParser(source, Encoding.UTF8))
            using (var code = new System.IO.StreamWriter(destination, false, Encoding.UTF8))
            {
                foreach (var rec in csvFile.Records())
                {
                    try
                    {
                        code.WriteLine(
                            "yield return new Models.Location {{ Name = \"{0}\", Longitude = \"{1}\", Latitude = \"{2}\", Timezone = \"-3:30\", DST = \"0\" }};"
                            , rec[2].Replace(";", string.Empty).Replace("-", string.Empty).Trim()
                            , TimeSpan.FromHours(double.Parse(rec[0])).ShowAsDegree()
                            , TimeSpan.FromHours(double.Parse(rec[1])).ShowAsDegree());
                    }
                    catch { }
                }

                code.Flush();
            }
        }
    }
}
