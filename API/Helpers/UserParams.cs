namespace API.Helpers
{
    public class UserParams
    {
        //dr
        private const int MaxPageSize=50;
        public int PageNumber { get; set; }=1; //deafault page size 1

        private int _pageSize=10; //default 10 
    
        public int PageSize
        {
               get=>_pageSize;
               set=>_pageSize = (value>MaxPageSize)?MaxPageSize:value; //is value more than max page size, if yes set _pageSize=value or else use the default maxpagesize which is 50.
        }

        public string CurrentUsername { get; set; }
        public string Gender { get; set; }

        public int MinAge { get; set; }=18;
        public int MaxAge { get; set; }=150;

        public string OrderBy { get; set; }="lastActive";

    }
}