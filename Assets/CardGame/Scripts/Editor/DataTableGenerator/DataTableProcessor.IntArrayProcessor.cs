using System;
using System.Collections.Generic;
using System.IO;

namespace CardGame.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class IntArrayProcessor:GenericDataProcessor<List<int>>
        {
            public override bool IsSystem { get=>false; }
            public override string LanguageKeyword { get=>"List<int>"; }
            public override int listCount { get; set; }

            public override string[] GetTypeStrings()
            {
                return new[]
                {
                    "List<int>"
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

            public override List<int> Parse(string value)
            {
                List<int> temp = new List<int>();
                if (value=="null")
                {
                    return temp;
                }
                string[] splitedValue = value.Split(',');
                foreach (var t in splitedValue)
                {
                    temp.Add(int.Parse(t));;
                }

                listCount = temp.Count;
                return temp;
            }
        }
    }
}