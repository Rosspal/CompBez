using System;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            string FilePath = "AllSets.txt";
            int[] a = new int[] { 0, 1, 2, 3, 4, 5 };
            string[] ABC = new string[] { "О", "И", "Н", "Т", "К", "_" };
            string[] tempABC = new string[] { "x", "x", "x", "x", "x", "x" };
            List<string> AllSets = new List<string>();



            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }

            string SetToWrite = "";
            int Nset = 1;
            while (NextSet(ref a, a.Length))
            {
                SetToWrite = Nset + ": ";
                for (int i = 0; i < a.Length; i++)
                {
                    SetToWrite += ABC[a[i]] + " ";
                }
                using (StreamWriter sw = new StreamWriter(FilePath, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(SetToWrite);

                    SetToWrite = SetToWrite.Replace(Nset + ": ", "");
                    AllSets.Add(SetToWrite);

                    SetToWrite = "";
                    Nset++;
                }
            }


            //
            // Шифратор
            //


            Console.WriteLine("Строка для шифрования");
            string StringEncrypt = Console.ReadLine();
            Console.WriteLine("номер перестановки");
            int N_set = int.Parse(Console.ReadLine());
            string EncryptedString = "";
            string ABC_set = AllSets[N_set - 1];
            Console.WriteLine("Перестановка под номером " + N_set + " = " + ABC_set);

            ABC_set = ABC_set.Replace(" ", "");

            for (int i = 0; i < tempABC.Length; i++)
            {
                tempABC[i] = ABC_set[i].ToString();
            }

            for (int i = 0; i < StringEncrypt.Length; i++)
            {
                string OrigChar = StringEncrypt[i].ToString();
                int OrigChar_InABC = Array.IndexOf(ABC, OrigChar);
                string EncryptedChar = tempABC[OrigChar_InABC];
                EncryptedString += EncryptedChar;
            }
            Console.WriteLine("Результат шифрования = " + EncryptedString);


            //
            // Дешифратор
            //



            string DecryptedString = "";

            for (int i = 0; i < tempABC.Length; i++)
            {
                tempABC[i] = ABC_set[i].ToString();
            }

            for (int i = 0; i < EncryptedString.Length; i++)
            {
                string EncryptedChar = EncryptedString[i].ToString();
                int EncryptedChar_InSetABC = Array.IndexOf(tempABC, EncryptedChar);
                string OrigChar = ABC[EncryptedChar_InSetABC];
                DecryptedString += OrigChar;
            }
            Console.WriteLine("Расшифрованния строка = " + DecryptedString);


            //
            // Перебор
            //

            Console.WriteLine("Строка для дешифровки перебором");
            //BruteForce(Console.ReadLine());
            BruteForce(EncryptedString);
        }

        static bool NextSet(ref int[] a, int n)
        {
            int j = n - 2;
            while (j != -1 && a[j] >= a[j + 1]) j--;
            if (j == -1)
                return false;

            int k = n - 1;
            while (a[j] >= a[k]) k--;
            Swap(ref a, j, k);

            int l = j + 1, r = n - 1; // сортируем оставшуюся часть последовательности
            while (l < r)
                Swap(ref a, l++, r--);
            return true;
        }

        static void Swap(ref int[] a, int i, int j)
        {
            int s = a[i];
            a[i] = a[j];
            a[j] = s;
        }

        static void BruteForce(string encryptedString)
        {
            string ABC = "";
            ABC += encryptedString[0];
            for (int i = 0; i < encryptedString.Length; i++)
            {
                for (int j = 0; j < ABC.Length; j++)
                {
                    if (!ABC.Contains(encryptedString[i]))
                    {
                        ABC += encryptedString[i];
                    }
                }
            }

            ABC = "ОИНТК_";

            string FilePath = "TempAllSets.txt";
            int[] a = new int[ABC.Length];

            for (int i = 0; i < ABC.Length; i++)
            {
                a[i] = i;
                Console.WriteLine("a[i] = " + a[i]);
            }

            string tempABC = ABC;
            List<string> AllSets = new List<string>();

            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }


            string SetToWrite = "";
            int Nset = 0;
            while (NextSet(ref a, a.Length))
            {
                SetToWrite = Nset + ": ";
                for (int i = 0; i < a.Length; i++)
                {
                    SetToWrite += ABC[a[i]] + " ";
                }
                using (StreamWriter sw = new StreamWriter(FilePath, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(SetToWrite);

                    SetToWrite = SetToWrite.Replace(Nset + ": ", "");
                    AllSets.Add(SetToWrite);

                    SetToWrite = "";
                    Nset++;
                }
            }

            



            string[] result = new string[AllSets.Count];
            Parallel.For(0, AllSets.Count,  (t) =>
            {
                string DecryptedString = "";
                string Set = AllSets[t];
                

                string tempABC_set = Set;

                tempABC_set = Set.Replace(" ", "");


                tempABC = tempABC_set;


                for (int i = 0; i < encryptedString.Length; i++)
                {
                    char EncryptedChar = encryptedString[i];
                    int EncryptedChar_InSetABC = tempABC.IndexOf(EncryptedChar);//String.IndexOf(tempABC, EncryptedChar);
                    string OrigChar = Char.ToString(ABC[EncryptedChar_InSetABC]);
                    DecryptedString += OrigChar;
                }

                
                result[t] = DecryptedString;
            });

            //Console.WriteLine("Подсказка = " + Array.IndexOf(result, "НИТКИ_ТОНКИ"));
            while (true)
            {
                Console.WriteLine("Новое значение");
                Console.WriteLine(result[int.Parse(Console.ReadLine()) - 1]);
            }
        }
    }
}