 public class Details { 
        public string pilotName { get; set; }   
        public Configuration [] configurations { get; set; }    
    }
    public class ConfigurationDetails {
        public string pilotName { get; set; }
        public Configuration[] configurations { get; set; }
        private static ConfigurationDetails uniqueConfigurationDetails;
        private ConfigurationDetails() { }
        public  static ConfigurationDetails GetInstance()
        {
            if (uniqueConfigurationDetails == null)
            {
                uniqueConfigurationDetails = new ConfigurationDetails();
                return uniqueConfigurationDetails;  
            }
            else return uniqueConfigurationDetails; 
        }
    }
    public class Configuration { 
    public string Name { get; set; }
    public string Bits { get; set; }
        public string ImageURL { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Details items=new Details();
            // string lines = System.IO.File.ReadAllLines(@"json1.json");
            using (StreamReader r = new StreamReader("json1.json"))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<Details>(json);
            }
            ConfigurationDetails configurationDetails =ConfigurationDetails.GetInstance();
            configurationDetails.pilotName=items.pilotName;
            configurationDetails.configurations=items.configurations;
            Console.WriteLine(configurationDetails.pilotName);
        }
    }
