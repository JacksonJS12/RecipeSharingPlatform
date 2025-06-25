using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeSharingPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropertyToRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SavedCount",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "57bef224-20ec-4109-9996-c83ced047d8a", "AQAAAAIAAYagAAAAEOtoiHcl5yqmNKXFy7DI7zUyEooJmQ4Sd2JfpGJolPlk+575BuQlqvnEv06gGA12bw==", "665bca5a-8739-4b6c-9a3b-9fa4d055d811" });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "SavedCount" },
                values: new object[] { new DateTime(2025, 6, 21, 11, 53, 14, 536, DateTimeKind.Local).AddTicks(2650), 0 });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "SavedCount" },
                values: new object[] { new DateTime(2025, 6, 21, 11, 53, 14, 536, DateTimeKind.Local).AddTicks(2698), 0 });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "SavedCount" },
                values: new object[] { new DateTime(2025, 6, 21, 11, 53, 14, 536, DateTimeKind.Local).AddTicks(2701), 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SavedCount",
                table: "Recipes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "08290941-78e8-47d0-ba98-7321edfce121", "AQAAAAIAAYagAAAAEHLIkAFBqySvLqIg1EHou6UDgiLZW15QjJEVGW6EGmSlIS46DkMH1GocQ5Eu1wgXIA==", "652dc987-6e0c-4d33-a4c2-c9fb39eaef92" });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2025, 6, 21, 10, 8, 15, 71, DateTimeKind.Local).AddTicks(6719));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2025, 6, 21, 10, 8, 15, 71, DateTimeKind.Local).AddTicks(6771));

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2025, 6, 21, 10, 8, 15, 71, DateTimeKind.Local).AddTicks(6773));
        }
    }
}
