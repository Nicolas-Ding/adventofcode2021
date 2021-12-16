using System.Collections;


public class BitCodeProgram
{
    private List<bool> bitArray = new List<bool>();

    public BitCodeProgram(byte[] bytes)
    {
        foreach (var b in bytes)
        {
            BitArray array = new BitArray(new int[] { b });
            bitArray.Add(array[3]);
            bitArray.Add(array[2]);
            bitArray.Add(array[1]);
            bitArray.Add(array[0]);
        }
    }

    public static Dictionary<int, Func<long, long, long>> _operators = new Dictionary<int, Func<long, long, long>>
    {
        [0] = (x, y) => x + y,
        [1] = (x, y) => x * y,
        [2] = (x, y) => Math.Min(x, y),
        [3] = (x, y) => Math.Max(x, y),
        [5] = (x, y) => x > y ? 1 : 0,
        [6] = (x, y) => x < y ? 1 : 0,
        [7] = (x, y) => x == y ? 1 : 0
    };

    public override string ToString()
    {
        return string.Join(" ", bitArray.Select(_ => _ ? '1': '0'));
    }

    /// <summary>
    /// Reads one packet and returns 1) the sum of the versions ids and 2) the new position at the start of the next packet
    /// </summary>
    public (int nextPosition, int versionIds, long result) ReadPacket(int position = 0)
    {
        int versionIds = 0;
        int newPosition = position;
        long result = 0;
        versionIds += GetIntFromPositions(position, 3); // version number

        int packageTypeId = GetIntFromPositions(position + 3, 3);
        switch (packageTypeId)
        {
            case 4:
                newPosition = position + 6;
                while (bitArray[newPosition] == true)
                {
                    result = AddIntFromPositions(newPosition + 1, 4, result);
                    newPosition += 5;
                }
                result = AddIntFromPositions(newPosition + 1, 4, result);
                newPosition += 5;
                break;
            default: // An operator for now
                bool lengthTypeId = bitArray[position + 6];
                var op = _operators[packageTypeId];
                if (lengthTypeId == false)
                {
                    int totalLengthInBytes = GetIntFromPositions(position + 7, 15);
                    newPosition = position + 22;
                    // extract the first number out of the loop to initialize
                    (newPosition, int v, long r) = ReadPacket(newPosition);
                    versionIds += v;
                    result = r;

                    while (newPosition < position + 22 + totalLengthInBytes)
                    {
                        (newPosition, v, r) = ReadPacket(newPosition);
                        versionIds += v;
                        result = op(result, r);
                    }
                }
                else
                {
                    int subPacketsNumber = GetIntFromPositions(position + 7, 11);
                    newPosition = position + 18;

                    // extract the first number out of the loop to initialize
                    (newPosition, int v, long r) = ReadPacket(newPosition);
                    versionIds += v;
                    result = r;

                    for (int i = 1; i < subPacketsNumber; i++)
                    {
                        (newPosition, v, r) = ReadPacket(newPosition);
                        versionIds += v;
                        result = op(result, r);
                    }
                }
                break;
        }

        return (newPosition, versionIds, result);
    }

    /// <summary>
    /// Reads a 5-bits value from a position and indicates if the value is terminating (starts with 0) or not. 
    /// </summary>
    /// <returns>0 if the value is terminating</returns>
    public bool ReadValue(int position = 0)
    {
        return bitArray[position];
    }

    public int GetIntFromPositions(int start, int count)
    {
        int value = 0;
        for (int i = start; i < start + count; i++)
        {
            value *= 2;
            value += bitArray[i] ? 1 : 0;
        }
        return value;
    }

    public long AddIntFromPositions(int start, int count, long value)
    {
        for (int i = start; i < start + count; i++)
        {
            value *= 2;
            value += bitArray[i] ? 1 : 0;
        }
        return value;
    }
}

