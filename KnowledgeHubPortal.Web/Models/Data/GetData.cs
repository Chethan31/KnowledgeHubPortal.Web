using KnowledgeHubPortal.Web.Models.Entities;

namespace KnowledgeHubPortal.Web.Models.Data
{
    public class GetData
    {  
        public static Catagory getCategory(int id)
        {
            KnowledgeHubPortalDbContext db = new KnowledgeHubPortalDbContext();
            var category = db.Catagories.Find(id);

            return category;
        }
    }
}
