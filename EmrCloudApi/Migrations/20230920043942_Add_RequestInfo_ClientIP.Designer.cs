﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PostgreDataContext;

#nullable disable

namespace EmrCloudApi.Migrations
{
    [DbContext(typeof(AdminDataContext))]
    [Migration("20230920043942_Add_RequestInfo_ClientIP")]
    partial class AddRequestInfoClientIP
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Entity.Logger.AuditLog", b =>
                {
                    b.Property<long>("LogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnOrder(1);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("LogId"));

                    b.Property<string>("ClientIP")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("character varying(60)");

                    b.Property<string>("Domain")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EventCd")
                        .HasMaxLength(11)
                        .HasColumnType("character varying(11)");

                    b.Property<int>("HpId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("LogDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("PtId")
                        .HasColumnType("bigint");

                    b.Property<long>("RaiinNo")
                        .HasColumnType("bigint");

                    b.Property<string>("RequestInfo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SinDay")
                        .HasColumnType("integer");

                    b.Property<string>("TenantId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("LogId");

                    b.ToTable("AuditLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
