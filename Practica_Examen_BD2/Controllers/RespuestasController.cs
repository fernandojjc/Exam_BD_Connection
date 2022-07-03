using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Practica_Examen_BD2;

namespace Practica_Examen_BD2.Controllers
{
    public class RespuestasController : Controller
    {
        private ExamenDBContext3 db = new ExamenDBContext3();

        // GET: Respuestas
        public ActionResult Index()
        {
            ViewBag.datos = null;
            var lstPreguntas = db.Preguntas.ToList();
            ViewBag.lstPreguntas = lstPreguntas;
            //var respuestas = db.Respuestas.Include(r => r.Alumnos).Include(r => r.Preguntas);
            return View();
        }

        [HttpPost]
        public ActionResult Index(string nombre, string apellidoPaterno)
        {
            var datos = db.Alumnos.SqlQuery("select * from dbo.Alumnos where Nombre=@Nombre and ApellidoPaterno=@ApellidoPaterno", new SqlParameter("@Nombre", nombre), new SqlParameter("@ApellidoPaterno", apellidoPaterno)).ToList();
            System.Diagnostics.Debug.WriteLine(datos.Count);
            string date = DateTime.Now.ToString("dd-MM-yyyy");
            DateTime dateTimeNow = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            Console.WriteLine(dateTimeNow);
            var fechaNac = new DateTime();
            decimal? calificacion = -1;
            foreach (var i in datos)
            {
                fechaNac = i.FechaNacimiento;
                if (i.Calificacion==null) {
                    calificacion = -1;
                }
                else 
                { 
                    calificacion = i.Calificacion;
                }
                System.Diagnostics.Debug.WriteLine(i.FechaNacimiento);
            }

            DateTime now = DateTime.Today;

            int edad = DateTime.Today.Year - fechaNac.Year;

            if (DateTime.Today < fechaNac.AddYears(edad))
            {
                 --edad;
            }
            else
                _ = edad;

           // int edad = dateTimeNow.Year - fechaNac.Year;

            // Comprueba que el mes de la fecha de nacimiento es mayor 
            // que el mes de la fecha actual:
            //if (fechaNac.Month > dateTimeNow.Month)
            //{
            //    --edad;
            //}
            ViewBag.edad = edad;
            ViewBag.calificacion = calificacion;
            

            
            if (datos.Count == 0) {
                ViewBag.datos = null;
            }
            else {
                ViewBag.datos = datos;
            }
            
            return View(db.Preguntas.ToList());
        }

        // GET: Respuestas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respuestas respuestas = db.Respuestas.Find(id);
            if (respuestas == null)
            {
                return HttpNotFound();
            }
            return View(respuestas);
        }

        // GET: Respuestas/Create
        public ActionResult Create()
        {
            ViewBag.IdAlumno = new SelectList(db.Alumnos, "IdAlumno", "Nombre");
            ViewBag.IdPregunta = new SelectList(db.Preguntas, "IdPregunta", "Sentencia");
            return View();
        }

