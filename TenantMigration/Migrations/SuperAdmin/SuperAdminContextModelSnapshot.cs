﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PostgreDataContext;

#nullable disable

namespace TenantMigration.Migrations.SuperAdmin
{
    [DbContext(typeof(SuperAdminContext))]
    partial class SuperAdminContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Entity.SuperAdmin.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CREATE_DATE");

                    b.Property<string>("FullName")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("FULL_NAME");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("integer")
                        .HasColumnName("IS_DELETED");

                    b.Property<int>("LoginId")
                        .HasColumnType("integer")
                        .HasColumnName("LOGIN_ID");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("NAME");

                    b.Property<string>("PassWord")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("PASSWORD");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("ROLE");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UPDATE_DATE");

                    b.HasKey("Id");

                    b.HasIndex("LoginId")
                        .IsUnique()
                        .HasFilter("\"IS_DELETED\" = 0");

                    b.ToTable("ADMIN");
                });

            modelBuilder.Entity("Entity.SuperAdmin.MigrationTenantHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("MigrationId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("MIGRATION_ID");

                    b.Property<int>("TenantId")
                        .HasColumnType("integer")
                        .HasColumnName("TENANT_ID");

                    b.HasKey("Id");

                    b.ToTable("MIGRATION_TENANT_HISTORY");
                });

            modelBuilder.Entity("Entity.SuperAdmin.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CREATE_DATE");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("integer")
                        .HasColumnName("IS_DELETED");

                    b.Property<byte>("IsRead")
                        .HasColumnType("smallint")
                        .HasColumnName("IS_READ");

                    b.Property<string>("Message")
                        .HasColumnType("text")
                        .HasColumnName("MESSAGE");

                    b.Property<byte>("Status")
                        .HasColumnType("smallint")
                        .HasColumnName("STATUS");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UPDATE_DATE");

                    b.HasKey("Id");

                    b.ToTable("NOTIFICATION");
                });

            modelBuilder.Entity("Entity.SuperAdmin.Scription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("ID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CREATE_DATE");

                    b.Property<string>("HOSPITAL")
                        .HasColumnType("text")
                        .HasColumnName("HOSPITAL");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("integer")
                        .HasColumnName("IS_DELETED");

                    b.Property<string>("ScriptString")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ScriptString");

                    b.Property<int>("TenantId")
                        .HasColumnType("integer")
                        .HasColumnName("TENANT_ID");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UPDATE_DATE");

                    b.HasKey("Id");

                    b.ToTable("SCRIPTION");
                });

            modelBuilder.Entity("Entity.SuperAdmin.Tenant", b =>
                {
                    b.Property<int>("TenantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("TENANT_ID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TenantId"));

                    b.Property<int>("Action")
                        .HasColumnType("integer")
                        .HasColumnName("ACTION");

                    b.Property<int>("AdminId")
                        .HasColumnType("integer")
                        .HasColumnName("ADMIN_ID");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CREATE_DATE");

                    b.Property<string>("Db")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("DB");

                    b.Property<string>("EndPointDb")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("END_POINT_DB");

                    b.Property<string>("EndSubDomain")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("END_SUB_DOMAIN");

                    b.Property<string>("Hospital")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("HOSPITAL");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("integer")
                        .HasColumnName("IS_DELETED");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("PASSWORD");

                    b.Property<string>("PasswordConnect")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("PASSWORD_CONNECT");

                    b.Property<string>("RdsIdentifier")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("RDS_IDENTIFIER");

                    b.Property<int>("ScheduleDate")
                        .HasColumnType("integer")
                        .HasColumnName("SCHEDULE_DATE");

                    b.Property<int>("ScheduleTime")
                        .HasColumnType("integer")
                        .HasColumnName("SCHEDULE_TIME");

                    b.Property<double>("Size")
                        .HasColumnType("double precision")
                        .HasColumnName("SIZE");

                    b.Property<int>("SizeType")
                        .HasColumnType("integer")
                        .HasColumnName("SIZE_TYPE");

                    b.Property<byte>("Status")
                        .HasColumnType("smallint")
                        .HasColumnName("STATUS");

                    b.Property<string>("SubDomain")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("SUB_DOMAIN");

                    b.Property<byte>("Type")
                        .HasColumnType("smallint")
                        .HasColumnName("TYPE");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UPDATE_DATE");

                    b.Property<string>("UserConnect")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("USER_CONNECT");

                    b.HasKey("TenantId");

                    b.HasIndex("Db")
                        .IsUnique()
                        .HasFilter("\"IS_DELETED\" = 0");

                    b.HasIndex("EndPointDb")
                        .IsUnique()
                        .HasFilter("\"IS_DELETED\" = 0");

                    b.HasIndex("EndSubDomain")
                        .IsUnique()
                        .HasFilter("\"IS_DELETED\" = 0");

                    b.HasIndex("Hospital")
                        .IsUnique()
                        .HasFilter("\"IS_DELETED\" = 0");

                    b.HasIndex("SubDomain")
                        .IsUnique()
                        .HasFilter("\"IS_DELETED\" = 0");

                    b.ToTable("TENANT");
                });

            modelBuilder.Entity("Entity.SuperAdmin.UserToken", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("USER_ID");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text")
                        .HasColumnName("REFRESH_TOKEN");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("TOKEN_EXPIRY_TIME");

                    b.Property<bool>("RefreshTokenIsUsed")
                        .HasColumnType("boolean")
                        .HasColumnName("REFRESH_TOKEN_IS_USED");

                    b.HasKey("UserId", "RefreshToken");

                    b.ToTable("USER_TOKEN");
                });
#pragma warning restore 612, 618
        }
    }
}
