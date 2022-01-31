using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blockchain.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    BCFileID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.BCFileID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DOB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Employer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnnualIncome = table.Column<decimal>(type: "money", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    LoginID = table.Column<string>(type: "nchar(8)", maxLength: 8, nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    PasswordHash = table.Column<string>(type: "nchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.LoginID);
                    table.ForeignKey(
                        name: "FK_Logins_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    PropertyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuildingDesignBCFileID = table.Column<int>(type: "int", nullable: false),
                    SellerLicense = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SellerUserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.PropertyID);
                    table.ForeignKey(
                        name: "FK_Properties_Files_BuildingDesignBCFileID",
                        column: x => x.BuildingDesignBCFileID,
                        principalTable: "Files",
                        principalColumn: "BCFileID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Properties_Users_SellerUserID",
                        column: x => x.SellerUserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "BCApplications",
                columns: table => new
                {
                    BCApplicationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuyerUserID = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoanAmount = table.Column<decimal>(type: "money", nullable: true),
                    Decision = table.Column<bool>(type: "bit", nullable: true),
                    PermitStatus = table.Column<bool>(type: "bit", nullable: true),
                    LoanID = table.Column<int>(type: "int", nullable: true),
                    LoanDecision_BuyerUserID = table.Column<int>(type: "int", nullable: true),
                    Approved = table.Column<bool>(type: "bit", nullable: true),
                    PropertyID = table.Column<int>(type: "int", nullable: true),
                    BuyerID = table.Column<int>(type: "int", nullable: true),
                    Accepted = table.Column<bool>(type: "bit", nullable: true),
                    PermitApplication_PropertyID = table.Column<int>(type: "int", nullable: true),
                    PermitApplication_Decision = table.Column<bool>(type: "bit", nullable: true),
                    PermitID = table.Column<int>(type: "int", nullable: true),
                    PermitDecision_Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermitDecision_Approved = table.Column<bool>(type: "bit", nullable: true),
                    PendingTransactionsID = table.Column<int>(type: "int", nullable: true),
                    TransactionDecision_Accepted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BCApplications", x => x.BCApplicationID);
                    table.ForeignKey(
                        name: "FK_BCApplications_Properties_PermitApplication_PropertyID",
                        column: x => x.PermitApplication_PropertyID,
                        principalTable: "Properties",
                        principalColumn: "PropertyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BCApplications_Users_BuyerUserID",
                        column: x => x.BuyerUserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_BCApplications_Users_LoanDecision_BuyerUserID",
                        column: x => x.LoanDecision_BuyerUserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BCApplications_BuyerUserID",
                table: "BCApplications",
                column: "BuyerUserID");

            migrationBuilder.CreateIndex(
                name: "IX_BCApplications_LoanDecision_BuyerUserID",
                table: "BCApplications",
                column: "LoanDecision_BuyerUserID");

            migrationBuilder.CreateIndex(
                name: "IX_BCApplications_PermitApplication_PropertyID",
                table: "BCApplications",
                column: "PermitApplication_PropertyID");

            migrationBuilder.CreateIndex(
                name: "IX_Logins_UserID",
                table: "Logins",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_BuildingDesignBCFileID",
                table: "Properties",
                column: "BuildingDesignBCFileID");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_SellerUserID",
                table: "Properties",
                column: "SellerUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BCApplications");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