        // POST: Respuestas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(String[] IdPregunta, String[] Respuesta, String IdAlumno)
        {
            
            try
            {
                var alum = new Alumnos();
                var idAlum = Convert.ToInt32(IdAlumno);
                //var alum = db.Alumnos.Include("Respuestas").FirstOrDefault(x => x.IdAlumno == idAlum);
                alum.IdAlumno = idAlum;
                db.Alumnos.Attach(alum);
                

                System.Diagnostics.Debug.WriteLine("Entra en try");
                var lstPreguntas = IdPregunta.ToList();
                var lstRespuestas = Respuesta.ToList();


                for (var i = 0;i < lstPreguntas.Count; i++) {
                    
                    for (var j = 0; j < lstRespuestas.Count; j++) {
                        if (i == j) {
                            Respuestas respuestas = new Respuestas();
                            var pre = new Preguntas();
                            var idPreg = Convert.ToInt32(lstPreguntas[i]);                            
                            pre.IdPregunta = idPreg;
                            db.Preguntas.Attach(pre);
                            respuestas.Preguntas = pre;

                            System.Diagnostics.Debug.WriteLine("***************");
                            respuestas.Alumnos = alum;
                            //respuestas.IdPregunta = Convert.ToInt32(lstPreguntas[i]);
                            respuestas.RespuestaAlumno = (lstRespuestas[j]);
                            System.Diagnostics.Debug.WriteLine(lstPreguntas[i]);
                            System.Diagnostics.Debug.WriteLine(lstRespuestas[j]);
                            db.Respuestas.Add(respuestas);
                            db.SaveChanges();
                        }
                    }
                }

                //foreach (var i in lstPreguntas) {
                //    System.Diagnostics.Debug.WriteLine("********");
                //    System.Diagnostics.Debug.WriteLine(i);
                //    foreach (var x in lstRespuestas)
                //    {
                //        if () { 
                //        }
                //        System.Diagnostics.Debug.WriteLine(x);
                //        System.Diagnostics.Debug.WriteLine("********");
                //        count2++;

                //    }
                //    count++;
                    
                //}
                
                System.Diagnostics.Debug.WriteLine(IdAlumno);

                
                Alumnos a = new Alumnos();
                var lstResp = from c in db.Respuestas
                              join oc in db.Preguntas on c.IdPregunta equals oc.IdPregunta into gj
                              from oc in gj.DefaultIfEmpty()
                              where (int?)c.IdAlumno == idAlum 
                              select new
                              {
                                  c.IdAlumno,
                                  c.RespuestaAlumno,
                                  oc.RespuestaCorrecta
                              };

                var correctas = 0;
                foreach (var h in lstResp)
                {
                    if (h.RespuestaAlumno == h.RespuestaCorrecta) 
                    {
                        
                        correctas += 1;
                    }
                    System.Diagnostics.Debug.WriteLine(h.RespuestaAlumno + '-' + h.RespuestaCorrecta);
                }
                System.Diagnostics.Debug.WriteLine("Correctas"+correctas);

                var total = lstResp.Count();
                decimal? promedio = (correctas *10)/total;


                var alum2 = new Alumnos();
                var idAlum2 = Convert.ToInt32(IdAlumno);
                //var alum = db.Alumnos.Include("Respuestas").FirstOrDefault(x => x.IdAlumno == idAlum);
                alum2.IdAlumno = idAlum2;

                db.Alumnos.SqlQuery("update dbo.Alumnos set Calificacion = @Promedio where IdAlumno=@IdAlumno", new SqlParameter("@Promedio", promedio), new SqlParameter("@IdAlumno", idAlum2)).ToList();
                db.SaveChanges();
                //System.Diagnostics.Debug.WriteLine(promedio);
                //foreach (var y in alumnoEdit) 
                //
                //    System.Diagnostics.Debug.WriteLine("-----TYPES----");
                //    //System.Diagnostics.Debug.WriteLine(i.IdAlumno.GetType());
                //    //System.Diagnostics.Debug.WriteLine(i.Nombre.GetType());
                //    //System.Diagnostics.Debug.WriteLine(i.ApellidoPaterno.GetType());
                //    //System.Diagnostics.Debug.WriteLine(i.ApellidoMaterno.GetType());

                //    //System.Diagnostics.Debug.WriteLine(i.Grupo.GetType());
                //    //System.Diagnostics.Debug.WriteLine(i.Calificacion.GetType());

                //    //alum2.Nombre = Convert.ToString(i.Nombre);
                //    //alum2.ApellidoPaterno = Convert.ToString(i.ApellidoPaterno);
                //    //alum2.ApellidoMaterno = Convert.ToString(i.ApellidoMaterno);
                //    //DateTime dateTimeFN = DateTime.ParseExact(i.FechaNacimiento.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                //    //System.Diagnostics.Debug.WriteLine(dateTimeFN.GetType());
                //    //System.Diagnostics.Debug.WriteLine("-----------------");
                //    //alum2.FechaNacimiento = dateTimeFN;
                //    //System.Diagnostics.Debug.WriteLine(dateTimeFN);
                //    //alum2.Grupo = Convert.ToString(i.Grupo);
                //    //alum2.Calificacion = (promedio);
                //    System.Diagnostics.Debug.WriteLine("\nAlumno for each: " + y.IdAlumno + '-' + y.Nombre + '-' + y.ApellidoPaterno + '-' + y.ApellidoMaterno + '-' + y.FechaNacimiento + y.Grupo + '-' + y.Calificacion);
                //    System.Diagnostics.Debug.WriteLine(promedio);

                //}

                //db.Alumnos.Attach(alum2);
                //System.Diagnostics.Debug.WriteLine("Alumno: "+ alum2.IdAlumno + '-' + alum2.Nombre + '-' + alum2.ApellidoPaterno + '-' + alum2.ApellidoMaterno + '-' + alum2.FechaNacimiento + alum2.Grupo + '-' + alum2.Calificacion);
                //System.Diagnostics.Debug.WriteLine("--------Antes de attach -----");
                //db.Alumnos.Attach(a);
                //System.Diagnostics.Debug.WriteLine("--------despues de attach -----");
                //try {
                //    if (ModelState.IsValid)
                //    {

                //        db.Entry(alum2).State = EntityState.Modified;
                //        db.SaveChanges();
                //    }
                //}
                //catch (Exception) { System.Diagnostics.Debug.WriteLine(alum2.Nombre); }
                
                //respuestas. = lstPreguntas;
                //respuestas.RespuestaAlumno = (lstRespuestas[count]);
                //db.Respuestas.Add(respuestas);
                //db.SaveChanges();
                //System.Diagnostics.Debug.WriteLine(i[count]);
                //count++;
                return RedirectToAction("MostrarLista");
            }
            catch (Exception) {

                return RedirectToAction("MostrarLista");
            }

            //ViewBag.IdAlumno = new SelectList(db.Alumnos, "IdAlumno", "Nombre", respuestas.IdAlumno);
            //ViewBag.IdPregunta = new SelectList(db.Preguntas, "IdPregunta", "Sentencia", respuestas.IdPregunta);
            
        }

