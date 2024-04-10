using ApiInClass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace ApiInClass.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IEnumerable<Staff> staffs = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61967/api/");
                //HTTP GET
                var responseTask = client.GetAsync("Staff");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Staff>>();
                    readTask.Wait();

                    staffs = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    staffs = Enumerable.Empty<Staff>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(staffs);
        }
        public ActionResult create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult create(Staff staff)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61967/api/Staff");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<Staff>("Staff", staff);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(staff);
        }
        public ActionResult Edit(int id)
        {
            Staff staff = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61967/api/");
                //HTTP GET
                var responseTask = client.GetAsync("Staff?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Staff>();
                    readTask.Wait();
                    staff = readTask.Result;
                }
            }
            return View(staff);
        }
        [HttpPost]
        public ActionResult Edit(Staff staff)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61967/api/Staff");

                //HTTP POST
                var putTask = client.PutAsJsonAsync<Staff>("Staff", staff);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }
            return View(staff);
        }
        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61967/api/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("Staff/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
    }
}
