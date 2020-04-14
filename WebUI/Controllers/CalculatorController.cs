using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using TaxCalculator.Domain.Entities;
using System.Net.Http;
using TaxCalculator.Application.Calculate;
using Newtonsoft.Json;
using System.Linq;

namespace TaxCalculator.WebUI.Controllers
{

    public class CalculatorController : Controller
    {
        private readonly Calculate _calculate;

        public CalculatorController()
        {
            _calculate = new Calculate();
        }

        public async Task<IActionResult> Index()
        {
            IList<IncomeTax> entriesList = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44324/api/incometaxapi");

                //HTTP GET
                var responseTask = client.GetAsync("IncomeTaxApi");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<IncomeTax>>();
                    readTask.Wait();

                    entriesList = readTask.Result;
                }
            }

            return View("List", entriesList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IncomeTax model)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44324/api/IncomeTaxApi");

                    model.TaxAmount = await CalculateTax(model.IncomeAmount, model.PostalCode);

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<IncomeTax>("incometaxapi", model);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44324/api/");

                //HTTP GET
                var getTask = client.GetAsync("incometaxapi?id=" + id.ToString());
                getTask.Wait();

                var result = getTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsAsync<List<IncomeTax>>();

                    return View(readTask.FirstOrDefault());
                }
                else 
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid Id, IncomeTax model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44324/api/");

                double taxAmount = await CalculateTax(model.IncomeAmount, model.PostalCode);
                model.TaxAmount = taxAmount;

                //HTTP POST
                var putTask = client.PutAsJsonAsync<IncomeTax>("incometaxapi/" + Id.ToString(), model);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44324/api/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("incometaxapi/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<double> CalculateTax(double incomeAmount, string postalCode)
        {
            double taxDetermindedValue = 0;
            taxDetermindedValue = _calculate.AnnualIncomeTax(incomeAmount, postalCode);

            return taxDetermindedValue;
        }

        [HttpPost]
        public async Task<string> CalculateType(string postalCode)
        {
            string taxCalculationType = "";
            taxCalculationType = _calculate.TaxType(postalCode);

            return taxCalculationType;
        }
    }
}
