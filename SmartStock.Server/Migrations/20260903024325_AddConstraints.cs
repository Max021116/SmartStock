using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStock.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "LineTotal",
                table: "SalesOrderItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                computedColumnSql: "[Quantity] * [UnitPrice]",
                stored: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LineTotal",
                table: "PurchaseOrderItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                computedColumnSql: "[Quantity] * [UnitCost]",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_ContactEmail",
                table: "Suppliers",
                column: "ContactEmail");

            migrationBuilder.AddCheckConstraint(
                name: "CK_StockMovements_Quantity",
                table: "StockMovements",
                sql: "[Quantity] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_SalesOrderItems_Quantity",
                table: "SalesOrderItems",
                sql: "[Quantity] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_SalesOrderItems_UnitPrice",
                table: "SalesOrderItems",
                sql: "[UnitPrice] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PurchaseOrderItems_Quantity",
                table: "PurchaseOrderItems",
                sql: "[Quantity] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PurchaseOrderItems_UnitCost",
                table: "PurchaseOrderItems",
                sql: "[UnitCost] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Products_Price",
                table: "Products",
                sql: "[Price] >= 0");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Suppliers_ContactEmail",
                table: "Suppliers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_StockMovements_Quantity",
                table: "StockMovements");

            migrationBuilder.DropCheckConstraint(
                name: "CK_SalesOrderItems_Quantity",
                table: "SalesOrderItems");

            migrationBuilder.DropCheckConstraint(
                name: "CK_SalesOrderItems_UnitPrice",
                table: "SalesOrderItems");

            migrationBuilder.DropCheckConstraint(
                name: "CK_PurchaseOrderItems_Quantity",
                table: "PurchaseOrderItems");

            migrationBuilder.DropCheckConstraint(
                name: "CK_PurchaseOrderItems_UnitCost",
                table: "PurchaseOrderItems");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Products_Price",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Customers_Email",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LineTotal",
                table: "SalesOrderItems");

            migrationBuilder.DropColumn(
                name: "LineTotal",
                table: "PurchaseOrderItems");
        }
    }
}
