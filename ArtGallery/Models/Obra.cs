namespace GaleryArt.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Obra
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Obra()
        {
            Premios = new HashSet<Premio>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre_obra { get; set; }

        public int? ano_creacion { get; set; }

        [StringLength(100)]
        public string autor { get; set; }

        [Column(TypeName = "money")]
        public decimal precio { get; set; }

        [Required]
        [StringLength(1000)]
        public string path_foto { get; set; }

        public int? manifestacion { get; set; }

        public int? tecnica { get; set; }

        public virtual Manifestacione Manifestacione { get; set; }

        public virtual Tecnica Tecnica1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Premio> Premios { get; set; }
    }
}
