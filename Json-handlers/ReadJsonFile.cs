  public static string ReadJsonFile(string resourcePath) {
            var assembly = Assembly.GetExecutingAssembly();
            string jsonFileContent;
            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                jsonFileContent = reader.ReadToEnd();
            }
            return jsonFileContent;
        }