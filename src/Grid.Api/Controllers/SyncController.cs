using System.Web.Mvc;
using Grid.Clients.ITSync.Models;
using Grid.Features.Common;
using Grid.Features.IMS.DAL.Interfaces;
using Grid.Features.IT.DAL.Interfaces;
using Grid.Features.IT.Entities;
using Grid.Infrastructure;
using Grid.Infrastructure.Filters;
using Newtonsoft.Json;

namespace Grid.Api.Controllers
{
    public class SyncController : GridBaseController
    {
        private readonly IAssetRepository _assetRepository;
        private readonly ISystemSnapshotRepository _systemSnapshotRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SyncController(IAssetRepository assetRepository,
                              ISystemSnapshotRepository systemSnapshotRepository,
                              IUnitOfWork unitOfWork)
        {
            _assetRepository = assetRepository;
            _systemSnapshotRepository = systemSnapshotRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [APIIdentityInjector]
        public ActionResult Create(SystemInfo vm)
        {
            var selectedAsset = _assetRepository.GetBy(a => a.TagNumber == vm.AssetId);

            if (selectedAsset != null && WebUser != null)
            {
                var previousSnapShot = _systemSnapshotRepository.GetBy(s => s.UserId == WebUser.Id && s.AssetId == selectedAsset.Id);

                // No previous snapshots exists
                if (previousSnapShot == null)
                {
                    var snapShot = new SystemSnapshot
                    {
                        UserId = WebUser.Id,
                        AssetId = selectedAsset.Id,
                        Softwares = JsonConvert.SerializeObject(vm.Softwares),
                        Hardwares = JsonConvert.SerializeObject(vm.Hardware),
                        RanOn = vm.RanOn
                    };

                    _systemSnapshotRepository.Create(snapShot);
                    _unitOfWork.Commit();
                }
                else
                {
                    previousSnapShot.Softwares = JsonConvert.SerializeObject(vm.Softwares);
                    previousSnapShot.Hardwares = JsonConvert.SerializeObject(vm.Hardware);
                    previousSnapShot.RanOn = vm.RanOn;

                    _systemSnapshotRepository.Update(previousSnapShot);
                    _unitOfWork.Commit();
                }

                return Json(true);
            }

            return Json(false);
        }
    }
}