﻿using System.IO;

namespace CardGame.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class IdProcessor : DataProcessor
        {
            public override System.Type Type
            {
                get
                {
                    return typeof(int);
                }
            }

            public override bool IsId
            {
                get
                {
                    return true;
                }
            }

            public override bool IsComment
            {
                get
                {
                    return false;
                }
            }

            public override bool IsSystem
            {
                get
                {
                    return false;
                }
            }

            public override string LanguageKeyword
            {
                get
                {
                    return "int";
                }
            }

            public override int listCount { get; set; }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "id"
                };
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write7BitEncodedInt32(int.Parse(value));
            }
        }
    }
}