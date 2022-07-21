using ConsoleCrypto.Models.Requests;

namespace ConsoleCrypto.Controllers.Requests
{
    public class ApiController:IController
    {
        public record Item(string name , int value);

        public async Task<Item[]> IndexAsync()
        {
            await Task.Delay(5);
            return new[]
            {
                new Item("Lojka",2),
                new Item("Vilka",3),
                new Item("Noj",1)
            };
        }
        public Item[] Index()
        {
            Thread.Sleep(5);
            return new[]
            {
                new Item("Lojka",2),
                new Item("Vilka",3),
                new Item("Noj",1)
            };
        }

        public Item[] First()
        {
            return new[]
            {
                new Item("Lojka",2),
                new Item("Vilka",3),
                new Item("Noj",1)
            };
        }
        public Item[] Second()
        {
            Task.Delay(2000);
            return new[]
            {
                new Item("Stul",6),
                new Item("Stol",4),
                new Item("Potolok",10)
            };
        }

    }
}
