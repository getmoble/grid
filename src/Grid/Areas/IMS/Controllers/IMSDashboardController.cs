using System.Linq;
using System.Text;
using System.Web.Mvc;
using Grid.Features.IMS.DAL.Interfaces;

namespace Grid.Areas.IMS.Controllers
{
    public class IMSDashboardController : InventoryBaseController
    {
        private readonly IAssetRepository _assetRepository;
        public IMSDashboardController(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }
        #region AjaxCalls
        public FileContentResult AssetsByVendorCSV()
        {
            var csv = new StringBuilder();
            var assetsByVendor = _assetRepository.GetAll()
                                   .GroupBy(l => l.Vendor.Title)                                 
                                  .Select(x => new
                                  {
                                      x.Key,
                                      Value = x.Count()
                                  })
                                  .ToList();

            var keys = string.Join(",", assetsByVendor.Select(x => x.Key).ToArray());
            csv.AppendLine(keys);
            var values = string.Join(",", assetsByVendor.Select(x => x.Value).ToArray());
            csv.AppendLine(values);
            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "AssetsByVendorCSV.csv");
        }

      
        public FileContentResult AssetsByStateCSV()
        {
            var csv = new StringBuilder();
            var assetsByState = _assetRepository.GetAll()
                                  .GroupBy(l => l.State)
                                  .Select(x => new
                                  {
                                      x.Key,
                                      Value = x.Count()
                                  })
                                  .ToList();

            var keys = string.Join(",", assetsByState.Select(x => x.Key).ToArray());
            csv.AppendLine(keys);
            var values = string.Join(",", assetsByState.Select(x => x.Value).ToArray());
            csv.AppendLine(values);
            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "AssetsByStateCSV.csv");
        }
        public FileContentResult AssetsByDepartmentCSV()
        {
            var csv = new StringBuilder();
            var assetsByDepartment = _assetRepository.GetAll()
                                  .GroupBy(l => l.Department.Title)
                                  .Select(x => new
                                  {
                                      x.Key,
                                      Value = x.Count()
                                  })
                                  .ToList();

            var keys = string.Join(",", assetsByDepartment.Select(x => x.Key).ToArray());
            csv.AppendLine(keys);
            var values = string.Join(",", assetsByDepartment.Select(x => x.Value).ToArray());
            csv.AppendLine(values);
            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "AssetsByDepartmentCSV.csv");
        }
        public FileContentResult AssetsByCategoryCSV()
        {
            var csv = new StringBuilder();
            var assetsByCategory = _assetRepository.GetAll()
                                  .GroupBy(l => l.AssetCategory.Title)
                                  .Select(x => new
                                  {
                                      x.Key,
                                      Value = x.Count()
                                  })
                                  .ToList();

            var keys = string.Join(",", assetsByCategory.Select(x => x.Key).ToArray());
            csv.AppendLine(keys);
            var values = string.Join(",", assetsByCategory.Select(x => x.Value).ToArray());
            csv.AppendLine(values);
            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "AssetsByCategoryCSV.csv");
        }
        #endregion
        public ActionResult Index()
        {
            var totalAssets = _assetRepository.Count();
            var total = _assetRepository.GetAll();
            var totalValue = total.Sum(i => i.Cost);
        
            ViewBag.Total = totalAssets;
            ViewBag.TotalValue = totalValue;
            return View();
        }
    }
}