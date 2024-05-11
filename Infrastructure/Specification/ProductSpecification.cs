using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specification
{
    public class ProductSpecification
    {
        private const int MAXPAGESIZE = 50;
        public int? BrandId { get; set; }

        public int? TypeId { get; set; }

        public string? sort { get; set; }

        public int PageIndex { get; set; } = 1;

        private int _pageSize = 6;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MAXPAGESIZE)? MAXPAGESIZE : value;
        }

        private string? searchName;

        public string? SearchName
        {
            get { return searchName?.Trim().ToLower(); }
            set { searchName = value; }
        }


    }
}
