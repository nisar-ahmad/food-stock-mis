
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Linq.Dynamic;

using PagedList;
//using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.DatabaseGeneratedOption;

namespace FMS.Models
{
    public enum BankAccountType { Current, Saving, Other }
    public enum CashTransactionType { Debit, Credit }

    public enum CommodityTransactionType { Dispatch, Receive }
    public enum CommodityTransactionStatus { Pending, Dispatched, Received }
    public enum CashTransactionStatus { Pending, Completed }
    public enum GodownType { PASSCO, StorageOffice }
    public enum PersonType { FoodOfficer, Contractor, Incharge }
    public enum PlaceType { BankAccount, Contractor, Dealer, Depot, Directorate, DistrictOffice, DivisionalOffice, FlourMill, Godown, StorageOffice }

    //public enum DamageType { Fire, Earthquake, Flood, Lost, Other }


    // POCO classes

    // Bank
    public class Bank
    {
        public int BankId { get; set; } // BankID (Primary key)
        public string BankName { get; set; } // BankName
        public string Description { get; set; } // Description

        // Reverse navigation
        public virtual ICollection<BankAccount> BankAccounts { get; set; } // BankAccount.FK_BankAccount_Bank

        public Bank()
        {
            BankAccounts = new List<BankAccount>();
        }
    }

    // BankAccount
    public class BankAccount
    {
        public int BankAccountId { get; set; } // BankAccountID (Primary key)
        public int BankId { get; set; } // BankID
        public int PlaceId { get; set; } // PlaceID
        
        [Required]
        public string Branch { get; set; } // Branch

        [Required]
        public string AccountNumber { get; set; } // AccountNumber

        public BankAccountType AccountType { get; set; } // AccountType
        public string Description { get; set; } // Description

        // Reverse navigation
        public virtual ICollection<CashTransaction> CashTransactions { get; set; } // CashTransaction.FK_CashTransaction_Bank
        public virtual ICollection<Depot> Depots { get; set; } // Depot.FK_Depot_BankAccount
        public virtual ICollection<Directorate> Directorates { get; set; } // Directorate.FK_Directorate_Bank

        // Foreign keys
        public virtual Bank Bank { get; set; } // FK_BankAccount_Bank
        public virtual Place Place { get; set; } // FK_BankAccount_Place

        public BankAccount()
        {
            CashTransactions = new List<CashTransaction>();
            Depots = new List<Depot>();
            Directorates = new List<Directorate>();
        }
    }

    // CashTransaction
    public class CashTransaction
    {
        public int CashTransactionId { get; set; } // CashTransactionID (Primary key)
        public string CompositeKey { get; set; } // CompositeKey
        public CashTransactionType Type { get; set; } // CashTransactionType
        public int BookNumber { get; set; }

        [Required]
        [DisplayName("Receipt No.")]
        public string CashVoucherNo { get; set; } // CashVoucherNo

        public int? FromPlaceId { get; set; } // FromPlaceID
        public int? ToPlaceId { get; set; } // ToPlaceID
        public int? CommodityTypeId { get; set; } // CommodityTypeID

        [DisplayFormat(DataFormatString="{0:dd MMM yyyy}")]
        public DateTime TransactionDate { get; set; } // TransactionDate

        [Range(1, 100000000)]
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public int Amount { get; set; } // Amount

        [Range(0, 100000000)]
        [DisplayFormat(DataFormatString="{0:0,0}")]
        public int? Bags { get; set; } // Bags

        [Range(0, 100000000)]
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public int? Quantity { get; set; } // Quantity
        public int? UnitId { get; set; } // UnitID

        [Range(0, 100000000)]
        public int? Rate { get; set; } // Rate
        public CashTransactionStatus Status { get; set; } // Status
        public int? CommodityTransactionId { get; set; } // CommodityTransactionID
        public int BankAccountId { get; set; } // BankAccountID

        [UIHint("MultilineText")]
        public string Remarks { get; set; } // Remarks

        // Foreign keys
        public virtual BankAccount BankAccount { get; set; } // FK_CashTransaction_Bank
        public virtual CommodityTransaction CommodityTransaction { get; set; } // FK_CashTransaction_CommodityTransaction
        public virtual CommodityType CommodityType { get; set; } // FK_CashTransaction_CommodityType

        [ForeignKey("FromPlaceId")]
        public virtual Place FromPlace { get; set; } // FK_CommodityTransaction_Place

        [ForeignKey("ToPlaceId")]
        public virtual Place ToPlace { get; set; } // FK_CommodityTransaction_Place

        public virtual Unit Unit { get; set; } // FK_CashTransaction_Units

        public CashTransaction()
        {
            TransactionDate = DateTime.Now;
        }
    }

    // CommodityTransaction
    public class CommodityTransaction
    {
        public int CommodityTransactionId { get; set; } // CommodityTransactionID (Primary key)
        public string CompositeKey { get; set; } // CompositeKey
        public CommodityTransactionType Type { get; set; } // Type
        public int? FromPlaceId { get; set; } // FromPlaceID
        public int? ToPlaceId { get; set; } // ToPlaceID
        public int? ContractorId { get; set; } // ContractorID

        [DisplayName("Book No.")]
        public int BookNumber { get; set; }

