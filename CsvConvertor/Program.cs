using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CsvConvertor
{
    internal class Program
    {
        static void Main(string[] args)
        {
         
            var createFile = new FileCreater();

            createFile.ConvertToCsv();


             

        }
    }
}
