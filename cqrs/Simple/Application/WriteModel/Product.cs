using System;
using System.Text.RegularExpressions;

namespace Simple.Application.WriteModel
{
    public class Product
    {
        private readonly Regex _validCodeRegex = new("[a-zA-Z]{3}-[0-9]{4}");

        public Guid Id { get; }
        public string Code { get; private set; }
        public string Name { get; private set; }

        public Product(string name, string code)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name must not be empty.", nameof(name));

            if (code is null)
                throw new ArgumentNullException(nameof(code));

            if (!_validCodeRegex.IsMatch(code))
                throw new ArgumentException("Code must have the following format: 'XXX-0000'.", nameof(code));

            Id = Guid.NewGuid();
            Name = name;
            Code = code;
        }

        public Product(Guid id, string name, string code)
        {
            Id = id;
            Name = name;
            Code = code;
        }

        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Name must not be empty.", nameof(newName));

            Name = newName;
        }
    }
}