using AutoMapper;
using Elfie.Serialization;
using Gauniv.WebServer.Data;
using Gauniv.WebServer.Dtos;

namespace Gauniv.WebServer.Dtos
{
    public class GameDto
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public decimal Prix { get; set; }
        public List<string> Categories { get; set; }
    }
}