using System;
using System.IO;
using System.Collections.Generic;

class ReadCsv
{
    static List<string[]> ReadCsvFile(string filePath)
    {
        List<string[]> rows = new List<string[]>();

        try
        {

            string[] lines = File.ReadAllLines(filePath);


            foreach (string line in lines)
            {
                string[] values = line.Split(',');


                rows.Add(values);
            }

            Console.WriteLine("CSV file read successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return rows;
    }
    static void WriteCSVFile(string filePath, List<string[]> rows)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (string[] row in rows)
                {
                    writer.WriteLine(string.Join(",", row));
                }
            }
            Console.WriteLine("Ghi file CSV thanh cong!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Loi ghi file CSV !");
        }
    }
    static void Main()
    {

        string csvFilePath = @"C:\datasets\BigML_Dataset_5f50a62795a9306aa200003e.csv";
        List<string[]> csvData = ReadCsvFile(csvFilePath);
        Console.WriteLine("Doc 5 dong dau tien trong file: ");
        for (int i = 0; i < Math.Min(5, csvData.Count); i++)
        {
            Console.WriteLine(string.Join(" | ", csvData[i]));
        }
        string outputFilePath = @"C:\datasets\output.csv";
        WriteCSVFile(outputFilePath, csvData);

        Console.WriteLine("\n Hoan tat qua trinh ghi file");
    }
}
