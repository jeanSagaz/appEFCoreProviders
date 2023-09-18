using ConsoleApp.Configurations;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    private static ICategoryRepository _categoryRepository;
    private static ICustomerRepository _customerRepository;
    private static IProductRepository _productRepository;
    private static IStateRepository _stateRepository;
    private static IGovernorRepository _governorRepository;

    static void Main(string[] args)
    {
        InitConfiguration();

        Console.WriteLine("Início");

        var console = new Category("Console");
        _categoryRepository.Add(console);

        var ps5 = new Product("Ps5", console.Id);
        var xboxOne = new Product("XboxOne", console.Id);
        var arcade = new Product("Arcade", console.Id);
        _productRepository.Add(ps5);
        _productRepository.Add(xboxOne);
        _productRepository.Add(arcade);        

        var jean = new Customer("Fulano", "fulano@gmail.com", DateTime.Now);        
        jean.Products.Add(ps5);
        jean.Products.Add(arcade);

        var candida = new Customer("Ciclano", "ciclano@gmail.com", DateTime.Now);
        candida.Products.Add(xboxOne);

        _customerRepository.Add(jean);
        _customerRepository.Add(candida);

        //Commit(_productRepository.UnitOfWork).Wait();
        Commit(_customerRepository.UnitOfWork).Wait();

        //var customers = _customerRepository.GetAll().Result;
        //var customers = _customerRepository.Search(c => c.Id != null, new[]
        //    {
        //      "Products"
        //    }).Result;
        //foreach (var customer in customers) 
        //{
        //    Console.WriteLine($"{customer.Id} - {customer.Name} - {customer.Email} - {customer.BirthDate}");

        //    var products = customer?.Products;
        //    if (products is not null)
        //    {
        //        foreach (var product in products)
        //        {
        //            Console.WriteLine($"{product.Id} - {product.Name} - {product.Active}");
        //        }
        //    }            
        //}

        //var products2 = _productRepository.Search(c => c.Id != null, new[]
        //    {
        //      "Customers"
        //    }).Result;

        var state = new State("Minas Gerais");
        _stateRepository.Add(state);
        var governor = new Governor("Fulano", 44, "Free", state.Id);
        _governorRepository.Add(governor);
        Commit(_stateRepository.UnitOfWork).Wait();
        //Commit(_governorRepository.UnitOfWork).Wait();

        Console.WriteLine("Fim");
        Console.ReadKey();
    }

    private static async Task<bool> Commit(IUnitOfWork uow)
    {
        if (!await uow.Commit())
        {
            Console.WriteLine("Commit - false");
            return false;
        }

        Console.WriteLine("Commit - true");
        return true;
    }

    private static void InitConfiguration()
    {
        var serviceProvider = new ServiceCollection();
        serviceProvider.ConfigureServices();
        var services = serviceProvider.BuildServiceProvider();

        _customerRepository = services.GetService<ICustomerRepository>();
        _productRepository = services.GetService<IProductRepository>();
        _categoryRepository = services.GetService<ICategoryRepository>();
        _stateRepository = services.GetService<IStateRepository>();
        _governorRepository = services.GetService<IGovernorRepository>();
    }
}
