using System.ComponentModel.DataAnnotations;

namespace KnowledgeHubPortal.Web.Models.Entities
{
    public class Catagory
    {
        public int CatagoryID { get; set; }
        [Required(ErrorMessage = "Kindly enter catagory name....")]
        [MaxLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
