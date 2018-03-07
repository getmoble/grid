using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;
using Grid.Features.IMS.Entities.Enums;

namespace Grid.Features.IMS.Entities
{
    public class Asset: EntityBase
    {
        [DisplayName("Serial Number")]
        public string SerialNumber { get; set; }
        
        [DisplayName("Tag Number")]
        public string TagNumber { get; set; }
        
        [Required]
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
        [ForeignKey("AssetCategoryId")]
        public virtual AssetCategory AssetCategory { get; set; }

        public int? DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public int? VendorId { get; set; }
        [ForeignKey("VendorId")]
        public virtual Vendor Vendor { get; set; }      
        public int? AllocatedEmployeeId { get; set; }
        [ForeignKey("AllocatedEmployeeId")]
        public Employee AllocatedEmployee { get; set; }
    }
}