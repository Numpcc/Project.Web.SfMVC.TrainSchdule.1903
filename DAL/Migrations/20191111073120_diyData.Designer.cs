﻿// <auto-generated />
using System;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20191111073120_diyData")]
    partial class diyData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DAL.Entities.AdminDivision", b =>
                {
                    b.Property<int>("Code");

                    b.Property<string>("Name");

                    b.Property<int>("ParentCode");

                    b.Property<string>("ShortName");

                    b.HasKey("Code");

                    b.ToTable("AdminDivisions");
                });

            modelBuilder.Entity("DAL.Entities.ApplyInfo.Apply", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuditLeader");

                    b.Property<Guid?>("BaseInfoId");

                    b.Property<DateTime?>("Create");

                    b.Property<bool>("Hidden");

                    b.Property<Guid?>("RequestInfoId");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("BaseInfoId");

                    b.HasIndex("RequestInfoId");

                    b.ToTable("Applies");
                });

            modelBuilder.Entity("DAL.Entities.ApplyInfo.ApplyBaseInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyCode");

                    b.Property<string>("CompanyName");

                    b.Property<DateTime>("CreateTime");

                    b.Property<int?>("DutiesCode");

                    b.Property<string>("DutiesName");

                    b.Property<string>("FromId");

                    b.Property<string>("RealName");

                    b.Property<Guid?>("SocialId");

                    b.HasKey("Id");

                    b.HasIndex("CompanyCode");

                    b.HasIndex("DutiesCode");

                    b.HasIndex("FromId");

                    b.HasIndex("SocialId");

                    b.ToTable("ApplyBaseInfos");
                });

            modelBuilder.Entity("DAL.Entities.ApplyInfo.ApplyRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ByTransportation");

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("OnTripLength");

                    b.Property<string>("Reason");

                    b.Property<DateTime?>("StampLeave");

                    b.Property<DateTime?>("StampReturn");

                    b.Property<int>("VocationLength");

                    b.Property<int?>("VocationPlaceCode");

                    b.Property<string>("VocationType");

                    b.HasKey("Id");

                    b.HasIndex("VocationPlaceCode");

                    b.ToTable("ApplyRequests");
                });

            modelBuilder.Entity("DAL.Entities.ApplyInfo.ApplyResponse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ApplyId");

                    b.Property<string>("AuditingById");

                    b.Property<string>("CompanyCode");

                    b.Property<DateTime?>("HandleStamp");

                    b.Property<string>("Remark");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("ApplyId");

                    b.HasIndex("AuditingById");

                    b.HasIndex("CompanyCode");

                    b.ToTable("ApplyResponses");
                });

            modelBuilder.Entity("DAL.Entities.Company", b =>
                {
                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.Property<bool>("IsPrivate");

                    b.Property<string>("Name");

                    b.HasKey("Code");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("DAL.Entities.CompanyManagers", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthById");

                    b.Property<string>("CompanyCode");

                    b.Property<DateTime?>("Create");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("AuthById");

                    b.HasIndex("CompanyCode");

                    b.HasIndex("UserId");

                    b.ToTable("CompanyManagers");
                });

            modelBuilder.Entity("DAL.Entities.Duties", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsMajorManager");

                    b.Property<string>("Name");

                    b.HasKey("Code");

                    b.ToTable("Duties");
                });

            modelBuilder.Entity("DAL.Entities.Permissions", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Regions");

                    b.Property<string>("Role");

                    b.HasKey("Id");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Settle.Moment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AddressCode");

                    b.Property<string>("AddressDetail");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("Valid");

                    b.HasKey("Id");

                    b.HasIndex("AddressCode");

                    b.ToTable("Moment");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Settle.Settle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("LoverId");

                    b.Property<Guid?>("ParentId");

                    b.Property<int>("PrevYearlyLength");

                    b.Property<Guid?>("SelfId");

                    b.HasKey("Id");

                    b.HasIndex("LoverId");

                    b.HasIndex("ParentId");

                    b.HasIndex("SelfId");

                    b.ToTable("Settles");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Train", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Time_Begin");

                    b.Property<DateTime>("Time_End");

                    b.Property<string>("TrainName");

                    b.Property<string>("TrainRankCode");

                    b.Property<string>("TrainTypeCode");

                    b.Property<Guid?>("UserTrainInfoId");

                    b.HasKey("Id");

                    b.HasIndex("TrainRankCode");

                    b.HasIndex("TrainTypeCode");

                    b.HasIndex("UserTrainInfoId");

                    b.ToTable("Train");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.TrainRank", b =>
                {
                    b.Property<string>("Code");

                    b.Property<string>("Name");

                    b.HasKey("Code");

                    b.ToTable("TrainRank");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.TrainType", b =>
                {
                    b.Property<string>("Code");

                    b.Property<string>("Name");

                    b.HasKey("Code");

                    b.ToTable("TrainType");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.User", b =>
                {
                    b.Property<string>("Id");

                    b.Property<Guid?>("ApplicationId");

                    b.Property<Guid?>("BaseInfoId");

                    b.Property<Guid?>("CompanyInfoId");

                    b.Property<Guid?>("DiyInfoId");

                    b.Property<Guid?>("SocialInfoId");

                    b.Property<Guid?>("TrainInfoId");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("BaseInfoId");

                    b.HasIndex("CompanyInfoId");

                    b.HasIndex("DiyInfoId");

                    b.HasIndex("SocialInfoId");

                    b.HasIndex("TrainInfoId");

                    b.ToTable("AppUsers");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.UserApplicationInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ApplicationSettingId");

                    b.Property<string>("AuthKey");

                    b.Property<DateTime?>("Create");

                    b.Property<string>("Email");

                    b.Property<string>("InvitedBy");

                    b.Property<Guid?>("PermissionId");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationSettingId");

                    b.HasIndex("PermissionId");

                    b.ToTable("AppUserApplicationInfos");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.UserApplicationSetting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("LastSubmitApplyTime");

                    b.Property<DateTime?>("LastVocationUpdateTime");

                    b.HasKey("Id");

                    b.ToTable("AppUserApplicationSettings");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.UserBaseInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Cid")
                        .IsRequired();

                    b.Property<int>("Gender");

                    b.Property<bool>("PrivateAccount");

                    b.Property<string>("RealName");

                    b.Property<DateTime>("Time_BirthDay");

                    b.Property<DateTime>("Time_Party");

                    b.Property<DateTime>("Time_Work");

                    b.HasKey("Id");

                    b.ToTable("AppUserBaseInfos");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.UserCompanyInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyCode");

                    b.Property<int?>("DutiesCode");

                    b.HasKey("Id");

                    b.HasIndex("CompanyCode");

                    b.HasIndex("DutiesCode");

                    b.ToTable("AppUserCompanyInfos");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.UserDiyInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("About");

                    b.Property<string>("Avatar");

                    b.HasKey("Id");

                    b.ToTable("UserDiyInfo");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.UserSocialInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AddressCode");

                    b.Property<string>("AddressDetail");

                    b.Property<string>("Phone");

                    b.Property<Guid?>("SettleId");

                    b.HasKey("Id");

                    b.HasIndex("AddressCode");

                    b.HasIndex("SettleId");

                    b.ToTable("AppUserSocialInfos");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.UserTrainInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.ToTable("UserTrainInfo");
                });

            modelBuilder.Entity("DAL.Entities.VocationDescription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Length");

                    b.Property<string>("Name");

                    b.Property<DateTime>("Start");

                    b.HasKey("Id");

                    b.ToTable("VocationDescriptions");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("DAL.Entities.ApplyInfo.Apply", b =>
                {
                    b.HasOne("DAL.Entities.ApplyInfo.ApplyBaseInfo", "BaseInfo")
                        .WithMany()
                        .HasForeignKey("BaseInfoId");

                    b.HasOne("DAL.Entities.ApplyInfo.ApplyRequest", "RequestInfo")
                        .WithMany()
                        .HasForeignKey("RequestInfoId");
                });

            modelBuilder.Entity("DAL.Entities.ApplyInfo.ApplyBaseInfo", b =>
                {
                    b.HasOne("DAL.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyCode");

                    b.HasOne("DAL.Entities.Duties", "Duties")
                        .WithMany()
                        .HasForeignKey("DutiesCode");

                    b.HasOne("DAL.Entities.UserInfo.User", "From")
                        .WithMany()
                        .HasForeignKey("FromId");

                    b.HasOne("DAL.Entities.UserInfo.UserSocialInfo", "Social")
                        .WithMany()
                        .HasForeignKey("SocialId");
                });

            modelBuilder.Entity("DAL.Entities.ApplyInfo.ApplyRequest", b =>
                {
                    b.HasOne("DAL.Entities.AdminDivision", "VocationPlace")
                        .WithMany()
                        .HasForeignKey("VocationPlaceCode");
                });

            modelBuilder.Entity("DAL.Entities.ApplyInfo.ApplyResponse", b =>
                {
                    b.HasOne("DAL.Entities.ApplyInfo.Apply")
                        .WithMany("Response")
                        .HasForeignKey("ApplyId");

                    b.HasOne("DAL.Entities.UserInfo.User", "AuditingBy")
                        .WithMany()
                        .HasForeignKey("AuditingById");

                    b.HasOne("DAL.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyCode");
                });

            modelBuilder.Entity("DAL.Entities.CompanyManagers", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.User", "AuthBy")
                        .WithMany()
                        .HasForeignKey("AuthById");

                    b.HasOne("DAL.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyCode");

                    b.HasOne("DAL.Entities.UserInfo.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Settle.Moment", b =>
                {
                    b.HasOne("DAL.Entities.AdminDivision", "Address")
                        .WithMany()
                        .HasForeignKey("AddressCode");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Settle.Settle", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.Settle.Moment", "Lover")
                        .WithMany()
                        .HasForeignKey("LoverId");

                    b.HasOne("DAL.Entities.UserInfo.Settle.Moment", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.HasOne("DAL.Entities.UserInfo.Settle.Moment", "Self")
                        .WithMany()
                        .HasForeignKey("SelfId");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Train", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.TrainRank", "TrainRank")
                        .WithMany()
                        .HasForeignKey("TrainRankCode");

                    b.HasOne("DAL.Entities.UserInfo.TrainType", "TrainType")
                        .WithMany()
                        .HasForeignKey("TrainTypeCode");

                    b.HasOne("DAL.Entities.UserInfo.UserTrainInfo")
                        .WithMany("Trains")
                        .HasForeignKey("UserTrainInfoId");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.User", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.UserApplicationInfo", "Application")
                        .WithMany()
                        .HasForeignKey("ApplicationId");

                    b.HasOne("DAL.Entities.UserInfo.UserBaseInfo", "BaseInfo")
                        .WithMany()
                        .HasForeignKey("BaseInfoId");

                    b.HasOne("DAL.Entities.UserInfo.UserCompanyInfo", "CompanyInfo")
                        .WithMany()
                        .HasForeignKey("CompanyInfoId");

                    b.HasOne("DAL.Entities.UserInfo.UserDiyInfo", "DiyInfo")
                        .WithMany()
                        .HasForeignKey("DiyInfoId");

                    b.HasOne("DAL.Entities.UserInfo.UserSocialInfo", "SocialInfo")
                        .WithMany()
                        .HasForeignKey("SocialInfoId");

                    b.HasOne("DAL.Entities.UserInfo.UserTrainInfo", "TrainInfo")
                        .WithMany()
                        .HasForeignKey("TrainInfoId");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.UserApplicationInfo", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.UserApplicationSetting", "ApplicationSetting")
                        .WithMany()
                        .HasForeignKey("ApplicationSettingId");

                    b.HasOne("DAL.Entities.Permissions", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.UserCompanyInfo", b =>
                {
                    b.HasOne("DAL.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyCode");

                    b.HasOne("DAL.Entities.Duties", "Duties")
                        .WithMany()
                        .HasForeignKey("DutiesCode");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.UserSocialInfo", b =>
                {
                    b.HasOne("DAL.Entities.AdminDivision", "Address")
                        .WithMany()
                        .HasForeignKey("AddressCode");

                    b.HasOne("DAL.Entities.UserInfo.Settle.Settle", "Settle")
                        .WithMany()
                        .HasForeignKey("SettleId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DAL.Entities.UserInfo.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
