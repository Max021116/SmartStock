using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStock.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddReportingView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
        CREATE VIEW vw_SalesSummaryByProduct AS
        SELECT
            p.Id AS ProductId,
            p.Name AS ProductName,
            SUM(soi.Quantity) AS TotalQuantitySold,
            SUM(soi.LineTotal) AS TotalRevenue
        FROM SalesOrderItems AS soi
        INNER JOIN Products AS p ON soi.ProductId = p.Id
        INNER JOIN SalesOrders AS so ON soi.SalesOrderId = so.Id
        WHERE p.IsDeleted = 0
          AND so.Status <> 'Cancelled'
        GROUP BY p.Id, p.Name
        """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_SalesSummaryByProduct");
        }
    }
}
