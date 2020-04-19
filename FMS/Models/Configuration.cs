using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace FMS.Models
{
    // ************************************************************************
    // POCO Configuration

    // Bank
    internal class BankConfiguration : EntityTypeConfiguration<Bank>
    {
        public BankConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Bank");
            HasKey(x => x.BankId);

            Property(x => x.BankId).HasColumnName("BankID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.BankName).HasColumnName("BankName").IsRequired().HasMaxLength(50);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(50);
        }
    }

    // BankAccount
    internal class BankAccountConfiguration : EntityTypeConfiguration<BankAccount>
    {
        public BankAccountConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".BankAccount");
            HasKey(x => x.BankAccountId);

            Property(x => x.BankAccountId).HasColumnName("BankAccountID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.BankId).HasColumnName("BankID").IsRequired();
            Property(x => x.PlaceId).HasColumnName("PlaceID").IsRequired();
            Property(x => x.Branch).HasColumnName("Branch").IsRequired().HasMaxLength(50);
            Property(x => x.AccountNumber).HasColumnName("AccountNumber").IsRequired().HasMaxLength(50);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(null);

            // Foreign keys
            HasRequired(a => a.Bank).WithMany(b => b.BankAccounts).HasForeignKey(c => c.BankId).WillCascadeOnDelete(false); // FK_BankAccount_Bank
            HasRequired(a => a.Place).WithMany(b => b.BankAccounts).HasForeignKey(c => c.PlaceId).WillCascadeOnDelete(false); // FK_BankAccount_Place
        }
    }

    // CashTransaction
    internal class CashTransactionConfiguration : EntityTypeConfiguration<CashTransaction>
    {
        public CashTransactionConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".CashTransaction");
            HasKey(x => x.CashTransactionId);

            Property(x => x.CashTransactionId).HasColumnName("CashTransactionID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.CompositeKey).HasColumnName("CompositeKey").HasMaxLength(50);
            Property(x => x.Type).HasColumnName("Type").IsRequired();
            Property(x => x.BookNumber).HasColumnName("BookNumber").IsRequired();
            Property(x => x.CashVoucherNo).HasColumnName("CashVoucherNo").IsRequired().HasMaxLength(50);
            //Property(x => x.FromPlaceId).HasColumnName("FromPlaceID").IsRequired();
            //Property(x => x.ToPlaceId).HasColumnName("ToPlaceID").IsRequired();
            Property(x => x.CommodityTypeId).HasColumnName("CommodityTypeID");
            Property(x => x.TransactionDate).HasColumnName("TransactionDate");
            Property(x => x.Amount).HasColumnName("Amount").IsRequired();
            Property(x => x.Bags).HasColumnName("Bags");
            Property(x => x.Quantity).HasColumnName("Quantity");
            Property(x => x.UnitId).HasColumnName("UnitID");
            Property(x => x.Rate).HasColumnName("Rate");
            Property(x => x.Status).HasColumnName("Status");
            Property(x => x.CommodityTransactionId).HasColumnName("CommodityTransactionID");
            Property(x => x.BankAccountId).HasColumnName("BankAccountID").IsRequired();
            Property(x => x.Remarks).HasColumnName("Remarks").IsOptional().HasMaxLength(null);

            // Foreign keys
            //HasRequired(a => a.FromPlace).WithMany(b => b.CashTransactions).HasForeignKey(c => c.FromPlaceId).WillCascadeOnDelete(false); // FK_CashTransaction_Place
            //HasRequired(a => a.ToPlace).WithMany(b => b.CashTransactions).HasForeignKey(c => c.ToPlaceId).WillCascadeOnDelete(false); // FK_CashTransaction_Place
            HasOptional(a => a.CommodityType).WithMany(b => b.CashTransactions).HasForeignKey(c => c.CommodityTypeId).WillCascadeOnDelete(false); // FK_CashTransaction_CommodityType
            HasOptional(a => a.CommodityTransaction).WithMany(b => b.CashTransactions).HasForeignKey(c => c.CommodityTransactionId).WillCascadeOnDelete(false); // FK_CashTransaction_CommodityTransaction
            HasRequired(a => a.BankAccount).WithMany(b => b.CashTransactions).HasForeignKey(c => c.BankAccountId).WillCascadeOnDelete(false); // FK_CashTransaction_Bank
            HasOptional(a => a.Unit).WithMany(b => b.CashTransactions).HasForeignKey(c => c.UnitId).WillCascadeOnDelete(false); // FK_CommodityTransaction_Units
        }
    }

    // CommodityTransaction
    internal class CommodityTransactionConfiguration : EntityTypeConfiguration<CommodityTransaction>
    {
        public CommodityTransactionConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".CommodityTransaction");
            HasKey(x => x.CommodityTransactionId);

            Property(x => x.CommodityTransactionId).HasColumnName("CommodityTransactionID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.CompositeKey).HasColumnName("CompositeKey").IsOptional().HasMaxLength(50);
            Property(x => x.Type).HasColumnName("Type").IsRequired();
            //Property(x => x.FromPlaceId).HasColumnName("FromPlaceID").IsRequired();
            //Property(x => x.ToPlaceId).HasColumnName("ToPlaceID").IsRequired();
            Property(x => x.ContractorId).HasColumnName("ContractorID").IsOptional();
            Property(x => x.ShortageTypeId).HasColumnName("ShortageTypeID").IsOptional();
            Property(x => x.BookNumber).HasColumnName("BookNumber").IsRequired();
            Property(x => x.VoucherNo).HasColumnName("VoucherNo").IsRequired().HasMaxLength(50);
            Property(x => x.DispatchDate).HasColumnName("DispatchDate").IsRequired();
            Property(x => x.ReceiveDate).HasColumnName("ReceiveDate").IsOptional();
            Property(x => x.CommodityTypeId).HasColumnName("CommodityTypeID").IsRequired();
            Property(x => x.Trucks).HasColumnName("Trucks").IsRequired();
            Property(x => x.Bags).HasColumnName("Bags").IsRequired();
            Property(x => x.Quantity).HasColumnName("Quantity").IsRequired();
            Property(x => x.BagsReceived).HasColumnName("BagsReceived").IsRequired();
            Property(x => x.QuantityReceived).HasColumnName("QuantityReceived").IsRequired();
            Property(x => x.UnitId).HasColumnName("UnitID").IsRequired();
            Property(x => x.Rate).HasColumnName("Rate").IsOptional();
            Property(x => x.Status).HasColumnName("Status").IsRequired();
            Property(x => x.Remarks).HasColumnName("Remarks").IsOptional().HasMaxLength(null);
            //Property(x => x.IssuingOfficerId).HasColumnName("IssuingOfficerID").IsOptional();
            //Property(x => x.ReceivingOfficerId).HasColumnName("ReceivingOfficerID").IsOptional();

            // Foreign keys
            //HasRequired(a => a.FromPlace).WithMany(b => b.CommodityTransactions).HasForeignKey(c => c.FromPlaceId).WillCascadeOnDelete(false); // FK_CommodityTransaction_Place
            //HasRequired(a => a.ToPlace).WithMany(b => b.CommodityTransactions).HasForeignKey(c => c.ToPlaceId).WillCascadeOnDelete(false); // FK_CashTransaction_Place
            HasOptional(a => a.Contractor).WithMany(b => b.CommodityTransactions).HasForeignKey(c => c.ContractorId).WillCascadeOnDelete(false); // FK_CommodityTransaction_Contractor
            HasRequired(a => a.CommodityType).WithMany(b => b.CommodityTransactions).HasForeignKey(c => c.CommodityTypeId).WillCascadeOnDelete(false); // FK_CommodityTransaction_CommodityType
            HasRequired(a => a.Unit).WithMany(b => b.CommodityTransactions).HasForeignKey(c => c.UnitId).WillCascadeOnDelete(false); // FK_CommodityTransaction_Units
            //HasOptional(a => a.IssuingOfficer).WithMany(b => b.CommodityTransactions).HasForeignKey(c => c.IssuingOfficerId); // FK_CommodityTransaction_Person
            //HasOptional(a => a.ReceivingOfficer).WithMany(b => b.CommodityTransactions).HasForeignKey(c => c.ReceivingOfficerId); // FK_CommodityTransaction_Person
        }
    }

    // CommodityType
    internal class CommodityTypeConfiguration : EntityTypeConfiguration<CommodityType>
    {
        public CommodityTypeConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".CommodityType");
            HasKey(x => x.CommodityTypeId);

            Property(x => x.CommodityTypeId).HasColumnName("CommodityTypeID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(null);
        }
    }

    // ShortageType
    internal class ShortageTypeConfiguration : EntityTypeConfiguration<ShortageType>
    {
        public ShortageTypeConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".ShortageType");
            HasKey(x => x.ShortageTypeId);

            Property(x => x.ShortageTypeId).HasColumnName("ShortageTypeID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(null);
        }
    }

    // DamageType
    internal class DamageTypeConfiguration : EntityTypeConfiguration<DamageType>
    {
        public DamageTypeConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".DamageType");
            HasKey(x => x.DamageTypeId);

            Property(x => x.DamageTypeId).HasColumnName("DamageTypeID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(null);
        }
    }

    // Contractor
    internal class ContractorConfiguration : EntityTypeConfiguration<Contractor>
    {
        public ContractorConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Contractor");
            HasKey(x => x.ContractorId);

            Property(x => x.ContractorId).HasColumnName("ContractorID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PlaceId).HasColumnName("PlaceID").IsRequired();

            // Foreign keys
            HasRequired(a => a.Place).WithMany(b => b.Contractors).HasForeignKey(c => c.PlaceId).WillCascadeOnDelete(false); // FK_Contractor_Place
        }
    }

    // ContractorVehicle
    internal class ContractorVehicleConfiguration : EntityTypeConfiguration<ContractorVehicle>
    {
        public ContractorVehicleConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".ContractorVehicle");
            HasKey(x => x.ContractorVehicleId);

            Property(x => x.ContractorVehicleId).HasColumnName("ContractorVehicleID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.RegistrationNumber).HasColumnName("RegistrationNumber").IsRequired().HasMaxLength(50);
            Property(x => x.VehicleType).HasColumnName("VehicleType").IsRequired();
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(null);
            Property(x => x.ContractorId).HasColumnName("ContractorID").IsRequired();

            // Foreign keys
            HasRequired(a => a.Contractor).WithMany(b => b.ContractorVehicles).HasForeignKey(c => c.ContractorId).WillCascadeOnDelete(false); // FK_ContractorVehicle_Contractor
        }
    }

    // Dealer
    internal class DealerConfiguration : EntityTypeConfiguration<Dealer>
    {
        public DealerConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Dealer");
            HasKey(x => x.DealerId);

            Property(x => x.DealerId).HasColumnName("DealerID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PlaceId).HasColumnName("PlaceID").IsRequired();

            // Foreign keys
            HasRequired(a => a.Place).WithMany(b => b.Dealers).HasForeignKey(c => c.PlaceId).WillCascadeOnDelete(false); // FK_Dealer_Place
        }
    }

    // Depot
    internal class DepotConfiguration : EntityTypeConfiguration<Depot>
    {
        public DepotConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Depot");
            HasKey(x => x.DepotId);

            Property(x => x.DepotId).HasColumnName("DepotID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PlaceId).HasColumnName("PlaceID").IsRequired();
            Property(x => x.DistrictOfficeId).HasColumnName("DistrictOfficeID").IsRequired();
            Property(x => x.BankAccountId).HasColumnName("BankAccountID").IsRequired();

            // Foreign keys
            HasRequired(a => a.Place).WithMany(b => b.Depots).HasForeignKey(c => c.PlaceId).WillCascadeOnDelete(false); // FK_Depot_Place
            HasRequired(a => a.DistrictOffice).WithMany(b => b.Depots).HasForeignKey(c => c.DistrictOfficeId).WillCascadeOnDelete(false); // FK_Depot_District
            HasRequired(a => a.BankAccount).WithMany(b => b.Depots).HasForeignKey(c => c.BankAccountId).WillCascadeOnDelete(false); // FK_Depot_BankAccount
        }
    }

    // DepotAllocation
    internal class DepotAllocationConfiguration : EntityTypeConfiguration<DepotAllocation>
    {
        public DepotAllocationConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".DepotAllocation");
            HasKey(x => x.DepotAllocationId);

            Property(x => x.DepotAllocationId).HasColumnName("DepotAllocationID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.DepotId).HasColumnName("DepotID").IsRequired();
            Property(x => x.FlourMillId).HasColumnName("FlourMillID").IsRequired();
            Property(x => x.CommodityTypeId).HasColumnName("CommodityTypeID").IsRequired();
            Property(x => x.Quantity).HasColumnName("Quantity").IsRequired();
            Property(x => x.UnitId).HasColumnName("UnitID").IsOptional();
            Property(x => x.Month).HasColumnName("Month").IsOptional();

            // Foreign keys
            HasRequired(a => a.Depot).WithMany(b => b.DepotAllocations).HasForeignKey(c => c.DepotId).WillCascadeOnDelete(false); // FK_DepotAllocation_Depot
            HasRequired(a => a.FlourMill).WithMany(b => b.DepotAllocations).HasForeignKey(c => c.FlourMillId).WillCascadeOnDelete(false); // FK_DepotAllocation_FlourMill
            HasRequired(a => a.Unit).WithMany(b => b.DepotAllocations).HasForeignKey(c => c.UnitId).WillCascadeOnDelete(false); // FK_CommodityTransaction_Units
        }
    }

    // Depot_Damage
    internal class DepotDamageConfiguration : EntityTypeConfiguration<DepotDamage>
    {
        public DepotDamageConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Depot_Damage");
            HasKey(x => x.DepotDamageId);

            Property(x => x.DepotDamageId).HasColumnName("DepotDamageID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.DepotId).HasColumnName("DepotID").IsRequired();
            Property(x => x.DamageTypeId).HasColumnName("DamageTypeID").IsRequired();
            Property(x => x.DamageDate).HasColumnName("DamageDate").IsRequired();
            Property(x => x.CommodityTypeId).HasColumnName("CommodityTypeID").IsRequired();
            Property(x => x.Bags).HasColumnName("Bags").IsRequired();
            Property(x => x.Quantity).HasColumnName("Quantity").IsRequired();
            Property(x => x.UnitId).HasColumnName("UnitID").IsRequired();

            // Foreign keys
            HasRequired(a => a.Depot).WithMany(b => b.DepotDamages).HasForeignKey(c => c.DepotId).WillCascadeOnDelete(false); // FK_Depot_Damage_Depot
            HasRequired(a => a.CommodityType).WithMany(b => b.DepotDamages).HasForeignKey(c => c.CommodityTypeId).WillCascadeOnDelete(false); // FK_Depot_Damage_CommodityType
            HasRequired(a => a.DamageType).WithMany(b => b.DepotDamages).HasForeignKey(c => c.DamageTypeId).WillCascadeOnDelete(false); // FK_Depot_Damage_DamageType
            HasRequired(a => a.Unit).WithMany(b => b.DepotDamages).HasForeignKey(c => c.UnitId).WillCascadeOnDelete(false); // FK_Depot_Damage_Units
        }
    }

    // Directorate
    internal class DirectorateConfiguration : EntityTypeConfiguration<Directorate>
    {
        public DirectorateConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Directorate");
            HasKey(x => x.DirectorateId);

            Property(x => x.DirectorateId).HasColumnName("DirectorateID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PlaceId).HasColumnName("PlaceID").IsRequired();
            Property(x => x.BankAccountId).HasColumnName("BankAccountID");

            // Foreign keys
            HasRequired(a => a.BankAccount).WithMany(b => b.Directorates).HasForeignKey(c => c.BankAccountId).WillCascadeOnDelete(false); // FK_Directorate_Bank
            HasRequired(a => a.Place).WithMany(b => b.Directorates).HasForeignKey(c => c.PlaceId).WillCascadeOnDelete(false); // FK_Depot_Place
        }
    }

    // District
    internal class DistrictConfiguration : EntityTypeConfiguration<District>
    {
        public DistrictConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".District");
            HasKey(x => x.DistrictId);

            Property(x => x.DistrictId).HasColumnName("DistrictID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(null);
            Property(x => x.ProvinceId).HasColumnName("ProvinceID").IsRequired();

            // Foreign keys
            HasRequired(a => a.Province).WithMany(b => b.Districts).HasForeignKey(c => c.ProvinceId).WillCascadeOnDelete(false); // FK_District_Province
        }
    }

    // DistrictOffice
    internal class DistrictOfficeConfiguration : EntityTypeConfiguration<DistrictOffice>
    {
        public DistrictOfficeConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".DistrictOffice");
            HasKey(x => x.DistrictOfficeId);

            Property(x => x.DistrictOfficeId).HasColumnName("DistrictOfficeID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PlaceId).HasColumnName("PlaceID").IsRequired();
            Property(x => x.DivisionalOfficeId).HasColumnName("DivisionalOfficeID").IsRequired();
            Property(x => x.Population).HasColumnName("Population").IsRequired();

            // Foreign keys
            HasRequired(a => a.DivisionalOffice).WithMany(b => b.DistrictOffices).HasForeignKey(c => c.DivisionalOfficeId).WillCascadeOnDelete(false); // FK_DistrictOffice_DivisionalOffice
            HasRequired(a => a.Place).WithMany(b => b.DistrictOffices).HasForeignKey(c => c.PlaceId).WillCascadeOnDelete(false); // FK_DistrictOffice_Place
        }
    }

    // DivisionalOffice
    internal class DivisionalOfficeConfiguration : EntityTypeConfiguration<DivisionalOffice>
    {
        public DivisionalOfficeConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".DivisionalOffice");
            HasKey(x => x.DivisionalOfficeId);

            Property(x => x.DivisionalOfficeId).HasColumnName("DivisionalOfficeID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PlaceId).HasColumnName("PlaceID").IsRequired();

            // Foreign keys
            HasRequired(a => a.Place).WithMany(b => b.DivisionalOffices).HasForeignKey(c => c.PlaceId).WillCascadeOnDelete(false); // FK_DivisionalOffice_Place
        }
    }

    // FlourMill
    internal class FlourMillConfiguration : EntityTypeConfiguration<FlourMill>
    {
        public FlourMillConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".FlourMill");
            HasKey(x => x.FlourMillId);

            Property(x => x.FlourMillId).HasColumnName("FlourMillID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.DistrictOfficeId).HasColumnName("DistrictOfficeID").IsRequired();
            Property(x => x.PlaceId).HasColumnName("PlaceID").IsRequired();

            // Foreign keys
            HasRequired(a => a.DistrictOffice).WithMany(b => b.FlourMills).HasForeignKey(c => c.DistrictOfficeId).WillCascadeOnDelete(false); // FK_FlourMill_District
            HasRequired(a => a.Place).WithMany(b => b.FlourMills).HasForeignKey(c => c.PlaceId).WillCascadeOnDelete(false); // FK_FlourMill_Place
        }
    }

    // FlourMill_Chokar
    internal class FlourMillChokarConfiguration : EntityTypeConfiguration<FlourMillChokar>
    {
        public FlourMillChokarConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".FlourMill_Chokar");
            HasKey(x => x.FlourMillChokarId);

            Property(x => x.FlourMillChokarId).HasColumnName("FlourMill_ChokarID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.FlourMillId).HasColumnName("FlourMillID").IsRequired();
            Property(x => x.ProductionDate).HasColumnName("ProductionDate").IsRequired();
            Property(x => x.Quantity).HasColumnName("Quantity").IsRequired();
            Property(x => x.UnitId).HasColumnName("UnitID").IsRequired();

            // Foreign keys
            HasRequired(a => a.FlourMill).WithMany(b => b.FlourMillChokars).HasForeignKey(c => c.FlourMillId).WillCascadeOnDelete(false); // FK_FlourMill_Chokar_FlourMill
            HasRequired(a => a.Unit).WithMany(b => b.FlourMillChokars).HasForeignKey(c => c.UnitId).WillCascadeOnDelete(false); // FK_CommodityTransaction_Units
        }
    }

    // Godown
    internal class GodownConfiguration : EntityTypeConfiguration<Godown>
    {
        public GodownConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Godown");
            HasKey(x => x.GodownId);

            Property(x => x.GodownId).HasColumnName("GodownID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PlaceId).HasColumnName("PlaceID").IsRequired();
            Property(x => x.Capacity).HasColumnName("Capacity").IsRequired();
            Property(x => x.UnitId).HasColumnName("UnitID").IsRequired();
            Property(x => x.Type).HasColumnName("Type").IsRequired();
            Property(x => x.StorageOfficeId).HasColumnName("StorageOfficeID").IsOptional();

            // Foreign keys
            HasRequired(a => a.Place).WithMany(b => b.Godowns).HasForeignKey(c => c.PlaceId).WillCascadeOnDelete(false); // FK_Godown_Place
            HasOptional(a => a.StorageOffice).WithMany(b => b.Godowns).HasForeignKey(c => c.StorageOfficeId).WillCascadeOnDelete(false); // FK_Godown_StorageOffice
            HasRequired(a => a.Unit).WithMany(b => b.Godowns).HasForeignKey(c => c.UnitId).WillCascadeOnDelete(false); // FK_CommodityTransaction_Units
        }
    }

    // Person
    internal class PersonConfiguration : EntityTypeConfiguration<Person>
    {
        public PersonConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Person");
            HasKey(x => x.PersonId);

            Property(x => x.PersonId).HasColumnName("PersonID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired();
            Property(x => x.CNIC).HasColumnName("CNIC").IsOptional().HasMaxLength(50);
            Property(x => x.Mobile).HasColumnName("Mobile").IsOptional().HasMaxLength(50);
            Property(x => x.Email).HasColumnName("Email").IsOptional().HasMaxLength(50);
            Property(x => x.Type).HasColumnName("Type").IsRequired();
        }
    }

    // Place
    internal class PlaceConfiguration : EntityTypeConfiguration<Place>
    {
        public PlaceConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Place");
            HasKey(x => x.PlaceId);

            Property(x => x.PlaceId).HasColumnName("PlaceID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(x => x.Type).HasColumnName("Type").IsRequired();
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(null);
            Property(x => x.Landline).HasColumnName("Landline").IsOptional().HasMaxLength(50);
            Property(x => x.Fax).HasColumnName("Fax").IsOptional().HasMaxLength(50);
            Property(x => x.ProvinceId).HasColumnName("ProvinceID").IsRequired();
            Property(x => x.DistrictId).HasColumnName("DistrictID").IsRequired();
            Property(x => x.TehsilId).HasColumnName("TehsilID").IsRequired();
            Property(x => x.UnionCouncilId).HasColumnName("UnionCouncilID").IsOptional();
            Property(x => x.Address).HasColumnName("Address").HasMaxLength(null);
            Property(x => x.Geography).HasColumnName("Geography").IsOptional();
            Property(x => x.InchargePersonId).HasColumnName("InchargePersonId").IsRequired();

            // Foreign keys
            HasRequired(a => a.Province).WithMany(b => b.Places).HasForeignKey(c => c.ProvinceId).WillCascadeOnDelete(false); // FK_Place_Province
            HasRequired(a => a.District).WithMany(b => b.Places).HasForeignKey(c => c.DistrictId).WillCascadeOnDelete(false); // FK_Place_District
            HasRequired(a => a.Tehsil).WithMany(b => b.Places).HasForeignKey(c => c.TehsilId).WillCascadeOnDelete(false); // FK_Place_Tehsil
            HasOptional(a => a.UnionCouncil).WithMany(b => b.Places).HasForeignKey(c => c.UnionCouncilId).WillCascadeOnDelete(false); // FK_Place_UnionCouncil
            HasRequired(a => a.InchargePerson).WithMany(b => b.Places).HasForeignKey(c => c.InchargePersonId).WillCascadeOnDelete(false); // FK_Place_Person
        }
    }

    // Province
    internal class ProvinceConfiguration : EntityTypeConfiguration<Province>
    {
        public ProvinceConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Province");
            HasKey(x => x.ProvinceId);

            Property(x => x.ProvinceId).HasColumnName("ProvinceID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(1073741823);
        }
    }

    // StorageOffice
    internal class StorageOfficeConfiguration : EntityTypeConfiguration<StorageOffice>
    {
        public StorageOfficeConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".StorageOffice");
            HasKey(x => x.StorageOfficeId);

            Property(x => x.StorageOfficeId).HasColumnName("StorageOfficeID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PlaceId).HasColumnName("PlaceID").IsRequired();
            Property(x => x.DirectorateId).HasColumnName("DirectorateID").IsRequired();

            // Foreign keys
            HasRequired(a => a.Place).WithMany(b => b.StorageOffices).HasForeignKey(c => c.PlaceId).WillCascadeOnDelete(false); // FK_StorageOffice_Place
            HasRequired(a => a.Directorate).WithMany(b => b.StorageOffices).HasForeignKey(c => c.DirectorateId).WillCascadeOnDelete(false); // FK_StorageOffice_Directorate
        }
    }

    // Tehsil
    internal class TehsilConfiguration : EntityTypeConfiguration<Tehsil>
    {
        public TehsilConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Tehsil");
            HasKey(x => x.TehsilId);

            Property(x => x.TehsilId).HasColumnName("TehsilID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(1073741823);
            Property(x => x.DistrictId).HasColumnName("DistrictID").IsRequired();

            // Foreign keys
            HasRequired(a => a.District).WithMany(b => b.Tehsils).HasForeignKey(c => c.DistrictId).WillCascadeOnDelete(false); // FK_Tehsil_District
        }
    }

    // UnionCouncil
    internal class UnionCouncilConfiguration : EntityTypeConfiguration<UnionCouncil>
    {
        public UnionCouncilConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".UnionCouncil");
            HasKey(x => x.UnionCouncilId);

            Property(x => x.UnionCouncilId).HasColumnName("UnionCouncilID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(1073741823);
            Property(x => x.TehsilId).HasColumnName("TehsilID").IsRequired();

            // Foreign keys
            HasRequired(a => a.Tehsil).WithMany(b => b.UnionCouncils).HasForeignKey(c => c.TehsilId).WillCascadeOnDelete(false); // FK_UnionCouncil_Tehsil
        }
    }

    // Units
    internal class UnitConfiguration : EntityTypeConfiguration<Unit>
    {
        public UnitConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Units");
            HasKey(x => x.UnitId);

            Property(x => x.UnitId).HasColumnName("UnitID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(50);
            Property(x => x.ConversionFactor).HasColumnName("ConversionFactor").IsRequired();
        }
    }

    // WheatRequest
    internal class WheatRequestConfiguration : EntityTypeConfiguration<WheatRequest>
    {
        public WheatRequestConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".WheatRequest");
            HasKey(x => x.WheatRequestId);

            Property(x => x.WheatRequestId).HasColumnName("WheatRequestID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.DirectorateId).HasColumnName("DirectorateID").IsRequired();
            Property(x => x.RequestDate).IsRequired();
            Property(x => x.Quantity).HasColumnName("Quantity").IsRequired();
            Property(x => x.UnitId).HasColumnName("UnitID").IsRequired();
            Property(x => x.ReferenceNo).HasColumnName("ReferenceNo").IsRequired().HasMaxLength(50);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(null);

            // Foreign keys
            HasRequired(a => a.Directorate).WithMany(b => b.WheatRequests).HasForeignKey(c => c.DirectorateId).WillCascadeOnDelete(false); // FK_WheatRequest_Directorate
            HasRequired(a => a.Unit).WithMany(b => b.WheatRequests).HasForeignKey(c => c.UnitId).WillCascadeOnDelete(false); // FK_CommodityTransaction_Units
        }
    }
}