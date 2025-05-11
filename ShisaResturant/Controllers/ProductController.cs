using Microsoft.AspNetCore.Mvc;
using ShisaResturant.Data;
using ShisaResturant.Models;

namespace ShisaResturant.Controllers
{
    public class ProductController : Controller
    {
        private Repository<Product> products;
        private Repository<Ingredient> ingredients;
        private Repository<Category> categories;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(ApplicationDbContext _context, IWebHostEnvironment webHostEnvironment)
        {
            products = new Repository<Product>(_context);
            ingredients = new Repository<Ingredient>(_context);
            categories = new Repository<Category>(_context);
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await products.GetAllSync());
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            ViewBag.Ingredients = await ingredients.GetAllSync();
            ViewBag.Categories = await categories.GetAllSync();

            if (id == 0) 
            {
                ViewBag.Operation = "Add";
                return View(new Product());
            }
            else
            {
                Product product = await products.GetByIdSync(id, new QueryOptions<Product>
                {
                    Includes = "ProductIngredients.Ingredient, Category"
                });

                ViewBag.Operation = "Edit";
                return View(product);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(Product product, int[] ingredientIds, int catId)
        {
            ViewBag.Ingredients = await ingredients.GetAllSync();
            ViewBag.Categories = await categories.GetAllSync();
            if (ModelState.IsValid)
            {
                if(product.ImageFile != null)
                {
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(fileStream);
                    }
                    product.ImageUrl = uniqueFileName;
                }

                if (product.ProductId == 0)
                {
                    product.CategoryId = catId;

                    // Add the selected ingredients to the product
                    foreach (int id in ingredientIds)
                    {
                        product.ProductIngredients?.Add(new ProductIngredient { IngredientId = id, ProductId = product.ProductId });
                    }

                    await products.AddSync(product);
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    var existingProduct = await products.GetByIdSync(product.ProductId, new QueryOptions<Product> { Includes = "ProductIngredients" });

                    if(existingProduct == null)
                    {
                        ModelState.AddModelError("", "Product not found.");
                        ViewBag.Ingredients = await ingredients.GetAllSync();
                        ViewBag.Categories = await categories.GetAllSync();
                        return View(product);
                    }

                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.Stock = product.Stock;
                    existingProduct.CategoryId = catId;

                    existingProduct.ProductIngredients?.Clear();
                    foreach (int id in ingredientIds)
                    {
                        existingProduct.ProductIngredients?.Add(new ProductIngredient { IngredientId = id, ProductId = product.ProductId });
                    }

                    try
                    {
                        await products.UpdateAsync(existingProduct);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", $"Error: {ex.GetBaseException().Message}");
                        ViewBag.Ingredients = await ingredients.GetAllSync();
                        ViewBag.Categories = await categories.GetAllSync();
                        return View(product);
                    }
                }
            }
            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await products.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Product not found");
                return RedirectToAction("Index");
            }
        }
    }
}
