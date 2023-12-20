﻿// <auto-generated />
using System;
using Hierarquias.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Hierarquias.Migrations
{
    [DbContext(typeof(HierarquiasDbContext))]
    [Migration("20231219233111_AtualizacaoFuncionarios")]
    partial class AtualizacaoFuncionarios
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Hierarquias.Models.Cargos", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Cargos");
                });

            modelBuilder.Entity("Hierarquias.Models.Funcionarios", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Cargo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FuncionariosId")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SuperiorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FuncionariosId");

                    b.HasIndex("SuperiorId");

                    b.ToTable("Funcionarios");
                });

            modelBuilder.Entity("Hierarquias.Models.Funcionarios", b =>
                {
                    b.HasOne("Hierarquias.Models.Funcionarios", null)
                        .WithMany("TodosOsFuncionarios")
                        .HasForeignKey("FuncionariosId");

                    b.HasOne("Hierarquias.Models.Funcionarios", "Superior")
                        .WithMany("Subordinados")
                        .HasForeignKey("SuperiorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Superior");
                });

            modelBuilder.Entity("Hierarquias.Models.Funcionarios", b =>
                {
                    b.Navigation("Subordinados");

                    b.Navigation("TodosOsFuncionarios");
                });
#pragma warning restore 612, 618
        }
    }
}
