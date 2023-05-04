using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class fix_insurance_petient_problem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insurances_Patients_PatientId",
                table: "Insurances");

            migrationBuilder.DropIndex(
                name: "IX_Insurances_PatientId",
                table: "Insurances");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Insurances");

            migrationBuilder.AddColumn<long>(
                name: "InsuranceId",
                table: "Patients",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_InsuranceId",
                table: "Patients",
                column: "InsuranceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Insurances_InsuranceId",
                table: "Patients",
                column: "InsuranceId",
                principalTable: "Insurances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Insurances_InsuranceId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_InsuranceId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "InsuranceId",
                table: "Patients");

            migrationBuilder.AddColumn<long>(
                name: "PatientId",
                table: "Insurances",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Insurances_PatientId",
                table: "Insurances",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Insurances_Patients_PatientId",
                table: "Insurances",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
