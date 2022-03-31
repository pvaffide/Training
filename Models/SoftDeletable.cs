using System.ComponentModel.DataAnnotations.Schema;

namespace Training.Models
{
    public abstract class SoftDeletable
    {
        public bool Deleted { get; set; }
        [NotMapped]
        public bool Visible
        {
            get => !Deleted;
            set => Deleted = !value;
        }
    }
}
