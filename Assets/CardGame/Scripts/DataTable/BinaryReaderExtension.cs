using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CardGame
{
    public static class BinaryReaderExtension
    {
        public static Vector4 ReadVector4(this BinaryReader binaryReader)
        {
            return new Vector4(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        }

        public static List<int> ReadInt32List(this BinaryReader binaryReader,int len)
        {
            var temp = new List<int>(len);
            for (int i = 0; i < len; i++)
            {
                temp.Add(binaryReader.ReadInt32());
            }
            return temp;
        }
        public static List<float> ReadSingleList(this BinaryReader binaryReader,int len)
        {
            var temp = new List<float>(len);
            for (int i = 0; i < len; i++)
            {
                temp.Add(binaryReader.ReadSingle());
            }
            return temp;
        }
    }
}