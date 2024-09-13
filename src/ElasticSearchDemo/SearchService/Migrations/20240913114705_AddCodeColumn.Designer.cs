﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SearchService.Data;

#nullable disable

namespace SearchService.Migrations
{
    [DbContext(typeof(SearchDbContext))]
    [Migration("20240913114705_AddCodeColumn")]
    partial class AddCodeColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("SearchService.Models.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<long>("Amount")
                        .HasColumnType("bigint")
                        .HasColumnName("amount");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("code");

                    b.Property<string>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("created_date");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("note");

                    b.HasKey("Id")
                        .HasName("pk_transactions");

                    b.ToTable("transactions", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
