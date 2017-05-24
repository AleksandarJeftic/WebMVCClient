using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebMVCClient.Models;
using WebMVCClient.ViewModels;
using System.IO;

namespace WebMVCClient.Controllers
{
    public class StudentController : Controller
    {
        HttpClient client = new HttpClient();
        // GET: Students
        public ActionResult Index()
        {
            IEnumerable<StudentWithImage> stwi = null;
            client.BaseAddress = new Uri("http://localhost:55973/api/");

            var task = client.GetAsync("student/getallstudents");
            task.Wait();

            var result = task.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<StudentWithImage>>();
                readTask.Wait();
               stwi = readTask.Result;
            }
            else
            {
                stwi = Enumerable.Empty<StudentWithImage>();
                ModelState.AddModelError(string.Empty, "Server Error");
            }

            return View(stwi);
        }





        // GET: Student
        public ActionResult Details(int id)
        {

            StudentWithImage stwi = null;
            client.BaseAddress= new Uri("http://localhost:55973/api/");

            var task = client.GetAsync("student/getdetails?id=" + id.ToString());
            task.Wait();

            var result = task.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<StudentWithImage>();
                readTask.Wait();
                stwi = readTask.Result;
            }
            else
            {
                
                ModelState.AddModelError(string.Empty, "Server Error");
            }

            return View(stwi);


        }

        public ActionResult Create ()
        {
            return View();
        }




        //POST:Student
        [HttpPost]
        public ActionResult Create(Student s,HttpPostedFileBase uploadFile)
        {
            string pic = Path.GetFileName(uploadFile.FileName);
            string path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Images"),pic);
            uploadFile.SaveAs(path);

            path = "~/Images/" + pic;

            var stwi = new StudentWithImage
            {
                StudentName = s.StudentName,
                StudentLastName = s.StudentLastName,
                ImagePath = path.ToString()
            };

            client.BaseAddress = new Uri("http://localhost:55973/api/");

            var task = client.PostAsJsonAsync("student/create",stwi);
            task.Wait();

            var result = task.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {

                ModelState.AddModelError(string.Empty, "Server Error");
                return View(stwi);
            }

        }


            //DELETE: Student
            public ActionResult Delete(int id)
            {
            client.BaseAddress = new Uri("http://localhost:55973/api/");

            var task = client.DeleteAsync("student/Delete/"+id.ToString() );
            task.Wait();

            var result = task.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
            }




            //GET EDIT
            [HttpGet]
            public ActionResult Edit(int id)

            {

                StudentWithImage stwi = null;
                

                client.BaseAddress = new Uri("http://localhost:55973/api/");

                var task = client.GetAsync("student/getdetails?id=" + id.ToString());
                task.Wait();

                var result = task.Result;

             if (result.IsSuccessStatusCode)
                {
                var readTask = result.Content.ReadAsAsync<StudentWithImage>();
                readTask.Wait();
                stwi = readTask.Result;
                 }
            else
                {

                ModelState.AddModelError(string.Empty, "Server Error");
                }
                    stwi.StudentID = id;
                    return View(stwi); 

            }

        //POST EDIT
        [HttpPost]
        public ActionResult Edit(StudentWithImage stwi,HttpPostedFileBase uploadFile)
        {
           
            if (uploadFile != null)
            {
                string pic = Path.GetFileName(uploadFile.FileName);
                string path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Images"), pic);
                uploadFile.SaveAs(path);

                path = "~/Images/" + pic;

                
                stwi.ImagePath = path;

            }

            client.BaseAddress = new Uri("http://localhost:55973/api/");

            var task = client.PostAsJsonAsync("student/update", stwi);
            task.Wait();

            var result = task.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {

                ModelState.AddModelError(string.Empty, "Server Error");
                return View(stwi);
            }

        }

    }
}