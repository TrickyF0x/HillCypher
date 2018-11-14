using System;

namespace HillConVer
{
    class Program
    {
        static void Main(string[] args)
        {
            string text;
            int variant = 0, method = 0, size = 0, i = 0, j = 0;
            int[,] key = new int[3, 3];
            int[,] key_old = new int[3, 3];
            int[,] key_new = new int[3, 3];
            char[] blok = new char[3];
            Console.WriteLine("Здравствуйте! Что Вы хотите сделать?");
            while (true)
            {
                while (true)
                {
                    Console.WriteLine("1 - зашифровать / 2 - расшифровать");
                    try
                    {
                        variant = Int32.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Вводите числа!");
                    }
                    if (variant == 1 || variant == 2)
                        break;
                }

                while (true)
                {
                    Console.WriteLine("1 - обычный / 2 - рекуррентный");
                    try
                    {
                        method = Int32.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Вводите числа!");
                    }
                    if (method == 1 || method == 2)
                        break;
                }

                while (true)
                {
                    try
                    {
                        Console.WriteLine("Введите ключевую матрицу #1");
                        for (i = 0; i < 3; ++i)
                        {
                            for (j = 0; j < 3; ++j)
                            {
                                Console.Write((i + 1).ToString() + (j + 1).ToString() + ") ");
                                key_old[i, j] = Int32.Parse(Console.ReadLine());
                            }
                        }
                        if (method == 2)
                        {
                            Console.WriteLine("Введите ключевую матрицу #2");
                            for (i = 0; i < 3; ++i)
                            {
                                for (j = 0; j < 3; ++j)
                                {
                                    Console.Write((i + 1).ToString() + (j + 1).ToString() + ") ");
                                    key[i, j] = Int32.Parse(Console.ReadLine());
                                }
                            }
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Вводите числа!");
                    }
                    if(method == 1)
                        if (determ(key_old) != 0)
                            break;
                        else
                            Console.WriteLine("Повторите ввод, нулевой детерминант");
                    else
                        if (determ(key_old) != 0 && determ(key) != 0)
                        break;
                    else
                        Console.WriteLine("Повторите ввод, нулевой детерминант");
                }
                if (variant == 1)
                {
                    Console.Write("Введите текст для шифрования: ");
                    text = Console.ReadLine();
                    text = text.ToLower();
                    while (text.Length % 3 != 0)
                        text += " ";
                    size = text.Length / 3;
                    if (method == 1)
                    {
                        int cnt = 0;
                        while (size != 0)
                        {
                            for (j = 0; j < 3; ++j)
                            {
                                blok[j] = text[cnt];
                                ++cnt;
                            }
                            Console.Write(crypt(blok, key_old));
                            --size;
                        }
                        Console.WriteLine();
                    }
                    else//crypt recur
                    {
                        int cnt = 0;
                        while (size != 0)
                        {
                            for (j = 0; j < 3; ++j)
                            {
                                blok[j] = text[cnt];
                                ++cnt;
                            }                            
                            
                            if (size == text.Length / 3)
                                Console.Write(crypt(blok, key_old));
                            else if (size == text.Length / 3 - 1)
                                Console.Write(crypt(blok, key));
                            else
                            {
                                key_new = key_gen(key, key_old);
                                key_old = key;
                                key = key_new;
                                Console.Write(crypt(blok, key_new));
                            }
                            --size;
                        }
                        Console.WriteLine();
                    }
                }
                else//decrypt
                {
                    Console.Write("Введите текст для расшифрования: ");
                    text = Console.ReadLine();
                    text = text.ToLower();
                    while (text.Length % 3 != 0)
                        text += " ";
                    size = text.Length / 3;
                    if (method == 1)
                    {
                        int cnt = 0;
                        while (size != 0)
                        {
                            for (j = 0; j < 3; ++j)
                            {
                                blok[j] = text[cnt];
                                ++cnt;
                            }
                            Console.Write(decrypt(blok, key_old));
                            --size;
                        }
                        Console.WriteLine();
                    }
                    else//decrypt recur
                    {
                        int cnt = 0;
                        while (size != 0)
                        {
                            for (j = 0; j < 3; ++j)
                            {
                                blok[j] = text[cnt];
                                ++cnt;
                            }
                            if (size == text.Length / 3)
                            {
                                Console.Write(decrypt(blok, key_old));
                            }
                            else if (size == text.Length / 3 - 1)
                            {
                                Console.Write(decrypt(blok, key));
                                key_old = recurse(key_old);
                                key = recurse(key);
                            }
                            else
                            {
                                key_new = key_gen(key_old, key);                            
                                key_old = key;
                                key = key_new;
                                Console.Write(crypt(blok, key_new));
                            }
                            --size;
                        }
                        Console.WriteLine();
                    }//decrypt recur
                }
                Console.WriteLine("Хотите продолжить работу? да / нет");
                if (Console.ReadLine() == "нет")
                    break;
            }//main

            int[,] recurse(int[,] matrix)
            {
                int[,] recursed = new int[3, 3];
                int det = 0, eucl = 0;

                det = determ(matrix);
                eucl = evklid(det, 37);

                if (det > 0 && eucl < 0)
                    eucl = eucl + 37;
                else if (det < 0 && eucl < 0)
                    eucl = eucl * -1;

                recursed[0, 0] = (((matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1]) % 37) * eucl) % 37;
                recursed[1, 0] = (((-1 * (matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0])) % 37) * eucl) % 37;
                recursed[2, 0] = (((matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]) % 37) * eucl) % 37;
                recursed[0, 1] = (((-1 * (matrix[0, 1] * matrix[2, 2] - matrix[0, 2] * matrix[2, 1])) % 37) * eucl) % 37;
                recursed[1, 1] = (((matrix[0, 0] * matrix[2, 2] - matrix[0, 2] * matrix[2, 0]) % 37) * eucl) % 37;
                recursed[2, 1] = (((-1 * (matrix[0, 0] * matrix[2, 1] - matrix[0, 1] * matrix[2, 0])) % 37) * eucl) % 37;
                recursed[0, 2] = (((matrix[0, 1] * matrix[1, 2] - matrix[0, 2] * matrix[1, 1]) % 37) * eucl) % 37;
                recursed[1, 2] = (((-1 * (matrix[0, 0] * matrix[1, 2] - matrix[0, 2] * matrix[1, 0])) % 37) * eucl) % 37;
                recursed[2, 2] = (((matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0]) % 37) * eucl) % 37;

