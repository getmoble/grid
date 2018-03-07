using Grid.Features.IMS.Entities;
using Grid.Features.IMS.Entities.Enums;
using System;

namespace Grid.Api.Models.IMS
{
    public class AssetModel : ApiModelBase
    {
        public string SerialNumber { get; set; }
        public string TagNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Specifications { get; set; }
        public string BrandAndModel { get; set; }
        public string Brand { get; set; }
        public decimal Cost { get; set; }
        public string ModelNumber { get; set; }
        public bool IsBrandNew { get; set; }
        public string StateType { get; set; }
        public AssetState State { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyExpiryDate { get; set; }
        public int AssetCategoryId { get; set; }
        public string AssetCategory { get; set; }
        public int? DepartmentId { get; set; }
        public string Department { get; set; }
        public int? VendorId { get; set; }
        public string Vendor { get; set; }
        public string TitleAndModel { get; set; }
        public int? AllocatedEmployeeId { get; set; }
        public string AllocatedEmployee { get; set; }
        public int? AllocatedByEmployeeId { get; set; }
        public string AllocatedByEmployee { get; set; }
        public DateTime? AllocatedOn { get; set; }

        public AssetModel()
        {

        }
        public AssetModel(Asset asset)
        {
            Id = asset.Id;

            if (asset.State != AssetState.Dead)
            {
                if (asset.AllocatedEmployee?.User.Person != null)
                {
                    AllocatedEmployee = asset.AllocatedEmployee.User.Person.Name;
                }
            }
            if (asset.Department != null)
            {
                Department = asset.Department.Title;
            }
            if (asset.AssetCategory != null)
            {
                AssetCategory = asset.AssetCategory.Title;
            }
            if (asset.Vendor != null)
            {
                Vendor = asset.Vendor.Title;
            }

            TitleAndModel = $"{asset.Title} {asset.ModelNumber}";
            TagNumber = asset.TagNumber;
            SerialNumber = asset.SerialNumber;
            StateType = GetEnumDescription(asset.State);
            Specifications = asset.Specifications;
            BrandAndModel = $"{asset.Brand} {asset.ModelNumber}";
            Title = asset.Title;
            Cost = asset.Cost;
            Description = asset.Description;
            Brand = asset.Brand;
            ModelNumber = asset.ModelNumber;
            IsBrandNew = asset.IsBrandNew;
            PurchaseDate = asset.PurchaseDate;
            WarrantyExpiryDate = asset.WarrantyExpiryDate;
        }
        public AssetModel(AssetAllocation asset)
        {
            Id = asset.Id;

            if (asset.AllocatedEmployee?.User.Person != null)
            {
                AllocatedEmployee = asset.AllocatedEmployee.User.Person.Name;
            }
            StateType = GetEnumDescription(asset.State);
            AllocatedOn = asset.AllocatedOn;
        }
    }
}
