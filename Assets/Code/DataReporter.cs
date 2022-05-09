using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;

/// <summary>
/// This class handles all the reports and formats them for the outgoing file
/// </summary>
public class DataReporter
{
    private List<string> header = new List<string>() { "t", "x(t)", "y(t)","z(t)", "v_x(t)", "v_y(t)", "v_z(t)", "cube", "stage" };
    private List<Dictionary<string, string>> cubeData = new List<Dictionary<string, string>>();


    /// <summary>
    /// Reports the data of the cube object in order of <time, position, velocity, acceleration, cube>
    /// </summary>
    /// <param name="data">of the cube in defined order</param>
    /// <param name="reporterName">name of cube</param>
    public void ReportData(List<float> data, string reporterName, string stagename)
    {
        List<string> processedData = new List<string>();
        data.ForEach(d => processedData.Add(d.ToString()));
        processedData.Add(reporterName);
        processedData.Add(stagename);

        Dictionary<string, string> dataDict = new Dictionary<string, string>();
        var zippedData = header.Zip(processedData, (h, d) => new { Header = h, Data = d});

        foreach(var x in zippedData)
        {
            dataDict.Add(x.Header, x.Data);
        }

        cubeData.Add(dataDict);
    }

    /// <returns>Formatted header and data for outgoing file</returns>
    public string GetText()
    {
        return GetHeaderLine() + GetDataLines();
    }

    private string GetHeaderLine()
    {
        return string.Join(",", header);
    }

    private string GetDataLines()
    {
        string ret = "";

        foreach (var dataEntry in cubeData)
        {
            var orderedValues = header.Select(h => dataEntry[h]);
            ret += Environment.NewLine + string.Join(",", orderedValues);
        }

        return ret;
    }
}
