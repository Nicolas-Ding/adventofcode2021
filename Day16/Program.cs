internal class Day16
{
    // Main Method
    static public void Main(String[] args)
    {
        var file = Utils.Utils.ReadLinesFromFile("input.txt");
        var byteArray = file.First().Select(_ => Convert.ToByte(_.ToString(), 16)).ToArray();

        BitCodeProgram program = new BitCodeProgram(byteArray);

        Console.WriteLine(program);
        Console.WriteLine(program.ReadPacket());
    }
    
}
