namespace API.Helpers
{
    public class ExchangesParams : PaginationParams
    {
        public int UserId { get; set; }
        public string Predicate { get; set; }
    }
}