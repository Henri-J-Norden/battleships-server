﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20181228140732_InitialDbCreation1")]
    partial class InitialDbCreation1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Guid")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasConversion(new ValueConverter<byte[], byte[]>(v => default(byte[]), v => default(byte[]), new ConverterMappingHints(size: 16)));

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired();

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
