namespace FMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankAccount",
                c => new
                    {
                        BankAccountID = c.Int(nullable: false, identity: true),
                        BankID = c.Int(nullable: false),
                        PlaceID = c.Int(nullable: false),
                        Branch = c.String(nullable: false, maxLength: 50),
                        AccountNumber = c.String(nullable: false, maxLength: 50),
                        AccountType = c.Int(nullable: false),
                        Description = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.BankAccountID)
                .ForeignKey("dbo.Bank", t => t.BankID)
                .Index(t => t.BankID);
            
            CreateTable(
                "dbo.Bank",
                c => new
                    {
                        BankID = c.Int(nullable: false, identity: true),
                        BankName = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.BankID);
            
            CreateTable(
                "dbo.CashTransaction",
                c => new
                    {
                        CashTransactionID = c.Int(nullable: false, identity: true),
                        CompositeKey = c.String(nullable: false, maxLength: 50),
                        Type = c.Int(nullable: false),
                        CashVoucherNo = c.String(nullable: false, maxLength: 50),
                        FromPlaceID = c.Int(nullable: false),
                        ToPlaceID = c.Int(nullable: false),
                        CommodityTypeID = c.Int(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                        Amount = c.Int(nullable: false),
                        Bags = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitID = c.Int(nullable: false),
                        Rate = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        CommodityTransactionID = c.Int(nullable: false),
                        BankAccountID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CashTransactionID)
                .ForeignKey("dbo.BankAccount", t => t.BankAccountID)
                .ForeignKey("dbo.CommodityTransaction", t => t.CommodityTransactionID)
                .ForeignKey("dbo.CommodityType", t => t.CommodityTypeID)
                .ForeignKey("dbo.Place", t => t.FromPlaceID)
                .ForeignKey("dbo.Units", t => t.UnitID, cascadeDelete: true)
                .Index(t => t.FromPlaceID)
                .Index(t => t.CommodityTypeID)
                .Index(t => t.UnitID)
                .Index(t => t.CommodityTransactionID)
                .Index(t => t.BankAccountID);
            
            CreateTable(
                "dbo.CommodityTransaction",
                c => new
                    {
                        CommodityTransactionID = c.Int(nullable: false, identity: true),
                        CompositeKey = c.String(maxLength: 50),
                        Type = c.Int(nullable: false),
                        FromPlaceID = c.Int(nullable: false),
                        ToPlaceID = c.Int(nullable: false),
                        ContractorID = c.Int(nullable: false),
                        VoucherNo = c.String(nullable: false, maxLength: 50),
                        DispatchDate = c.DateTime(nullable: false),
                        ReceiveDate = c.DateTime(),
                        CommodityTypeID = c.Int(nullable: false),
                        Trucks = c.Int(nullable: false),
                        Bags = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitID = c.Int(nullable: false),
                        Rate = c.Int(),
                        Status = c.Int(nullable: false),
                        Remarks = c.String(),
                        IssuingOfficerId = c.Int(),
                        ReceivingOfficerId = c.Int(),
                        Person_PersonId = c.Int(),
                    })
                .PrimaryKey(t => t.CommodityTransactionID)
                .ForeignKey("dbo.Person", t => t.Person_PersonId)
                .ForeignKey("dbo.CommodityType", t => t.CommodityTypeID)
                .ForeignKey("dbo.Contractor", t => t.ContractorID)
                .ForeignKey("dbo.Person", t => t.IssuingOfficerId)
                .ForeignKey("dbo.Place", t => t.FromPlaceID)
                .ForeignKey("dbo.Person", t => t.ReceivingOfficerId)
                .ForeignKey("dbo.Units", t => t.UnitID)
                .Index(t => t.FromPlaceID)
                .Index(t => t.ContractorID)
                .Index(t => t.CommodityTypeID)
                .Index(t => t.UnitID)
                .Index(t => t.IssuingOfficerId)
                .Index(t => t.ReceivingOfficerId)
                .Index(t => t.Person_PersonId);
            
            CreateTable(
                "dbo.CommodityType",
                c => new
                    {
                        CommodityTypeID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.CommodityTypeID);
            
            CreateTable(
                "dbo.Depot_Damage",
                c => new
                    {
                        DepotDamageID = c.Int(nullable: false, identity: true),
                        DepotID = c.Int(nullable: false),
                        DamageType = c.Int(nullable: false),
                        DamageDate = c.DateTime(nullable: false),
                        CommodityTypeID = c.Int(nullable: false),
                        Bags = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DepotDamageID)
                .ForeignKey("dbo.CommodityType", t => t.CommodityTypeID)
                .ForeignKey("dbo.Depot", t => t.DepotID)
                .ForeignKey("dbo.Units", t => t.UnitID)
                .Index(t => t.DepotID)
                .Index(t => t.CommodityTypeID)
                .Index(t => t.UnitID);
            
            CreateTable(
                "dbo.Depot",
                c => new
                    {
                        DepotID = c.Int(nullable: false, identity: true),
                        PlaceID = c.Int(nullable: false),
                        DistrictOfficeID = c.Int(nullable: false),
                        BankAccountID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DepotID)
                .ForeignKey("dbo.BankAccount", t => t.BankAccountID)
                .ForeignKey("dbo.DistrictOffice", t => t.DistrictOfficeID)
                .ForeignKey("dbo.Place", t => t.PlaceID)
                .Index(t => t.PlaceID)
                .Index(t => t.DistrictOfficeID)
                .Index(t => t.BankAccountID);
            
            CreateTable(
                "dbo.DepotAllocation",
                c => new
                    {
                        DepotAllocationID = c.Int(nullable: false, identity: true),
                        DepotID = c.Int(nullable: false),
                        FlourMillID = c.Int(nullable: false),
                        CommodityTypeID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitID = c.Int(),
                        Month = c.DateTime(),
                    })
                .PrimaryKey(t => t.DepotAllocationID)
                .ForeignKey("dbo.Depot", t => t.DepotID)
                .ForeignKey("dbo.FlourMill", t => t.FlourMillID)
                .ForeignKey("dbo.Units", t => t.UnitID)
                .Index(t => t.DepotID)
                .Index(t => t.FlourMillID)
                .Index(t => t.UnitID);
            
            CreateTable(
                "dbo.FlourMill",
                c => new
                    {
                        FlourMillID = c.Int(nullable: false, identity: true),
                        DistrictOfficeID = c.Int(nullable: false),
                        PlaceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FlourMillID)
                .ForeignKey("dbo.DistrictOffice", t => t.DistrictOfficeID)
                .ForeignKey("dbo.Place", t => t.PlaceID)
                .Index(t => t.DistrictOfficeID)
                .Index(t => t.PlaceID);
            
            CreateTable(
                "dbo.DistrictOffice",
                c => new
                    {
                        DistrictOfficeID = c.Int(nullable: false, identity: true),
                        PlaceID = c.String(nullable: false, maxLength: 50),
                        DivisionalOfficeID = c.Int(nullable: false),
                        Population = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DistrictOfficeID)
                .ForeignKey("dbo.DivisionalOffice", t => t.DivisionalOfficeID)
                .Index(t => t.DivisionalOfficeID);
            
            CreateTable(
                "dbo.DivisionalOffice",
                c => new
                    {
                        DivisionalOfficeID = c.Int(nullable: false, identity: true),
                        PlaceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DivisionalOfficeID)
                .ForeignKey("dbo.Place", t => t.PlaceID)
                .Index(t => t.PlaceID);
            
            CreateTable(
                "dbo.Place",
                c => new
                    {
                        PlaceID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Type = c.Int(nullable: false),
                        Description = c.String(maxLength: 50),
                        Landline = c.String(maxLength: 50),
                        Fax = c.String(maxLength: 50),
                        ProvinceID = c.Int(nullable: false),
                        DistrictID = c.Int(nullable: false),
                        TehsilID = c.Int(nullable: false),
                        UnionCouncilID = c.Int(nullable: true),
                        Address = c.String(nullable: false, maxLength: 100),
                        Geography = c.Geography(),
                        InchargePersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlaceID)
                .ForeignKey("dbo.District", t => t.DistrictID)
                .ForeignKey("dbo.Person", t => t.InchargePersonId)
                .ForeignKey("dbo.Province", t => t.ProvinceID)
                .ForeignKey("dbo.Tehsil", t => t.TehsilID)
                .ForeignKey("dbo.UnionCouncil", t => t.UnionCouncilID)
                .Index(t => t.ProvinceID)
                .Index(t => t.DistrictID)
                .Index(t => t.TehsilID)
                .Index(t => t.UnionCouncilID)
                .Index(t => t.InchargePersonId);
            
            CreateTable(
                "dbo.Contractor",
                c => new
                    {
                        ContractorID = c.Int(nullable: false, identity: true),
                        PlaceID = c.Int(nullable: false),
                        ContractorTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ContractorID)
                .ForeignKey("dbo.ContractorTypes", t => t.ContractorTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Place", t => t.PlaceID)
                .Index(t => t.PlaceID)
                .Index(t => t.ContractorTypeId);
            
            CreateTable(
                "dbo.ContractorTypes",
                c => new
                    {
                        ContractorTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ContractorTypeId);
            
            CreateTable(
                "dbo.ContractorVehicle",
                c => new
                    {
                        ContractorVehicleID = c.Int(nullable: false, identity: true),
                        RegistrationNumber = c.String(nullable: false, maxLength: 50),
                        VehicleType = c.Int(nullable: false),
                        Description = c.String(),
                        ContractorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ContractorVehicleID)
                .ForeignKey("dbo.Contractor", t => t.ContractorID)
                .Index(t => t.ContractorID);
            
            CreateTable(
                "dbo.Dealer",
                c => new
                    {
                        DealerID = c.Int(nullable: false, identity: true),
                        PlaceID = c.Int(nullable: false),
                        DepotId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DealerID)
                .ForeignKey("dbo.Depot", t => t.DepotId, cascadeDelete: true)
                .ForeignKey("dbo.Place", t => t.PlaceID)
                .Index(t => t.PlaceID)
                .Index(t => t.DepotId);
            
            CreateTable(
                "dbo.Directorate",
                c => new
                    {
                        DirectorateID = c.Int(nullable: false, identity: true),
                        PlaceID = c.Int(nullable: false),
                        BankAccountID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DirectorateID)
                .ForeignKey("dbo.BankAccount", t => t.BankAccountID)
                .ForeignKey("dbo.Place", t => t.PlaceID)
                .Index(t => t.PlaceID)
                .Index(t => t.BankAccountID);
            
            CreateTable(
                "dbo.StorageOffice",
                c => new
                    {
                        StorageOfficeID = c.Int(nullable: false, identity: true),
                        PlaceID = c.Int(nullable: false),
                        DirectorateID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StorageOfficeID)
                .ForeignKey("dbo.Directorate", t => t.DirectorateID)
                .ForeignKey("dbo.Place", t => t.PlaceID)
                .Index(t => t.PlaceID)
                .Index(t => t.DirectorateID);
            
            CreateTable(
                "dbo.Godown",
                c => new
                    {
                        GodownID = c.Int(nullable: false, identity: true),
                        PlaceID = c.Int(nullable: false),
                        Capacity = c.Int(nullable: false),
                        UnitID = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        StorageOfficeID = c.Int(),
                    })
                .PrimaryKey(t => t.GodownID)
                .ForeignKey("dbo.Place", t => t.PlaceID)
                .ForeignKey("dbo.StorageOffice", t => t.StorageOfficeID)
                .ForeignKey("dbo.Units", t => t.UnitID, cascadeDelete: true)
                .Index(t => t.PlaceID)
                .Index(t => t.UnitID)
                .Index(t => t.StorageOfficeID);
            
            CreateTable(
                "dbo.Units",
                c => new
                    {
                        UnitID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 50),
                        ConversionFactor = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.UnitID);
            
            CreateTable(
                "dbo.WheatRequest",
                c => new
                    {
                        WheatRequestID = c.Int(nullable: false, identity: true),
                        DirectorateID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitID = c.Int(nullable: false),
                        ReferenceNo = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 10, fixedLength: true),
                    })
                .PrimaryKey(t => t.WheatRequestID)
                .ForeignKey("dbo.Directorate", t => t.DirectorateID)
                .ForeignKey("dbo.Units", t => t.UnitID, cascadeDelete: true)
                .Index(t => t.DirectorateID)
                .Index(t => t.UnitID);
            
            CreateTable(
                "dbo.District",
                c => new
                    {
                        DistrictID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                        ProvinceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DistrictID)
                .ForeignKey("dbo.Province", t => t.ProvinceID)
                .Index(t => t.ProvinceID);
            
            CreateTable(
                "dbo.Province",
                c => new
                    {
                        ProvinceID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ProvinceID);
            
            CreateTable(
                "dbo.Tehsil",
                c => new
                    {
                        TehsilID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                        DistrictID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TehsilID)
                .ForeignKey("dbo.District", t => t.DistrictID)
                .Index(t => t.DistrictID);
            
            CreateTable(
                "dbo.UnionCouncil",
                c => new
                    {
                        UnionCouncilID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                        TehsilID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UnionCouncilID)
                .ForeignKey("dbo.Tehsil", t => t.TehsilID)
                .Index(t => t.TehsilID);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        PersonID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CNIC = c.String(maxLength: 50),
                        Mobile = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PersonID);
            
            CreateTable(
                "dbo.FlourMill_Chokar",
                c => new
                    {
                        FlourMill_ChokarID = c.Int(nullable: false, identity: true),
                        FlourMillID = c.Int(nullable: false),
                        ProductionDate = c.DateTime(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FlourMill_ChokarID)
                .ForeignKey("dbo.FlourMill", t => t.FlourMillID)
                .ForeignKey("dbo.Units", t => t.UnitID, cascadeDelete: true)
                .Index(t => t.FlourMillID)
                .Index(t => t.UnitID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CashTransaction", "UnitID", "dbo.Units");
            DropForeignKey("dbo.CashTransaction", "FromPlaceID", "dbo.Place");
            DropForeignKey("dbo.CashTransaction", "CommodityTypeID", "dbo.CommodityType");
            DropForeignKey("dbo.CashTransaction", "CommodityTransactionID", "dbo.CommodityTransaction");
            DropForeignKey("dbo.CommodityTransaction", "UnitID", "dbo.Units");
            DropForeignKey("dbo.CommodityTransaction", "ReceivingOfficerId", "dbo.Person");
            DropForeignKey("dbo.CommodityTransaction", "FromPlaceID", "dbo.Place");
            DropForeignKey("dbo.CommodityTransaction", "IssuingOfficerId", "dbo.Person");
            DropForeignKey("dbo.CommodityTransaction", "ContractorID", "dbo.Contractor");
            DropForeignKey("dbo.CommodityTransaction", "CommodityTypeID", "dbo.CommodityType");
            DropForeignKey("dbo.Depot_Damage", "UnitID", "dbo.Units");
            DropForeignKey("dbo.Depot_Damage", "DepotID", "dbo.Depot");
            DropForeignKey("dbo.Depot", "PlaceID", "dbo.Place");
            DropForeignKey("dbo.Depot", "DistrictOfficeID", "dbo.DistrictOffice");
            DropForeignKey("dbo.DepotAllocation", "UnitID", "dbo.Units");
            DropForeignKey("dbo.DepotAllocation", "FlourMillID", "dbo.FlourMill");
            DropForeignKey("dbo.FlourMill", "PlaceID", "dbo.Place");
            DropForeignKey("dbo.FlourMill_Chokar", "UnitID", "dbo.Units");
            DropForeignKey("dbo.FlourMill_Chokar", "FlourMillID", "dbo.FlourMill");
            DropForeignKey("dbo.FlourMill", "DistrictOfficeID", "dbo.DistrictOffice");
            DropForeignKey("dbo.DistrictOffice", "DivisionalOfficeID", "dbo.DivisionalOffice");
            DropForeignKey("dbo.DivisionalOffice", "PlaceID", "dbo.Place");
            DropForeignKey("dbo.Place", "UnionCouncilID", "dbo.UnionCouncil");
            DropForeignKey("dbo.Place", "TehsilID", "dbo.Tehsil");
            DropForeignKey("dbo.Place", "ProvinceID", "dbo.Province");
            DropForeignKey("dbo.Place", "InchargePersonId", "dbo.Person");
            DropForeignKey("dbo.CommodityTransaction", "Person_PersonId", "dbo.Person");
            DropForeignKey("dbo.Place", "DistrictID", "dbo.District");
            DropForeignKey("dbo.UnionCouncil", "TehsilID", "dbo.Tehsil");
            DropForeignKey("dbo.Tehsil", "DistrictID", "dbo.District");
            DropForeignKey("dbo.District", "ProvinceID", "dbo.Province");
            DropForeignKey("dbo.WheatRequest", "UnitID", "dbo.Units");
            DropForeignKey("dbo.WheatRequest", "DirectorateID", "dbo.Directorate");
            DropForeignKey("dbo.StorageOffice", "PlaceID", "dbo.Place");
            DropForeignKey("dbo.Godown", "UnitID", "dbo.Units");
            DropForeignKey("dbo.Godown", "StorageOfficeID", "dbo.StorageOffice");
            DropForeignKey("dbo.Godown", "PlaceID", "dbo.Place");
            DropForeignKey("dbo.StorageOffice", "DirectorateID", "dbo.Directorate");
            DropForeignKey("dbo.Directorate", "PlaceID", "dbo.Place");
            DropForeignKey("dbo.Directorate", "BankAccountID", "dbo.BankAccount");
            DropForeignKey("dbo.Dealer", "PlaceID", "dbo.Place");
            DropForeignKey("dbo.Dealer", "DepotId", "dbo.Depot");
            DropForeignKey("dbo.Contractor", "PlaceID", "dbo.Place");
            DropForeignKey("dbo.ContractorVehicle", "ContractorID", "dbo.Contractor");
            DropForeignKey("dbo.Contractor", "ContractorTypeId", "dbo.ContractorTypes");
            DropForeignKey("dbo.DepotAllocation", "DepotID", "dbo.Depot");
            DropForeignKey("dbo.Depot", "BankAccountID", "dbo.BankAccount");
            DropForeignKey("dbo.Depot_Damage", "CommodityTypeID", "dbo.CommodityType");
            DropForeignKey("dbo.CashTransaction", "BankAccountID", "dbo.BankAccount");
            DropForeignKey("dbo.BankAccount", "BankID", "dbo.Bank");
            DropIndex("dbo.FlourMill_Chokar", new[] { "UnitID" });
            DropIndex("dbo.FlourMill_Chokar", new[] { "FlourMillID" });
            DropIndex("dbo.UnionCouncil", new[] { "TehsilID" });
            DropIndex("dbo.Tehsil", new[] { "DistrictID" });
            DropIndex("dbo.District", new[] { "ProvinceID" });
            DropIndex("dbo.WheatRequest", new[] { "UnitID" });
            DropIndex("dbo.WheatRequest", new[] { "DirectorateID" });
            DropIndex("dbo.Godown", new[] { "StorageOfficeID" });
            DropIndex("dbo.Godown", new[] { "UnitID" });
            DropIndex("dbo.Godown", new[] { "PlaceID" });
            DropIndex("dbo.StorageOffice", new[] { "DirectorateID" });
            DropIndex("dbo.StorageOffice", new[] { "PlaceID" });
            DropIndex("dbo.Directorate", new[] { "BankAccountID" });
            DropIndex("dbo.Directorate", new[] { "PlaceID" });
            DropIndex("dbo.Dealer", new[] { "DepotId" });
            DropIndex("dbo.Dealer", new[] { "PlaceID" });
            DropIndex("dbo.ContractorVehicle", new[] { "ContractorID" });
            DropIndex("dbo.Contractor", new[] { "ContractorTypeId" });
            DropIndex("dbo.Contractor", new[] { "PlaceID" });
            DropIndex("dbo.Place", new[] { "InchargePersonId" });
            DropIndex("dbo.Place", new[] { "UnionCouncilID" });
            DropIndex("dbo.Place", new[] { "TehsilID" });
            DropIndex("dbo.Place", new[] { "DistrictID" });
            DropIndex("dbo.Place", new[] { "ProvinceID" });
            DropIndex("dbo.DivisionalOffice", new[] { "PlaceID" });
            DropIndex("dbo.DistrictOffice", new[] { "DivisionalOfficeID" });
            DropIndex("dbo.FlourMill", new[] { "PlaceID" });
            DropIndex("dbo.FlourMill", new[] { "DistrictOfficeID" });
            DropIndex("dbo.DepotAllocation", new[] { "UnitID" });
            DropIndex("dbo.DepotAllocation", new[] { "FlourMillID" });
            DropIndex("dbo.DepotAllocation", new[] { "DepotID" });
            DropIndex("dbo.Depot", new[] { "BankAccountID" });
            DropIndex("dbo.Depot", new[] { "DistrictOfficeID" });
            DropIndex("dbo.Depot", new[] { "PlaceID" });
            DropIndex("dbo.Depot_Damage", new[] { "UnitID" });
            DropIndex("dbo.Depot_Damage", new[] { "CommodityTypeID" });
            DropIndex("dbo.Depot_Damage", new[] { "DepotID" });
            DropIndex("dbo.CommodityTransaction", new[] { "Person_PersonId" });
            DropIndex("dbo.CommodityTransaction", new[] { "ReceivingOfficerId" });
            DropIndex("dbo.CommodityTransaction", new[] { "IssuingOfficerId" });
            DropIndex("dbo.CommodityTransaction", new[] { "UnitID" });
            DropIndex("dbo.CommodityTransaction", new[] { "CommodityTypeID" });
            DropIndex("dbo.CommodityTransaction", new[] { "ContractorID" });
            DropIndex("dbo.CommodityTransaction", new[] { "FromPlaceID" });
            DropIndex("dbo.CashTransaction", new[] { "BankAccountID" });
            DropIndex("dbo.CashTransaction", new[] { "CommodityTransactionID" });
            DropIndex("dbo.CashTransaction", new[] { "UnitID" });
            DropIndex("dbo.CashTransaction", new[] { "CommodityTypeID" });
            DropIndex("dbo.CashTransaction", new[] { "FromPlaceID" });
            DropIndex("dbo.BankAccount", new[] { "BankID" });
            DropTable("dbo.FlourMill_Chokar");
            DropTable("dbo.Person");
            DropTable("dbo.UnionCouncil");
            DropTable("dbo.Tehsil");
            DropTable("dbo.Province");
            DropTable("dbo.District");
            DropTable("dbo.WheatRequest");
            DropTable("dbo.Units");
            DropTable("dbo.Godown");
            DropTable("dbo.StorageOffice");
            DropTable("dbo.Directorate");
            DropTable("dbo.Dealer");
            DropTable("dbo.ContractorVehicle");
            DropTable("dbo.ContractorTypes");
            DropTable("dbo.Contractor");
            DropTable("dbo.Place");
            DropTable("dbo.DivisionalOffice");
            DropTable("dbo.DistrictOffice");
            DropTable("dbo.FlourMill");
            DropTable("dbo.DepotAllocation");
            DropTable("dbo.Depot");
            DropTable("dbo.Depot_Damage");
            DropTable("dbo.CommodityType");
            DropTable("dbo.CommodityTransaction");
            DropTable("dbo.CashTransaction");
            DropTable("dbo.Bank");
            DropTable("dbo.BankAccount");
        }
    }
}
