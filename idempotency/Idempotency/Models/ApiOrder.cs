using System;

namespace Idempotency.Models
{
    public class ApiOrder
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
    }
}