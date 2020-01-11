
namespace PvScout.Server.Model
{

    // this should come from a WCF service - so each property becomes automaticly a PropertyChanged property if we add the service reference
    public class PvModul
    {
        public int Id { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
    }
}
