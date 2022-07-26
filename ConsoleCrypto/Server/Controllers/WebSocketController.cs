﻿using ConsoleCrypto.Models.Requests;

namespace ConsoleCrypto.Controllers
{
    public record Item(string name, int value);
    public class WebSocketController:IController
    {
        public WebSocketController()
        {

        }
        public async Task<Item[]> Index()
        {
            await Task.Delay(1000);
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
        public async Task<Item[]> Second()
        {
            await Task.Delay(2000);
            return new[]
            {
                new Item("Stul",6),
                new Item("Stol",4),
                new Item("Potolok",10)
            };
        }
    }
}
