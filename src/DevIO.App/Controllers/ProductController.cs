using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using AutoMapper;
using DevIO.Business.Models;
using System.Linq;

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
            return View(/*_mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProducsProviders())*/);
        }


        //Action to be called by js in details page when search, sort or page numbers clicked
        // Search is applied only to Firstname and Lastname properties
        public async Task<JsonResult> GetFilteredItems()
        {
            System.Threading.Thread.Sleep(2000);//Used to display loading spinner in demonstration, remove this line in production
            int draw = Convert.ToInt32(Request.Query["draw"]);

            // Data to be skipped , 
            // if 0 first "length" records will be fetched
            // if 1 second "length" of records will be fethced ...
            int start = Convert.ToInt32(Request.Query["start"]);

            // Records count to be fetched after skip
            int length = Convert.ToInt32(Request.Query["length"]);

            // Getting Sort Column Name
            int sortColumnIdx = Convert.ToInt32(Request.Query["order[0][column]"]);
            string sortColumnName = Request.Query["columns[" + sortColumnIdx + "][name]"];

            // Sort Column Direction  
            string sortColumnDirection = Request.Query["order[0][dir]"];

            // Search Value
            string searchValue = Request.Query["search[value]"].FirstOrDefault()?.Trim();

            // Records Count matching search criteria 
            var recordsFiltered = await _productRepository.Find(p => p.Name.Contains(searchValue));
            int recordsFilteredCount = recordsFiltered.Count();

            //.Where(a => a.Lastname.Contains(searchValue) || a.Firstname.Contains(searchValue))
            //.Count();

            // Total Records Count
            var recordsTotal = await _productRepository.GetAll();
            int recordsTotalCount = recordsTotal.Count();

            // Filtered & Sorted & Paged data to be sent from server to view
            ICollection<ProductViewModel> filteredData = null;
            if (sortColumnDirection == "asc")
            {
                filteredData = _mapper.Map<ICollection<ProductViewModel>>(await _productRepository.Find(p => p.Name.Contains(searchValue)))
                    .OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x))
                    .Skip(start)
                    .Take(length)
                    .ToList();


                //Data.StudentContext.StudentList
                //.Where(a => a.Lastname.Contains(searchValue) || a.Firstname.Contains(searchValue))
                //.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x))//Sort by sortColumn
                //.Skip(start)
                //.Take(length)
                //.ToList<Student>();
            }
            else
            {
                filteredData = _mapper.Map<ICollection<ProductViewModel>>(await _productRepository.Find(p => p.Name.Contains(searchValue)))
                    .OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x))
                    .Skip(start)
                    .Take(length)
                    .ToList();

                //filteredData =
                //   Data.StudentContext.StudentList
                //   .Where(a => a.Lastname.Contains(searchValue) || a.Firstname.Contains(searchValue))
                //   .OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x))
                //   .Skip(start)
                //   .Take(length)
                //   .ToList<Student>();
            }
            // Send data 
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

            await _productRepository.Add(_mapper.Map<Product>(productViewModel));

            return View(productViewModel);
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

            if (ModelState.IsValid) return RedirectToAction(nameof(Index));

            await _productRepository.Update(_mapper.Map<Product>(productViewModel));
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
    }
}
