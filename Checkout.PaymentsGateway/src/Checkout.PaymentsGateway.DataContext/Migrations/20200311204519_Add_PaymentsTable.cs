using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Checkout.PaymentsGateway.DataContext.Migrations
{
    public partial class Add_PaymentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Payments",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    CardNumber = table.Column<string>(nullable: false, maxLength: 200),
                    CardName = table.Column<string>(nullable: false, maxLength: 200),
                    CardExpirationDate = table.Column<DateTime>(nullable: false),
                    Cvv = table.Column<string>(nullable: false, maxLength: 3),
                    Amount = table.Column<decimal>(nullable: false),
                    Currency = table.Column<string>(nullable: false, maxLength: 3),
                    TransactionStatus = table.Column<string>(nullable: false, maxLength: 100),
                    TransactionDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Payments", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Payments");
        }
    }
}