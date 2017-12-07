using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DBComparator.DBConection;

namespace DBComparator
{
    class Program
    {
        static void Main(string[] args)
        {
            UInt64 words = 0, compared = 0;
            DBConection db = new DBConection();
            List<DBKeyword> db1 = db.list(Con.CON1);
            List<DBKeyword> db2 = db.list(Con.CON2);
            List<String> buf = new List<string>();
          

            Console.WriteLine("Количество элементов в 1й БД "+db1.Count);
            Console.WriteLine("Количество элементов во 2й БД " + db2.Count);
            Console.ReadLine();
            //foreach (DBKeyword kw in db.list(Con.CON2))
            //{
            //    if (db.isExist(kw, Con.CON1))
            //    {
            //        Console.WriteLine("-------------->"+ words + "[" + kw.keyword + "] найдено в обоих БД");
            //        compared++;
            //    }
            //    else
            //        Console.WriteLine(words+" [" + kw.keyword + "] НЕ найдено в обоих БД");

            //    words++;
            //}
            foreach (DBKeyword kw1 in db1)
            {
                bool flag = false;
                foreach (DBKeyword kw2 in db2)
                {
                    if (kw1.keyword.Equals(kw2.keyword.Trim()))
                    {
                        compared++;
                        buf.Add(kw1.keyword);
                        flag = true;
                        break;
                    }
                }

                Console.WriteLine(flag?"--->Найдено слово ["+kw1.keyword+"]": "НЕ Найдено слово [" + kw1.keyword + "]");
                words++;
            }
            Console.WriteLine("Просмотрено слов "+words+" из них совпало "+compared);
            File.WriteAllLines("log.txt", buf.ToArray());
            Console.ReadLine();
            buf.Clear();

            foreach (DBKeyword kw in db2)
                buf.Add(kw.keyword);

            File.WriteAllLines("log_db2.txt", buf.ToArray());

            buf.Clear();
            foreach (DBKeyword kw in db1)
                buf.Add(kw.keyword);

            File.WriteAllLines("log_db1.txt", buf.ToArray());
        }
    }
}
