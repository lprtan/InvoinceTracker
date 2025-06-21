using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoinceModule.Migrations
{
    /// <inheritdoc />
    public partial class updateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiverTitle",
                table: "InvoiceHeaders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderTitle",
                table: "InvoiceHeaders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverTitle",
                table: "InvoiceHeaders");

            migrationBuilder.DropColumn(
                name: "SenderTitle",
                table: "InvoiceHeaders");
        }
    }
}
