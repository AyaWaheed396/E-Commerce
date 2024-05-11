using Admin_Dashboard.Helpers;
using Admin_Dashboard.Models;
using AutoMapper;
using Core.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Admin_Dashboard.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _unitOfWork.Reposatory<Product>().GetAllAsync();
            var mappedProducts = _mapper.Map<IReadOnlyList<ProductViewModel>>(products);
            return View(mappedProducts);
        }

        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                if (productViewModel.Image != null)
                    productViewModel.PictureUrl = DocumentSettings.UploadFile(productViewModel.Image, "products");

                var mappedProduct = _mapper.Map<Product>(productViewModel);

                await _unitOfWork.Reposatory<Product>().Add(mappedProduct);
                await _unitOfWork.Complete();

                return RedirectToAction("Index");

            }
            return View(productViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Reposatory<Product>().GetByIdAsync(id);
            if (product.PictureUrl != null)
                DocumentSettings.DeleteFile(product.PictureUrl, "products");


            _unitOfWork.Reposatory<Product>().Delete(product);

            await _unitOfWork.Complete();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _unitOfWork.Reposatory<Product>().GetByIdAsync(id);
            var mappedProduct = _mapper.Map<ProductViewModel>(product);

            return View(mappedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                if (productViewModel.Image != null)
                {
                    if(productViewModel.PictureUrl  != null)
                        DocumentSettings.DeleteFile(productViewModel.PictureUrl, "products");

                    productViewModel.PictureUrl = DocumentSettings.UploadFile(productViewModel.Image, "products");

                }

                var mappedProduct = _mapper.Map<Product>(productViewModel);

                _unitOfWork.Reposatory<Product>().Update(mappedProduct);
                await _unitOfWork.Complete();

                return RedirectToAction("Index");

            }
            return View(productViewModel);
        }




    }
}
