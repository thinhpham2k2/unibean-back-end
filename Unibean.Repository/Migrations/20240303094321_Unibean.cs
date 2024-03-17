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
                name: "tbl_account",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    role = table.Column<string>(type: "enum('Admin', 'Staff', 'Brand', 'Store', 'Student')", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone = table.Column<string>(type: "char(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(320)", maxLength: 320, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avatar = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file_name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_verify = table.Column<ulong>(type: "bit(1)", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_verified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_account", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_area",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    area_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file_name = table.Column<string>(type: "text", nullable: true)
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
                    file_name = table.Column<string>(type: "text", nullable: true)
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
                    file_name = table.Column<string>(type: "text", nullable: true)
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
                name: "tbl_challenge",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type = table.Column<string>(type: "enum('Verify', 'Welcome', 'Spread', 'Consume')", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    challenge_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    condition = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file_name = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_tbl_challenge", x => x.id);
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
                    file_name = table.Column<string>(type: "text", nullable: true)
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
                    file_name = table.Column<string>(type: "text", nullable: true)
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
                    state = table.Column<string>(type: "enum('Active', 'Inactive', 'Closed')", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_station", x => x.id);
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
                    file_name = table.Column<string>(type: "text", nullable: true)
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
                name: "tbl_voucher_type",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file_name = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_tbl_voucher_type", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_admin",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    account_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    full_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_admin", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_admin_tbl_account_account_id",
                        column: x => x.account_id,
                        principalTable: "tbl_account",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_brand",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    account_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    brand_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    acronym = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cover_photo = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cover_file_name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    link = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    opening_hours = table.Column<TimeOnly>(type: "time(6)", nullable: true),
                    closing_hours = table.Column<TimeOnly>(type: "time(6)", nullable: true),
                    total_income = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    total_spending = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_brand", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_brand_tbl_account_account_id",
                        column: x => x.account_id,
                        principalTable: "tbl_account",
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_staff",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    station_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    account_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    full_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_staff", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_staff_tbl_account_account_id",
                        column: x => x.account_id,
                        principalTable: "tbl_account",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_staff_tbl_station_station_id",
                        column: x => x.station_id,
                        principalTable: "tbl_station",
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
                    address = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone = table.Column<string>(type: "char(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(320)", maxLength: 320, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    link = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file_name = table.Column<string>(type: "text", nullable: true)
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
                name: "tbl_campaign",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    brand_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    campaign_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image_name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file_name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    condition = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    link = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_on = table.Column<DateOnly>(type: "date", nullable: true),
                    end_on = table.Column<DateOnly>(type: "date", nullable: true),
                    duration = table.Column<int>(type: "int", nullable: true),
                    total_income = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    total_spending = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_campaign", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_campaign_tbl_brand_brand_id",
                        column: x => x.brand_id,
                        principalTable: "tbl_brand",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_campaign_tbl_campaign_type_type_id",
                        column: x => x.type_id,
                        principalTable: "tbl_campaign_type",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_request",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    brand_id = table.Column<string>(type: "char(26)", nullable: true)
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
                        name: "FK_tbl_request_tbl_brand_brand_id",
                        column: x => x.brand_id,
                        principalTable: "tbl_brand",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_store",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    brand_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    area_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    account_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    store_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    opening_hours = table.Column<TimeOnly>(type: "time(6)", nullable: true),
                    closing_hours = table.Column<TimeOnly>(type: "time(6)", nullable: true),
                    file = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file_name = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_tbl_store", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_store_tbl_account_account_id",
                        column: x => x.account_id,
                        principalTable: "tbl_account",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_store_tbl_area_area_id",
                        column: x => x.area_id,
                        principalTable: "tbl_area",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_store_tbl_brand_brand_id",
                        column: x => x.brand_id,
                        principalTable: "tbl_brand",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_voucher",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    brand_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    voucher_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    price = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    rate = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    condition = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image_name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file_name = table.Column<string>(type: "text", nullable: true)
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
                        name: "FK_tbl_voucher_tbl_brand_brand_id",
                        column: x => x.brand_id,
                        principalTable: "tbl_brand",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_voucher_tbl_voucher_type_type_id",
                        column: x => x.type_id,
                        principalTable: "tbl_voucher_type",
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
                    file_name = table.Column<string>(type: "text", nullable: true)
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
                name: "tbl_student",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    major_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    campus_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    account_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    student_card_front = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file_name_front = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    student_card_back = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file_name_back = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    full_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    code = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    gender = table.Column<string>(type: "enum('Female', 'Male')", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    total_income = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    total_spending = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    state = table.Column<string>(type: "enum('Pending', 'Active', 'Inactive', 'Rejected')", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_student", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_student_tbl_account_account_id",
                        column: x => x.account_id,
                        principalTable: "tbl_account",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_student_tbl_campus_campus_id",
                        column: x => x.campus_id,
                        principalTable: "tbl_campus",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_student_tbl_major_major_id",
                        column: x => x.major_id,
                        principalTable: "tbl_major",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_campaign_activity",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    campaign_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<string>(type: "enum('Pending', 'Rejected', 'Active', 'Inactive', 'Finished', 'Closed', 'Cancelled')", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_campaign_activity", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_campaign_activity_tbl_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "tbl_campaign",
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
                name: "tbl_campaign_detail",
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
                    quantity = table.Column<int>(type: "int", nullable: true),
                    from_index = table.Column<int>(type: "int", nullable: true),
                    to_index = table.Column<int>(type: "int", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_campaign_detail", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_campaign_detail_tbl_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "tbl_campaign",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_campaign_detail_tbl_voucher_voucher_id",
                        column: x => x.voucher_id,
                        principalTable: "tbl_voucher",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_bonus",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    brand_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    student_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    store_id = table.Column<string>(type: "char(26)", nullable: true)
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
                    table.PrimaryKey("PK_tbl_bonus", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_bonus_tbl_brand_brand_id",
                        column: x => x.brand_id,
                        principalTable: "tbl_brand",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_bonus_tbl_store_store_id",
                        column: x => x.store_id,
                        principalTable: "tbl_store",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_bonus_tbl_student_student_id",
                        column: x => x.student_id,
                        principalTable: "tbl_student",
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
                    brand_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type = table.Column<string>(type: "enum('Green', 'Red')", nullable: true)
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
                        name: "FK_tbl_wallet_tbl_brand_brand_id",
                        column: x => x.brand_id,
                        principalTable: "tbl_brand",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_wallet_tbl_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "tbl_campaign",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_wallet_tbl_student_student_id",
                        column: x => x.student_id,
                        principalTable: "tbl_student",
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
                    brand_id = table.Column<string>(type: "char(26)", nullable: true)
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
                        name: "FK_tbl_wishlist_tbl_brand_brand_id",
                        column: x => x.brand_id,
                        principalTable: "tbl_brand",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_wishlist_tbl_student_student_id",
                        column: x => x.student_id,
                        principalTable: "tbl_student",
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
                    campaign_detail_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    voucher_code = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    index = table.Column<int>(type: "int", nullable: true),
                    is_locked = table.Column<ulong>(type: "bit(1)", nullable: true),
                    is_bought = table.Column<ulong>(type: "bit(1)", nullable: true),
                    is_used = table.Column<ulong>(type: "bit(1)", nullable: true),
                    valid_on = table.Column<DateOnly>(type: "date", nullable: true),
                    expire_on = table.Column<DateOnly>(type: "date", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    date_issued = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_voucher_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_voucher_item_tbl_campaign_detail_campaign_detail_id",
                        column: x => x.campaign_detail_id,
                        principalTable: "tbl_campaign_detail",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_voucher_item_tbl_voucher_voucher_id",
                        column: x => x.voucher_id,
                        principalTable: "tbl_voucher",
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
                    state = table.Column<string>(type: "enum('Order', 'Confirmation', 'Preparation', 'Arrival', 'Receipt', 'Abort')", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_bonus_transaction",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    wallet_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bonus_id = table.Column<string>(type: "char(26)", nullable: true)
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
                    table.PrimaryKey("PK_tbl_bonus_transaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_bonus_transaction_tbl_bonus_bonus_id",
                        column: x => x.bonus_id,
                        principalTable: "tbl_bonus",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_bonus_transaction_tbl_wallet_wallet_id",
                        column: x => x.wallet_id,
                        principalTable: "tbl_wallet",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_campaign_transaction",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    campaign_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    wallet_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    rate = table.Column<decimal>(type: "decimal(38,2)", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<ulong>(type: "bit(1)", nullable: true),
                    status = table.Column<ulong>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_campaign_transaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_campaign_transaction_tbl_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "tbl_campaign",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_campaign_transaction_tbl_wallet_wallet_id",
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
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
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
                name: "tbl_activity",
                columns: table => new
                {
                    id = table.Column<string>(type: "char(26)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    store_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    student_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    voucher_item_id = table.Column<string>(type: "char(26)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type = table.Column<string>(type: "enum('Buy', 'Use', 'Refund')", nullable: true)
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
                        name: "FK_tbl_activity_tbl_voucher_item_voucher_item_id",
                        column: x => x.voucher_item_id,
                        principalTable: "tbl_voucher_item",
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

            migrationBuilder.InsertData(
                table: "tbl_challenge",
                columns: new[] { "id", "amount", "challenge_name", "condition", "date_created", "date_updated", "description", "file_name", "image", "state", "status", "type" },
                values: new object[,]
                {
                    { "01HR1V4KY2PSZ2970BGMZ00YA1", 100000m, "Xác nhận tài khoản sinh viên", 1m, new DateTime(2024, 3, 3, 16, 43, 21, 282, DateTimeKind.Local).AddTicks(7422), new DateTime(2024, 3, 3, 16, 43, 21, 282, DateTimeKind.Local).AddTicks(7443), "Tài khoản sinh viên đã được xác thực sẽ hoàn thành thử thách", "01HKYHZ6MVH2JKG32NX15K3CDG", "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HKYHZ6MVH2JKG32NX15K3CDG?alt=media&token=17dfbee0-8324-4dcd-9f73-320436766c5", 1ul, 1ul, "Verify" },
                    { "01HR1V4KYBWFPAV12XS8GJ2T9Y", 50000m, "Đồng cam cộng hưởng", 1m, new DateTime(2024, 3, 3, 16, 43, 21, 291, DateTimeKind.Local).AddTicks(2428), new DateTime(2024, 3, 3, 16, 43, 21, 291, DateTimeKind.Local).AddTicks(2451), "Nhập mã giới thiệu của bạn bè để hoàn thành thử thách", "01HKYJ4PDDV2QZJCFZDDEVQ0PJ", "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HKYJ4PDDV2QZJCFZDDEVQ0PJ?alt=media&token=7397417a-2a73-47af-968a-9197cceaf43c", 1ul, 1ul, "Welcome" },
                    { "01HR1V4KYVRJEQPVMVZWZ0X83M", 10000m, "Mời bạn cùng vui 1", 1m, new DateTime(2024, 3, 3, 16, 43, 21, 307, DateTimeKind.Local).AddTicks(2541), new DateTime(2024, 3, 3, 16, 43, 21, 307, DateTimeKind.Local).AddTicks(2548), "Được 1 tài khoản nhập mã giới thiệu để hoàn thành thử thách", "01HKYJFX60DHMR6FRY4532VSX7", "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HKYJFX60DHMR6FRY4532VSX7?alt=media&token=d86a619f-01ac-4a0a-941d-95f7bd64fa40", 1ul, 1ul, "Spread" },
                    { "01HR1V4KZAT92G99VKS09JRAVY", 100000m, "Mời bạn cùng vui 10", 10m, new DateTime(2024, 3, 3, 16, 43, 21, 322, DateTimeKind.Local).AddTicks(7863), new DateTime(2024, 3, 3, 16, 43, 21, 322, DateTimeKind.Local).AddTicks(7875), "Được 10 tài khoản nhập mã giới thiệu để hoàn thành thử thách", "01HKYJFX60DHMR6FRY4532VSX7", "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HKYJFX60DHMR6FRY4532VSX7?alt=media&token=d86a619f-01ac-4a0a-941d-95f7bd64fa40", 1ul, 1ul, "Spread" },
                    { "01HR1V4KZT2VTJJVSHTS2B78NF", 300000m, "Mời bạn cùng vui 30", 30m, new DateTime(2024, 3, 3, 16, 43, 21, 338, DateTimeKind.Local).AddTicks(6650), new DateTime(2024, 3, 3, 16, 43, 21, 339, DateTimeKind.Local).AddTicks(6810), "Được 30 tài khoản nhập mã giới thiệu để hoàn thành thử thách", "01HKYJFX60DHMR6FRY4532VSX7", "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HKYJFX60DHMR6FRY4532VSX7?alt=media&token=d86a619f-01ac-4a0a-941d-95f7bd64fa40", 1ul, 1ul, "Spread" },
                    { "01HR1V4M0ATJW3T329DGA4DZ0W", 500000m, "Mời bạn cùng vui 50", 50m, new DateTime(2024, 3, 3, 16, 43, 21, 354, DateTimeKind.Local).AddTicks(6390), new DateTime(2024, 3, 3, 16, 43, 21, 354, DateTimeKind.Local).AddTicks(6397), "Được 50 tài khoản nhập mã giới thiệu để hoàn thành thử thách", "01HKYJFX60DHMR6FRY4532VSX7", "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HKYJFX60DHMR6FRY4532VSX7?alt=media&token=d86a619f-01ac-4a0a-941d-95f7bd64fa40", 1ul, 1ul, "Spread" },
                    { "01HR1V4M0T6HTW7HR7F1C1XX8N", 1000000m, "Mời bạn cùng vui 100", 100m, new DateTime(2024, 3, 3, 16, 43, 21, 370, DateTimeKind.Local).AddTicks(6481), new DateTime(2024, 3, 3, 16, 43, 21, 370, DateTimeKind.Local).AddTicks(6487), "Được 100 tài khoản nhập mã giới thiệu để hoàn thành thử thách", "01HKYJFX60DHMR6FRY4532VSX7", "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HKYJFX60DHMR6FRY4532VSX7?alt=media&token=d86a619f-01ac-4a0a-941d-95f7bd64fa40", 1ul, 1ul, "Spread" },
                    { "01HR1V4M1AA3AAWSF1970X9ZK9", 10000000m, "Mời bạn cùng vui 1000", 1000m, new DateTime(2024, 3, 3, 16, 43, 21, 386, DateTimeKind.Local).AddTicks(6448), new DateTime(2024, 3, 3, 16, 43, 21, 386, DateTimeKind.Local).AddTicks(6460), "Được 1000 tài khoản nhập mã giới thiệu để hoàn thành thử thách", "01HKYJFX60DHMR6FRY4532VSX7", "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HKYJFX60DHMR6FRY4532VSX7?alt=media&token=d86a619f-01ac-4a0a-941d-95f7bd64fa40", 1ul, 1ul, "Spread" },
                    { "01HR1V4M1TEWDTF894P6572Q7S", 100000000m, "Mời bạn cùng vui 10000", 10000m, new DateTime(2024, 3, 3, 16, 43, 21, 402, DateTimeKind.Local).AddTicks(6483), new DateTime(2024, 3, 3, 16, 43, 21, 402, DateTimeKind.Local).AddTicks(6488), "Được 10000 tài khoản nhập mã giới thiệu để hoàn thành thử thách", "01HKYJFX60DHMR6FRY4532VSX7", "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HKYJFX60DHMR6FRY4532VSX7?alt=media&token=d86a619f-01ac-4a0a-941d-95f7bd64fa40", 1ul, 1ul, "Spread" },
                    { "01HR1V4M2A3V4FV1SX5T629D7G", 1000m, "Tiền tiêu như nước 1", 10000m, new DateTime(2024, 3, 3, 16, 43, 21, 418, DateTimeKind.Local).AddTicks(6405), new DateTime(2024, 3, 3, 16, 43, 21, 418, DateTimeKind.Local).AddTicks(6417), "Tích trữ tiêu thụ 10000 đậu xanh từ việc mua khuyến mãi để hoàn thành thử thách", "01HNM20GGS9Q5QPJQF4N29Y10F.png", "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HNM20GGS9Q5QPJQF4N29Y10F.png?alt=media&token=62c5862e-5ee5-4747-b431-f15a3f3ee93c", 1ul, 1ul, "Consume" },
                    { "01HR1V4M2T84354502GHK520PK", 10000m, "Tiền tiêu như nước 2", 100000m, new DateTime(2024, 3, 3, 16, 43, 21, 434, DateTimeKind.Local).AddTicks(6587), new DateTime(2024, 3, 3, 16, 43, 21, 434, DateTimeKind.Local).AddTicks(6599), "Tích trữ tiêu thụ 100000 đậu xanh từ việc mua khuyến mãi để hoàn thành thử thách", "01HNM20GGS9Q5QPJQF4N29Y10F.png", "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HNM20GGS9Q5QPJQF4N29Y10F.png?alt=media&token=62c5862e-5ee5-4747-b431-f15a3f3ee93c", 1ul, 1ul, "Consume" },
                    { "01HR1V4M3A9JWM650FZPRA3MAK", 50000m, "Tiền tiêu như nước 3", 500000m, new DateTime(2024, 3, 3, 16, 43, 21, 450, DateTimeKind.Local).AddTicks(7009), new DateTime(2024, 3, 3, 16, 43, 21, 450, DateTimeKind.Local).AddTicks(7017), "Tích trữ tiêu thụ 500000 đậu xanh từ việc mua khuyến mãi để hoàn thành thử thách", "01HNM20GGS9Q5QPJQF4N29Y10F.png", "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HNM20GGS9Q5QPJQF4N29Y10F.png?alt=media&token=62c5862e-5ee5-4747-b431-f15a3f3ee93c", 1ul, 1ul, "Consume" },
                    { "01HR1V4M3TB5VK86N3BWED0AGN", 100000m, "Tiền tiêu như nước 4", 1000000m, new DateTime(2024, 3, 3, 16, 43, 21, 466, DateTimeKind.Local).AddTicks(7011), new DateTime(2024, 3, 3, 16, 43, 21, 466, DateTimeKind.Local).AddTicks(7033), "Tích trữ tiêu thụ 1000000 đậu xanh từ việc mua khuyến mãi để hoàn thành thử thách", "01HNM20GGS9Q5QPJQF4N29Y10F.png", "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HNM20GGS9Q5QPJQF4N29Y10F.png?alt=media&token=62c5862e-5ee5-4747-b431-f15a3f3ee93c", 1ul, 1ul, "Consume" },
                    { "01HR1V4M4A6GBQ4F6KX2ZYYFC1", 1000000m, "Tiền tiêu như nước 5", 10000000m, new DateTime(2024, 3, 3, 16, 43, 21, 482, DateTimeKind.Local).AddTicks(6605), new DateTime(2024, 3, 3, 16, 43, 21, 482, DateTimeKind.Local).AddTicks(6623), "Tích trữ tiêu thụ 10000000 đậu xanh từ việc mua khuyến mãi để hoàn thành thử thách", "01HNM20GGS9Q5QPJQF4N29Y10F.png", "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HNM20GGS9Q5QPJQF4N29Y10F.png?alt=media&token=62c5862e-5ee5-4747-b431-f15a3f3ee93c", 1ul, 1ul, "Consume" },
                    { "01HR1V4M4TY2HXTXDDM8ZYPMFX", 5000000m, "Tiền tiêu như nước 6", 50000000m, new DateTime(2024, 3, 3, 16, 43, 21, 498, DateTimeKind.Local).AddTicks(4337), new DateTime(2024, 3, 3, 16, 43, 21, 498, DateTimeKind.Local).AddTicks(4353), "Tích trữ tiêu thụ 50000000 đậu xanh từ việc mua khuyến mãi để hoàn thành thử thách", "01HNM20GGS9Q5QPJQF4N29Y10F.png", "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HNM20GGS9Q5QPJQF4N29Y10F.png?alt=media&token=62c5862e-5ee5-4747-b431-f15a3f3ee93c", 1ul, 1ul, "Consume" },
                    { "01HR1V4M5AJ287R2HZFXST554J", 10000000m, "Tiền tiêu như nước 7", 100000000m, new DateTime(2024, 3, 3, 16, 43, 21, 514, DateTimeKind.Local).AddTicks(4560), new DateTime(2024, 3, 3, 16, 43, 21, 514, DateTimeKind.Local).AddTicks(4587), "Tích trữ tiêu thụ 100000000 đậu xanh từ việc mua khuyến mãi để hoàn thành thử thách", "01HNM20GGS9Q5QPJQF4N29Y10F.png", "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/challenges%2F01HNM20GGS9Q5QPJQF4N29Y10F.png?alt=media&token=62c5862e-5ee5-4747-b431-f15a3f3ee93c", 1ul, 1ul, "Consume" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_activity_store_id",
                table: "tbl_activity",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_activity_student_id",
                table: "tbl_activity",
                column: "student_id");

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
                name: "IX_tbl_admin_account_id",
                table: "tbl_admin",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_bonus_brand_id",
                table: "tbl_bonus",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_bonus_store_id",
                table: "tbl_bonus",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_bonus_student_id",
                table: "tbl_bonus",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_bonus_transaction_bonus_id",
                table: "tbl_bonus_transaction",
                column: "bonus_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_bonus_transaction_wallet_id",
                table: "tbl_bonus_transaction",
                column: "wallet_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_brand_account_id",
                table: "tbl_brand",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campaign_brand_id",
                table: "tbl_campaign",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campaign_type_id",
                table: "tbl_campaign",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campaign_activity_campaign_id",
                table: "tbl_campaign_activity",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campaign_campus_campaign_id",
                table: "tbl_campaign_campus",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campaign_campus_campus_id",
                table: "tbl_campaign_campus",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campaign_detail_campaign_id",
                table: "tbl_campaign_detail",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campaign_detail_voucher_id",
                table: "tbl_campaign_detail",
                column: "voucher_id");

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
                name: "IX_tbl_campaign_transaction_campaign_id",
                table: "tbl_campaign_transaction",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campaign_transaction_wallet_id",
                table: "tbl_campaign_transaction",
                column: "wallet_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campus_area_id",
                table: "tbl_campus",
                column: "area_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_campus_university_id",
                table: "tbl_campus",
                column: "university_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_challenge_transaction_challenge_id",
                table: "tbl_challenge_transaction",
                column: "challenge_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_challenge_transaction_wallet_id",
                table: "tbl_challenge_transaction",
                column: "wallet_id");

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
                name: "IX_tbl_order_transaction_order_id",
                table: "tbl_order_transaction",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_order_transaction_wallet_id",
                table: "tbl_order_transaction",
                column: "wallet_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_category_id",
                table: "tbl_product",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_request_admin_id",
                table: "tbl_request",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_request_brand_id",
                table: "tbl_request",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_request_transaction_request_id",
                table: "tbl_request_transaction",
                column: "request_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_request_transaction_wallet_id",
                table: "tbl_request_transaction",
                column: "wallet_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_staff_account_id",
                table: "tbl_staff",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_staff_station_id",
                table: "tbl_staff",
                column: "station_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_store_account_id",
                table: "tbl_store",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_store_area_id",
                table: "tbl_store",
                column: "area_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_store_brand_id",
                table: "tbl_store",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_student_account_id",
                table: "tbl_student",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_student_campus_id",
                table: "tbl_student",
                column: "campus_id");

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
                name: "IX_tbl_voucher_brand_id",
                table: "tbl_voucher",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_voucher_type_id",
                table: "tbl_voucher",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_voucher_item_campaign_detail_id",
                table: "tbl_voucher_item",
                column: "campaign_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_voucher_item_voucher_id",
                table: "tbl_voucher_item",
                column: "voucher_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_wallet_brand_id",
                table: "tbl_wallet",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_wallet_campaign_id",
                table: "tbl_wallet",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_wallet_student_id",
                table: "tbl_wallet",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_wishlist_brand_id",
                table: "tbl_wishlist",
                column: "brand_id");

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
                name: "tbl_bonus_transaction");

            migrationBuilder.DropTable(
                name: "tbl_campaign_activity");

            migrationBuilder.DropTable(
                name: "tbl_campaign_campus");

            migrationBuilder.DropTable(
                name: "tbl_campaign_major");

            migrationBuilder.DropTable(
                name: "tbl_campaign_store");

            migrationBuilder.DropTable(
                name: "tbl_campaign_transaction");

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
                name: "tbl_request_transaction");

            migrationBuilder.DropTable(
                name: "tbl_staff");

            migrationBuilder.DropTable(
                name: "tbl_wishlist");

            migrationBuilder.DropTable(
                name: "tbl_activity");

            migrationBuilder.DropTable(
                name: "tbl_bonus");

            migrationBuilder.DropTable(
                name: "tbl_student_challenge");

            migrationBuilder.DropTable(
                name: "tbl_product");

            migrationBuilder.DropTable(
                name: "tbl_order");

            migrationBuilder.DropTable(
                name: "tbl_request");

            migrationBuilder.DropTable(
                name: "tbl_wallet");

            migrationBuilder.DropTable(
                name: "tbl_voucher_item");

            migrationBuilder.DropTable(
                name: "tbl_store");

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
                name: "tbl_campaign_detail");

            migrationBuilder.DropTable(
                name: "tbl_campus");

            migrationBuilder.DropTable(
                name: "tbl_major");

            migrationBuilder.DropTable(
                name: "tbl_campaign");

            migrationBuilder.DropTable(
                name: "tbl_voucher");

            migrationBuilder.DropTable(
                name: "tbl_area");

            migrationBuilder.DropTable(
                name: "tbl_university");

            migrationBuilder.DropTable(
                name: "tbl_campaign_type");

            migrationBuilder.DropTable(
                name: "tbl_brand");

            migrationBuilder.DropTable(
                name: "tbl_voucher_type");

            migrationBuilder.DropTable(
                name: "tbl_account");
        }
    }
}
