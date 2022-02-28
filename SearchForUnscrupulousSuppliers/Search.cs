using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SearchForUnscrupulousSuppliers
{
    public class Search
    {
        public async Task SearchByInn(string inn)
        {
            string correctInn = inn.Replace(" ", "");

            if (correctInn.Any(c => !Char.IsDigit(c)))
            {
                throw new ArgumentException("Значение 'ИНН' должно состоять только из цифр", nameof(inn));
            }

            if (correctInn.Length != 10)
            {
                throw new ArgumentException("Значение 'ИНН' должно состоять из 10 цифр", nameof(inn));
            }

            string data =
                $"results.html?searchString={correctInn}&morphology=on&search-filter=%D0%94%D0%B0%D1%82%D0%B5+%D1%80%D0%B0%D0%B7%D0%BC%D0%B5%D1%89%D0%B5%D0%BD%D0%B8%D1%8F&sortBy=UPDATE_DATE&pageNumber=1&sortDirection=false&recordsPerPage=_10&showLotsInfoHidden=false&fz94=on&fz223=on&ppRf615=on";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://zakupki.gov.ru/epz/dishonestsupplier/search/");

                HttpResponseMessage response = await client.GetAsync(data);

                if (response.IsSuccessStatusCode)
                {
                    var html = @$"https://zakupki.gov.ru/epz/dishonestsupplier/search/results.html?searchString={inn}&morphology=on&search-filter=%D0%94%D0%B0%D1%82%D0%B5+%D1%80%D0%B0%D0%B7%D0%BC%D0%B5%D1%89%D0%B5%D0%BD%D0%B8%D1%8F&sortBy=UPDATE_DATE&pageNumber=1&sortDirection=false&recordsPerPage=_10&showLotsInfoHidden=false&fz94=on&fz223=on&ppRf615=on";

                    HtmlWeb web = new HtmlWeb();

                    var htmlDoc = web.Load(html);

                    HtmlNodeCollection span = htmlDoc.DocumentNode.SelectNodes("//span[contains(@class,'search-results__total')]");

                    Console.WriteLine(span.ToString());

                    //var node = htmlDoc.DocumentNode.SelectSingleNode("//body");

                    //Console.WriteLine(node.OuterHtml);
                }
                else
                {
                    throw new Exception($"Запрос потерпел неудачу! Статус ошибки: {response.StatusCode}");
                }
            }
        }
    }
}