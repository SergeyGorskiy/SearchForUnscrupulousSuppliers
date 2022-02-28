using System;

namespace SearchForUnscrupulousSuppliers
{
    class Program
    {
        static void Main(string[] args)
        {
            Search search = new Search();

            search.SearchByInn("7733349959").Wait();
        }
    }
}
