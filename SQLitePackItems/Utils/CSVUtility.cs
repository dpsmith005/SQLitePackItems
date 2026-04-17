using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLitePackItems
{
    public static class CSVUtility        
    {
        public static void ToCSV(this DataTable dtDataTable, string strFilePath)
        {
            StreamWriter sw = new StreamWriter(strFilePath, false);
            //headers
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            { 
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(","))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
        public static void ListToCSV<T>(List<T> genericList, string strFilePath)
        {
            var sb = new StringBuilder();
            //var basePath = AppDomain.CurrentDomain.BaseDirectory;
            //var finalPath = Path.Combine(basePath, fileName + ".csv");
            var header = "";
            var info = typeof(T).GetProperties();
            if (File.Exists(strFilePath))
            {
                File.Delete(strFilePath);
            } 
            // Create file and write header
            var file = File.Create(strFilePath);
            file.Close();
            foreach (var prop in typeof(T).GetProperties())
            {
                header += prop.Name + ",";
            }
            header = header.Substring(0, header.Length - 1);
            sb.AppendLine(header);
            TextWriter sw = new StreamWriter(strFilePath, true);
            sw.Write(sb.ToString());
            sw.Close();

            foreach (var obj in genericList)
            {
                sb = new StringBuilder();
                var line = "";
                foreach (var prop in info)
                {
                    line += prop.GetValue(obj, null) + "; ";
                }
                line = line.Substring(0, line.Length - 2);
                sb.AppendLine(line);
                TextWriter sw2 = new StreamWriter(strFilePath, true);
                sw2.Write(sb.ToString());
                sw2.Close();
            }
        }
       }
}
