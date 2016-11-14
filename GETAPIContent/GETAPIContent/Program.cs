using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GETAPIContent
{
    class Program
    {
        protected static string url = @"https://health.data.ny.gov/api/views/5pme-xbs5/rows.json";
        protected static Json json;
        protected static Product product;
        static void Main(string[] args)
        {
            json = new Json(url);
            var data = json.RequestContent();
            product = new Product(data);
            product.ParseData();
            product.getBadData();
        }
    }

}
