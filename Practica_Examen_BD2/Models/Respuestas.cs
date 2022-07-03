namespace Practica_Examen_BD2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Respuestas
    {
        [Key]
        public int IdRespuesta { get; set; }

        public int IdAlumno { get; set; }

        public int IdPregunta { get; set; }

        [Required]
        public string RespuestaAlumno { get; set; }

        public virtual Alumnos Alumnos { get; set; }

        public virtual Preguntas  Preguntas { get; set; }


        //public Alumnos Alumnos { get;set }
    }
}
