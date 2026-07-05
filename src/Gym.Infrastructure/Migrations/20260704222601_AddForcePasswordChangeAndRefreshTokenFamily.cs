using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    public partial class AddForcePasswordChangeAndRefreshTokenFamily : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'IsPasswordChangeRequired')
                    ALTER TABLE [Users] ADD [IsPasswordChangeRequired] bit NOT NULL DEFAULT CAST(0 AS bit);

                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'PreviousRefreshTokenHash')
                    ALTER TABLE [Users] ADD [PreviousRefreshTokenHash] nvarchar(500) NULL;

                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'FailedLoginAttempts')
                    ALTER TABLE [Users] ADD [FailedLoginAttempts] int NOT NULL DEFAULT 0;

                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'LockoutEnd')
                    ALTER TABLE [Users] ADD [LockoutEnd] datetime2 NULL;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'LockoutEnd')
                    ALTER TABLE [Users] DROP COLUMN [LockoutEnd];

                IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'FailedLoginAttempts')
                    ALTER TABLE [Users] DROP COLUMN [FailedLoginAttempts];

                IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'PreviousRefreshTokenHash')
                    ALTER TABLE [Users] DROP COLUMN [PreviousRefreshTokenHash];

                IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'IsPasswordChangeRequired')
                    ALTER TABLE [Users] DROP COLUMN [IsPasswordChangeRequired];
            ");
        }
    }
}
