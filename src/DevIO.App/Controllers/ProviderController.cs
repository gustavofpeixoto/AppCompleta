using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using AutoMapper;
using DevIO.Business.Models;
using System.Collections.Generic;
using System.Linq;

namespace DevIO.App.Controllers
{
    public class ProviderController : BaseController
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IMapper _mapper;

        public ProviderController(IProviderRepository providerRepository, IMapper mapper)
        {
            _providerRepository = providerRepository;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetFilteredItems()
        {
            int draw = Convert.ToInt32(Request.Query["draw"]);
            int start = Convert.ToInt32(Request.Query["start"]);
            int length = Convert.ToInt32(Request.Query["length"]);
            int sortColumnIdx = Convert.ToInt32(Request.Query["order[0][column]"]);
            string sortColumnName = Request.Query["columns[" + sortColumnIdx + "][name]"];
            string sortColumnDirection = Request.Query["order[0][dir]"];
            string searchValue = Request.Query["search[value]"].FirstOrDefault()?.Trim();
            var recordsFiltered = await _providerRepository.Find(p => p.Name.Contains(searchValue));
            int recordsFilteredCount = recordsFiltered.Count();
            var recordsTotal = await _providerRepository.GetAll();
            int recordsTotalCount = recordsTotal.Count();

            var columns = Request.Query["columns"].Count;

            ICollection<ProviderViewModel> filteredData = null;

            if (sortColumnDirection == "asc")
                filteredData = _mapper.Map<ICollection<ProviderViewModel>>(await _providerRepository.Find(p => p.Name.Contains(searchValue)))
                    .OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x))
                    .Skip(start)
                    .Take(length)
                    .ToList();

            else
                filteredData = _mapper.Map<ICollection<ProviderViewModel>>(await _providerRepository.Find(p => p.Name.Contains(searchValue)))
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
            var providerViewModel = await GetProviderAddress(id);
            if (providerViewModel == null) return NotFound();

            return View(providerViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProviderViewModel providerViewModel)
        {
            if (!ModelState.IsValid) return View(providerViewModel);

            await _providerRepository.Add(_mapper.Map<Provider>(providerViewModel));

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var providerViewModel = await GetProviderProductAddress(id);

            if (providerViewModel == null) return NotFound();

            return View(providerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProviderViewModel providerViewModel)
        {
            if (id != providerViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(providerViewModel);

            await _providerRepository.Update(_mapper.Map<Provider>(providerViewModel));

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            ProviderViewModel providerViewModel = await GetProviderProductAddress(id);
            if (providerViewModel == null) return NotFound();

            return View(providerViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            ProviderViewModel providerViewModel = await GetProviderProductAddress(id);
            if (providerViewModel == null) return NotFound();

            await _providerRepository.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<ProviderViewModel> GetProviderAddress(Guid id)
        {
            return _mapper.Map<ProviderViewModel>(await _providerRepository.GetProviderAddress(id));
        }

        private async Task<ProviderViewModel> GetProviderProductAddress(Guid id)
        {
            return _mapper.Map<ProviderViewModel>(await _providerRepository.GetProviderProductsAddress(id));
        }
    }
}
