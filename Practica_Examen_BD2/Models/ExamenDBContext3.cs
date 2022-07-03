using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Practica_Examen_BD2
{
    public partial class ExamenDBContext3 : DbContext
    {
        public ExamenDBContext3()
            : base("name=ExamenDBContext")
        {
        }

        public virtual DbSet<Alumnos> Alumnos { get; set; }
        public virtual DbSet<Preguntas> Preguntas { get; set; }
        public virtual DbSet<Respuestas> Respuestas { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alumnos>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Alumnos>()
                .Property(e => e.ApellidoPaterno)
                .IsUnicode(false);

            modelBuilder.Entity<Alumnos>()
                .Property(e => e.ApellidoMaterno)
                .IsUnicode(false);

            modelBuilder.Entity<Alumnos>()
                .Property(e => e.Grupo)
                .IsUnicode(false);

            modelBuilder.Entity<Alumnos>()
                .Property(e => e.Calificacion)
                .HasPrecision(18, 0);

           

            modelBuilder.Entity<Preguntas>()
                .Property(e => e.Sentencia)
                .IsUnicode(false);

            modelBuilder.Entity<Preguntas>()
                .Property(e => e.Respuesta1)
                .IsUnicode(false);

            modelBuilder.Entity<Preguntas>()
                .Property(e => e.Respuesta2)
                .IsUnicode(false);

            modelBuilder.Entity<Preguntas>()
                .Property(e => e.Respuesta3)
                .IsUnicode(false);

            modelBuilder.Entity<Preguntas>()
                .Property(e => e.Respuesta4)
                .IsUnicode(false);

            modelBuilder.Entity<Preguntas>()
                .Property(e => e.RespuestaCorrecta)
                .IsUnicode(false);

            

            modelBuilder.Entity<Respuestas>()
                .Property(e => e.RespuestaAlumno)
                .IsUnicode(false);
        }
    }
}
