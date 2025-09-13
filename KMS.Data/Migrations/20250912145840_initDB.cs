using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class initDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Application",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Tên ứng dụng"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Mã"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Mô tả"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Hình ảnh"),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Công ty"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Trạng thái"),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Mã số"),
                    Count = table.Column<int>(type: "int", nullable: false, comment: "Số thứ tự"),
                    UserIdCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Người tạo"),
                    UserIdModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Người sửa"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Application", x => x.Id);
                },
                comment: "Quản lý danh sách các ứng dụng");

            migrationBuilder.CreateTable(
                name: "AppRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserLogins",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserLogins", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AppUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRoles", x => new { x.RoleId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Tên hiển thị"),
                    PasswordSha256 = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Mật khẩu"),
                    Type = table.Column<int>(type: "int", nullable: true, comment: "Loại tài khoản"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Trạng thái"),
                    UserIdCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Người tạo"),
                    UserIdModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Người sửa"),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Ghi chú"),
                    BankNumber = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Số tài khoản"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Sinh nhật"),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Chức vụ"),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTokens", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Tên công ty"),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Mã"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Mô tả"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Hình ảnh"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Địa chỉ"),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Điện thoại"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Email"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Trạng thái"),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Mã số"),
                    Count = table.Column<int>(type: "int", nullable: false, comment: "Số thứ tự"),
                    UserIdCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Người tạo"),
                    UserIdModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Người sửa"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                },
                comment: "Quản lý danh sách công ty");

            migrationBuilder.CreateTable(
                name: "DatabaseCache",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ExpiresAtTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    SlidingExpirationInSeconds = table.Column<long>(type: "bigint", nullable: true),
                    AbsoluteExpiration = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseCache", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogAction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ObjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PropertyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schema",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Mô tả"),
                    Slug = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Ứng dụng"),
                    Database = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Thông tin DB"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Trạng thái"),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Vị trí"),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Mã số"),
                    Count = table.Column<int>(type: "int", nullable: false, comment: "Số thứ tự"),
                    UserIdCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Người tạo"),
                    UserIdModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Người sửa"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schema", x => x.Id);
                },
                comment: "Quản lý danh sách các Schema");

            migrationBuilder.CreateTable(
                name: "Table",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SchemaId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Trạng thái"),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Mã số"),
                    Count = table.Column<int>(type: "int", nullable: false, comment: "Số thứ tự"),
                    UserIdCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Người tạo"),
                    UserIdModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Người sửa"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table", x => x.Id);
                },
                comment: "Quản lý table của Schema");

            migrationBuilder.CreateTable(
                name: "TableDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Tên trường"),
                    TableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Table"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Trạng thái"),
                    DataType = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Kiểu dữ liệu"),
                    IsEncrypt = table.Column<bool>(type: "bit", nullable: false, comment: "Mã hóa"),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Mã số"),
                    Count = table.Column<int>(type: "int", nullable: false, comment: "Số thứ tự"),
                    UserIdCreated = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Người tạo"),
                    UserIdModified = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Người sửa"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableDetail", x => x.Id);
                },
                comment: "Quản lý các trường trong table");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Application");

            migrationBuilder.DropTable(
                name: "AppRoleClaims");

            migrationBuilder.DropTable(
                name: "AppRoles");

            migrationBuilder.DropTable(
                name: "AppUserClaims");

            migrationBuilder.DropTable(
                name: "AppUserLogins");

            migrationBuilder.DropTable(
                name: "AppUserRoles");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "AppUserTokens");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "DatabaseCache");

            migrationBuilder.DropTable(
                name: "LogAction");

            migrationBuilder.DropTable(
                name: "LogHistory");

            migrationBuilder.DropTable(
                name: "Schema");

            migrationBuilder.DropTable(
                name: "Table");

            migrationBuilder.DropTable(
                name: "TableDetail");
        }
    }
}
