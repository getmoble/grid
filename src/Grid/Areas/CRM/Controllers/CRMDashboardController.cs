using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Grid.Features.CRM.DAL.Interfaces;

namespace Grid.Areas.CRM.Controllers
{
    public class CRMDashboardController : CRMBaseController
    {
        private readonly ICRMLeadRepository _crmLeadRepository;

        public CRMDashboardController(ICRMLeadRepository crmLeadRepository)
        {
            _crmLeadRepository = crmLeadRepository;
        }

        #region AjaxCalls
        public FileContentResult LeadsByStatusCSV()
        {
            var csv = new StringBuilder();
            var leadsByStatus = _crmLeadRepository.GetAll().ToList()
                                  .GroupBy(l => l.LeadStatus.Name)
                                  .Select(x => new
                                  {
                                      x.Key,
                                      Value = x.Count()
                                  })
                                  .ToList();

            var keys = string.Join(",", leadsByStatus.Select(x => x.Key).ToArray());
            csv.AppendLine(keys);
            var values = string.Join(",", leadsByStatus.Select(x => x.Value).ToArray());
            csv.AppendLine(values);
            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "LeadsByStatusCSV.csv");
        }
        public FileContentResult LeadsBySourceCSV()
        {
            var csv = new StringBuilder();
            var leadsByStatus = _crmLeadRepository.GetAll().ToList()
                                  .GroupBy(l => l.LeadSource.Title)
                                  .Select(x => new
                                  {
                                      x.Key,
                                      Value = x.Count()
                                  })
                                  .ToList();

            var keys = string.Join(",", leadsByStatus.Select(x => x.Key).ToArray());
            csv.AppendLine(keys);
            var values = string.Join(",", leadsByStatus.Select(x => x.Value).ToArray());
            csv.AppendLine(values);
            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "LeadsBySourceCSV.csv");
        }
        public FileContentResult LeadsByDateCSV()
        {
            var csv = new StringBuilder();
            csv.AppendLine("x, leads count");
            
            var firstDay = DateTime.Now.AddDays(-31);
            var leads = _crmLeadRepository.GetAllBy(l => l.CreatedOn >= firstDay).ToList()
                          .GroupBy(x => x.CreatedOn.Date)
                          .Select(x => new
                          {
                            Value = x.Count(),
                            Day = (DateTime)x.Key
                          })
                          .ToList();

            foreach (var lead in leads)
            {
                var date = lead.Day.ToShortDateString();
                csv.AppendLine($"{date}, {lead.Value}");
            }

            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "LeadsByDateCSV.csv");
        }
        #endregion

        public ActionResult Index()
        {
            return View();
        }
    }
}