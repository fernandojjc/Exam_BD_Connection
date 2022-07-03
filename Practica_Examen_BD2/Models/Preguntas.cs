namespace Practica_Examen_BD2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Preguntas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Preguntas()
        {
            Respuestas = new HashSet<Respuestas>();
        }

        [Key]
        public int IdPregunta { get; set; }

        [Required]
        public string Sentencia { get; set; }

        [Required]
        public string Respuesta1 { get; set; }

        [Required]
        public string Respuesta2 { get; set; }

        [Required]
        public string Respuesta3 { get; set; }

        [Required]
        public string Respuesta4 { get; set; }

        [Required]
        public string RespuestaCorrecta { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Respuestas> Respuestas { get; set; }
    }
}
