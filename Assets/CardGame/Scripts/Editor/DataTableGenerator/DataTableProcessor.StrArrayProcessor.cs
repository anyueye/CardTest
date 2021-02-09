using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CardGame.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class StrArrayProcessor:GenericDataProcessor<List<string>>
        {
            public override bool IsSystem { get=>false; }
            public override string LanguageKeyword { get=>"List<string>"; }
            public override int listCount { get; set; }

            public override string[] GetTypeStrings()
            {
                return new[]
                {
                    "List<string>"
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

            public override List<string> Parse(string value)
            {
                List<string> temp = new List<string>();
                if (value=="null")
                {
                    return temp;
                }
                string[] splitedValue = value.Split(',');
                temp = splitedValue.ToList();
                listCount = temp.Count;
                return temp;
            }
        }
    }
}