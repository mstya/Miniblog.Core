using System.Collections.Generic;
using System.Linq;
using Miniblog.Core.Db.Entities;
using Miniblog.Core.Infrastructure.DTO;

namespace Miniblog.Core.Infrastructure.Mappers
{
    public static class CategoryMappers
    {
        public static CategoryDto ToCategoryDto(this Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public static List<CategoryDto> ToCategoryDto(this List<Category> categories)
        {
            return categories.Select(x => x.ToCategoryDto()).ToList();
        }
    }
}