using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTextValidation
{
    class Program
    {
        static void Main(string[] args)
        {
            string highGermanString = "";
            string swissGermanString = "";
            bool validFlag = true;
            try
            {
                ValidateSentencePair.ValidatePairSentence(highGermanString, swissGermanString);
            }
            catch(MyException e)
            {
                // Note: for now I suggest all the problematic data should be able to SAVE, but NOT SUBMIT.
                Console.WriteLine(e.Message);
                validFlag = false;
            }

            Console.WriteLine(validFlag);
        }        
    }
}
