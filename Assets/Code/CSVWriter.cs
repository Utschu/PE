using System.Collections;
using System.Collections.Generic;

using System.IO;

/// <summary>
/// This class is responsible to write the reported data to the csv file
/// </summary>
public class CSVWriter
{
    /// <summary>
    /// Writes all the reported data into a csv.
    /// </summary>
    /// <param name="reporter">the reporter object with the reported data</param>
    public static void WriteToCSV(DataReporter reporter)
    {
        using (var streamWriter = new StreamWriter("time_series.csv"))
        {
            streamWriter.WriteLine(reporter.GetText());
            streamWriter.Flush();
        }
    }
}