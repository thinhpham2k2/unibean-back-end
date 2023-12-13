﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unibean.Repository.Migrations
{
    public partial class Unibean : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_admin",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    full_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone = table.Column<string>(type: "char(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avatar = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_admin", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_campaign_type",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_campaign_type", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_category",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    category_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_category", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_challenge_type",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_challenge_type", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_city",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    city_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_city", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_gender",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    gender_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_gender", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_level",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    level_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    condition = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_level", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_major",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    major_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_major", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_partner",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    brand_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    acronym = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    logo = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cover_photo = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(320)", maxLength: 320, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone = table.Column<string>(type: "char(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    opening_hours = table.Column<TimeOnly>(type: "time(6)", nullable: true),
                    closing_hours = table.Column<TimeOnly>(type: "time(6)", nullable: true),
                    link = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    total_income = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    total_spending = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_partner", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_state",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_state", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_station",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    station_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    opening_hours = table.Column<TimeOnly>(type: "time(6)", nullable: true),
                    closing_hours = table.Column<TimeOnly>(type: "time(6)", nullable: true),
                    phone = table.Column<string>(type: "char(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(320)", maxLength: 320, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_station", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_type",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_type", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_university",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    university_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone = table.Column<string>(type: "char(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(320)", maxLength: 320, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    link = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_university", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_wallet_type",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_wallet_type", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_challenge",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    condition = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_challenge", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_challenge_tbl_challenge_type_type_id",
                        column: x => x.type_id,
                        principalTable: "tbl_challenge_type",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_district",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    city_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    district_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_district", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_district_tbl_city_city_id",
                        column: x => x.city_id,
                        principalTable: "tbl_city",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_product",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    category_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    level_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    product_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    price = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    weight = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_product_tbl_category_category_id",
                        column: x => x.category_id,
                        principalTable: "tbl_category",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_product_tbl_level_level_id",
                        column: x => x.level_id,
                        principalTable: "tbl_level",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_campaign",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    partner_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    campaign_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    condition = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    link = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_on = table.Column<DateOnly>(type: "date", nullable: true),
                    end_on = table.Column<DateOnly>(type: "date", nullable: true),
                    duration = table.Column<int>(type: "int", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_campaign", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_campaign_tbl_campaign_type_type_id",
                        column: x => x.type_id,
                        principalTable: "tbl_campaign_type",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_campaign_tbl_partner_partner_id",
                        column: x => x.partner_id,
                        principalTable: "tbl_partner",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_request",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    partner_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    admin_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_request", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_request_tbl_admin_admin_id",
                        column: x => x.admin_id,
                        principalTable: "tbl_admin",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_request_tbl_partner_partner_id",
                        column: x => x.partner_id,
                        principalTable: "tbl_partner",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_voucher",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    partner_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    voucher_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    price = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    rate = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_voucher", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_voucher_tbl_partner_partner_id",
                        column: x => x.partner_id,
                        principalTable: "tbl_partner",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_area",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    district_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    area_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_area", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_area_tbl_district_district_id",
                        column: x => x.district_id,
                        principalTable: "tbl_district",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_image",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    product_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    url = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_cover = table.Column<ulong>(type: "bit(1)", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_image", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_image_tbl_product_product_id",
                        column: x => x.product_id,
                        principalTable: "tbl_product",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_campaign_gender",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    campaign_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    gender_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_campaign_gender", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_campaign_gender_tbl_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "tbl_campaign",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_campaign_gender_tbl_gender_gender_id",
                        column: x => x.gender_id,
                        principalTable: "tbl_gender",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_campaign_major",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    campaign_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    major_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_campaign_major", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_campaign_major_tbl_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "tbl_campaign",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_campaign_major_tbl_major_major_id",
                        column: x => x.major_id,
                        principalTable: "tbl_major",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_voucher_item",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    voucher_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    campaign_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    price = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    rate = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    is_bought = table.Column<ulong>(type: "bit(1)", nullable: true),
                    is_used = table.Column<ulong>(type: "bit(1)", nullable: true),
                    valid_on = table.Column<DateOnly>(type: "date", nullable: true),
                    expire_on = table.Column<DateOnly>(type: "date", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_voucher_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_voucher_item_tbl_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "tbl_campaign",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_voucher_item_tbl_voucher_voucher_id",
                        column: x => x.voucher_id,
                        principalTable: "tbl_voucher",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_campus",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    university_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    area_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    campus_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    opening_hours = table.Column<TimeOnly>(type: "time(6)", nullable: true),
                    closing_hours = table.Column<TimeOnly>(type: "time(6)", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone = table.Column<string>(type: "char(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(320)", maxLength: 320, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    link = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_campus", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_campus_tbl_area_area_id",
                        column: x => x.area_id,
                        principalTable: "tbl_area",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_campus_tbl_university_university_id",
                        column: x => x.university_id,
                        principalTable: "tbl_university",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_store",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    partner_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    area_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    store_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(320)", maxLength: 320, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone = table.Column<string>(type: "char(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    opening_hours = table.Column<TimeOnly>(type: "time(6)", nullable: true),
                    closing_hours = table.Column<TimeOnly>(type: "time(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_store", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_store_tbl_area_area_id",
                        column: x => x.area_id,
                        principalTable: "tbl_area",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_store_tbl_partner_partner_id",
                        column: x => x.partner_id,
                        principalTable: "tbl_partner",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_campaign_campus",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    campaign_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    campus_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_campaign_campus", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_campaign_campus_tbl_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "tbl_campaign",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_campaign_campus_tbl_campus_campus_id",
                        column: x => x.campus_id,
                        principalTable: "tbl_campus",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_student",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    level_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    gender_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    major_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    campus_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    student_card = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    full_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    code = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(320)", maxLength: 320, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    phone = table.Column<string>(type: "char(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avatar = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    total_income = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    total_spending = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_verified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    is_verify = table.Column<ulong>(type: "bit(1)", nullable: true),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_student", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_student_tbl_campus_campus_id",
                        column: x => x.campus_id,
                        principalTable: "tbl_campus",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_student_tbl_gender_gender_id",
                        column: x => x.gender_id,
                        principalTable: "tbl_gender",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_student_tbl_level_level_id",
                        column: x => x.level_id,
                        principalTable: "tbl_level",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_student_tbl_major_major_id",
                        column: x => x.major_id,
                        principalTable: "tbl_major",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_campaign_store",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    campaign_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    store_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_campaign_store", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_campaign_store_tbl_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "tbl_campaign",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_campaign_store_tbl_store_store_id",
                        column: x => x.store_id,
                        principalTable: "tbl_store",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_activity",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    store_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    student_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    voucher_item_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_activity", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_activity_tbl_store_store_id",
                        column: x => x.store_id,
                        principalTable: "tbl_store",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_activity_tbl_student_student_id",
                        column: x => x.student_id,
                        principalTable: "tbl_student",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_activity_tbl_type_type_id",
                        column: x => x.type_id,
                        principalTable: "tbl_type",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_activity_tbl_voucher_item_voucher_item_id",
                        column: x => x.voucher_item_id,
                        principalTable: "tbl_voucher_item",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_invitation",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    inviter_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    invitee_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_invitation", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_invitation_tbl_student_invitee_id",
                        column: x => x.invitee_id,
                        principalTable: "tbl_student",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_invitation_tbl_student_inviter_id",
                        column: x => x.inviter_id,
                        principalTable: "tbl_student",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_order",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    student_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    station_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_order", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_order_tbl_station_station_id",
                        column: x => x.station_id,
                        principalTable: "tbl_station",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_order_tbl_student_student_id",
                        column: x => x.student_id,
                        principalTable: "tbl_student",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_payment",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    student_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    token = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    method = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    message = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_payment", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_payment_tbl_student_student_id",
                        column: x => x.student_id,
                        principalTable: "tbl_student",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_student_challenge",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    challenge_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    student_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    current = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    condition = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    is_completed = table.Column<ulong>(type: "bit(1)", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_student_challenge", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_student_challenge_tbl_challenge_challenge_id",
                        column: x => x.challenge_id,
                        principalTable: "tbl_challenge",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_student_challenge_tbl_student_student_id",
                        column: x => x.student_id,
                        principalTable: "tbl_student",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_wallet",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    campaign_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    student_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    partner_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    balance = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_wallet", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_wallet_tbl_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "tbl_campaign",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_wallet_tbl_partner_partner_id",
                        column: x => x.partner_id,
                        principalTable: "tbl_partner",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_wallet_tbl_student_student_id",
                        column: x => x.student_id,
                        principalTable: "tbl_student",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_wallet_tbl_wallet_type_type_id",
                        column: x => x.type_id,
                        principalTable: "tbl_wallet_type",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_wishlist",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    student_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    partner_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_wishlist", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_wishlist_tbl_partner_partner_id",
                        column: x => x.partner_id,
                        principalTable: "tbl_partner",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_wishlist_tbl_student_student_id",
                        column: x => x.student_id,
                        principalTable: "tbl_student",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_order_detail",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    product_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    order_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    price = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    amount = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_order_detail", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_order_detail_tbl_order_order_id",
                        column: x => x.order_id,
                        principalTable: "tbl_order",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_order_detail_tbl_product_product_id",
                        column: x => x.product_id,
                        principalTable: "tbl_product",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_order_state",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    order_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_order_state", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_order_state_tbl_order_order_id",
                        column: x => x.order_id,
                        principalTable: "tbl_order",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_order_state_tbl_state_state_id",
                        column: x => x.state_id,
                        principalTable: "tbl_state",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_activity_transaction",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    activity_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    wallet_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    rate = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_activity_transaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_activity_transaction_tbl_activity_activity_id",
                        column: x => x.activity_id,
                        principalTable: "tbl_activity",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_activity_transaction_tbl_wallet_wallet_id",
                        column: x => x.wallet_id,
                        principalTable: "tbl_wallet",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_challenge_transaction",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    wallet_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    challenge_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    rate = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_challenge_transaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_challenge_transaction_tbl_student_challenge_challenge_id",
                        column: x => x.challenge_id,
                        principalTable: "tbl_student_challenge",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_challenge_transaction_tbl_wallet_wallet_id",
                        column: x => x.wallet_id,
                        principalTable: "tbl_wallet",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_order_transaction",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    order_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    wallet_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    rate = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_order_transaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_order_transaction_tbl_order_order_id",
                        column: x => x.order_id,
                        principalTable: "tbl_order",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_order_transaction_tbl_wallet_wallet_id",
                        column: x => x.wallet_id,
                        principalTable: "tbl_wallet",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_payment_transaction",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    wallet_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    payment_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    rate = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_payment_transaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_payment_transaction_tbl_payment_payment_id",
                        column: x => x.payment_id,
                        principalTable: "tbl_payment",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_payment_transaction_tbl_wallet_wallet_id",
                        column: x => x.wallet_id,
                        principalTable: "tbl_wallet",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_request_transaction",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    wallet_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    request_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    rate = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_request_transaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_request_transaction_tbl_request_request_id",
                        column: x => x.request_id,
                        principalTable: "tbl_request",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_request_transaction_tbl_wallet_wallet_id",
                        column: x => x.wallet_id,
                        principalTable: "tbl_wallet",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_wallet_transaction",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    wallet_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    campaign_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    rate = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_wallet_transaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_wallet_transaction_tbl_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "tbl_campaign",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_wallet_transaction_tbl_wallet_wallet_id",
                        column: x => x.wallet_id,
                        principalTable: "tbl_wallet",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_activity_store_id",
                table: "tbl_activity",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_activity_student_id",
                table: "tbl_activity",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_activity_type_id",
                table: "tbl_activity",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_activity_voucher_item_id",
                table: "tbl_activity",
                column: "voucher_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_activity_transaction_activity_id",
                table: "tbl_activity_transaction",
                column: "activity_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_activity_transaction_wallet_id",
                table: "tbl_activity_transaction",
                column: "wallet_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_area_district_id",
                table: "tbl_area",
                column: "district_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campaign_partner_id",
                table: "tbl_campaign",
                column: "partner_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campaign_type_id",
                table: "tbl_campaign",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campaign_campus_campaign_id",
                table: "tbl_campaign_campus",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campaign_campus_campus_id",
                table: "tbl_campaign_campus",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campaign_gender_campaign_id",
                table: "tbl_campaign_gender",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campaign_gender_gender_id",
                table: "tbl_campaign_gender",
                column: "gender_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campaign_major_campaign_id",
                table: "tbl_campaign_major",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campaign_major_major_id",
                table: "tbl_campaign_major",
                column: "major_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campaign_store_campaign_id",
                table: "tbl_campaign_store",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campaign_store_store_id",
                table: "tbl_campaign_store",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campus_area_id",
                table: "tbl_campus",
                column: "area_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campus_university_id",
                table: "tbl_campus",
                column: "university_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_challenge_type_id",
                table: "tbl_challenge",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_challenge_transaction_challenge_id",
                table: "tbl_challenge_transaction",
                column: "challenge_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_challenge_transaction_wallet_id",
                table: "tbl_challenge_transaction",
                column: "wallet_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_district_city_id",
                table: "tbl_district",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_image_product_id",
                table: "tbl_image",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_invitation_invitee_id",
                table: "tbl_invitation",
                column: "invitee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_invitation_inviter_id",
                table: "tbl_invitation",
                column: "inviter_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_order_station_id",
                table: "tbl_order",
                column: "station_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_order_student_id",
                table: "tbl_order",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_order_detail_order_id",
                table: "tbl_order_detail",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_order_detail_product_id",
                table: "tbl_order_detail",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_order_state_order_id",
                table: "tbl_order_state",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_order_state_state_id",
                table: "tbl_order_state",
                column: "state_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_order_transaction_order_id",
                table: "tbl_order_transaction",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_order_transaction_wallet_id",
                table: "tbl_order_transaction",
                column: "wallet_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_student_id",
                table: "tbl_payment",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_transaction_payment_id",
                table: "tbl_payment_transaction",
                column: "payment_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_transaction_wallet_id",
                table: "tbl_payment_transaction",
                column: "wallet_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_category_id",
                table: "tbl_product",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_level_id",
                table: "tbl_product",
                column: "level_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_request_admin_id",
                table: "tbl_request",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_request_partner_id",
                table: "tbl_request",
                column: "partner_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_request_transaction_request_id",
                table: "tbl_request_transaction",
                column: "request_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_request_transaction_wallet_id",
                table: "tbl_request_transaction",
                column: "wallet_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_store_area_id",
                table: "tbl_store",
                column: "area_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_store_partner_id",
                table: "tbl_store",
                column: "partner_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_student_campus_id",
                table: "tbl_student",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_student_gender_id",
                table: "tbl_student",
                column: "gender_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_student_level_id",
                table: "tbl_student",
                column: "level_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_student_major_id",
                table: "tbl_student",
                column: "major_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_student_challenge_challenge_id",
                table: "tbl_student_challenge",
                column: "challenge_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_student_challenge_student_id",
                table: "tbl_student_challenge",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_voucher_partner_id",
                table: "tbl_voucher",
                column: "partner_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_voucher_item_campaign_id",
                table: "tbl_voucher_item",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_voucher_item_voucher_id",
                table: "tbl_voucher_item",
                column: "voucher_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_wallet_campaign_id",
                table: "tbl_wallet",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_wallet_partner_id",
                table: "tbl_wallet",
                column: "partner_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_wallet_student_id",
                table: "tbl_wallet",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_wallet_type_id",
                table: "tbl_wallet",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_wallet_transaction_campaign_id",
                table: "tbl_wallet_transaction",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_wallet_transaction_wallet_id",
                table: "tbl_wallet_transaction",
                column: "wallet_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_wishlist_partner_id",
                table: "tbl_wishlist",
                column: "partner_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_wishlist_student_id",
                table: "tbl_wishlist",
                column: "student_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_activity_transaction");

            migrationBuilder.DropTable(
                name: "tbl_campaign_campus");

            migrationBuilder.DropTable(
                name: "tbl_campaign_gender");

            migrationBuilder.DropTable(
                name: "tbl_campaign_major");

            migrationBuilder.DropTable(
                name: "tbl_campaign_store");

            migrationBuilder.DropTable(
                name: "tbl_challenge_transaction");

            migrationBuilder.DropTable(
                name: "tbl_image");

            migrationBuilder.DropTable(
                name: "tbl_invitation");

            migrationBuilder.DropTable(
                name: "tbl_order_detail");

            migrationBuilder.DropTable(
                name: "tbl_order_state");

            migrationBuilder.DropTable(
                name: "tbl_order_transaction");

            migrationBuilder.DropTable(
                name: "tbl_payment_transaction");

            migrationBuilder.DropTable(
                name: "tbl_request_transaction");

            migrationBuilder.DropTable(
                name: "tbl_wallet_transaction");

            migrationBuilder.DropTable(
                name: "tbl_wishlist");

            migrationBuilder.DropTable(
                name: "tbl_activity");

            migrationBuilder.DropTable(
                name: "tbl_student_challenge");

            migrationBuilder.DropTable(
                name: "tbl_product");

            migrationBuilder.DropTable(
                name: "tbl_state");

            migrationBuilder.DropTable(
                name: "tbl_order");

            migrationBuilder.DropTable(
                name: "tbl_payment");

            migrationBuilder.DropTable(
                name: "tbl_request");

            migrationBuilder.DropTable(
                name: "tbl_wallet");

            migrationBuilder.DropTable(
                name: "tbl_store");

            migrationBuilder.DropTable(
                name: "tbl_type");

            migrationBuilder.DropTable(
                name: "tbl_voucher_item");

            migrationBuilder.DropTable(
                name: "tbl_challenge");

            migrationBuilder.DropTable(
                name: "tbl_category");

            migrationBuilder.DropTable(
                name: "tbl_station");

            migrationBuilder.DropTable(
                name: "tbl_admin");

            migrationBuilder.DropTable(
                name: "tbl_student");

            migrationBuilder.DropTable(
                name: "tbl_wallet_type");

            migrationBuilder.DropTable(
                name: "tbl_campaign");

            migrationBuilder.DropTable(
                name: "tbl_voucher");

            migrationBuilder.DropTable(
                name: "tbl_challenge_type");

            migrationBuilder.DropTable(
                name: "tbl_campus");

            migrationBuilder.DropTable(
                name: "tbl_gender");

            migrationBuilder.DropTable(
                name: "tbl_level");

            migrationBuilder.DropTable(
                name: "tbl_major");

            migrationBuilder.DropTable(
                name: "tbl_campaign_type");

            migrationBuilder.DropTable(
                name: "tbl_partner");

            migrationBuilder.DropTable(
                name: "tbl_area");

            migrationBuilder.DropTable(
                name: "tbl_university");

            migrationBuilder.DropTable(
                name: "tbl_district");

            migrationBuilder.DropTable(
                name: "tbl_city");
        }
    }
}
