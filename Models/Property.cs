using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RealEstate.Api.Models
{
	public class Property
	{
		public int Id { get; set; }

        //[Required, MinLength(5), MaxLength(12)]
        public string Name { get; set; }

        //[Required, MinLength(10), MaxLength(50)]
        public string Detail { get; set; }

        //[Required, MinLength(10), MaxLength(50)]
        public string Address { get; set; }

        public string ImageUrl { get; set; }

		public double Price { get; set; }

		public bool IsTrending { get; set; }

		// FK
		public int CategoryId { get; set; }

        // FK
        [JsonIgnore]
        public Category Category { get; set; }

		// FK
		public int UserId { get; set; }

        // FK
        [JsonIgnore]
        public User User { get; set; }
	}
}

