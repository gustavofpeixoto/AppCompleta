using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using AutoMapper;
using DevIO.Business.Models;

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

        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProducsProviders()));
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
