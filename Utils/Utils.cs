namespace Utils
{
    public static class Utils
    {
        public static IEnumerable<string> ReadLinesFromConsole()
        {
            string line = Console.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                yield return line;
                line = Console.ReadLine();
            }
        }

        public static IEnumerable<string> ReadLinesFromFile(string filename)
        {
            using StreamReader file = new StreamReader(filename);
            string line = file.ReadLine();
            while (line != null)
            {
                yield return line;
                line = file.ReadLine();
            }
        }

        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
            => self.Select((item, index) => (item, index)); 

        public static T[][] Transpose<T>(this T[][] array)
        {
            int h = array.Length;
            int w = array[0].Length;

            T[][] result = new T[w][];
            for (int i = 0; i < w; i++)
            {
                result[i] = new T[h];
            }

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[i][j] = array[j][i];
                }
            }

            return result;
        }
    }
}