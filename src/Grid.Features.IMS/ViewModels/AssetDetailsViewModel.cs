using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Clients.ITSync.Models;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;
using Grid.Features.IMS.Entities;
using Grid.Features.IMS.Entities.Enums;

namespace Grid.Features.IMS.ViewModels
{
    public class AssetDetailsViewModel: ViewModelBase
    {
        [DisplayName("Serial Number")]
        public string SerialNumber { get; set; }

        [DisplayName("Tag Number")]
        public string TagNumber { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        public string Specifications { get; set; }

        [NotMapped]
        public string SpecificationsTrimmed
        {
            get
            {
                if (!string.IsNullOrEmpty(Specifications))
                {
                    if (Specifications.Length > 50)
                        return Specifications.Substring(0, 50) + "...";

                    return Specifications;
                }

                return string.Empty;
            }
        }

        public string Brand { get; set; }

        public decimal Cost { get; set; }

        [DisplayName("Model Number")]
        public string ModelNumber { get; set; }

        [DisplayName("Is Brand New")]
        public bool IsBrandNew { get; set; }
        public AssetState State { get; set; }

        [DisplayName("Purchase Date")]
        [UIHint("Date")]
        public DateTime? PurchaseDate { get; set; }

        [DisplayName("Warranty Expiry Date")]
        [UIHint("Date")]
        public DateTime? WarrantyExpiryDate { get; set; }

        public int AssetCategoryId { get; set; }
        public AssetCategory AssetCategory { get; set; }

        public int? DepartmentId { get; set; }
        public Department Department { get; set; }

        public int? VendorId { get; set; }
        public Vendor Vendor { get; set; }

        public int? AllocatedUserId { get; set; }
        public Features.HRMS.Entities.Employee AllocatedUser { get; set; }
        public List<AssetAllocation> AssetAllocations { get; set; }

        public bool HasSoftwareInfo { get; set; }
        public List<SoftwareModel> Softwares { get; set; }

        public bool HasHardwareInfo { get; set; }
        public HardwareModel Hardware { get; set; }

        public List<AssetDocument> AssetDocuments { get; set; }

        public AssetDetailsViewModel()
        {
            AssetAllocations = new List<AssetAllocation>();
            Softwares = new List<SoftwareModel>();
            AssetDocuments = new List<AssetDocument>();
        }

        public AssetDetailsViewModel(Asset asset): this()
        {
            Id = asset.Id;
            SerialNumber = asset.SerialNumber;
            TagNumber = asset.TagNumber;
            Title = asset.Title;
            Description = asset.Description;
            Specifications = asset.Specifications;
            Brand = asset.Brand;
            Cost = asset.Cost;
            ModelNumber = asset.ModelNumber;
            IsBrandNew = asset.IsBrandNew;
            State = asset.State;
            PurchaseDate = asset.PurchaseDate;
            WarrantyExpiryDate = asset.WarrantyExpiryDate;
            AssetCategoryId = asset.AssetCategoryId;
            AssetCategory = asset.AssetCategory;
            DepartmentId = asset.DepartmentId;
            Department = asset.Department;
            VendorId = asset.VendorId;
            Vendor = asset.Vendor;
            AllocatedUserId = asset.AllocatedEmployeeId;
            AllocatedUser = asset.AllocatedEmployee;
        }
    }
}