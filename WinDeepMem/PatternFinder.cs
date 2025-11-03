namespace WinDeepMem
{
    public class PatternFinder
    {
        public static byte[] Transform(string pattern)
        {
            // Удаляем лишние пробелы и разделяем
            string[] parts = pattern.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] result = new byte[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] == "?")
                {
                    result[i] = 0xFF; // wildcard
                }
                else
                {
                    // Проверяем, что это hex-число
                    if (parts[i].Length != 2)
                        throw new ArgumentException($"Invalid hex byte: {parts[i]}");

                    result[i] = Convert.ToByte(parts[i], 16);
                }
            }
            return result;
        }

        public static bool Find(byte[] data, byte[] pattern, out long offsetFound, long offset = 0)
        {
            offsetFound = -1;
            if (data == null || pattern == null) return false;
            if (data.Length == 0 || pattern.Length == 0) return false;

            for (long i = offset; i <= data.LongLength - pattern.LongLength; i++)
            {
                bool match = true;

                for (long j = 0; j < pattern.LongLength; j++)
                {
                    // 0xFF = wildcard, пропускаем проверку
                    if (pattern[j] != 0xFF && data[i + j] != pattern[j])
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    offsetFound = i;
                    return true;
                }
            }

            return false;
        }
    }
}
