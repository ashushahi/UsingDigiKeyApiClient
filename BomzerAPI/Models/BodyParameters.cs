namespace BomzerAPI.Models
{

    public class Rootobject
    {
        public Searchbypartrequest SearchByPartRequest
        {
            get; set;
        }
    }

    public class Searchbypartrequest
    {
        public string mouserPartNumber
        {
            get; set;
        }
    }

}
