using KnowledgeHubPortal.Web.Models.Data;
using KnowledgeHubPortal.Web.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KnowledgeHubPortal.Web.Controllers
{
    public class ArticlesController : Controller
    {
        KnowledgeHubPortalDbContext db = new KnowledgeHubPortalDbContext();
        public IActionResult Index(string catagoryName,string Approved,string UnApproved)
        {
            List<Article> articles = null;
            ViewBag.CatagoryName = from c in db.Catagories
                                 select new SelectListItem
                                 {
                                     Text = c.Name,
                                     Value = c.Name.ToString()
                                 };


            if (catagoryName != null && Approved == null && UnApproved == null)
            {
                var catagoryId = (from c in db.Catagories
                                  where c.Name == catagoryName
                                  select c.CatagoryID).FirstOrDefault();

                articles = (from a in db.Articles
                            where a.CatagoryID == catagoryId
                            select a).ToList();
            }
            else if (catagoryName == null && Approved == "approved" && UnApproved == null)
            {
                articles = (from a in db.Articles
                            where a.IsApproved == true
                            select a).ToList();
            }
            else if (catagoryName == null && Approved == null && UnApproved == "unapproved")
            {
                articles = (from a in db.Articles
                            where a.IsApproved == false
                            select a).ToList();
            }
            else if (catagoryName != null && Approved == "approved" && UnApproved == null)
            {
                var catagoryId = (from c in db.Catagories
                                  where c.Name == catagoryName
                                  select c.CatagoryID).FirstOrDefault();
                articles = (from a in db.Articles
                            where a.IsApproved == true && a.CatagoryID == catagoryId
                            select a).ToList();
            }
            else if (catagoryName != null && Approved ==null  && UnApproved == "unapproved")
            {
                var catagoryId = (from c in db.Catagories
                                  where c.Name == catagoryName
                                  select c.CatagoryID).FirstOrDefault();
                articles = (from a in db.Articles
                            where a.IsApproved == false && a.CatagoryID == catagoryId
                            select a).ToList();
            }
            else if (catagoryName != null && Approved == "approved" && UnApproved == "unapproved")
            {
                var catagoryId = (from c in db.Catagories
                                  where c.Name == catagoryName
                                  select c.CatagoryID).FirstOrDefault();
                articles = (from a in db.Articles
                            where a.CatagoryID == catagoryId
                            select a).ToList();
            }
            else {
                articles = db.Articles.ToList();
            }

            return View(articles);
        }

        public IActionResult UserList()
        {
            List<Article> articles = (from a in db.Articles
                                      select a).ToList();
            ViewBag.CatagoryID = from c in db.Catagories
                                 select new SelectListItem
                                 {
                                     Text = c.Name,
                                     Value = c.CatagoryID.ToString()
                                 };
            return View(articles);
        }
        public IActionResult Browse(string catagoryName)
        {
            List<Article> articles = null;
            ViewBag.CatagoryName = from c in db.Catagories
                                   select new SelectListItem
                                   {
                                       Text = c.Name,
                                       Value = c.Name.ToString()
                                   };
            if (catagoryName != null)
            {
                var catagoryId = (from c in db.Catagories
                                  where c.Name == catagoryName
                                  select c.CatagoryID).FirstOrDefault();
                articles = (from a in db.Articles
                            where a.IsApproved == true && a.CatagoryID == catagoryId
                            select a).ToList();
            }
            else
            {
                articles = (from a in db.Articles
                            where a.IsApproved == true
                            select a).ToList();
            }

            return View(articles);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CatagoryID = from c in db.Catagories
                                 select new SelectListItem
                                 {
                                     Text = c.Name,
                                     Value = c.CatagoryID.ToString()
                                 };
            return View();
        }
        [HttpPost]
        public IActionResult Create(Article article)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Create");
            article.DateSubmited = DateTime.Now;
            article.PostedBy = User.Identity.Name;
            article.IsApproved = false;
            db.Articles.Add(article);
            db.SaveChanges();
            TempData["Message"] = $"Article {article.Title} created successfully.";
            // send email to adminstrator
            return RedirectToAction("UserList");
        }
        public IActionResult Reject(int id)
        {
            Article articleToReject = db.Articles.Find(id);

            if (articleToReject == null)
            {
                return NotFound();
            }

            return View("ConfirmReject", articleToReject);
        }
        public IActionResult ConfirmReject(int ArticleId)
        {
            Article articleToReject = db.Articles.Find(ArticleId);

            db.Articles.Remove(articleToReject);
            db.SaveChanges();
            TempData["Message"] = $"Article {articleToReject.Title} rejected successfully.";

            return RedirectToAction("Index");
        }
        public IActionResult Remove(int id)
        {
            Article articleToReject = db.Articles.Find(id);

            if (articleToReject == null)
            {
                return NotFound();
            }

            return View("ConfirmRemove", articleToReject);
        }
        public IActionResult ConfirmRemove(int ArticleId)
        {
            Article articleToReject = db.Articles.Find(ArticleId);

            db.Articles.Remove(articleToReject);
            db.SaveChanges();
            TempData["Message"] = $"Article {articleToReject.Title} removed successfully.";

            return RedirectToAction("Index");
        }
        public IActionResult Approve(int id)
        {
            Article articleToApprove = db.Articles.Find(id);

            if (articleToApprove == null)
            {
                return NotFound();
            }

            return View("ConfirmApprove", articleToApprove);
        }
        public IActionResult ConfirmApprove(int ArticleId)
        {
            Article articleToApprove = db.Articles.Find(ArticleId);
            articleToApprove.IsApproved = true;
            db.SaveChanges();
            TempData["Message"] = $"Article {articleToApprove.Title} Approved successfully.";

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Article articleToDelete = db.Articles.Find(id);

            if (articleToDelete == null)
            {
                return NotFound();
            }

            return View("ConfirmDelete", articleToDelete);
        }
        public IActionResult ConfirmDelete(int ArticleId)
        {
            Article articleToDelete = db.Articles.Find(ArticleId);

            db.Articles.Remove(articleToDelete);
            db.SaveChanges();
            TempData["Message"] = $"Article {articleToDelete.Title} deleted successfully.";

            return RedirectToAction("UserList");
        }
        
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Article articleToEdit = db.Articles.Find(id);
            if (articleToEdit == null)
                return NotFound();
            ViewBag.CatagoryID = from c in db.Catagories
                                 select new SelectListItem
                                 {
                                     Text = c.Name,
                                     Value = c.CatagoryID.ToString()
                                 };
            return View(articleToEdit);
        }
        [HttpPost]
        public IActionResult Edit(Article editedArticle)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            editedArticle.DateSubmited = DateTime.Now;
            db.Entry(editedArticle).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            TempData["Message"] = $"Article {editedArticle.Title} updated successfully.";
            return RedirectToAction("UserList");
        }
    }
}
