using System;
using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.IMS.DAL.Interfaces;
using Grid.Features.IMS.Entities;
using Grid.Features.IMS.ViewModels;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.IMS.Controllers
{
    [GridPermission(PermissionCode = 250)]
    public class VendorsController : InventoryBaseController
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public VendorsController(IVendorRepository vendorRepository,
                                 IUnitOfWork unitOfWork)
        {
            _vendorRepository = vendorRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(VendorSearchViewModel vm)
        {
            Func<IQueryable<Vendor>, IQueryable<Vendor>> vendorFilter = q => q;
            vm.Vendors = _vendorRepository.SearchPage(vendorFilter, o => o.OrderByDescending(c => c.CreatedOn), vm.GetPageNo(), vm.PageSize);
            return View(vm);
        }

    }
}
