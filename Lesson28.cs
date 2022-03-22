using System;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] fullnamesRecords = null;
            string[] positionsRecords = null;


        }

        private static void AddDossier(string fullnameRecord, string positionRecord)
        {

        }

        private static void AddValueToArray(string[] currentArray,string record)
        {
            ExtendStringArray(currentArray);

            currentArray[currentArray.Length - 1] = record;
        }

        private static void ExtendStringArray(string[] currentArray)
        {
            string[] newArray;
            
            if(currentArray == null)
            {
                currentArray = new string[1];

                return;
            }

            newArray = new string[currentArray.Length + 1];

            for(int i = 0; i < currentArray.Length; i++) 
            {
                newArray[i] = currentArray[i];
            }
        }
    }
}
