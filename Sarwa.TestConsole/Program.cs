using Sarwa.Common;
using System;
using System.Collections.Generic;

namespace Sarwa.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string valEmpty = string.Empty;
            string valNotEmpty = "Mohamed amine bouchalkha";

            if (valEmpty.IsNullOrEmpty() && valEmpty.IsNullOrWhiteSpace())
                Console.WriteLine($"String Empty {valEmpty} you see nothing");

            if (!valNotEmpty.IsNullOrEmpty() && !valNotEmpty.IsNullOrWhiteSpace())
                Console.WriteLine($"String Not Empty {valNotEmpty} you see something");

            Console.WriteLine(Environment.NewLine);

            List<string> listOfString = new List<string>
            {
                "amine",
                "arwa",
                "sarrah"
            };

            List<string> listEmpty = new List<string>();

            if (!listOfString.IsNullOrEmpty())
                Console.WriteLine($"List isn't empty");

            if (listEmpty.IsNullOrEmpty())
                Console.WriteLine($"List is empty");

            Console.WriteLine(Environment.NewLine);

            listOfString.AddIfNotContains("bahija");
            listOfString.AddIfNotContains("amine");

            foreach (string item in listOfString)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine(Environment.NewLine);

            listEmpty.AddIfNotContains("bahija");
            listEmpty.AddIfNotContains("bahija");

            foreach (string item in listEmpty)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine(Environment.NewLine);

            List<string> listChaining = new List<string> { };

            listChaining.AddIfNotContainsFluent("Amine")
                        .AddIfNotContainsFluent("Arwa");

            foreach (string item in listChaining)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine(Environment.NewLine);

            Disposable
                .Using(
                    () => new System.Net.WebClient(),
                    webClient => webClient.DownloadString("http://time.gov/actualtime.cgi"))
                .Tee(Console.WriteLine);

            Console.WriteLine(Environment.NewLine);

            Console.ReadLine();
        }
    }
}
