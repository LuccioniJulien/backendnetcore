using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace BaseApi.Models {
    public class Author {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public ICollection<Written> Writtens { get; set; }

        public static IEnumerable<Author> GetAuthorsFromJSON (JToken json) {
            return json["volumeInfo"]["authors"].Select (x =>
                new Author { Name = (string) x }
            );
        }
    }
}