        [DisplayName("Voucher No.")]
        [Required]
        public string VoucherNo { get; set; } // VoucherNo

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DisplayName("Dispatch Date")]
        public DateTime DispatchDate { get; set; } // DispatchDate

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DisplayName("Receive Date")]
        [DateGreaterThanOrEqualTo("DispatchDate")]
        public DateTime? ReceiveDate { get; set; } // ReceiveDate

        public int CommodityTypeId { get; set; } // CommodityTypeID

        [Range(0, 100000000)]
        public int Trucks { get; set; } // Trucks

        [Range(1, 100000000)]
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public int Bags { get; set; } // Bags

        [Range(1, 100000000)]
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public int Quantity { get; set; } // Quantity
        public int UnitId { get; set; } // UnitID

        [Range(0, 100000000)]
        public int? Rate { get; set; } // Rate
        public CommodityTransactionStatus Status { get; set; } // Status

        [DisplayName("Bags Received")]
        [Range(0, 100000000)]
        public int BagsReceived { get; set; }

        [DisplayName("Quantity Received")]
        [Range(0, 100000000)]
        public int QuantityReceived { get; set; }

        [DisplayName("Shortage Type")]
        public int? ShortageTypeId { get; set; } // ShortageTypeId

        [DisplayName("Shortage Reasons")]
        [UIHint("MultilineText")]
        public string ReasonsForShortage { get; set; }

        [UIHint("MultilineText")]
        public string Remarks { get; set; } // Remarks

        [DisplayName("Issuing Officer")]
        public int? IssuingOfficerId { get; set; } // IssuingOfficerID

        [DisplayName("Receiving Officer")]
        public int? ReceivingOfficerId { get; set; } // ReceivingOfficerID

        // Reverse navigation
        public virtual ICollection<CashTransaction> CashTransactions { get; set; } // CashTransaction.FK_CashTransaction_CommodityTransaction

        // Foreign keys
        [DisplayName("Commodity")]
        public virtual CommodityType CommodityType { get; set; } // FK_CommodityTransaction_CommodityType
        public virtual Contractor Contractor { get; set; } // FK_CommodityTransaction_Contractor

        [ForeignKey("IssuingOfficerId")]
        public virtual Person IssuingOfficer { get; set; } // FK_CommodityTransaction_IssuingOfficer

        [ForeignKey("ReceivingOfficerId")]
        public virtual Person ReceivingOfficer { get; set; } // FK_CommodityTransaction_ReceivingOfficer

        [ForeignKey("ShortageTypeId")]
        public virtual ShortageType ShortageType { get; set; } // FK_CommodityTransaction_ShortageType

        [ForeignKey("FromPlaceId")]
        [DisplayName("From")]
        public virtual Place FromPlace { get; set; } // FK_CommodityTransaction_Place

        [ForeignKey("ToPlaceId")]
        [DisplayName("To")]
        public virtual Place ToPlace { get; set; } // FK_CommodityTransaction_Place

        public virtual Unit Unit { get; set; } // FK_CommodityTransaction_Units

        public CommodityTransaction()
        {
            CashTransactions = new List<CashTransaction>();
        }
    }

    // CommodityType
    public class CommodityType
    {
        public int CommodityTypeId { get; set; } // CommodityTypeID (Primary key)
        public string Name { get; set; } // Name
        public string Description { get; set; } // Description

        [DisplayName("Quantity in Kg")]
        public float KgConversionFactor { get; set; }

        [DisplayName("Rate Per Bag")]
        public int RatePerBag { get; set; }

        // Reverse navigation
        public virtual ICollection<CashTransaction> CashTransactions { get; set; } // CashTransaction.FK_CashTransaction_CommodityType
        public virtual ICollection<CommodityTransaction> CommodityTransactions { get; set; } // CommodityTransaction.FK_CommodityTransaction_CommodityType
        public virtual ICollection<DepotDamage> DepotDamages { get; set; } // Depot_Damage.FK_Depot_Damage_CommodityType

        public CommodityType()
        {
            CashTransactions = new List<CashTransaction>();
            CommodityTransactions = new List<CommodityTransaction>();
            DepotDamages = new List<DepotDamage>();
        }
    }

    // ContractorType
    public class ContractorType
    {
        public int ContractorTypeId { get; set; } //ContractorTypeID (Primary key)
        public string Name { get; set; } // Name
        public string Description { get; set; } // Description

        public virtual ICollection<Contractor> Contractors { get; set; }

        public ContractorType()
        {
            Contractors = new List<Contractor>();
        }
    }

    // ShortageType
    public class ShortageType
    {
        public int ShortageTypeId { get; set; } // ShortageTypeId (Primary key)
        public string Name { get; set; } // Name
        public string Description { get; set; } // Description

        // Reverse navigation
        public virtual ICollection<CommodityTransaction> CommodityTransactions { get; set; } // CommodityTransaction.FK_CommodityTransaction_ShortageType

        public ShortageType()
        {
            CommodityTransactions = new List<CommodityTransaction>();
        }
    }

    // DamageType
    public class DamageType
    {
        public int DamageTypeId { get; set; } // DamageTypeId (Primary key)
        public string Name { get; set; } // Name
        public string Description { get; set; } // Description

        // Reverse navigation
        public virtual ICollection<DepotDamage> DepotDamages { get; set; } // Depot_Damage.FK_Depot_Damage_DamageType

        public DamageType()
        {
            DepotDamages = new List<DepotDamage>();
        }
    }

    // Contractor
    public class Contractor
    {
        public int ContractorId { get; set; } // ContractorID (Primary key)
        public int PlaceId { get; set; } // PlaceID
        public int ContractorTypeId { get; set; } // Type

        // Reverse navigation
        public virtual ICollection<CommodityTransaction> CommodityTransactions { get; set; } // CommodityTransaction.FK_CommodityTransaction_Contractor
        public virtual ICollection<ContractorVehicle> ContractorVehicles { get; set; } // ContractorVehicle.FK_ContractorVehicle_Contractor

        // Foreign keys
        public virtual Place Place { get; set; } // FK_Contractor_Place
        public virtual ContractorType ContractorType { get; set; } // FK_Contractor_ContractorType

        public Contractor()
        {
            CommodityTransactions = new List<CommodityTransaction>();
            ContractorVehicles = new List<ContractorVehicle>();
        }
    }

    // ContractorVehicle
    public class ContractorVehicle
    {
        public int ContractorVehicleId { get; set; } // ContractorVehicleID (Primary key)
        public string RegistrationNumber { get; set; } // RegistrationNumber
        public int VehicleType { get; set; } // VehicleType
        public string Description { get; set; } // Description
        public int ContractorId { get; set; } // ContractorID

        // Foreign keys
        public virtual Contractor Contractor { get; set; } // FK_ContractorVehicle_Contractor
    }

    // Dealer
    public class Dealer
    {
        public int DealerId { get; set; } // DealerID (Primary key)
        public int PlaceId { get; set; } // PlaceID
        public int DepotId { get; set; } // DepotId

        // Foreign keys
        public virtual Place Place { get; set; } // FK_Dealer_Place
        public virtual Depot Depot { get; set; } // FK_Dealer_Depot

        public Dealer()
        {
        }
    }

    // Depot
    public class Depot
    {
        public int DepotId { get; set; } // DepotID (Primary key)
        public int PlaceId { get; set; } // PlaceID
        public int DistrictOfficeId { get; set; } // DistrictOfficeID
        public int BankAccountId { get; set; } // BankAccountID

        // Reverse navigation
        public virtual ICollection<DepotAllocation> DepotAllocations { get; set; } // DepotAllocation.FK_DepotAllocation_Depot
        public virtual ICollection<DepotDamage> DepotDamages { get; set; } // Depot_Damage.FK_Depot_Damage_Depot

        // Foreign keys
        public virtual BankAccount BankAccount { get; set; } // FK_Depot_BankAccount
        public virtual DistrictOffice DistrictOffice { get; set; } // FK_Depot_District
        public virtual Place Place { get; set; } // FK_Depot_Place

        public Depot()
        {
            DepotDamages = new List<DepotDamage>();
            DepotAllocations = new List<DepotAllocation>();
        }
    }

    // DepotAllocation
    public class DepotAllocation
    {
        public int DepotAllocationId { get; set; } // DepotAllocationID (Primary key)
        public int DepotId { get; set; } // DepotID
        public int FlourMillId { get; set; } // FlourMillID
        public int CommodityTypeId { get; set; } // CommodityTypeID
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public int Quantity { get; set; } // Quantity
        public int UnitId { get; set; } // UnitID

        [DisplayFormat(DataFormatString = "{0:MMM-yyyy}")]
        public DateTime? Month { get; set; } // Month

        // Foreign keys
        public virtual Depot Depot { get; set; } // FK_DepotAllocation_Depot
        public virtual FlourMill FlourMill { get; set; } // FK_DepotAllocation_FlourMill
        public virtual Unit Unit { get; set; } // FK_DepotAllocation_Unit
        public virtual CommodityType CommodityType { get; set; } // FK_DepotAllocation_Unit

    }

    // Depot_Damage
    public class DepotDamage
    {
        public DepotDamage()
        {
            DamageDate = DateTime.Now;
        }

        public int DepotDamageId { get; set; } // DepotDamageID (Primary key)
        public int DepotId { get; set; } // DepotID
        public int DamageTypeId { get; set; } // DamageTypeId

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime DamageDate { get; set; } // DamageDate
        public int CommodityTypeId { get; set; } // CommodityTypeID

        [Range(1, 100000000)]
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public int Bags { get; set; } // Bags

        [Range(1, 100000000)]
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        public int Quantity { get; set; } // Quantity

        public int UnitId { get; set; } // UnitID

        // Foreign keys
        public virtual CommodityType CommodityType { get; set; } // FK_Depot_Damage_CommodityType
        public virtual DamageType DamageType { get; set; } // FK_Depot_Damage_DamageType
        public virtual Depot Depot { get; set; } // FK_Depot_Damage_Depot
        public virtual Unit Unit { get; set; } // FK_Depot_Damage_Units
    }

    // Directorate
    public class Directorate
    {
        public int DirectorateId { get; set; } // DirectorateID (Primary key)
        public int PlaceId { get; set; } // PlaceID
        public int BankAccountId { get; set; } // BankAccountID

        // Reverse navigation
        public virtual ICollection<StorageOffice> StorageOffices { get; set; } // StorageOffice.FK_StorageOffice_Directorate
        public virtual ICollection<WheatRequest> WheatRequests { get; set; } // WheatRequest.FK_WheatRequest_Directorate

        // Foreign keys
        public virtual Place Place { get; set; } // FK_Directorate_Bank
        public virtual BankAccount BankAccount { get; set; } // FK_Directorate_Bank

        public Directorate()
        {
            StorageOffices = new List<StorageOffice>();
            WheatRequests = new List<WheatRequest>();
        }
    }

    // DistrictOffice
    public class DistrictOffice
    {
        public int DistrictOfficeId { get; set; } // DistrictOfficeID (Primary key)
        public int PlaceId { get; set; } // PlaceID
        public int DivisionalOfficeId { get; set; } // DivisionalOfficeID
        public int Population { get; set; } // Population

        // Reverse navigation
        public virtual ICollection<Depot> Depots { get; set; } // Depot.FK_Depot_District
        public virtual ICollection<FlourMill> FlourMills { get; set; } // FlourMill.FK_FlourMill_District

        // Foreign keys
        [ForeignKey("PlaceId")]
        public virtual Place Place { get; set; } // FK_District_Place
        public virtual DivisionalOffice DivisionalOffice { get; set; } // FK_District_Division

        public DistrictOffice()
        {
            Depots = new List<Depot>();
            FlourMills = new List<FlourMill>();
        }
    }

    // DivisionalOffice
    public class DivisionalOffice
    {
        public int DivisionalOfficeId { get; set; } // DivisionalOfficeID (Primary key)
        public int PlaceId { get; set; } // PlaceID

        // Reverse navigation
        public virtual ICollection<DistrictOffice> DistrictOffices { get; set; } // DistrictOffice.FK_District_DIvision

        // Foreign keys
        public virtual Place Place { get; set; } // FK_DivisionalOffice_Place

        public DivisionalOffice()
        {
            DistrictOffices = new List<DistrictOffice>();
        }
    }

    // FlourMill
    public class FlourMill
    {
        public int FlourMillId { get; set; } // FlourMillID (Primary key)
        public int DistrictOfficeId { get; set; } // DistrictOfficeID
        public int PlaceId { get; set; } // PlaceID

        // Reverse navigation
        public virtual ICollection<DepotAllocation> DepotAllocations { get; set; } // DepotAllocation.FK_DepotAllocation_FlourMill
        public virtual ICollection<FlourMillChokar> FlourMillChokars { get; set; } // FlourMill_Chokar.FK_FlourMill_Chokar_FlourMill

        // Foreign keys
        public virtual DistrictOffice DistrictOffice { get; set; } // FK_FlourMill_District
        public virtual Place Place { get; set; } // FK_FlourMill_Place

        public FlourMill()
        {
            DepotAllocations = new List<DepotAllocation>();
            FlourMillChokars = new List<FlourMillChokar>();
        }
    }

    // FlourMill_Chokar
    public class FlourMillChokar
    {
        public int FlourMillChokarId { get; set; } // FlourMill_ChokarID (Primary key)
        public int FlourMillId { get; set; } // FlourMillID

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DisplayName("Crushing Date")]
        public DateTime ProductionDate { get; set; } // ProductionDate

        [Range(1, 100000000)]
        [DisplayName("Wheat Bags")]
        public int Bags { get; set; }

        [Range(1, 100000000)]
        [DisplayName("Quantity of Chokar")]
        public int Quantity { get; set; } // Quantity
        public int UnitId { get; set; } // UnitID

        // Foreign keys
        public virtual FlourMill FlourMill { get; set; } // FK_FlourMill_Chokar_FlourMill
        public virtual Unit Unit { get; set; } // FK_FlourMillChokar_Unit
    }

    // Godown
    public class Godown
    {
        public int GodownId { get; set; } // GodownID (Primary key)
        public int PlaceId { get; set; } // PlaceID
        public int Capacity { get; set; } // Capacity
        public int UnitId { get; set; } // UnitID
        public GodownType Type { get; set; } // Type
        public int? StorageOfficeId { get; set; } // StorageOfficeID

        // Foreign keys
        public virtual Place Place { get; set; } // FK_Godown_Place
        public virtual Unit Unit { get; set; } // FK_Godown_Unit
        public virtual StorageOffice StorageOffice { get; set; } // FK_Godown_StorageOffice

        public Godown()
        {
        }
    }

    // Person
    public class Person
    {
        public int PersonId { get; set; } // PersonID (Primary key)

        [Required]
        public string Name { get; set; } // Name
        public string CNIC { get; set; } // CNIC
        public string Mobile { get; set; } // Mobile
        public string Email { get; set; } // Email
        public PersonType Type { get; set; } // Type

        // Reverse navigation
        public virtual ICollection<CommodityTransaction> CommodityTransactions { get; set; } // CommodityTransaction.FK_CommodityTransaction_Person
        public virtual ICollection<Place> Places { get; set; } // Place.FK_Place_Person

        public Person()
        {
            CommodityTransactions = new List<CommodityTransaction>();
            Places = new List<Place>();
        }
    }

    // Place
    public class Place
    {
        public int PlaceId { get; set; } // PlaceID (Primary key)

        [Required]
        public string Name { get; set; } // Name

        public PlaceType Type { get; set; } // Type
        public string Description { get; set; } // Description
        public string Landline { get; set; } // Landline
        public string Fax { get; set; } // Fax
        public int ProvinceId { get; set; } // ProvinceID
        public int DistrictId { get; set; } // DistrictID
        public int TehsilId { get; set; } // TehsilID
        public int? UnionCouncilId { get; set; } // UnionCouncilID
        public string Address { get; set; } // Address
        public DbGeography Geography { get; set; } // Geography

        public int InchargePersonId { get; set; } // InchargePersonID

        // Reverse navigation
        public virtual ICollection<CashTransaction> CashTransactions { get; set; } // CashTransaction.FK_CashTransaction_Place
        public virtual ICollection<CommodityTransaction> CommodityTransactions { get; set; } // CommodityTransaction.FK_CommodityTransaction_Place

        public virtual ICollection<Contractor> Contractors { get; set; } // Contractor.FK_Contractor_Place
        public virtual ICollection<Dealer> Dealers { get; set; } // Dealer.FK_Dealer_Place
        public virtual ICollection<Depot> Depots { get; set; } // Depot.FK_Depot_Place
        public virtual ICollection<DistrictOffice> DistrictOffices { get; set; } // DivisionalOffice.FK_DivisionalOffice_Place
        public virtual ICollection<DivisionalOffice> DivisionalOffices { get; set; } // DivisionalOffice.FK_DivisionalOffice_Place
        public virtual ICollection<FlourMill> FlourMills { get; set; } // FlourMill.FK_FlourMill_Place
        public virtual ICollection<Godown> Godowns { get; set; } // Godown.FK_Godown_Place
        public virtual ICollection<StorageOffice> StorageOffices { get; set; } // StorageOffice.FK_StorageOffice_Place
        public virtual ICollection<Directorate> Directorates { get; set; } // Godown.FK_Directorate_Place
        public virtual ICollection<BankAccount> BankAccounts { get; set; } // Godown.FK_Directorate_Place

        // Foreign keys
        public virtual District District { get; set; } // FK_Place_District

        [ForeignKey("InchargePersonId")]
        public virtual Person InchargePerson { get; set; } // FK_Place_Person
        public virtual Province Province { get; set; } // FK_Place_Province
        public virtual Tehsil Tehsil { get; set; } // FK_Place_Tehsil
        public virtual UnionCouncil UnionCouncil { get; set; } // FK_Place_UnionCouncil

        public Place()
        {
            CashTransactions = new List<CashTransaction>();
            CommodityTransactions = new List<CommodityTransaction>();

            Contractors = new List<Contractor>();
            Dealers = new List<Dealer>();
            Depots = new List<Depot>();
            DistrictOffices = new List<DistrictOffice>();
            DivisionalOffices = new List<DivisionalOffice>();
            FlourMills = new List<FlourMill>();
            Godowns = new List<Godown>();
            StorageOffices = new List<StorageOffice>();
            Directorates = new List<Directorate>();
            BankAccounts = new List<BankAccount>();
        }
    }

    // StorageOffice
    public class StorageOffice
    {
        public int StorageOfficeId { get; set; } // StorageOfficeID (Primary key)
        public int PlaceId { get; set; } // PlaceID
        public int DirectorateId { get; set; } // DirectorateID

        // Reverse navigation
        public virtual ICollection<Godown> Godowns { get; set; } // Godown.FK_Godown_StorageOffice

        // Foreign keys
        public virtual Directorate Directorate { get; set; } // FK_StorageOffice_Directorate
        public virtual Place Place { get; set; } // FK_StorageOffice_Place

        public StorageOffice()
        {
            Godowns = new List<Godown>();
        }
    }

    // Province
    public class Province
    {
        public int ProvinceId { get; set; } // ProvinceID (Primary key)
        public string Name { get; set; } // Name
        public string Description { get; set; } // Description

        // Reverse navigation
        public virtual ICollection<District> Districts { get; set; } // District.FK_District_Province
        public virtual ICollection<Place> Places { get; set; } // Place.FK_Place_Province

        public Province()
        {
            Districts = new List<District>();
            Places = new List<Place>();
        }
    }

    // District
    public class District
    {
        public int DistrictId { get; set; } // DistrictID (Primary key)
        public string Name { get; set; } // Name
        public string Description { get; set; } // Description
        public int ProvinceId { get; set; } // ProvinceID

        // Reverse navigation
        public virtual ICollection<Place> Places { get; set; } // Place.FK_Place_District
        public virtual ICollection<Tehsil> Tehsils { get; set; } // Tehsil.FK_Tehsil_District

        // Foreign keys
        public virtual Province Province { get; set; } // FK_District_Province

        public District()
        {
            Places = new List<Place>();
            Tehsils = new List<Tehsil>();
        }
    }

    // Tehsil
    public class Tehsil
    {
        public int TehsilId { get; set; } // TehsilID (Primary key)
        public string Name { get; set; } // Name
        public string Description { get; set; } // Description
        public int DistrictId { get; set; } // DistrictID

        // Reverse navigation
        public virtual ICollection<Place> Places { get; set; } // Place.FK_Place_Tehsil
        public virtual ICollection<UnionCouncil> UnionCouncils { get; set; } // UnionCouncil.FK_UnionCouncil_Tehsil

        // Foreign keys
        public virtual District District { get; set; } // FK_Tehsil_District

        public Tehsil()
        {
            Places = new List<Place>();
            UnionCouncils = new List<UnionCouncil>();
        }
    }

    // UnionCouncil
    public class UnionCouncil
    {
        public int UnionCouncilId { get; set; } // UnionCouncilID (Primary key)
        public string Name { get; set; } // Name
        public string Description { get; set; } // Description
        public int TehsilId { get; set; } // TehsilID

        // Reverse navigation
        public virtual ICollection<Place> Places { get; set; } // Place.FK_Place_UnionCouncil

        // Foreign keys
        public virtual Tehsil Tehsil { get; set; } // FK_UnionCouncil_Tehsil

        public UnionCouncil()
        {
            Places = new List<Place>();
        }
    }

    // Units
    public class Unit
    {
        public int UnitId { get; set; } // UnitID (Primary key)
        public string Name { get; set; } // Name
        public string Description { get; set; } // Description
        public double ConversionFactor { get; set; } // ConversionFactor

        // Reverse navigation
        public virtual ICollection<CommodityTransaction> CommodityTransactions { get; set; } // CommodityTransaction.FK_CommodityTransaction_Units
        public virtual ICollection<CashTransaction> CashTransactions { get; set; } // Depot_Damage.FK_Depot_Damage_Units
        public virtual ICollection<DepotDamage> DepotDamages { get; set; } // Depot_Damage.FK_Depot_Damage_Units
        public virtual ICollection<DepotAllocation> DepotAllocations { get; set; } // Depot_Damage.FK_Depot_Damage_Units
        public virtual ICollection<WheatRequest> WheatRequests { get; set; } // Depot_Damage.FK_Depot_Damage_Units
        public virtual ICollection<Godown> Godowns { get; set; } // Depot_Damage.FK_Depot_Damage_Units
        public virtual ICollection<FlourMillChokar> FlourMillChokars { get; set; } // Depot_Damage.FK_Depot_Damage_Units

        public Unit()
        {
            CommodityTransactions = new List<CommodityTransaction>();
            CashTransactions = new List<CashTransaction>();
            DepotDamages = new List<DepotDamage>();
            DepotAllocations = new List<DepotAllocation>();
            WheatRequests = new List<WheatRequest>();
            Godowns = new List<Godown>();
            FlourMillChokars = new List<FlourMillChokar>();
        }
    }

    // WheatRequest
    public class WheatRequest
    {
        public int WheatRequestId { get; set; } // WheatRequestID (Primary key)
        public int DirectorateId { get; set; } // DirectorateID

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime RequestDate { get; set; }
        public int Quantity { get; set; } // Quantity
        public int UnitId { get; set; } // UnitID
        public string ReferenceNo { get; set; } // ReferenceNo
        public string Description { get; set; } // Description

        // Foreign keys
        public virtual Directorate Directorate { get; set; } // FK_WheatRequest_Directorate
        public virtual Unit Unit { get; set; } // FK_WheatRequest_Unit
    }

    public class SortableList<T>
    {
        public const int PageSize = 20;

        public int Page { get; set; }
        public string SortBy { get; set; }
        public bool SortAscending { get; set; }

        public IQueryable<T> Query { get; set; }

        public IPagedList<T> PagedList 
        { 
            get
            {
                var query = Query.OrderBy("DispatchDate");

                if(!string.IsNullOrWhiteSpace(SortBy))
                    query = query.OrderBy( SortBy + (SortAscending ? "" : " desc"));
                
                return query.ToPagedList(Page, PageSize);
            }
        }

        public SortableList(IQueryable<T> query, int page = 1, string sortBy = null, bool sortAscending = true)
        {
            Query = query;
            Page = page;
            SortBy = sortBy;
            SortAscending = sortAscending;
        }
    }

    public class QuantityItem
    {
        public string Name { get; set; }

        public int Bags { get; set; }
        public int Quantity { get; set; }
    }

    public class SOGodownViewModel
    {
        public int PlaceId { get; set; }

        public int OpeningBags { get; set; }
        public int OpeningQuantity { get; set; }

        public List<QuantityItem> ReceivedItems { get; set; }
        public List<QuantityItem> DispatchedItems { get; set; }

        public int BagsReceived
        {
            get { return ReceivedItems.Sum(o => o.Bags); }
        }

        public int QuantityReceived
        {
            get { return ReceivedItems.Sum(o => o.Quantity);}
        }

        public int BagsDispatched
        {
            get { return DispatchedItems.Sum(o => o.Bags); }
        }

        public int QuantityDispatched
        {
            get { return DispatchedItems.Sum(o => o.Quantity); }
        }

        public SOGodownViewModel()
        {
            ReceivedItems = new List<QuantityItem>();
            DispatchedItems = new List<QuantityItem>();
        }
    }

    public class VoucherItem
    {
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime Date { get; set; }
        public string VoucherNo { get; set; }
        public string PlaceName { get; set; }

        public CommodityItem Item { get; set; }

        public VoucherItem(){}

        public VoucherItem(DateTime date, string voucherNo, string placeName, string commodity, int bags, int quantity)
        {
            Date = date;
            VoucherNo = voucherNo;
            PlaceName = placeName;
            Item = new CommodityItem(commodity, bags, quantity);
        }
    }

    public class CommodityItem
    {
        public string Commodity { get; set; }

        public int Bags { get; set; }
        public int Quantity { get; set; }

        public CommodityItem() { }

        public CommodityItem(string commodity, int bags, int quantity)
        {
            Commodity = commodity;
            Bags = bags;
            Quantity = quantity;
        }
    }
    
    public class CommoditySummary
    {
        public string PlaceName { get; set; }
        public List<CommodityItem> Items { get; set; }

        public CommoditySummary()
        {
            Items = new List<CommodityItem>();
        }
    }

    public class VoucherDetail
    {
        public List<VoucherItem> ReceivedItems { get; set; }
        public List<VoucherItem> DispatchedItems { get; set; }

        public Dictionary<string, CommodityItem> TotalReceived
        {
            get 
            {
                var items = ReceivedItems.GroupBy(o => o.Item.Commodity).Select(c => new CommodityItem 
                { Commodity = c.Key, Bags = c.Sum(o => o.Item.Bags), Quantity = c.Sum(o => o.Item.Quantity) });

                var dictionary = new Dictionary<string, CommodityItem>();

                foreach (var i in items)
                    dictionary.Add(i.Commodity, i);

                return dictionary;
            }        
        }

        public Dictionary<string, CommodityItem> TotalDispatched
        {
            get
            {
                var items = DispatchedItems.GroupBy(o => o.Item.Commodity).Select(c => new CommodityItem 
                { Commodity = c.Key, Bags = c.Sum(o => o.Item.Bags), Quantity = c.Sum(o => o.Item.Quantity) });

                var dictionary = new Dictionary<string, CommodityItem>();

                foreach (var i in items)
                    dictionary.Add(i.Commodity, i);

                return dictionary;
            }
        }

        public int GrandTotalBagsReceived
        {
            get
            {
                return TotalReceived.Sum(o => o.Value.Bags);
            }
        }

        public int GrandTotalQuantityReceived
        {
            get
            {
                return TotalReceived.Sum(o => o.Value.Quantity);
            }
        }

        public int GrandTotalBagsDispatched
        {
            get
            {
                return TotalDispatched.Sum(o => o.Value.Bags);
            }
        }

        public int GrandTotalQuantityDispatched
        {
            get
            {
                return TotalDispatched.Sum(o => o.Value.Quantity);
            }
        }

        public VoucherDetail()
        {
            ReceivedItems = new List<VoucherItem>();
            DispatchedItems = new List<VoucherItem>();
        }
    }

    public class CommodityDetail
    {
        public List<CommoditySummary> ReceivedItems { get; set; }
        public List<CommoditySummary> DispatchedItems { get; set; }

        public Dictionary<string, CommodityItem> TotalReceived
        {
            get 
            {
                return GetTotals(ReceivedItems);
            }        
        }

        public Dictionary<string, CommodityItem> TotalDispatched
        {
            get
            {
                return GetTotals(DispatchedItems);
            }
        }

        public int GrandTotalBagsReceived
        {
            get
            {
                return TotalReceived.Sum(o => o.Value.Bags);
            }
        }

        public int GrandTotalQuantityReceived
        {
            get
            {
                return TotalReceived.Values.Sum(o => o.Quantity);
            }
        }

        public int GrandTotalBagsDispatched
        {
            get
            {
                return TotalDispatched.Sum(o => o.Value.Bags);
            }
        }

        public int GrandTotalQuantityDispatched
        {
            get
            {
                return TotalDispatched.Sum(o => o.Value.Quantity);
            }
        }

        private Dictionary<string, CommodityItem> GetTotals(IEnumerable<CommoditySummary> items)
        {
            var total = new Dictionary<string, CommodityItem>();

            foreach (var item in items)
            {
                foreach (var i in item.Items)
                {
                    if (!total.ContainsKey(i.Commodity))
                    {
                        total.Add(i.Commodity, new CommodityItem());
                        total[i.Commodity].Commodity = i.Commodity;
                    }

                    total[i.Commodity].Bags += i.Bags;
                    total[i.Commodity].Quantity += i.Quantity;
                }
            }

            return total;
        }

        public CommodityDetail()
        {
            ReceivedItems = new List<CommoditySummary>();
            DispatchedItems = new List<CommoditySummary>();
        }
    }

    public class SummaryItem
    {
        public string Commodity { get; set; }
        
        [DisplayFormat(DataFormatString="{0:#,0}")]
        public int OpeningBags { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public int OpeningQuantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public int ReceivedBags { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public int ReceivedQuantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public int TotalBags { get { return OpeningBags + ReceivedBags; } }

        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public int TotalQuantity { get { return OpeningQuantity + ReceivedQuantity; } }

        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public int DispatchedBags { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public int DispatchedQuantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public int BookBalanceBags { get { return TotalBags - DispatchedBags; } }

        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public int BookBalanceQuantity { get { return TotalQuantity - DispatchedQuantity; } }

        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public int ShortageBags { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public int ShortageQuantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public int GroundBalanceBags { get { return BookBalanceBags - ShortageBags; } }

        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public int GroundBalanceQuantity { get { return BookBalanceQuantity - ShortageQuantity; } }
    }

    public class PlaceSummary
    {
        public Dictionary<string, SummaryItem> CommoditySummary { get; set; }

        public PlaceSummary()
        {
            CommoditySummary = new Dictionary<string, SummaryItem>();
        }
    }

    public class DailyStockReport
    {
        public Dictionary<string, PlaceSummary> StockByPASSCOCenter { get; set; }
        public Dictionary<string, PlaceSummary> StockByStorageOffice { get; set; }
        public Dictionary<string, PlaceSummary> StockByFlourMill { get; set; }
        public Dictionary<string, PlaceSummary> StockByDepot { get; set; }

        public DailyStockReport()
        {
            StockByPASSCOCenter = new Dictionary<string, PlaceSummary>();
            StockByStorageOffice = new Dictionary<string, PlaceSummary>();
            StockByFlourMill = new Dictionary<string, PlaceSummary>();
            StockByDepot = new Dictionary<string, PlaceSummary>();
        }
    }

    public class DistrictByDepot
    {
        public Dictionary<string, SummaryItem> DistrictSummary
        {
            get
            {
                var summary = new Dictionary<string, SummaryItem>();

                foreach(var depot in DepotSummary.Values)
                {
                    foreach(var c in depot.CommoditySummary.Values)
                    {
                        if (!summary.ContainsKey(c.Commodity))
                            summary[c.Commodity] = new SummaryItem() { Commodity = c.Commodity };

                        var item = summary[c.Commodity];

                        item.OpeningBags += c.OpeningBags;
                        item.OpeningQuantity += c.OpeningQuantity;
                        
                        item.ReceivedBags += c.ReceivedBags;
                        item.ReceivedQuantity += c.ReceivedQuantity;

                        item.DispatchedBags += c.DispatchedBags;
                        item.DispatchedQuantity += c.DispatchedQuantity;

                        item.ShortageBags += c.ShortageBags;
                        item.ShortageQuantity += c.ShortageQuantity;
                    }
                }

                return summary;
            }
        }

        public Dictionary<string, PlaceSummary> DepotSummary { get; set; }

        public DistrictByDepot()
        {
            DepotSummary = new Dictionary<string, PlaceSummary>();
        }
    }

    public class CommoditySold
    {
        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public int Bags { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public int Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public string RatePerBag { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public int AmountReleased { get; set; }
    }

    public class PlaceCash
    {
        public Dictionary<string, CommoditySold> CommoditiesSold {get; set;}

        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public int TotalAmount { get { return CommoditiesSold.Values.Sum(o => o.AmountReleased); } }

        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public int AmountDeposited { get; set; }

        public string Bank { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public int? Less { get { return (AmountDeposited < TotalAmount ? TotalAmount - AmountDeposited : (int?)null); } }

        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public int? Excess { get { return (AmountDeposited > TotalAmount ? AmountDeposited - TotalAmount : (int?)null); } }

        public PlaceCash()
        {
            CommoditiesSold = new Dictionary<string, CommoditySold>();
        }
    }

    public class CashReport
    {
        public Dictionary<string, PlaceCash> CashDetail { get; set; }

        public PlaceCash CashSummary
        {
            get
            {
                var summary = new PlaceCash();

                foreach (var depot in CashDetail.Values)
                {
                    foreach (var key in depot.CommoditiesSold.Keys)
                    {
                        var c = depot.CommoditiesSold[key];

                        if (!summary.CommoditiesSold.ContainsKey(key))
                            summary.CommoditiesSold[key] = new CommoditySold { RatePerBag = c.RatePerBag };

                        var item = summary.CommoditiesSold[key];

                        item.Bags += c.Bags;
                        item.Quantity += c.Quantity;
                        item.AmountReleased += c.AmountReleased;
                    }

                    summary.AmountDeposited += depot.AmountDeposited;
                }

                return summary;
            }
        }

        public CashReport()
        {
            CashDetail = new Dictionary<string, PlaceCash>();
        }
    }

    public class DepotByVoucher
    {
        public Dictionary<string, SummaryItem> SummaryItems { get; set; }
        public VoucherDetail VoucherDetail { get; set; }

        public DepotByVoucher()
        {
            SummaryItems = new Dictionary<string, SummaryItem>();
            VoucherDetail = new VoucherDetail();
        }
    }

    public class DepotByDealer
    {
        public Dictionary<string, SummaryItem> SummaryItems { get; set; }
        public CommodityDetail CommodityDetail { get; set; }

        public DepotByDealer()
        {
            SummaryItems = new Dictionary<string, SummaryItem>();
            CommodityDetail = new CommodityDetail();
        }
    }

    public class FlourMillSummary
    {
        public int OpeningBags { get; set; }
        public int OpeningQuantity { get; set; }

        public int ReceivedBags { get; set; }
        public int ReceivedQuantity { get; set; }

        public int TotalBags { get { return OpeningBags + ReceivedBags; } }
        public int TotalQuantity { get { return OpeningQuantity + ReceivedQuantity; } }

        public List<CommodityItem> DispatchedCommodities { get; set; }

        public int DispatchedBags { get { return DispatchedCommodities.Sum(o => o.Bags); } }
        public int DispatchedQuantity { get { return DispatchedCommodities.Sum(o => o.Quantity); } }

        public int CrushedBags { get; set; }
        public int ChokarQuantity { get; set; }

        public int BalanceBags { get { return TotalBags - CrushedBags; } }
        public int BalanceQuantity { get { return TotalQuantity - (DispatchedQuantity + ChokarQuantity); } }
        public int BalanceChokar { get; set; }

        public FlourMillSummary ()
	    {
            DispatchedCommodities = new List<CommodityItem>();
	    }
    }

    public class FlourMillByVoucher
    {
        public FlourMillSummary Summary { get; set; }
        public VoucherDetail VoucherDetail { get; set; }

        public FlourMillByVoucher()
        {
            Summary = new FlourMillSummary();
            VoucherDetail = new VoucherDetail();
        }
    }

    public class FlourMillByDepot
    {
        public FlourMillSummary Summary { get; set; }
        public CommodityDetail CommodityDetail { get; set; }

        public FlourMillByDepot()
        {
            Summary = new FlourMillSummary();
            CommodityDetail = new CommodityDetail();
        }
    }

    public class BaseFilter
    {
        public int? Page
        {
            get
            {
                if (_page == null)
                    _page = 1;

                return _page;
            }
            set
            {
                _page = value;
            }
        }
        private int? _page = 1;
    }

    // Commodity Transaction Filter
    public class CommodityTransactionFilter : BaseFilter
    {
        public CommodityTransactionType? Type { get; set; }
        public int? CommodityTypeId { get; set; }
        public int? FromPlaceId { get; set; }
        public int? ToPlaceId { get; set; }
        public int? ContractorId { get; set; }
        public DateTime? DispatchDateStart { get; set; }
        public DateTime? DispatchDateEnd { get; set; }
        public DateTime? ReceiveDateStart { get; set; }
        public DateTime? ReceiveDateEnd { get; set; }
    }

}

