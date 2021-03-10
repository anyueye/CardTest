using System;
using System.Collections.Generic;
using System.IO;

namespace CardGame.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class FloatArrayProcessor:GenericDataProcessor<List<float>>
        {
            public override bool IsSystem { get=>false; }
            public override string LanguageKeyword { get=>"List<float>"; }
            public override int listCount { get; set; }

            public override string[] GetTypeStrings()
            {
                return new[]
                {
                    "List<float>"
                };
            }
            
            public override void WriteToStream(DataTableProcessor ddataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                var list = Parse(value);
                foreach (var t in list)
                {
                    binaryWriter.Write(t);
                }
            }

            public override List<float> Parse(string value)
            {
                List<float> temp = new List<float>();
                if (value=="null")
                {
                    return temp;
                }
                string[] splitedValue = value.Split('|');
                foreach (var t in splitedValue)
                {
                    temp.Add(float.Parse(t));;
                }

                listCount = temp.Count;
                return temp;
            }
        }
    }
}