        // GET: Respuestas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respuestas respuestas = db.Respuestas.Find(id);
            if (respuestas == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdAlumno = new SelectList(db.Alumnos, "IdAlumno", "Nombre", respuestas.IdAlumno);
            ViewBag.IdPregunta = new SelectList(db.Preguntas, "IdPregunta", "Sentencia", respuestas.IdPregunta);
            return View(respuestas);
        }




        // POST: Respuestas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdRespuesta,IdAlumno,IdPregunta,RespuestaAlumno")] Respuestas respuestas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(respuestas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdAlumno = new SelectList(db.Alumnos, "IdAlumno", "Nombre", respuestas.IdAlumno);
            ViewBag.IdPregunta = new SelectList(db.Preguntas, "IdPregunta", "Sentencia", respuestas.IdPregunta);
            return View(respuestas);
        }

        // GET: Respuestas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respuestas respuestas = db.Respuestas.Find(id);
            if (respuestas == null)
            {
                return HttpNotFound();
            }
            return View(respuestas);
        }

        // POST: Respuestas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Respuestas respuestas = db.Respuestas.Find(id);
            db.Respuestas.Remove(respuestas);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
        public ActionResult MostrarLista()
        {
            var grupos = db.Alumnos.Select(m => m.Grupo).Distinct().ToList();
            ViewBag.grupos = grupos;
            ViewBag.datos = null;
            return View();

        }

        [HttpPost]
        public ActionResult MostrarLista(string grupo)
        {
            var grupos = db.Alumnos.Select(m => m.Grupo).Distinct().ToList();
            var datos = db.Alumnos.SqlQuery("select * from dbo.Alumnos where Grupo=@Grupo", new SqlParameter("@Grupo", grupo)).ToList();


            ViewBag.grupos = grupos;
            ViewBag.datos = datos;
            return View();
        }
    }
}
