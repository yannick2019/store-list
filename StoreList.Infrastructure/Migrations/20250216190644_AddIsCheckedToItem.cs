﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryList.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCheckedToItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsChecked",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChecked",
                table: "Items");
        }
    }
}
