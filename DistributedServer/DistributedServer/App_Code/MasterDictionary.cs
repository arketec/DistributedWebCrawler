using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;

using DistributedServer.Models;
using Newtonsoft.Json;

namespace DistributedServer.App_Code
{
    public sealed class MasterDictionary
    {
        private static volatile MasterDictionary instance;
        private static object syncRoot = new Object();
        private static ConcurrentDictionary<string, PathObject> mastDictionary;
        private static string DictionaryPath;
        private static bool isDictionaryRead;
        private static bool isDictionarySaved;

        private MasterDictionary()
        {
            DictionaryPath = @"C:\Users\Noah\Documents\Visual Studio 2015\Projects\DistributedServer\DistributedServer\Dictionary\MasterDictionary.json";
            mastDictionary = GetDictionary();
        }

        public static MasterDictionary Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new MasterDictionary();
                    }
                }

                return instance;
            }
        }

        public static int GetCount()
        {
            return ( mastDictionary != null ) ? mastDictionary.Count() : 0 ;
        }

        public static BatchData GetBatch(string name)
        {
            string document = null;

            PathObject pathObject = mastDictionary[name];
            if (pathObject.isAvail)
            {
                var reader = new StreamReader(pathObject.path);
                document = reader.ReadToEnd();
                reader.Close();

                // update dictionary entry
                pathObject.isAvail = false;
                mastDictionary[name] = pathObject;
                SaveDictionaryToDisk();
            }

            BatchData batch = new BatchData();

            batch.Data = document.Replace("\r", "");
            batch.Id = pathObject.id;
            batch.Key = name;
            batch.LastComplete = pathObject.LastIndex;

            return batch;
        }

        public static BatchData GetRandomBatch(int? any)
        {
            if (any != null)
            {
                Random rand = new Random();
                int randId = rand.Next(GetCount());
                foreach (var path in mastDictionary)
                {
                    if (path.Value.id == randId)
                    {
                        if (path.Value.isAvail)
                            return GetBatch(path.Key);
                        else
                            GetRandomBatch(0);
                    }
                }
            }
            return null;
        }

        public static bool AddEntry(string key, PathObject obj)
        {
            var result = mastDictionary.TryAdd(key, obj);

            if (result)
                SaveDictionaryToDisk();

            return result;
        }

        public static void UpdateEntry(string key, bool isComplete, int lastIndex)
        {
            // update dictionary entry
            PathObject tmp = mastDictionary[key];
            if (!tmp.isCompleted)
            {
                tmp.isCompleted = isComplete;
                tmp.isAvail = !isComplete;
                tmp.LastIndex = lastIndex;
                mastDictionary[key] = tmp;
                SaveDictionaryToDisk();
            }
        }

        static ConcurrentDictionary<string, PathObject> GetDictionary()
        {
            ConcurrentDictionary<string, PathObject> dictionary;
            if (File.Exists(DictionaryPath))
            {
                using (StreamReader reader = new StreamReader(DictionaryPath))
                {
                    try
                    {
                        dictionary = JsonConvert.DeserializeObject<ConcurrentDictionary<string, PathObject>>(reader.ReadToEnd());
                        isDictionaryRead = true;
                    }
                    catch (Exception ex)
                    {
                        dictionary = null;
                        isDictionaryRead = false;
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
            }
            else
                dictionary = new ConcurrentDictionary<string, PathObject>();

            if (!dictionary.ContainsKey(""))
            {
                var pathObj = new PathObject();
                pathObj.id = dictionary.Count() + 1;
                pathObj.dirName = "";
                pathObj.path = "";
                pathObj.isAvail = false;
                pathObj.isCompleted = true;
                dictionary.TryAdd("", pathObj);
            }
            return dictionary;
        }

        static void SaveDictionaryToDisk()
        {
            StreamWriter writer = new StreamWriter(DictionaryPath, false);

            string stream = JsonConvert.SerializeObject(mastDictionary);

            try
            {
                writer.Write(stream.ToString());
                isDictionarySaved = true;
            }
            catch
            {
                isDictionarySaved = false;
            }
            finally
            {
                writer.Close();
            }
        }
    }
}