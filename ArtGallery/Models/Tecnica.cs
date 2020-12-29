namespace GaleryArt.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tecnica
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tecnica()
        {
            Obras = new HashSet<Obra>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre_tecnica { get; set; }

        public int manifestacion { get; set; }

        public virtual Manifestacione Manifestacione { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Obra> Obras { get; set; }
    }
}
