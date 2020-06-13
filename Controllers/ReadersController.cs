using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ReaderStore.WebApp.Models;
using ReaderStore.WebApp.Models.Entities;
using ReaderStore.WebApp.Providers.Repositories;

namespace ReaderStore.WebApp.Controllers
{
    public class ReadersController : Controller
    {
        private readonly IReadersRepository _repo;
        private readonly IHostEnvironment _env;

        // default constructor
        // where any instance level
        // assignments happen
        public ReadersController(IReadersRepository repo, IHostEnvironment environment)
        {
            _repo = repo;
            _env = environment;
        }

        public IActionResult Index()
        {
            return View(_repo.Readers);
        }

        [Route("[controller]/{id}")]
        public IActionResult Index(Guid id)
        {
            var reader = _repo.GetReader(id);
            return View(reader);
        }

        // default GET Endpoint which
        // renders the View for us
        // from ~/Views/Readers/New.cshtml
        public IActionResult New()
        {
            return View();
        }

        // default POST Endpoint which
        // receives data from the Form submit
        // at ~/Views/Readers/New.cshtml
        // and returns the response to
        // the same View
        [HttpPost]
        public async Task<IActionResult> New(ReaderRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var res = await AddReader(model);

            if (res.IsSuccess)
            {
                return RedirectToActionPermanent("Index");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ReaderResponseModel> Add(ReaderRequestModel model)
        {
            return await AddReader(model);
        }

        private async Task<ReaderResponseModel> AddReader(ReaderRequestModel model)
        {
            var res = new ReaderResponseModel();

            // magic happens here
            // check if model is not empty
            if (model != null)
            {
                // create new entity
                var reader = new Reader();

                // add non-file attributes
                reader.Name = model.Name;
                reader.EmailAddress = model.EmailAddress;

                // check if any file is uploaded
                var work = model.Work;
                if (work != null)
                {
                    // get the file extension and 
                    // create a new File Name using Guid
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(work.FileName)}";

                    // create full file path using
                    // the IHostEnvironment.ContentRootPath
                    // which is basically the execution directory
                    // and append a sub directory workFiles 
                    // [Should be present before hand!!!]
                    // and lastly append the file name
                    var filePath = Path.Combine(_env.ContentRootPath, "Files", fileName);

                    // open-create the file in a stream and
                    // copy the uploaded file content into
                    // the new file (IFormFile contains a stream)
                    using (var fileSteam = new FileStream(filePath, FileMode.Create))
                    {
                        await work.CopyToAsync(fileSteam);
                    }

                    // assign the generated filePath to the 
                    // workPath property in the entity
                    reader.WorkPath = $"{Request.Scheme}://{Request.Host}/Files/{fileName}";
                }

                // add the created entity to the datastore
                // using a Repository class IReadersRepository
                // which is registered as a Scoped Service
                // in Startup.cs
                var created = _repo.AddReader(reader);

                // Set the Success flag and generated details
                // to show in the View 
                res.IsSuccess = true;
                res.ReaderId = created.Id.ToString();
                res.WorkPath = created.WorkPath;
                res.RedirectTo = Url.Action("Index");
            }

            // return the model back to view
            // with added changes and flags
            return res;
        }
    }
}