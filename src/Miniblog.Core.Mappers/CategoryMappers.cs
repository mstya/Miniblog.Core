﻿using System.Collections.Generic;
using System.Linq;
using Miniblog.Core.Db.Entities;
using Miniblog.Core.DTO;

namespace Miniblog.Core.Mappers
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