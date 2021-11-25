using System;

namespace Simple.Infrastructure.Persistence
{
    public class EfProduct
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
    }
}