#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CaseItauWeb2.Data;
using CaseItauWeb2.Models;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace CaseItauWeb2.Controllers
{
    public class FundoController : Controller
    {
        private readonly CaseItauWeb2Context _context;

        public FundoController(CaseItauWeb2Context context)
        {
            _context = context;
        }

        // GET: Fundo
        public async Task<IActionResult> Index()
        {
            var retornoJson = ConsultarIntegracao("https://localhost:44378/api/fundo");
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            return View(JsonConvert.DeserializeObject<List<Fundo>>(retornoJson, settings));
        }

        // GET: Fundo/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retornoJson = ConsultarIntegracao("https://localhost:44378/api/fundo/" + id);
            if (retornoJson == null)
            {
                return NotFound();
            }

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            return View(JsonConvert.DeserializeObject<Fundo>(retornoJson, settings));

        }

        // GET: Fundo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fundo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("codigo,nome,cnpj,codigoTipo,nomeTipo,patrimonio")] Fundo fundo)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = JsonConvert.SerializeObject(fundo);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                PersistirDadosPost("https://localhost:44378/api/fundo", contentString);
            }
            return View();
        }

        // GET: Fundo/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retornoJson = ConsultarIntegracao("https://localhost:44378/api/fundo/" + id);

            if (retornoJson == null)
            {
                return NotFound();
            }

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            return View(JsonConvert.DeserializeObject<Fundo>(retornoJson, settings));

        }

        // POST: Fundo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("codigo,nome,cnpj,codigoTipo,nomeTipo,patrimonio")] Fundo fundo)
        {
            if (id != fundo.codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var jsonContent = JsonConvert.SerializeObject(fundo);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                PersistirDadosPut("https://localhost:44378/api/fundo/" + id, contentString);

                return RedirectToAction(nameof(Index));
            }
            return View(fundo);
        }

        // GET: Fundo/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retornoJson = ConsultarIntegracao("https://localhost:44378/api/fundo/" + id);

            if (retornoJson == null)
            {
                return NotFound();
            }

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            return View(JsonConvert.DeserializeObject<Fundo>(retornoJson, settings));
        }

        // POST: Fundo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var jsonContent = JsonConvert.SerializeObject(new Fundo());
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            ExcluirDados("https://localhost:44378/api/fundo/" + id, contentString);
            return RedirectToAction(nameof(Index));
        }

        // GET: Fundo/Movimentar/5
        public async Task<IActionResult> Movimentar(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retornoJson = ConsultarIntegracao("https://localhost:44378/api/fundo/" + id);

            if (retornoJson == null)
            {
                return NotFound();
            }

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            return View(JsonConvert.DeserializeObject<Fundo>(retornoJson, settings));

        }

        // PUT: Fundo/Movimentar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Movimentar(string id, [Bind("codigo,nome,cnpj,codigoTipo,nomeTipo,patrimonio")] Fundo fundo)
        {
            if (id != fundo.codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var jsonContent = JsonConvert.SerializeObject(fundo);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                PersistirDadosPut("https://localhost:44378/api/fundo/" + id + "/" + fundo.patrimonio, contentString);

                return RedirectToAction(nameof(Index));
            }
            return View(fundo);
        }


    private bool FundoExists(string id)
        {
            return _context.Fundo.Any(e => e.codigo == id);
        }

        protected static string ConsultarIntegracao(string url)
        {
            var retorno = string.Empty;

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(10);

                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                var response = (Task.Run(async () => await client.GetAsync(url))).Result;

                retorno = response.Content.ReadAsStringAsync().Result;

            }
            return retorno;
        }

        protected string PersistirDadosPost(string url, StringContent contentString)
        {
            var retorno = string.Empty;


            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(10);

                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                var response = (
            Task.Run
            (async () => await client.PostAsync(url, contentString))).Result;

                retorno = response.Content.ReadAsStringAsync().Result;
            }

            return retorno;
        }

        protected string PersistirDadosPut(string url, StringContent contentString)
        {
            var retorno = string.Empty;


            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(10);

                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                var response = (
            Task.Run
            (async () => await client.PutAsync(url, contentString))).Result;

                retorno = response.Content.ReadAsStringAsync().Result;
            }

            return retorno;
        }


        protected string ExcluirDados(string url, StringContent contentString)
        {
            var retorno = string.Empty;

            using (var client = new HttpClient())
            {

                client.Timeout = TimeSpan.FromMinutes(10);

                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url),
                    Content = contentString
                };

                var response = (
                Task.Run
                (async () => await client.SendAsync(request))).Result;

                retorno = response.Content.ReadAsStringAsync().Result;
            }

            return retorno;
        }

    }
}