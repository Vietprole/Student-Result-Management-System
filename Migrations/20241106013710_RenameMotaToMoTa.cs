﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Student_Result_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class RenameMotaToMoTa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Mota",
                table: "PLOs",
                newName: "MoTa");

            migrationBuilder.RenameColumn(
                name: "Mota",
                table: "CLOs",
                newName: "MoTa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MoTa",
                table: "PLOs",
                newName: "Mota");

            migrationBuilder.RenameColumn(
                name: "MoTa",
                table: "CLOs",
                newName: "Mota");
        }
    }
}