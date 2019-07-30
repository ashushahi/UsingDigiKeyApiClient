using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using UsingDigiKeyApiClient.Models;

namespace UsingDigiKeyApiClient
{

    class Program
    {
        static void Main(string[] args)

        {
            
            var settings = ApiClientSettings.CreateFromConfigFile();
            //UsingDigiKeyApiClient.Models.DigiKeyRoot Digikey = null;
            //using (StreamReader sr = new StreamReader("DigiKeyData.json"))
            //{
            //    string json = sr.ReadToEnd();
            //    Digikey = JsonConvert.DeserializeObject<Models.DigiKeyRoot>(json);
            //}
            //UsingDigiKeyApiClient.Models.MouserRoot mouser = null;
            //using (StreamReader sr = new StreamReader("MouserData.json"))
            //{
            //    string json = sr.ReadToEnd();
            //    mouser = JsonConvert.DeserializeObject<Models.MouserRoot>(json);
            //}
        }
    }
}
