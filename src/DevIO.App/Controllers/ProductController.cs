using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using AutoMapper;
using DevIO.Business.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace DevIO.App.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository,
            IProviderRepository providerRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _providerRepository = providerRepository;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetFilteredItems()
        {
            int draw = Convert.ToInt32(Request.Query["draw"]);
            int start = Convert.ToInt32(Request.Query["start"]);
            int length = Convert.ToInt32(Request.Query["length"]);
            int sortColumnIdx = Convert.ToInt32(Request.Query["order[0][column]"]);
            string sortColumnName = Request.Query["columns[" + sortColumnIdx + "][name]"];
            string sortColumnDirection = Request.Query["order[0][dir]"];
            string searchValue = Request.Query["search[value]"].FirstOrDefault()?.Trim();
            var recordsFiltered = await _productRepository.Find(p => p.Name.Contains(searchValue));
            int recordsFilteredCount = recordsFiltered.Count();
            var recordsTotal = await _productRepository.GetAll();
            int recordsTotalCount = recordsTotal.Count();

            ICollection<ProductViewModel> filteredData = null;

            if (sortColumnDirection == "asc")
                filteredData = _mapper.Map<ICollection<ProductViewModel>>(await _productRepository.GetProducsProviders())
                    .Where(p => p.Name.Contains(searchValue))
                    .OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x))
                    .Skip(start)
                    .Take(length)
                    .ToList();

            else
                filteredData = _mapper.Map<ICollection<ProductViewModel>>(await _productRepository.GetProducsProviders())
                    .Where(p => p.Name.Contains(searchValue))
                    .OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x))
                    .Skip(start)
                    .Take(length)
                    .ToList();

            return Json(new
            {
                data = filteredData,
                draw = Request.Query["draw"],
                recordsFiltered = recordsFilteredCount,
                recordsTotal = recordsTotalCount
            });

        }


        public async Task<IActionResult> Details(Guid id)
        {
            var productViewModel = await GetProductAndAllProviders(id);
            if (productViewModel == null) return NotFound();

            return View(productViewModel);
        }

        public async Task<IActionResult> Create()
        {
            return View(await FillProviders(new ProductViewModel()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            productViewModel = await FillProviders(productViewModel);

            if (!ModelState.IsValid) return View(productViewModel);

            string prefixo = string.Format("{0}_", Guid.NewGuid());

            if (!await UploadFile(productViewModel.PictureUpload, prefixo))
                return View(productViewModel);

            productViewModel.Picture = string.Format("{0}{1}", prefixo, productViewModel.PictureUpload.FileName);
            await _productRepository.Add(_mapper.Map<Product>(productViewModel));

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var productViewModel = await GetProductAndAllProviders(id);
            if (productViewModel == null) return NotFound();

            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id) return NotFound();

            ProductViewModel product = await GetProductAndAllProviders(id);
            productViewModel.Providers = product.Providers;
            productViewModel.Picture = product.Picture;

            if (!ModelState.IsValid) return View(productViewModel);

            if (productViewModel.PictureUpload != null)
            {
                string prefixo = string.Format("{0}_", Guid.NewGuid());

                if (!await UploadFile(productViewModel.PictureUpload, prefixo))
                    return View(productViewModel);

                product.Picture = string.Format("{0}{1}", prefixo, productViewModel.PictureUpload.FileName);
            }

            product.Name = productViewModel.Name;
            product.Price = productViewModel.Price;
            product.Name = productViewModel.Name;
            product.Active = productViewModel.Active;

            await _productRepository.Update(_mapper.Map<Product>(product));

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var productViewModel = await GetProductAndAllProviders(id);
            if (productViewModel == null) return NotFound();

            return View(productViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var productViewModel = await GetProductAndAllProviders(id);

            if (productViewModel == null) return NotFound();

            await _productRepository.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<ProductViewModel> GetProductAndAllProviders(Guid id)
        {
            ProductViewModel productViewModel = _mapper.Map<ProductViewModel>(await _productRepository.GetProductProvider(id));
            productViewModel.Providers = _mapper.Map<IEnumerable<ProviderViewModel>>(await _providerRepository.GetAll());
            return productViewModel;
        }

        private async Task<ProductViewModel> FillProviders(ProductViewModel productViewModel)
        {
            productViewModel.Providers = _mapper.Map<IEnumerable<ProviderViewModel>>(await _providerRepository.GetAll());
            return productViewModel;
        }

        private async Task<bool> UploadFile(IFormFile file, string prefixo)
        {
            if (file.Length <= 0) return false;

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", string.Concat(prefixo, file.FileName));

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return true;
        }
    }
}
