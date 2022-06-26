public static PrerequisitesJson DeserialzeJsonFile(string resourcePath)
        {
            var prerequisites = JsonSerializer.Deserialize<JsonModel>
                (ReadJsonFile(resourcePath));                      
            return prerequisites;
        }


