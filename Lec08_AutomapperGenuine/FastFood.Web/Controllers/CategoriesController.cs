using System.Collections;
using System.Linq;
using AutoMapper.QueryableExtensions;
using FastFood.Models;

namespace FastFood.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using System;

    using Data;
    using ViewModels.Categories;

    public class CategoriesController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;
       

        public CategoriesController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
            
           }
        
        public IActionResult Create()
        {
            
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryInputModel model)
        {
            var cat = mapper.Map<Category>(model);
            context.Categories.Add(cat);
            context.SaveChanges();

            return RedirectToAction("All", "Categories");
        }

        public IActionResult All()
        {
            var categs = context.Categories
                .ProjectTo<CategoryAllViewModel>(mapper.ConfigurationProvider)
                .ToList();

            return this.View(categs);
        }
    }
}
