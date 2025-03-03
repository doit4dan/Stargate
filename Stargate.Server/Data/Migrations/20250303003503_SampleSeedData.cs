using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Stargate.Server.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class SampleSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AstronautDetail",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CareerEndDate", "CareerStartDate", "CurrentDutyTitle" },
                values: new object[] { new DateTime(1971, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1962, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "RETIRED" });

            migrationBuilder.InsertData(
                table: "AstronautDetail",
                columns: new[] { "Id", "CareerEndDate", "CareerStartDate", "CurrentDutyTitle", "CurrentRank", "PersonId" },
                values: new object[] { 2, null, new DateTime(2004, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Commander", "LTCOL", 2 });

            migrationBuilder.UpdateData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DutyEndDate", "DutyStartDate", "DutyTitle", "Rank" },
                values: new object[] { new DateTime(1962, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1962, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mission Specialist", "SPC1" });

            migrationBuilder.InsertData(
                table: "AstronautDuty",
                columns: new[] { "Id", "DutyEndDate", "DutyStartDate", "DutyTitle", "PersonId", "Rank" },
                values: new object[,]
                {
                    { 2, new DateTime(1964, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1963, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mission Specialist", 1, "SPC2" },
                    { 3, new DateTime(1966, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1965, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pilot", 1, "SGT" },
                    { 4, new DateTime(1967, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1967, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Commander", 1, "2LT" },
                    { 5, new DateTime(1971, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1968, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Commander", 1, "1LT" },
                    { 6, null, new DateTime(1971, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "RETIRED", 1, "1LT" },
                    { 7, new DateTime(2005, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2004, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Flight Engineer", 2, "SPC2" },
                    { 8, new DateTime(2007, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2006, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Flight Engineer", 2, "SPC4" },
                    { 9, new DateTime(2009, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2008, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pilot", 2, "SGT" },
                    { 10, new DateTime(2013, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Command Pilot", 2, "MSGT" },
                    { 11, new DateTime(2015, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2014, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Commander", 2, "2LT" },
                    { 12, new DateTime(2017, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Commander", 2, "1LT" },
                    { 13, new DateTime(2021, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2018, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Commander", 2, "CAPT" },
                    { 14, new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Commander", 2, "MAJ" },
                    { 15, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Commander", 2, "LTCOL" }
                });

            migrationBuilder.UpdateData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Joseph Acaba");

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 3, "Deniz Burnham" },
                    { 4, "Zena Cardman" },
                    { 5, "Christopher Cassidy" },
                    { 6, "Raja Chari" },
                    { 7, "Dan Carson" },
                    { 8, "Dallas Davis" }
                });

            migrationBuilder.InsertData(
                table: "AstronautDetail",
                columns: new[] { "Id", "CareerEndDate", "CareerStartDate", "CurrentDutyTitle", "CurrentRank", "PersonId" },
                values: new object[,]
                {
                    { 3, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mission Specialist", "SPC4", 3 },
                    { 4, null, new DateTime(2017, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Command Pilot", "SGT", 4 },
                    { 5, new DateTime(2011, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2004, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "RETIRED", "SPC2", 5 },
                    { 6, null, new DateTime(2017, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pilot", "SPC4", 6 }
                });

            migrationBuilder.InsertData(
                table: "AstronautDuty",
                columns: new[] { "Id", "DutyEndDate", "DutyStartDate", "DutyTitle", "PersonId", "Rank" },
                values: new object[,]
                {
                    { 16, new DateTime(2022, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mission Specialist", 3, "SPC2" },
                    { 17, new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mission Specialist", 3, "SPC3" },
                    { 18, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mission Specialist", 3, "SPC4" },
                    { 19, new DateTime(2018, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2017, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mission Specialist", 4, "SPC1" },
                    { 20, new DateTime(2020, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mission Specialist", 4, "SPC3" },
                    { 21, new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pilot", 4, "SPC4" },
                    { 22, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Command Pilot", 4, "SGT" },
                    { 23, new DateTime(2009, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2004, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Flight Engineer", 5, "SPC1" },
                    { 24, new DateTime(2011, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Flight Engineer", 5, "SPC2" },
                    { 25, null, new DateTime(2012, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "RETIRED", 5, "SPC2" },
                    { 26, new DateTime(2019, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2017, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mission Specialist", 6, "SPC1" },
                    { 27, new DateTime(2021, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mission Specialist", 6, "SPC2" },
                    { 28, null, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pilot", 6, "SPC4" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AstronautDetail",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AstronautDetail",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AstronautDetail",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AstronautDetail",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AstronautDetail",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "AstronautDetail",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CareerEndDate", "CareerStartDate", "CurrentDutyTitle" },
                values: new object[] { null, new DateTime(2025, 2, 28, 22, 34, 55, 45, DateTimeKind.Local).AddTicks(2416), "Commander" });

            migrationBuilder.UpdateData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DutyEndDate", "DutyStartDate", "DutyTitle", "Rank" },
                values: new object[] { null, new DateTime(2025, 2, 28, 22, 34, 55, 45, DateTimeKind.Local).AddTicks(2473), "Commander", "1LT" });

            migrationBuilder.UpdateData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Dan Carson");
        }
    }
}