                for (i = 0; i < 3; ++i)
                {
                    for (j = 0; j < 3; ++j)
                    {
                        if (recursed[i, j] < 0)
                            recursed[i, j] += 37;
                    }
                }

                return recursed;
            }

            int determ(int[,] matrix)
            {
                return (matrix[0, 0] * matrix[1, 1] * matrix[2, 2] + matrix[2, 0] * matrix[0, 1] * matrix[1, 2] + matrix[1, 0] * matrix[2, 1] * matrix[0, 2]
                  - matrix[0, 2] * matrix[1, 1] * matrix[2, 0] - matrix[2, 1] * matrix[1, 2] * matrix[0, 0] - matrix[1, 0] * matrix[0, 1] * matrix[2, 2]);                    
            }//determ

            char char_index_finder(int index)
            {
                char[] alf = {'а','б','в','г','д','е','ё','ж','з','и','й','к','л','м','н',
                            'о','п','р','с','т','у','ф','х','ц','ч','ш','щ','ъ','ы','ь','э','ю','я','.',',',' ','?'};
                return alf[index];

            }//char index_finder

            int index_finder(char leter)
            {
                int ch = 0;
                char[] alh = {'а','б','в','г','д','е','ё','ж','з','и','й','к','л','м','н',
                            'о','п','р','с','т','у','ф','х','ц','ч','ш','щ','ъ','ы','ь','э','ю','я','.',',',' ','?'};
                for (ch = 0; ch < 37; ++ch)
                    if (alh[ch] == leter)
                        break;
                return ch;
            }//index_finder

            int[,] key_gen(int[,] key_matrixx, int[,] key_matrix_old)
            {
                int[,] new_matrixx = new int[3, 3];
                int sum = 0;
                for ( i = 0; i < 3; i++)  // умножение
                {
                    for ( j = 0; j < 3; j++)
                    {
                        for (int r = 0; r < 3; r++)
                        {
                            sum += key_matrixx[i, r] * key_matrix_old[r, j];
                        }
                        new_matrixx[i, j] = sum % 37;
                        sum = 0;
                    }
                }
                return new_matrixx;          
            }//key gen

            string crypt(char[] txt, int[,] key_matrix)
            {
                int[] c = new int[3];
                c[0] = (key_matrix[0, 0] * index_finder(txt[0]) +
                    key_matrix[1, 0] * index_finder(txt[1]) +
                    key_matrix[2, 0] * index_finder(txt[2])) % 37;
                c[1] = (key_matrix[0, 1] * index_finder(txt[0]) +
                    key_matrix[1, 1] * index_finder(txt[1]) +
                    key_matrix[2, 1] * index_finder(txt[2])) % 37; 
                c[2] = (key_matrix[0, 2] * index_finder(txt[0]) +
                    key_matrix[1, 2] * index_finder(txt[1]) +
                    key_matrix[2, 2] * index_finder(txt[2])) % 37;
                return char_index_finder(c[0]).ToString() + char_index_finder(c[1]).ToString() + char_index_finder(c[2]).ToString();
            }//crypt

            string decrypt(char[] txt, int[,] key_matrix)
            {
                int det=0, eucl=0;
                int[,] minor = new int[3, 3];
                det = determ(key_matrix);
                eucl = evklid(det, 37);

                if (det > 0 && eucl < 0)
                    eucl = eucl + 37;
                else if (det < 0 && eucl < 0)
                    eucl = eucl * -1;

                minor[0, 0] = (((key_matrix[1, 1] * key_matrix[2, 2] - key_matrix[1, 2] * key_matrix[2, 1]) % 37) * eucl) % 37;
                minor[1, 0] = (((-1 *(key_matrix[1, 0] * key_matrix[2, 2] - key_matrix[1, 2] * key_matrix[2, 0])) % 37) *eucl) % 37;
                minor[2, 0] = (((key_matrix[1, 0] * key_matrix[2, 1] - key_matrix[1, 1] * key_matrix[2, 0]) % 37) *eucl) % 37;
                minor[0, 1] = (((-1 * (key_matrix[0, 1] * key_matrix[2, 2] - key_matrix[0, 2] * key_matrix[2, 1])) % 37) *eucl) % 37;
                minor[1, 1] = (((key_matrix[0, 0] * key_matrix[2, 2] - key_matrix[0, 2] * key_matrix[2, 0]) % 37) *eucl) % 37;
                minor[2, 1] = (((-1 * (key_matrix[0, 0] * key_matrix[2, 1] - key_matrix[0, 1] * key_matrix[2, 0])) % 37) *eucl) % 37;
                minor[0, 2] = (((key_matrix[0, 1] * key_matrix[1, 2] - key_matrix[0, 2] * key_matrix[1, 1]) % 37) *eucl) % 37;
                minor[1, 2] = (((-1 * (key_matrix[0, 0] * key_matrix[1, 2] - key_matrix[0, 2] * key_matrix[1, 0])) % 37) *eucl) % 37;
                minor[2, 2] = (((key_matrix[0, 0] * key_matrix[1, 1] - key_matrix[0, 1] * key_matrix[1, 0]) % 37) *eucl) % 37;
                
                for (i = 0; i < 3; ++i)
                {
                    for (j = 0; j < 3; ++j)
                    {
                        if(minor[i, j] < 0)
                            minor[i, j] += 37;
                    }
                }
                
                return crypt(txt, minor);
            }//decrypt

            int evklid(int a, int b)
            {
                int q, r, x, y, x1 = 0, y1 = 1, x2 = 1, y2 = 0;
                while (true)
                {
                    q = a / b;
                    r = a % b;
                    x = x2 - q * x1;
                    y = y2 - q * y1;
                    a = b;
                    b = r;                    
                    x2 = x1;
                    x1 = x;
                    y2 = y1;
                    y1 = y;
                    if (a == 0 || b == 0)
                        break;
                }
                return x2;
            }//evklid
        }
    }
}