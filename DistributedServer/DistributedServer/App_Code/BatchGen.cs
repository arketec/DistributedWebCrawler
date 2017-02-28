using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Text;
using System.Net.Http;

using DistributedServer.Models;

namespace DistributedServer.App_Code
{
    public class BatchGen
    {
        public List<string> splitDocument(string input, int size, bool isOutputAsCsl)
        {
            var delimiters = new char[] { ',', '\n' };
            var stringArray = input.Split(delimiters);

            var listSize = size;
            var totalLines = stringArray.Length;
            int totalLists = (int)totalLines / listSize;
            int remainder = totalLines % listSize;
            int fudgeNumber = (remainder == 0) ? 0 : 1;

            List<string> result = new List<string>();

            int lineNum = 0;
            for (int i = 0; i < totalLists + fudgeNumber; i++)
            {
                if (i == totalLists)
                {
                    var builder = new StringBuilder();
                    for (int j = 0; j < remainder; j++)
                    {
                        if (isOutputAsCsl)
                            builder.Append(stringArray[lineNum] + ",");
                        else
                            builder.AppendLine(stringArray[lineNum]);
                        lineNum++;
                    }
                    result.Add(builder.ToString());
                    builder.Clear();
                }
                else
                {
                    var builder = new StringBuilder();
                    for (int j = 0; j < listSize; j++)
                    {
                        if (isOutputAsCsl)
                            builder.Append(stringArray[lineNum] + ",");
                        else
                            builder.AppendLine(stringArray[lineNum]);
                        lineNum++;
                    }
                    result.Add(builder.ToString());
                    builder.Clear();
                }
            }

            return result;
        }


        public bool saveDocument(string document, string serverPath, string dirName, string name)
        {
            var path = serverPath + @"\Generated_Lists" + @"\" + dirName;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = path + @"\" + name + @".txt";
            if (File.Exists(path))
                return false;

            var writer = new StreamWriter(path, false);
            writer.Write(document);
            writer.Close();

            var pathObj = new PathObject();
            pathObj.id = MasterDictionary.GetCount() + 1;
            pathObj.dirName = dirName;
            pathObj.path = path;
            pathObj.isAvail = (dirName == name) ? false : true;
            pathObj.isCompleted = false;
            pathObj.LastIndex = 0;
            MasterDictionary.AddEntry(name, pathObj);

            return true;
        }

    }
}