using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using Xceed.Document.NET;
using Xceed.Words.NET;
using ReceiveMaker.Models;
using Microsoft.AspNetCore.Http;

namespace ReceiveMaker.Modules.ReceiveCreator
{
    public class ReceiveCreator
    {
        static List<List<string>> MakeTableData(Row row, Item item)
        {
            List<List<string>> TableData = new();

            foreach (var cell in row.Cells)
            {
                List<string> cellData = new();

                foreach (var field in item.Fields)
                {
                    if (cell.FindUniqueByPattern(field.Key,RegexOptions.IgnoreCase).Count != 0)
                    {
                        cellData = field.Values;
                    }
                }

                TableData.Add(cellData);
            }
            return TableData;
        }

        static Row FillRow(Row row, Item item, int index)
        {

            foreach(var field in item.Fields)
            {
                if(row.FindUniqueByPattern(field.Key, RegexOptions.IgnoreCase).Count != 0)
                {
                    for(int i = 0; i < row.FindUniqueByPattern(field.Key, RegexOptions.IgnoreCase).Count; i++)
                    {
                        row.ReplaceText(field.Key, field.Values[index]);
                    }
                }
            }
            return row;
        }

        static void FillDataInTable(Item item, Table table, int index)
        {

            for (int valueIndex = 0; valueIndex < item.Fields[0].Values.Count; valueIndex++)
            {
                Row row = table.Rows[index];
                Row insertingRow = FillRow(row, item, valueIndex);

                table.InsertRow(insertingRow, valueIndex + index + 1, true);


            }
            table.RemoveRow(index);

        }
        static void FillField(FieldField field, Document document)
        {
            int count = document.FindUniqueByPattern(field.Key, RegexOptions.IgnoreCase).Count;
            if (count > 0)
            {
                // Do the replacement of all the found tags and with green bold strings.
                for (int i = 0; i < count; ++i)
                {
                    document.ReplaceText(field.Key, field.Value, false, RegexOptions.IgnoreCase, new Formatting() { });
                }
                // Save this document to disk.
                //document.SaveAs("ReplacedText.docx");

            }
        }
        static void FillFields(List<FieldField> fields, Document document)
        {
            fields = fields.OrderByDescending(x => x.Key).ToList();
            foreach (var field in fields)
            {
                FillField(field, document);
            }
        }
        static void tableWork(Item item, Document document)
        {
            foreach (var table in document.Tables)
            {
                
                for(int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i].Cells[0].Paragraphs[0].Text.Contains(item.Key))
                    {
                        table.Rows[i].Cells[0].Paragraphs[0].Remove(false);


                        FillDataInTable(item, table, i);
                        break;
                    }
                }
                

            }
        }
        public static MemoryStream MakeReceive(ReceiveTemplateModel model)
        {
            using (var document = DocX.Load(model.templatePath))
            {

                foreach (var item in model.RepeatingFields)
                {
                    tableWork(item, document);

                }
                FillFields(model.Fields, document);

                Directory.CreateDirectory("templates");
                MemoryStream memoryStream = new MemoryStream();
                document.SaveAs(memoryStream);
                memoryStream.Position = 0;
                return memoryStream;

            }
        }
        public static string saveTemplate(IFormFile file, string templateName)
        {
            string saveLocation = Directory.GetCurrentDirectory()+ "templates/" + templateName + ".docx";
            using (var fstream = new FileInfo(file.FileName).Create())
            {
                file.CopyTo(fstream);

                return saveLocation;
            }
            return null;
        }
        public static string saveTemplate(FileStream file, string templateName)
        {
            string saveLocation = Directory.GetCurrentDirectory() + "templates/" + templateName + ".docx";
            using (var fstream = new FileInfo(saveLocation).Create())
            {
                file.CopyTo(fstream);

                return saveLocation;
            }
            throw new Exception("No save");
        }


    }
}

