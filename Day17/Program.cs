using System;
using Utils;

internal class Day17
{
    //static int _xMin = 20;
    //static int _xMax = 30;
    //static int _yMin = -10;
    //static int _yMax = -5;
    static int _xMin = 195;
    static int _xMax = 238;
    static int _yMin = -93;
    static int _yMax = -67;

    static double GetInitialXForPosition(int pos)
    {
        return 1f / 2f * (Math.Sqrt(8 * pos + 1f) - 1f);
    }

    static bool Test(int vx, int vy)
    {
        int x = 0;
        int y = 0;
        while (x <= _xMax && y >= _yMin)
        {
            x = x + vx;
            y = y + vy;
            vx = Math.Max(0, vx - 1);
            vy = vy - 1;
            if (x >= _xMin && x <= _xMax && y >= _yMin && y <= _yMax)
            {
                return true;
            }
        }
        return false;
    }

    // Main Method
    static public void Main(String[] args)
    {
        int count = 0;

        for (int x = (int)GetInitialXForPosition(_xMin);
                 x <= _xMax;
                 x++)
        {
            for (int y = -_yMin; y >= _yMin; y--)
            {
                if (Test(x, y))
                {
                    count++;
                }
            }
        }
        Console.WriteLine(count);
    }
}


