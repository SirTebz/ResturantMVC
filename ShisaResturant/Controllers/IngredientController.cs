using Microsoft.AspNetCore.Mvc;
using ShisaResturant.Data;
using ShisaResturant.Models;

namespace ShisaResturant.Controllers
{
    public class IngredientController : Controller
    {
        private Repository<Ingredient> ingredients;
        public IngredientController(ApplicationDbContext context)
        {
            ingredients = new Repository<Ingredient>(context);
        }
        public async Task<IActionResult> Index()
        {
            return View(await ingredients.GetAllSync());
        }
        public async Task<IActionResult> Details(int id)
        {
            return View(await ingredients.GetByIdSync(id, new QueryOptions<Ingredient>() { Includes = "ProductIngredients.Product" }));
        }

        //Ingredient/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create([Bind("IngredientId, Name")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                await ingredients.AddSync(ingredient);
                return RedirectToAction("Index");
            }
            return View(ingredient);
        }

        //Ingredient/Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //var ingredient = await ingredients.GetByIdSync(id, new QueryOptions<Ingredient>() { Includes = "ProductIngredients.Product" });
            //if (ingredient == null)
            //{
            //    return NotFound();
            //}
            //return View(ingredient);
            return View(await ingredients.GetByIdSync(id, new QueryOptions<Ingredient> { Includes = "ProductIngredients.Product" }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
            //var ingredient = await ingredients.GetByIdSync(id, new QueryOptions<Ingredient>() { Includes = "ProductIngredients.Product" });
            //if (ingredient == null)
            //{
            //    return NotFound();
            //}
            //await ingredients.DeleteAsync(id);
            //return RedirectToAction("Index"); 
        //}
        public async Task<IActionResult> Delete(Ingredient ingredient)
        {
            await ingredients.DeleteAsync(ingredient.IngredientId);
            return RedirectToAction("Index");
        }

        //Ingredient/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //var ingredient = await ingredients.GetByIdSync(id, new QueryOptions<Ingredient>() { Includes = "ProductIngredients.Product" });
            //if (ingredient == null)
            //{
            //    return NotFound();
            //}
            //return View(ingredient);
            return View(await ingredients.GetByIdSync(id, new QueryOptions<Ingredient> { Includes = "ProductIngredients.Product" }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("IngredientId, Name")] Ingredient ingredient)
        //{
        //    if (id != ingredient.IngredientId)
        //    {
        //        return NotFound();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await ingredients.UpdateAsync(ingredient);
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!await ingredients.GetByIdSync(id, new QueryOptions<Ingredient> { Includes = "ProductIngredients.Product" }).Equals(ingredient))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    return View(ingredient);
        //}
        public async Task<IActionResult> Edit(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                await ingredients.UpdateAsync(ingredient);
                return RedirectToAction("Index");
            }
            return View(ingredient);
        }
    }
}
