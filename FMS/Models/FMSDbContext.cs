using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FMS.Models
{
    // ************************************************************************
    // Unit of work
    public interface IFMSDbContext : IDisposable
    {
        IDbSet<Bank> Banks { get; set; } // Bank
        IDbSet<BankAccount> BankAccounts { get; set; } // BankAccount
        IDbSet<CashTransaction> CashTransactions { get; set; } // CashTransaction
        IDbSet<CommodityTransaction> CommodityTransactions { get; set; } // CommodityTransaction
        IDbSet<CommodityType> CommodityTypes { get; set; } // CommodityType
        IDbSet<ShortageType> ShortageTypes { get; set; } // ShortageType
        IDbSet<DamageType> DamageTypes { get; set; } // DamageType
        IDbSet<Contractor> Contractors { get; set; } // Contractor
        IDbSet<ContractorType> ContractorTypes { get; set; } // ContractorType
        IDbSet<ContractorVehicle> ContractorVehicles { get; set; } // ContractorVehicle
        IDbSet<Dealer> Dealers { get; set; } // Dealer
        IDbSet<Depot> Depots { get; set; } // Depot
        IDbSet<DepotAllocation> DepotAllocations { get; set; } // DepotAllocation
        IDbSet<DepotDamage> DepotDamages { get; set; } // Depot_Damage
        IDbSet<Directorate> Directorates { get; set; } // Directorate
        IDbSet<District> Districts { get; set; } // District
        IDbSet<DistrictOffice> DistrictOffices { get; set; } // DistrictOffice
        IDbSet<DivisionalOffice> DivisionalOffices { get; set; } // DivisionalOffice
        IDbSet<FlourMill> FlourMills { get; set; } // FlourMill
        IDbSet<FlourMillChokar> FlourMillChokars { get; set; } // FlourMill_Chokar
        IDbSet<Godown> Godowns { get; set; } // Godown
        IDbSet<Person> People { get; set; } // Person
        IDbSet<Place> Places { get; set; } // Place
        IDbSet<Province> Provinces { get; set; } // Province
        IDbSet<StorageOffice> StorageOffices { get; set; } // StorageOffice

        IDbSet<Tehsil> Tehsils { get; set; } // Tehsil
        IDbSet<UnionCouncil> UnionCouncils { get; set; } // UnionCouncil
        IDbSet<Unit> Units { get; set; } // Units
        IDbSet<WheatRequest> WheatRequests { get; set; } // WheatRequest

        int SaveChanges();
    }

    // ************************************************************************
    // Database context
    public class FMSDbContext : DbContext, IFMSDbContext
    {
        public IDbSet<Bank> Banks { get; set; } // Bank
        public IDbSet<BankAccount> BankAccounts { get; set; } // BankAccount
        public IDbSet<CashTransaction> CashTransactions { get; set; } // CashTransaction
        public IDbSet<CommodityTransaction> CommodityTransactions { get; set; } // CommodityTransaction
        public IDbSet<CommodityType> CommodityTypes { get; set; } // CommodityType
        public IDbSet<ShortageType> ShortageTypes { get; set; } // ShortageType
        public IDbSet<DamageType> DamageTypes { get; set; } // DamageType
        public IDbSet<Contractor> Contractors { get; set; } // Contractor
        public IDbSet<ContractorType> ContractorTypes { get; set; } // ContractorType
        public IDbSet<ContractorVehicle> ContractorVehicles { get; set; } // ContractorVehicle
        public IDbSet<Dealer> Dealers { get; set; } // Dealer
        public IDbSet<Depot> Depots { get; set; } // Depot
        public IDbSet<DepotAllocation> DepotAllocations { get; set; } // DepotAllocation
        public IDbSet<DepotDamage> DepotDamages { get; set; } // Depot_Damage
        public IDbSet<Directorate> Directorates { get; set; } // Directorate
        public IDbSet<District> Districts { get; set; } // District
        public IDbSet<DistrictOffice> DistrictOffices { get; set; } // DistrictOffice
        public IDbSet<DivisionalOffice> DivisionalOffices { get; set; } // DivisionalOffice
        public IDbSet<FlourMill> FlourMills { get; set; } // FlourMill
        public IDbSet<FlourMillChokar> FlourMillChokars { get; set; } // FlourMill_Chokar
        public IDbSet<Godown> Godowns { get; set; } // Godown
        public IDbSet<Person> People { get; set; } // Person
        public IDbSet<Place> Places { get; set; } // Place
        public IDbSet<Province> Provinces { get; set; } // Province
        public IDbSet<StorageOffice> StorageOffices { get; set; } // StorageOffice
        public IDbSet<Tehsil> Tehsils { get; set; } // Tehsil
        public IDbSet<UnionCouncil> UnionCouncils { get; set; } // UnionCouncil
        public IDbSet<Unit> Units { get; set; } // Units
        public IDbSet<WheatRequest> WheatRequests { get; set; } // WheatRequest

        public FMSDbContext()
            : base("Name=DefaultConnection")
        {
        }

        public FMSDbContext(string connectionString)
            : base(connectionString)
        {
        }

        public FMSDbContext(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model)
            : base(connectionString, model)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new BankConfiguration());
            modelBuilder.Configurations.Add(new BankAccountConfiguration());
            modelBuilder.Configurations.Add(new CashTransactionConfiguration());
            modelBuilder.Configurations.Add(new CommodityTransactionConfiguration());
            modelBuilder.Configurations.Add(new CommodityTypeConfiguration());
            modelBuilder.Configurations.Add(new ContractorConfiguration());
            modelBuilder.Configurations.Add(new ContractorVehicleConfiguration());
            modelBuilder.Configurations.Add(new DealerConfiguration());
            modelBuilder.Configurations.Add(new DepotConfiguration());
            modelBuilder.Configurations.Add(new DepotAllocationConfiguration());
            modelBuilder.Configurations.Add(new DepotDamageConfiguration());
            modelBuilder.Configurations.Add(new DirectorateConfiguration());
            modelBuilder.Configurations.Add(new DistrictConfiguration());
            modelBuilder.Configurations.Add(new DistrictOfficeConfiguration());
            modelBuilder.Configurations.Add(new DivisionalOfficeConfiguration());
            modelBuilder.Configurations.Add(new FlourMillConfiguration());
            modelBuilder.Configurations.Add(new FlourMillChokarConfiguration());
            modelBuilder.Configurations.Add(new GodownConfiguration());
            modelBuilder.Configurations.Add(new PersonConfiguration());
            modelBuilder.Configurations.Add(new PlaceConfiguration());
            modelBuilder.Configurations.Add(new ProvinceConfiguration());
            modelBuilder.Configurations.Add(new StorageOfficeConfiguration());
            modelBuilder.Configurations.Add(new TehsilConfiguration());
            modelBuilder.Configurations.Add(new UnionCouncilConfiguration());
            modelBuilder.Configurations.Add(new UnitConfiguration());
            modelBuilder.Configurations.Add(new WheatRequestConfiguration());
        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new BankConfiguration(schema));
            modelBuilder.Configurations.Add(new BankAccountConfiguration(schema));
            modelBuilder.Configurations.Add(new CashTransactionConfiguration(schema));
            modelBuilder.Configurations.Add(new CommodityTransactionConfiguration(schema));
            modelBuilder.Configurations.Add(new CommodityTypeConfiguration(schema));
            modelBuilder.Configurations.Add(new ContractorConfiguration(schema));
            modelBuilder.Configurations.Add(new ContractorVehicleConfiguration(schema));
            modelBuilder.Configurations.Add(new DealerConfiguration(schema));
            modelBuilder.Configurations.Add(new DepotConfiguration(schema));
            modelBuilder.Configurations.Add(new DepotAllocationConfiguration(schema));
            modelBuilder.Configurations.Add(new DepotDamageConfiguration(schema));
            modelBuilder.Configurations.Add(new DirectorateConfiguration(schema));
            modelBuilder.Configurations.Add(new DistrictConfiguration(schema));
            modelBuilder.Configurations.Add(new DistrictOfficeConfiguration(schema));
            modelBuilder.Configurations.Add(new DivisionalOfficeConfiguration(schema));
            modelBuilder.Configurations.Add(new FlourMillConfiguration(schema));
            modelBuilder.Configurations.Add(new FlourMillChokarConfiguration(schema));
            modelBuilder.Configurations.Add(new GodownConfiguration(schema));
            modelBuilder.Configurations.Add(new PersonConfiguration(schema));
            modelBuilder.Configurations.Add(new PlaceConfiguration(schema));
            modelBuilder.Configurations.Add(new ProvinceConfiguration(schema));
            modelBuilder.Configurations.Add(new StorageOfficeConfiguration(schema));
            modelBuilder.Configurations.Add(new TehsilConfiguration(schema));
            modelBuilder.Configurations.Add(new UnionCouncilConfiguration(schema));
            modelBuilder.Configurations.Add(new UnitConfiguration(schema));
            modelBuilder.Configurations.Add(new WheatRequestConfiguration(schema));
            return modelBuilder;
        }        
    }
}