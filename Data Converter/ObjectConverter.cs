using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Messanger.Data.ObjectConverterService
{
    public class ObjectConverter /*: IObjectConverter*/
    {
        //public byte[] ObjectToBytes(object sender)
        //{
        //    if (sender == null)
        //    {
        //        Console.WriteLine("Conversion is failed.");
        //        return null;
        //    }

        //    BinaryFormatter binaryFormatter = new BinaryFormatter();
        //    MemoryStream memoryStream = new MemoryStream();
        //    binaryFormatter.Serialize(memoryStream, sender);

        //    Console.WriteLine($"{memoryStream} - converted to bytes successfully.");
        //    return memoryStream.ToArray();
        //}

        //public object BytesToObject(byte[] bytes)
        //{
        //    if (bytes == null)
        //    {
        //        Console.WriteLine("Conversion is failed.");
        //        return null;
        //    }

        //    BinaryFormatter binaryFormatter = new BinaryFormatter();
        //    MemoryStream memoryStream = new MemoryStream(bytes);
        //    object getter = binaryFormatter.Deserialize(memoryStream);

        //    Console.WriteLine("Converted to object successfully.");
        //    return getter;
        //}
    }
}
