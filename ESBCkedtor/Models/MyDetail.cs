using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace ESBCkedtor.Models
{
    public class MyDetail
    {
        public Body body { get;set;}
    }
   public class Body {
        public View view { get; set; }
    }
    public class View
    {
        public string value { get; set; }
    }
}
