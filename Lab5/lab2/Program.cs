using System;

namespace Lab2
{
    class Program
    {
        static int[] KEY = new int[256]; //Ключ
        static int[] GEN = new int[256]; //Генератор случайных чисел

        static string MESSAGE_str = "";  //Сообщение, передаваемое по каналу связи
        static int[] MESSAGE_int_Unicode;//Сообщение в кодах символов Юникода
        static int[] MESSAGE_int_Bytes;  //Сообщение в байтах

        static int[] GAMMA;              //Гамма
        static int[] ENCRYPT;            //Зашифрованное сообщение в байтах
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Введит строку");
            MESSAGE_str = Console.ReadLine();
            MESSAGE_int_Unicode = new int[MESSAGE_str.Length]; //коды Юникода символов сообщения
            MESSAGE_int_Bytes = new int[MESSAGE_str.Length * 2]; //пары байтов сообщения

            //Получение кодов Юникода символов сообщения
            for (int i = 0; i < MESSAGE_int_Unicode.Length; i++)
            {
                MESSAGE_int_Unicode[i] = (int)MESSAGE_str[i];
            }

            //Получение пары байтов сообщения
            int j = 0;
            for (int i = 0; i < MESSAGE_str.Length; i++)
            {
                string tmpBit = Convert.ToString(MESSAGE_str[i], 2); //перевести сообщение в последовательность битов
                if (tmpBit.Length < 8) //если на 1 символ - 1 байт (8 бит)
                {
                    tmpBit = CompleteZeroes(tmpBit, 8); //дописать вначале нули, чтобы было 8 бит, если не хватает

                    for (j = j ; j < MESSAGE_int_Bytes.Length; j++)
                    {
                        MESSAGE_int_Bytes[j] = 0; //1й байт всегда нулевой, т.к. на 1 символ - 1 байт
                        j++;
                        MESSAGE_int_Bytes[j] = Convert.ToInt32(BinToDec(tmpBit)); //перевести 2й байт из двоичного вида в десятичный
                        j++;
                        break;
                    }
                }
                else //если на 1 символ - 2 байта
                {
                    tmpBit = CompleteZeroes(tmpBit, 16); //дописать вначале нули, чтобы было 16 бит (2 байта), если не хватает
                    string tmpBit_p1 = "";
                    string tmpBit_p2 = "";

                    for (int p1 = 0; p1 < tmpBit.Length / 2; p1++) //получить первый байт в двоичном виде
                    {
                        tmpBit_p1 += tmpBit[p1];
                    }
                    for (int p2 = tmpBit.Length / 2; p2 < tmpBit.Length; p2++) //получить второй байт в двоичном виде
                    {
                        tmpBit_p2 += tmpBit[p2];
                    }
                    for (j = j; j < MESSAGE_int_Bytes.Length; j++)
                    {
                        MESSAGE_int_Bytes[j] = Convert.ToInt32(BinToDec(tmpBit_p1)); //перевести 1й байт из двоичного вида в десятичный
                        j++;
                        MESSAGE_int_Bytes[j] = Convert.ToInt32(BinToDec(tmpBit_p2)); //перевести 2й байт из двоичного вида в десятичный
                        j++;
                        break;
                    }
                }
            }

            //3. Получение гаммы. 
            //Сообщение MESSAGE, которое передаёт по каналу связи отправитель, представляет собой байтовую последовательность. 
            //Количество состояний одного байта – 256.
            //Гамма GAMMA представляет собой массив чисел, взятых из диапазона от 0 до 255(соответствуют возможным состояниям байтов сообщения).
            //Длина гаммы равна длине сообщения(в байтах). Чтобы построить гамму, необходимо выполнить следующие действия:
            int i1 = 0;
            int j1 = 0;
            GAMMA = new int[MESSAGE_int_Bytes.Length];

            //k – индекс буквы сообщения; перебрать все k в количестве, равном длине сообщения(в байтах),
            //и на каждой итерации выполняя операции:
            for (int k = 0; k < MESSAGE_int_Bytes.Length; k++)
            {
                int tmp = 0;
                int t = 0;

                i1 = (i1 + 1) % 256;
                j1 = (j1 + GEN[i1]) % 256;

                //поменять местами GEN[i] и GEN[j];
                tmp = GEN[i1];
                GEN[i1] = GEN[j1];
                GEN[j1] = tmp;

                t = (GEN[i1] + GEN[j]) % 256;
                GAMMA[k] = GEN[t];
            }

            //4. Шифрование сообщения. 
            //К каждому байту сообщения применить операцию XOR c соответствующим байтом гаммы.
            ENCRYPT = new int[MESSAGE_int_Bytes.Length];
            for (int i = 0; i < MESSAGE_int_Bytes.Length; i++)
            {
                ENCRYPT[i] = MESSAGE_int_Bytes[i] ^ GAMMA[i];
            }


