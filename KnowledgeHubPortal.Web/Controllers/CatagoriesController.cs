using KnowledgeHubPortal.Web.Models.Data;
using KnowledgeHubPortal.Web.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeHubPortal.Web.Controllers
{
    public class CatagoriesController : Controller
    {
        KnowledgeHubPortalDbContext db = new KnowledgeHubPortalDbContext();
        public IActionResult Index(string searchValue)
        {
            List<Catagory> catagories = null;

            if (searchValue != null && searchValue.Length != 0)
            {
                catagories = (from c in db.Catagories
                              where c.Name.Contains(searchValue) || c.Description.Contains(searchValue)
                              select c).ToList();
            }
            else
            {
                catagories = db.Catagories.ToList();
            }
            return View(catagories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Catagory catagory)
        {
            if (!ModelState.IsValid)
                return View("Create");

            db.Catagories.Add(catagory);
            db.SaveChanges();
            TempData["Message"] = $"Catagory {catagory.Name} created successfully.";

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Catagory catagoryToDelete = db.Catagories.Find(id);
            
            if (catagoryToDelete == null)
            {
                return NotFound();
            }

            return View("ConfirmDelete", catagoryToDelete);
        }
        public IActionResult ConfirmDelete(int CatagoryId)
        {
            Catagory catagoryToDelete = db.Catagories.Find(CatagoryId);
            
            db.Catagories.Remove(catagoryToDelete);
            db.SaveChanges();
            TempData["Message"] = $"Catagory {catagoryToDelete.Name} deleted successfully.";
            
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Catagory catagoryToEdit = db.Catagories.Find(id);
            if (catagoryToEdit == null)
                return NotFound();

            return View(catagoryToEdit);
        }
        [HttpPost]
        public IActionResult Edit(Catagory editedCatagory)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            db.Entry(editedCatagory).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            TempData["Message"] = $"Catagory {editedCatagory.Name} updated successfully.";
            return RedirectToAction("Index");
        }

    }
}
