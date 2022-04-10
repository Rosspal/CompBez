using lab3;

namespace Lab3
{
    class Program
    {
        static char[] abc = new char[34];    //алфавит
        static string stroka; //слово для зашифровки
        static int size = 10;

        static void Main(string[] args)
        {
            
            

            Matrix WordMatrix;    //слово для зашифровки в виде матрицы
            Matrix HelpMatrix;    //вспомогательная матрица
            Matrix HelpMatrixInv;//вспомогательная обратная по модулю матрица
            Matrix EncryptMatrix; // зашифрованное слово в виде матрицы
            Matrix DecryptMatrix; // расшифрованное слово в виде матрицы

            InitAbc();
            //Console.WriteLine("Введите строку");
            //stroka = Console.ReadLine();
            //stroka = stroka.ToUpper();
            //stroka = stroka.PadRight(size * size,' ');


            //WordMatrix = new Matrix(size, size);
            //int n = 0;
            //int char_position = 0;

            //// состовляем матрицу по строке
            //for (int i = 0; i < WordMatrix.Row; i++)
            //{
            //    for (int j = 0; j < WordMatrix.Column; j++)
            //    {
            //        char_position = FindCharInAbc(stroka[n]);
            //        WordMatrix.array[i, j] = char_position;
            //        n++;
            //    }
            //}

            //Console.WriteLine("Строка в матрице");
            //Console.WriteLine(WordMatrix.ToString());

            HelpMatrix = GenerateMatrix(3, abc.Length);
            HelpMatrixInv = HelpMatrix.Inverse();

            Console.WriteLine(HelpMatrixInv);
        }

        static Matrix GenerateMatrix(int m, int N)
        {
            Matrix matrix = new Matrix(m, m);
            matrix.array[0, 0] = 6; matrix.array[0, 1] = 27; matrix.array[0, 2] = 1;
            matrix.array[1, 0] = 13; matrix.array[1, 1] = 16; matrix.array[1, 2] = 32;
            matrix.array[2, 0] = 28; matrix.array[2, 1] = 17; matrix.array[2, 2] = 15;

            return matrix;
        }

        static int FindCharInAbc(char C)
        {
            for (int i = 0; i < abc.Length; i++)
            {
                if (C == abc[i])
                {
                    return i;
                }
            }
            return 0;
        }

        static void InitAbc()
        {
            abc = new char[34];

            abc[0] = 'А';
            abc[1] = 'Б';
            abc[2] = 'В';
            abc[3] = 'Г';
            abc[4] = 'Д';

            abc[5] = 'Е';
            abc[6] = 'Ё';
            abc[7] = 'Ж';
            abc[8] = 'З';
            abc[9] = 'И';

            abc[10] = 'Й';

            abc[11] = 'К';
            abc[12] = 'Л';
            abc[13] = 'М';
            abc[14] = 'Н';

            abc[15] = 'О';
            abc[16] = 'П';
            abc[17] = 'Р';
            abc[18] = 'С';
            abc[19] = 'Т';

            abc[20] = 'У';
            abc[21] = 'Ф';
            abc[22] = 'Х';
            abc[23] = 'Ц';
            abc[24] = 'Ч';

            abc[25] = 'Ш';
            abc[26] = 'Щ';

            abc[27] = 'Ъ';

            abc[28] = 'Ы';

            abc[29] = 'Ь';

            abc[30] = 'Э';
            abc[31] = 'Ю';
            abc[32] = 'Я';
            abc[33] = ' ';
        }
    }
}