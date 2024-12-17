namespace Foodie.Common.Models
{
    public class ListVM<t>
    {
        public int Count { get; set; }
        public IList<t> List { get; set; }
    }
}
