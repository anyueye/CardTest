using System.Text;

namespace CardGame.Editor.DataTableTools
{
    public delegate void DataTableCodeGenerator(DataTableProcessor dataTableProcessor, StringBuilder codeContent, object userData);
}