            string result = "";
            //
            for (int i = 0; i < ENCRYPT.Length; i++)
            {
                string EncryptBin = "";
                char EncryptChar = '0';

                EncryptBin += DecToBin(ENCRYPT[i].ToString()); //перевести 1й байт в двоичный код
                i++;

                EncryptBin += DecToBin(ENCRYPT[i].ToString()); //перевести 2й байт в двоичный код и сложить с первым   
                Console.WriteLine(Convert.ToInt32(EncryptBin, 2));
                EncryptChar = (char)Convert.ToInt32(EncryptBin, 2); //перевести полученный двоичный код в символ
                
                result += EncryptChar;
            }
            Console.WriteLine("Зашифрованная строка = " + result);
            result = "";

            //5. Расшифрование сообщения. 
            //К каждому байту сообщения применить операцию XOR c соответствующим байтом гаммы.
            int[] DECRYPT = new int[ENCRYPT.Length];
            for (int i = 0; i < ENCRYPT.Length; i++)
            {
                DECRYPT[i] = ENCRYPT[i] ^ GAMMA[i];
            }

            for (int i = 0; i < DECRYPT.Length; i++)
            {
                string DecryptBin = "";
                char DecryptChar = '0';

                DecryptBin += DecToBin(DECRYPT[i].ToString()); //перевести 1й байт в двоичный код
                i++;

                //ДЛЯ АНГЛИЙСКИХ БУКВ (1 байт на символ)
                if (DecryptBin == "0") //если 1й байт оказался 0
                {
                    DecryptBin += DecToBin(DECRYPT[i].ToString()); //перевести 2й байт в двоичный код и сложить с первым

                    DecryptChar = (char)Convert.ToInt32(DecryptBin, 2); //перевести полученный двоичный код в символ
                    result += DecryptChar;
                }
                //ДЛЯ РУССКИХ БУКВ (2 байта на символ)
                else
                {
                    string DecryptBin_p1 = DecryptBin; //первый байт в двоичном коде
                    DecryptBin_p1 = CompleteZeroes(DecryptBin_p1, 8); //дописать вначале нули, чтобы стало 8 символов

                    string DecryptBin_p2 = DecToBin(DECRYPT[i].ToString()); //перевести 2й байт в двоичный код
                    DecryptBin_p2 = CompleteZeroes(DecryptBin_p2, 8); //дописать вначале нули, чтобы стало 8 символов

                    DecryptBin = DecryptBin_p1 + DecryptBin_p2; //сложить 1й и 2й байты в двоичном виде

                    DecryptChar = (char)Convert.ToInt32(DecryptBin, 2); //перевести полученный двоичный код в символ
                    result += DecryptChar;
                }
            }
            Console.WriteLine("Ответ = " + result);
        }

        //Перевести двоичную строку в десятичную
        static string BinToDec(string bin)
        {
            return Convert.ToInt32(bin, 2).ToString();
        }

        //Перевести десятичную строку в двоичную
        static string DecToBin(string bin)
        {
            return Convert.ToString(Int32.Parse(bin), 2);
        }

        //Дополнить двоичное число нулями, в соответствии с разрядом
        static string CompleteZeroes(string bin, int R)
        {
            if (bin.Length != R)
            {
                int dif = R - bin.Length;
                string difZeroes = "";

                for (int i = 0; i < dif; i++)
                {
                    difZeroes += "0";
                }

                return difZeroes + bin;
            }
            else
                return bin;
        }

        static void init()
        {
            for (int i = 0; i < 256; i++)
            {
                KEY[i] = i;
                GEN[i] = i;
            }

            //1. Создание ключа. Ключ KEY представляет собой перестановку массива целых чисел от 0 до 255
            Shuffle(KEY); // случайный ключ
            for (int i = 0; i < 256; i++)
            {
                GEN[i] = KEY[i];
            }

            //2. Инициализация генератора случайных чисел. 
            //Генератор случайных чисел GEN представляет собой массив целых чисел от 0 до 255. 
            //В качестве исходного его состояния необходимо взять случайный ключ, полученный в пункте (1). 
            //Далее состояние генератора инициализируется, то есть преобразуется по следующему правилу:
            int j = 0;
            int tmp = 0;
            for (int i = 0; i < 256; i++)
            {
                j = (j + GEN[i] + KEY[i]) % 256;

                tmp = GEN[i];
                GEN[i] = GEN[j];
                GEN[j] = tmp;
            }
        }

        //Случайно перемешать список из чисел
        public static Random rng = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
        public static void Shuffle(int[] list)
        {
            int n = list.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                int value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}