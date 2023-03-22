namespace ESBCkedtor.Models
{
    public class Confluence
    {
        public Version version { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public ConfluenceBody body { get; set; }
    }
    public class Version
    {
        public int number { get; set; }
    }
    public class ConfluenceBody {
        public Storage storage { get; set; }
    }
    public class Storage {
        public string value { get; set; }
        public string representation { get; set; }

    }
}
