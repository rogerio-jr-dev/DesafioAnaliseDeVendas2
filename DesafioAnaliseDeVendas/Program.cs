using DesafioAnaliseDeVendas.Entities;
using System.IO;
using System;
using System.Globalization;
using System.Collections.Generic;

namespace Desafio
{
    class Program
    {
        static void Main(string[] args)
        {

            CultureInfo CI = CultureInfo.InvariantCulture;

            Console.Write("Entre o caminho do arquivo: ");
            string path = Console.ReadLine();
            Console.WriteLine();//Pular linha

            List<Sale> list = new List<Sale>();

            try
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] fields = sr.ReadLine().Split(',');

                        int month = int.Parse(fields[0]);
                        int year = int.Parse(fields[1]);
                        string seller = fields[2];
                        int items = int.Parse(fields[3]);
                        double total = double.Parse(fields[4], CI);

                        list.Add(new Sale(month, year, seller, items, total));
                    }
                }

                var totalPorVendedor = list
                      .GroupBy(sale => sale.Seller) // Agrupa vendas pelo nome do vendedor
                      .Select(group => new {
                          Seller = group.Key,
                          TotalSales = group.Sum(sale => sale.Total)
                      }) 
                      .ToDictionary(x => x.Seller, x => x.TotalSales);

                Console.WriteLine("Total de vendas por vendedor:");
                foreach (var entry in totalPorVendedor)
                {
                    Console.WriteLine($"{entry.Key} - R$ {entry.Value.ToString("F2", CI)}");
                }

            }
            catch (Exception e)
            {
                Console.Write("O sistema não pode encontrar o arquivo especificado ");
                Console.WriteLine(e.Message);
            }
        }
    }
}