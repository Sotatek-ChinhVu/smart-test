using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHpId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "yoyaku_odr_inf");

            migrationBuilder.DropTable(
                name: "yoyaku_odr_inf_detail");

            migrationBuilder.DropTable(
                name: "yoyaku_sbt_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_yakka_syusai_mst",
                table: "yakka_syusai_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_unit_mst",
                table: "unit_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tokki_mst",
                table: "tokki_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_syouki_kbn_mst",
                table: "syouki_kbn_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sta_mst",
                table: "sta_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sta_grp",
                table: "sta_grp");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sokatu_mst",
                table: "sokatu_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_rousai_gosei_mst",
                table: "rousai_gosei_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_roudou_mst",
                table: "roudou_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pi_product_inf",
                table: "pi_product_inf");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pi_inf_detail",
                table: "pi_inf_detail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pi_inf",
                table: "pi_inf");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pi_image",
                table: "pi_image");

            migrationBuilder.DropPrimaryKey(
                name: "PK_physical_average",
                table: "physical_average");

            migrationBuilder.DropPrimaryKey(
                name: "PK_permission_mst",
                table: "permission_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_yj_drug_class",
                table: "m56_yj_drug_class");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_usage_code",
                table: "m56_usage_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_prodrug_cd",
                table: "m56_prodrug_cd");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_ex_ingrdt_main",
                table: "m56_ex_ingrdt_main");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_ex_ing_code",
                table: "m56_ex_ing_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_ex_ed_ingredients",
                table: "m56_ex_ed_ingredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_ex_analogue",
                table: "m56_ex_analogue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_drvalrgy_code",
                table: "m56_drvalrgy_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_drug_class",
                table: "m56_drug_class");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_analogue_cd",
                table: "m56_analogue_cd");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_alrgy_derivatives",
                table: "m56_alrgy_derivatives");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m46_dosage_drug",
                table: "m46_dosage_drug");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m46_dosage_dosage",
                table: "m46_dosage_dosage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m42_contraindi_drug_main_ex",
                table: "m42_contraindi_drug_main_ex");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m42_contraindi_dis_con",
                table: "m42_contraindi_dis_con");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m42_contraindi_dis_class",
                table: "m42_contraindi_dis_class");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m42_contraindi_dis_bc",
                table: "m42_contraindi_dis_bc");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m42_contra_cmt",
                table: "m42_contra_cmt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m41_supple_ingre",
                table: "m41_supple_ingre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m41_supple_indexdef",
                table: "m41_supple_indexdef");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m41_supple_indexcode",
                table: "m41_supple_indexcode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m38_otc_maker_code",
                table: "m38_otc_maker_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m38_otc_main",
                table: "m38_otc_main");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m38_otc_form_code",
                table: "m38_otc_form_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m38_major_div_code",
                table: "m38_major_div_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m38_ing_code",
                table: "m38_ing_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m38_class_code",
                table: "m38_class_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_sar_symptom_code",
                table: "m34_sar_symptom_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_property_code",
                table: "m34_property_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_precautions",
                table: "m34_precautions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_precaution_code",
                table: "m34_precaution_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_interaction_pat_code",
                table: "m34_interaction_pat_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_interaction_pat",
                table: "m34_interaction_pat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_indication_code",
                table: "m34_indication_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_form_code",
                table: "m34_form_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_drug_info_main",
                table: "m34_drug_info_main");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_ar_discon_code",
                table: "m34_ar_discon_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_ar_discon",
                table: "m34_ar_discon");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_ar_code",
                table: "m34_ar_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m28_drug_mst",
                table: "m28_drug_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m14_cmt_code",
                table: "m14_cmt_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m14_age_check",
                table: "m14_age_check");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m12_food_alrgy_kbn",
                table: "m12_food_alrgy_kbn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m12_food_alrgy",
                table: "m12_food_alrgy");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m10_day_limit",
                table: "m10_day_limit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m01_kinki_cmt",
                table: "m01_kinki_cmt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m01_kinki",
                table: "m01_kinki");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m01_kijyo_cmt",
                table: "m01_kijyo_cmt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_koui_kbn_mst",
                table: "koui_kbn_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_kohi_priority",
                table: "kohi_priority");

            migrationBuilder.DropPrimaryKey(
                name: "PK_kogaku_limit",
                table: "kogaku_limit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_kantoku_mst",
                table: "kantoku_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_kacode_rece_yousiki",
                table: "kacode_rece_yousiki");

            migrationBuilder.DropPrimaryKey(
                name: "PK_kacode_mst",
                table: "kacode_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ipn_name_mst",
                table: "ipn_name_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ipn_min_yakka_mst",
                table: "ipn_min_yakka_mst");

            migrationBuilder.DropIndex(
                name: "ipn_min_yakka_mst_idx01",
                table: "ipn_min_yakka_mst");

            migrationBuilder.DropIndex(
                name: "ipn_min_yakka_mst_idx02",
                table: "ipn_min_yakka_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ipn_kasan_mst",
                table: "ipn_kasan_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ipn_kasan_exclude_item",
                table: "ipn_kasan_exclude_item");

            migrationBuilder.DropIndex(
                name: "ipn_kasan_exclude_item_idx01",
                table: "ipn_kasan_exclude_item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ipn_kasan_exclude",
                table: "ipn_kasan_exclude");

            migrationBuilder.DropIndex(
                name: "ipn_kasan_exclude_idx01",
                table: "ipn_kasan_exclude");

            migrationBuilder.DropPrimaryKey(
                name: "PK_gc_std_mst",
                table: "gc_std_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_function_mst",
                table: "function_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_event_mst",
                table: "event_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_drug_unit_conv",
                table: "drug_unit_conv");

            migrationBuilder.DropPrimaryKey(
                name: "PK_byomei_mst_aftercare",
                table: "byomei_mst_aftercare");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "yakka_syusai_mst");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "tokki_mst");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "sta_mst");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "sta_grp");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "sokatu_mst");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "rousai_gosei_mst");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "pi_image");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "koui_kbn_mst");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "kogaku_limit");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "ipn_name_mst");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "ipn_min_yakka_mst");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "ipn_kasan_mst");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "ipn_kasan_exclude_item");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "ipn_kasan_exclude");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "gc_std_mst");

            migrationBuilder.AlterColumn<int>(
                name: "start_date",
                table: "yakka_syusai_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<string>(
                name: "item_cd",
                table: "yakka_syusai_mst",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<string>(
                name: "yakka_cd",
                table: "yakka_syusai_mst",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(12)",
                oldMaxLength: 12)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "unit_cd",
                table: "unit_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "unit_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "tokki_cd",
                table: "tokki_mst",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "syouki_kbn_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "report_id",
                table: "sta_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "report_id",
                table: "sta_grp",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "grp_id",
                table: "sta_grp",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "report_id",
                table: "sokatu_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<int>(
                name: "report_eda_no",
                table: "sokatu_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 5);

            migrationBuilder.AlterColumn<int>(
                name: "start_ym",
                table: "sokatu_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "pref_no",
                table: "sokatu_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "start_date",
                table: "rousai_gosei_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 6);

            migrationBuilder.AlterColumn<int>(
                name: "sisi_kbn",
                table: "rousai_gosei_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 5);

            migrationBuilder.AlterColumn<string>(
                name: "item_cd",
                table: "rousai_gosei_mst",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10)
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<string>(
                name: "gosei_item_cd",
                table: "rousai_gosei_mst",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "gosei_grp",
                table: "rousai_gosei_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "roudou_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "pi_product_inf",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "pi_inf_detail",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "pi_inf",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "item_cd",
                table: "pi_image",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "image_type",
                table: "pi_image",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "physical_average",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "permission_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m56_yj_drug_class",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m56_usage_code",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m56_prodrug_cd",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m56_ex_ingrdt_main",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m56_ex_ing_code",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m56_ex_ed_ingredients",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m56_ex_analogue",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m56_drvalrgy_code",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m56_drug_class",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m56_analogue_cd",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m56_alrgy_derivatives",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m46_dosage_drug",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m46_dosage_dosage",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m42_contraindi_drug_main_ex",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m42_contraindi_dis_con",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m42_contraindi_dis_class",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m42_contraindi_dis_bc",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m42_contra_cmt",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m41_supple_ingre",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m41_supple_indexdef",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m41_supple_indexcode",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m38_otc_maker_code",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "serial_num",
                table: "m38_otc_main",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m38_otc_main",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m38_otc_form_code",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m38_major_div_code",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m38_ingredients",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m38_ing_code",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m38_class_code",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m34_sar_symptom_code",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "property_cd",
                table: "m34_property_code",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m34_property_code",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m34_precautions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m34_precaution_code",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m34_interaction_pat_code",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m34_interaction_pat",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m34_indication_code",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m34_form_code",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m34_drug_info_main",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m34_ar_discon_code",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m34_ar_discon",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m34_ar_code",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m28_drug_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m14_cmt_code",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m14_age_check",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m12_food_alrgy_kbn",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m12_food_alrgy",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m10_day_limit",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m01_kinki_cmt",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m01_kinki",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "m01_kijyo_cmt",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "koui_kbn_id",
                table: "koui_kbn_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "kohi_priority",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "start_date",
                table: "kogaku_limit",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<int>(
                name: "kogaku_kbn",
                table: "kogaku_limit",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "age_kbn",
                table: "kogaku_limit",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "kantoku_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "kacode_rece_yousiki",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "kacode_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "seq_no",
                table: "ipn_name_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<int>(
                name: "start_date",
                table: "ipn_name_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<string>(
                name: "ipn_name_cd",
                table: "ipn_name_mst",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(12)",
                oldMaxLength: 12)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "seq_no",
                table: "ipn_min_yakka_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<string>(
                name: "ipn_name_cd",
                table: "ipn_min_yakka_mst",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(12)",
                oldMaxLength: 12)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "ipn_min_yakka_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "seq_no",
                table: "ipn_kasan_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<string>(
                name: "ipn_name_cd",
                table: "ipn_kasan_mst",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(12)",
                oldMaxLength: 12)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "start_date",
                table: "ipn_kasan_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<string>(
                name: "item_cd",
                table: "ipn_kasan_exclude_item",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "start_date",
                table: "ipn_kasan_exclude_item",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "seq_no",
                table: "ipn_kasan_exclude",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<string>(
                name: "ipn_name_cd",
                table: "ipn_kasan_exclude",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(12)",
                oldMaxLength: 12)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "start_date",
                table: "ipn_kasan_exclude",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<double>(
                name: "point",
                table: "gc_std_mst",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision")
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<int>(
                name: "sex",
                table: "gc_std_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "std_kbn",
                table: "gc_std_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "function_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "event_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "drug_unit_conv",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "byomei_mst_aftercare",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_yakka_syusai_mst",
                table: "yakka_syusai_mst",
                columns: new[] { "yakka_cd", "item_cd", "start_date" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_unit_mst",
                table: "unit_mst",
                columns: new[] { "hp_id", "unit_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_tokki_mst",
                table: "tokki_mst",
                column: "tokki_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_syouki_kbn_mst",
                table: "syouki_kbn_mst",
                columns: new[] { "hp_id", "syouki_kbn", "start_ym" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_sta_mst",
                table: "sta_mst",
                column: "report_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sta_grp",
                table: "sta_grp",
                columns: new[] { "grp_id", "report_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_sokatu_mst",
                table: "sokatu_mst",
                columns: new[] { "pref_no", "start_ym", "report_eda_no", "report_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_rousai_gosei_mst",
                table: "rousai_gosei_mst",
                columns: new[] { "gosei_grp", "gosei_item_cd", "item_cd", "sisi_kbn", "start_date" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_roudou_mst",
                table: "roudou_mst",
                columns: new[] { "hp_id", "roudou_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_pi_product_inf",
                table: "pi_product_inf",
                columns: new[] { "hp_id", "pi_id_full", "pi_id", "branch", "jpn" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_pi_inf_detail",
                table: "pi_inf_detail",
                columns: new[] { "hp_id", "pi_id", "branch", "jpn", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_pi_inf",
                table: "pi_inf",
                columns: new[] { "hp_id", "pi_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_pi_image",
                table: "pi_image",
                columns: new[] { "image_type", "item_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_physical_average",
                table: "physical_average",
                columns: new[] { "hp_id", "jissi_year", "age_year", "age_month", "age_day" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_permission_mst",
                table: "permission_mst",
                columns: new[] { "hp_id", "function_cd", "permission" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_yj_drug_class",
                table: "m56_yj_drug_class",
                columns: new[] { "hp_id", "yj_cd", "class_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_usage_code",
                table: "m56_usage_code",
                columns: new[] { "hp_id", "yoho_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_prodrug_cd",
                table: "m56_prodrug_cd",
                columns: new[] { "hp_id", "seq_no", "seibun_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_ex_ingrdt_main",
                table: "m56_ex_ingrdt_main",
                columns: new[] { "hp_id", "yj_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_ex_ing_code",
                table: "m56_ex_ing_code",
                columns: new[] { "hp_id", "seibun_cd", "seibun_index_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_ex_ed_ingredients",
                table: "m56_ex_ed_ingredients",
                columns: new[] { "hp_id", "yj_cd", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_ex_analogue",
                table: "m56_ex_analogue",
                columns: new[] { "hp_id", "seibun_cd", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_drvalrgy_code",
                table: "m56_drvalrgy_code",
                columns: new[] { "hp_id", "drvalrgy_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_drug_class",
                table: "m56_drug_class",
                columns: new[] { "hp_id", "class_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_analogue_cd",
                table: "m56_analogue_cd",
                columns: new[] { "hp_id", "analogue_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_alrgy_derivatives",
                table: "m56_alrgy_derivatives",
                columns: new[] { "hp_id", "seibun_cd", "drvalrgy_cd", "yj_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m46_dosage_drug",
                table: "m46_dosage_drug",
                columns: new[] { "hp_id", "doei_cd", "yj_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m46_dosage_dosage",
                table: "m46_dosage_dosage",
                columns: new[] { "hp_id", "doei_cd", "doei_seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m42_contraindi_drug_main_ex",
                table: "m42_contraindi_drug_main_ex",
                columns: new[] { "hp_id", "yj_cd", "tenpu_level", "byotai_cd", "cmt_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m42_contraindi_dis_con",
                table: "m42_contraindi_dis_con",
                columns: new[] { "hp_id", "byotai_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m42_contraindi_dis_class",
                table: "m42_contraindi_dis_class",
                columns: new[] { "hp_id", "byotai_class_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m42_contraindi_dis_bc",
                table: "m42_contraindi_dis_bc",
                columns: new[] { "hp_id", "byotai_cd", "byotai_class_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m42_contra_cmt",
                table: "m42_contra_cmt",
                columns: new[] { "hp_id", "cmt_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m41_supple_ingre",
                table: "m41_supple_ingre",
                columns: new[] { "hp_id", "seibun_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m41_supple_indexdef",
                table: "m41_supple_indexdef",
                columns: new[] { "hp_id", "seibun_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m41_supple_indexcode",
                table: "m41_supple_indexcode",
                columns: new[] { "hp_id", "seibun_cd", "index_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m38_otc_maker_code",
                table: "m38_otc_maker_code",
                columns: new[] { "hp_id", "maker_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m38_otc_main",
                table: "m38_otc_main",
                columns: new[] { "hp_id", "serial_num" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m38_otc_form_code",
                table: "m38_otc_form_code",
                columns: new[] { "hp_id", "form_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m38_major_div_code",
                table: "m38_major_div_code",
                columns: new[] { "hp_id", "major_div_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m38_ing_code",
                table: "m38_ing_code",
                columns: new[] { "hp_id", "seibun_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m38_class_code",
                table: "m38_class_code",
                columns: new[] { "hp_id", "class_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_sar_symptom_code",
                table: "m34_sar_symptom_code",
                columns: new[] { "hp_id", "fukusayo_init_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_property_code",
                table: "m34_property_code",
                columns: new[] { "hp_id", "property_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_precautions",
                table: "m34_precautions",
                columns: new[] { "hp_id", "yj_cd", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_precaution_code",
                table: "m34_precaution_code",
                columns: new[] { "hp_id", "precaution_cd", "extend_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_interaction_pat_code",
                table: "m34_interaction_pat_code",
                columns: new[] { "hp_id", "interaction_pat_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_interaction_pat",
                table: "m34_interaction_pat",
                columns: new[] { "hp_id", "yj_cd", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_indication_code",
                table: "m34_indication_code",
                columns: new[] { "hp_id", "kono_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_form_code",
                table: "m34_form_code",
                columns: new[] { "hp_id", "form_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_drug_info_main",
                table: "m34_drug_info_main",
                columns: new[] { "hp_id", "yj_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_ar_discon_code",
                table: "m34_ar_discon_code",
                columns: new[] { "hp_id", "fukusayo_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_ar_discon",
                table: "m34_ar_discon",
                columns: new[] { "hp_id", "yj_cd", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_ar_code",
                table: "m34_ar_code",
                columns: new[] { "hp_id", "fukusayo_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m28_drug_mst",
                table: "m28_drug_mst",
                columns: new[] { "hp_id", "yj_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m14_cmt_code",
                table: "m14_cmt_code",
                columns: new[] { "hp_id", "attention_cmt_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m14_age_check",
                table: "m14_age_check",
                columns: new[] { "hp_id", "yj_cd", "attention_cmt_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m12_food_alrgy_kbn",
                table: "m12_food_alrgy_kbn",
                columns: new[] { "hp_id", "food_kbn" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m12_food_alrgy",
                table: "m12_food_alrgy",
                columns: new[] { "hp_id", "yj_cd", "food_kbn", "tenpu_level" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m10_day_limit",
                table: "m10_day_limit",
                columns: new[] { "hp_id", "yj_cd", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m01_kinki_cmt",
                table: "m01_kinki_cmt",
                columns: new[] { "hp_id", "cmt_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m01_kinki",
                table: "m01_kinki",
                columns: new[] { "hp_id", "a_cd", "b_cd", "cmt_cd", "sayokijyo_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m01_kijyo_cmt",
                table: "m01_kijyo_cmt",
                columns: new[] { "cmt_cd", "hp_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_koui_kbn_mst",
                table: "koui_kbn_mst",
                column: "koui_kbn_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_kohi_priority",
                table: "kohi_priority",
                columns: new[] { "pref_no", "houbetu", "priority_no", "hp_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_kogaku_limit",
                table: "kogaku_limit",
                columns: new[] { "age_kbn", "kogaku_kbn", "start_date" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_kantoku_mst",
                table: "kantoku_mst",
                columns: new[] { "hp_id", "roudou_cd", "kantoku_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_kacode_rece_yousiki",
                table: "kacode_rece_yousiki",
                columns: new[] { "hp_id", "rece_ka_cd", "yousiki_ka_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_kacode_mst",
                table: "kacode_mst",
                columns: new[] { "hp_id", "rece_ka_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ipn_name_mst",
                table: "ipn_name_mst",
                columns: new[] { "ipn_name_cd", "start_date", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ipn_min_yakka_mst",
                table: "ipn_min_yakka_mst",
                columns: new[] { "id", "ipn_name_cd", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ipn_kasan_mst",
                table: "ipn_kasan_mst",
                columns: new[] { "start_date", "ipn_name_cd", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ipn_kasan_exclude_item",
                table: "ipn_kasan_exclude_item",
                columns: new[] { "start_date", "item_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ipn_kasan_exclude",
                table: "ipn_kasan_exclude",
                columns: new[] { "start_date", "ipn_name_cd", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_gc_std_mst",
                table: "gc_std_mst",
                columns: new[] { "std_kbn", "sex", "point" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_function_mst",
                table: "function_mst",
                columns: new[] { "hp_id", "function_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_event_mst",
                table: "event_mst",
                columns: new[] { "hp_id", "event_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_drug_unit_conv",
                table: "drug_unit_conv",
                columns: new[] { "hp_id", "item_cd", "start_date" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_byomei_mst_aftercare",
                table: "byomei_mst_aftercare",
                columns: new[] { "hp_id", "byomei_cd", "byomei", "start_date" });

            migrationBuilder.CreateIndex(
                name: "ipn_min_yakka_mst_idx01",
                table: "ipn_min_yakka_mst",
                columns: new[] { "ipn_name_cd", "start_date" });

            migrationBuilder.CreateIndex(
                name: "ipn_min_yakka_mst_idx02",
                table: "ipn_min_yakka_mst",
                columns: new[] { "start_date", "end_date", "ipn_name_cd" });

            migrationBuilder.CreateIndex(
                name: "ipn_kasan_exclude_item_idx01",
                table: "ipn_kasan_exclude_item",
                columns: new[] { "start_date", "end_date", "item_cd" });

            migrationBuilder.CreateIndex(
                name: "ipn_kasan_exclude_idx01",
                table: "ipn_kasan_exclude",
                columns: new[] { "start_date", "end_date", "ipn_name_cd" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_yakka_syusai_mst",
                table: "yakka_syusai_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_unit_mst",
                table: "unit_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tokki_mst",
                table: "tokki_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_syouki_kbn_mst",
                table: "syouki_kbn_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sta_mst",
                table: "sta_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sta_grp",
                table: "sta_grp");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sokatu_mst",
                table: "sokatu_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_rousai_gosei_mst",
                table: "rousai_gosei_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_roudou_mst",
                table: "roudou_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pi_product_inf",
                table: "pi_product_inf");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pi_inf_detail",
                table: "pi_inf_detail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pi_inf",
                table: "pi_inf");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pi_image",
                table: "pi_image");

            migrationBuilder.DropPrimaryKey(
                name: "PK_physical_average",
                table: "physical_average");

            migrationBuilder.DropPrimaryKey(
                name: "PK_permission_mst",
                table: "permission_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_yj_drug_class",
                table: "m56_yj_drug_class");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_usage_code",
                table: "m56_usage_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_prodrug_cd",
                table: "m56_prodrug_cd");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_ex_ingrdt_main",
                table: "m56_ex_ingrdt_main");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_ex_ing_code",
                table: "m56_ex_ing_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_ex_ed_ingredients",
                table: "m56_ex_ed_ingredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_ex_analogue",
                table: "m56_ex_analogue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_drvalrgy_code",
                table: "m56_drvalrgy_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_drug_class",
                table: "m56_drug_class");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_analogue_cd",
                table: "m56_analogue_cd");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m56_alrgy_derivatives",
                table: "m56_alrgy_derivatives");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m46_dosage_drug",
                table: "m46_dosage_drug");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m46_dosage_dosage",
                table: "m46_dosage_dosage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m42_contraindi_drug_main_ex",
                table: "m42_contraindi_drug_main_ex");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m42_contraindi_dis_con",
                table: "m42_contraindi_dis_con");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m42_contraindi_dis_class",
                table: "m42_contraindi_dis_class");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m42_contraindi_dis_bc",
                table: "m42_contraindi_dis_bc");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m42_contra_cmt",
                table: "m42_contra_cmt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m41_supple_ingre",
                table: "m41_supple_ingre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m41_supple_indexdef",
                table: "m41_supple_indexdef");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m41_supple_indexcode",
                table: "m41_supple_indexcode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m38_otc_maker_code",
                table: "m38_otc_maker_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m38_otc_main",
                table: "m38_otc_main");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m38_otc_form_code",
                table: "m38_otc_form_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m38_major_div_code",
                table: "m38_major_div_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m38_ing_code",
                table: "m38_ing_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m38_class_code",
                table: "m38_class_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_sar_symptom_code",
                table: "m34_sar_symptom_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_property_code",
                table: "m34_property_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_precautions",
                table: "m34_precautions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_precaution_code",
                table: "m34_precaution_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_interaction_pat_code",
                table: "m34_interaction_pat_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_interaction_pat",
                table: "m34_interaction_pat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_indication_code",
                table: "m34_indication_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_form_code",
                table: "m34_form_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_drug_info_main",
                table: "m34_drug_info_main");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_ar_discon_code",
                table: "m34_ar_discon_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_ar_discon",
                table: "m34_ar_discon");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m34_ar_code",
                table: "m34_ar_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m28_drug_mst",
                table: "m28_drug_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m14_cmt_code",
                table: "m14_cmt_code");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m14_age_check",
                table: "m14_age_check");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m12_food_alrgy_kbn",
                table: "m12_food_alrgy_kbn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m12_food_alrgy",
                table: "m12_food_alrgy");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m10_day_limit",
                table: "m10_day_limit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m01_kinki_cmt",
                table: "m01_kinki_cmt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m01_kinki",
                table: "m01_kinki");

            migrationBuilder.DropPrimaryKey(
                name: "PK_m01_kijyo_cmt",
                table: "m01_kijyo_cmt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_koui_kbn_mst",
                table: "koui_kbn_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_kohi_priority",
                table: "kohi_priority");

            migrationBuilder.DropPrimaryKey(
                name: "PK_kogaku_limit",
                table: "kogaku_limit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_kantoku_mst",
                table: "kantoku_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_kacode_rece_yousiki",
                table: "kacode_rece_yousiki");

            migrationBuilder.DropPrimaryKey(
                name: "PK_kacode_mst",
                table: "kacode_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ipn_name_mst",
                table: "ipn_name_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ipn_min_yakka_mst",
                table: "ipn_min_yakka_mst");

            migrationBuilder.DropIndex(
                name: "ipn_min_yakka_mst_idx01",
                table: "ipn_min_yakka_mst");

            migrationBuilder.DropIndex(
                name: "ipn_min_yakka_mst_idx02",
                table: "ipn_min_yakka_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ipn_kasan_mst",
                table: "ipn_kasan_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ipn_kasan_exclude_item",
                table: "ipn_kasan_exclude_item");

            migrationBuilder.DropIndex(
                name: "ipn_kasan_exclude_item_idx01",
                table: "ipn_kasan_exclude_item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ipn_kasan_exclude",
                table: "ipn_kasan_exclude");

            migrationBuilder.DropIndex(
                name: "ipn_kasan_exclude_idx01",
                table: "ipn_kasan_exclude");

            migrationBuilder.DropPrimaryKey(
                name: "PK_gc_std_mst",
                table: "gc_std_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_function_mst",
                table: "function_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_event_mst",
                table: "event_mst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_drug_unit_conv",
                table: "drug_unit_conv");

            migrationBuilder.DropPrimaryKey(
                name: "PK_byomei_mst_aftercare",
                table: "byomei_mst_aftercare");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "unit_mst");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "syouki_kbn_mst");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "roudou_mst");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "pi_product_inf");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "pi_inf_detail");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "pi_inf");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "physical_average");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "permission_mst");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m56_yj_drug_class");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m56_usage_code");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m56_prodrug_cd");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m56_ex_ingrdt_main");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m56_ex_ing_code");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m56_ex_ed_ingredients");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m56_ex_analogue");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m56_drvalrgy_code");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m56_drug_class");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m56_analogue_cd");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m56_alrgy_derivatives");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m46_dosage_drug");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m46_dosage_dosage");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m42_contraindi_drug_main_ex");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m42_contraindi_dis_con");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m42_contraindi_dis_class");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m42_contraindi_dis_bc");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m42_contra_cmt");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m41_supple_ingre");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m41_supple_indexdef");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m41_supple_indexcode");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m38_otc_maker_code");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m38_otc_main");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m38_otc_form_code");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m38_major_div_code");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m38_ingredients");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m38_ing_code");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m38_class_code");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m34_sar_symptom_code");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m34_property_code");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m34_precautions");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m34_precaution_code");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m34_interaction_pat_code");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m34_interaction_pat");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m34_indication_code");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m34_form_code");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m34_drug_info_main");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m34_ar_discon_code");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m34_ar_discon");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m34_ar_code");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m28_drug_mst");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m14_cmt_code");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m14_age_check");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m12_food_alrgy_kbn");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m12_food_alrgy");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m10_day_limit");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m01_kinki_cmt");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m01_kinki");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "m01_kijyo_cmt");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "kohi_priority");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "kantoku_mst");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "kacode_rece_yousiki");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "kacode_mst");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "function_mst");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "event_mst");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "drug_unit_conv");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "byomei_mst_aftercare");

            migrationBuilder.AlterColumn<int>(
                name: "start_date",
                table: "yakka_syusai_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<string>(
                name: "item_cd",
                table: "yakka_syusai_mst",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10)
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<string>(
                name: "yakka_cd",
                table: "yakka_syusai_mst",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(12)",
                oldMaxLength: 12)
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "yakka_syusai_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "unit_cd",
                table: "unit_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<string>(
                name: "tokki_cd",
                table: "tokki_mst",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2)
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "tokki_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "report_id",
                table: "sta_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 2)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "sta_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "report_id",
                table: "sta_grp",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "grp_id",
                table: "sta_grp",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "sta_grp",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "report_id",
                table: "sokatu_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<int>(
                name: "report_eda_no",
                table: "sokatu_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 5);

            migrationBuilder.AlterColumn<int>(
                name: "start_ym",
                table: "sokatu_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "pref_no",
                table: "sokatu_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "sokatu_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "start_date",
                table: "rousai_gosei_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 6);

            migrationBuilder.AlterColumn<int>(
                name: "sisi_kbn",
                table: "rousai_gosei_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 5);

            migrationBuilder.AlterColumn<string>(
                name: "item_cd",
                table: "rousai_gosei_mst",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10)
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<string>(
                name: "gosei_item_cd",
                table: "rousai_gosei_mst",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10)
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "gosei_grp",
                table: "rousai_gosei_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "rousai_gosei_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<string>(
                name: "item_cd",
                table: "pi_image",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10)
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "image_type",
                table: "pi_image",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "pi_image",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "serial_num",
                table: "m38_otc_main",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "property_cd",
                table: "m34_property_code",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "koui_kbn_id",
                table: "koui_kbn_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 2)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "koui_kbn_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "start_date",
                table: "kogaku_limit",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<int>(
                name: "kogaku_kbn",
                table: "kogaku_limit",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "age_kbn",
                table: "kogaku_limit",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "kogaku_limit",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "seq_no",
                table: "ipn_name_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<int>(
                name: "start_date",
                table: "ipn_name_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<string>(
                name: "ipn_name_cd",
                table: "ipn_name_mst",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(12)",
                oldMaxLength: 12)
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "ipn_name_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "seq_no",
                table: "ipn_min_yakka_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<string>(
                name: "ipn_name_cd",
                table: "ipn_min_yakka_mst",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(12)",
                oldMaxLength: 12)
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "ipn_min_yakka_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 1)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "ipn_min_yakka_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "seq_no",
                table: "ipn_kasan_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<string>(
                name: "ipn_name_cd",
                table: "ipn_kasan_mst",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(12)",
                oldMaxLength: 12)
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "start_date",
                table: "ipn_kasan_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "ipn_kasan_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<string>(
                name: "item_cd",
                table: "ipn_kasan_exclude_item",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10)
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "start_date",
                table: "ipn_kasan_exclude_item",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "ipn_kasan_exclude_item",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "seq_no",
                table: "ipn_kasan_exclude",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<string>(
                name: "ipn_name_cd",
                table: "ipn_kasan_exclude",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(12)",
                oldMaxLength: 12)
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "start_date",
                table: "ipn_kasan_exclude",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "ipn_kasan_exclude",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<double>(
                name: "point",
                table: "gc_std_mst",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision")
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<int>(
                name: "sex",
                table: "gc_std_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "std_kbn",
                table: "gc_std_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "gc_std_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_yakka_syusai_mst",
                table: "yakka_syusai_mst",
                columns: new[] { "hp_id", "yakka_cd", "item_cd", "start_date" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_unit_mst",
                table: "unit_mst",
                column: "unit_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tokki_mst",
                table: "tokki_mst",
                columns: new[] { "hp_id", "tokki_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_syouki_kbn_mst",
                table: "syouki_kbn_mst",
                columns: new[] { "syouki_kbn", "start_ym" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_sta_mst",
                table: "sta_mst",
                columns: new[] { "hp_id", "report_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_sta_grp",
                table: "sta_grp",
                columns: new[] { "hp_id", "grp_id", "report_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_sokatu_mst",
                table: "sokatu_mst",
                columns: new[] { "hp_id", "pref_no", "start_ym", "report_eda_no", "report_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_rousai_gosei_mst",
                table: "rousai_gosei_mst",
                columns: new[] { "hp_id", "gosei_grp", "gosei_item_cd", "item_cd", "sisi_kbn", "start_date" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_roudou_mst",
                table: "roudou_mst",
                column: "roudou_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_pi_product_inf",
                table: "pi_product_inf",
                columns: new[] { "pi_id_full", "pi_id", "branch", "jpn" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_pi_inf_detail",
                table: "pi_inf_detail",
                columns: new[] { "pi_id", "branch", "jpn", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_pi_inf",
                table: "pi_inf",
                column: "pi_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_pi_image",
                table: "pi_image",
                columns: new[] { "hp_id", "image_type", "item_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_physical_average",
                table: "physical_average",
                columns: new[] { "jissi_year", "age_year", "age_month", "age_day" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_permission_mst",
                table: "permission_mst",
                columns: new[] { "function_cd", "permission" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_yj_drug_class",
                table: "m56_yj_drug_class",
                columns: new[] { "yj_cd", "class_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_usage_code",
                table: "m56_usage_code",
                column: "yoho_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_prodrug_cd",
                table: "m56_prodrug_cd",
                columns: new[] { "seq_no", "seibun_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_ex_ingrdt_main",
                table: "m56_ex_ingrdt_main",
                column: "yj_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_ex_ing_code",
                table: "m56_ex_ing_code",
                columns: new[] { "seibun_cd", "seibun_index_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_ex_ed_ingredients",
                table: "m56_ex_ed_ingredients",
                columns: new[] { "yj_cd", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_ex_analogue",
                table: "m56_ex_analogue",
                columns: new[] { "seibun_cd", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_drvalrgy_code",
                table: "m56_drvalrgy_code",
                column: "drvalrgy_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_drug_class",
                table: "m56_drug_class",
                column: "class_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_analogue_cd",
                table: "m56_analogue_cd",
                column: "analogue_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m56_alrgy_derivatives",
                table: "m56_alrgy_derivatives",
                columns: new[] { "seibun_cd", "drvalrgy_cd", "yj_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m46_dosage_drug",
                table: "m46_dosage_drug",
                columns: new[] { "doei_cd", "yj_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m46_dosage_dosage",
                table: "m46_dosage_dosage",
                columns: new[] { "doei_cd", "doei_seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m42_contraindi_drug_main_ex",
                table: "m42_contraindi_drug_main_ex",
                columns: new[] { "yj_cd", "tenpu_level", "byotai_cd", "cmt_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m42_contraindi_dis_con",
                table: "m42_contraindi_dis_con",
                column: "byotai_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m42_contraindi_dis_class",
                table: "m42_contraindi_dis_class",
                column: "byotai_class_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m42_contraindi_dis_bc",
                table: "m42_contraindi_dis_bc",
                columns: new[] { "byotai_cd", "byotai_class_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m42_contra_cmt",
                table: "m42_contra_cmt",
                column: "cmt_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m41_supple_ingre",
                table: "m41_supple_ingre",
                column: "seibun_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m41_supple_indexdef",
                table: "m41_supple_indexdef",
                column: "seibun_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m41_supple_indexcode",
                table: "m41_supple_indexcode",
                columns: new[] { "seibun_cd", "index_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m38_otc_maker_code",
                table: "m38_otc_maker_code",
                column: "maker_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m38_otc_main",
                table: "m38_otc_main",
                column: "serial_num");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m38_otc_form_code",
                table: "m38_otc_form_code",
                column: "form_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m38_major_div_code",
                table: "m38_major_div_code",
                column: "major_div_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m38_ing_code",
                table: "m38_ing_code",
                column: "seibun_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m38_class_code",
                table: "m38_class_code",
                column: "class_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_sar_symptom_code",
                table: "m34_sar_symptom_code",
                column: "fukusayo_init_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_property_code",
                table: "m34_property_code",
                column: "property_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_precautions",
                table: "m34_precautions",
                columns: new[] { "yj_cd", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_precaution_code",
                table: "m34_precaution_code",
                columns: new[] { "precaution_cd", "extend_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_interaction_pat_code",
                table: "m34_interaction_pat_code",
                column: "interaction_pat_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_interaction_pat",
                table: "m34_interaction_pat",
                columns: new[] { "yj_cd", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_indication_code",
                table: "m34_indication_code",
                column: "kono_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_form_code",
                table: "m34_form_code",
                column: "form_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_drug_info_main",
                table: "m34_drug_info_main",
                column: "yj_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_ar_discon_code",
                table: "m34_ar_discon_code",
                column: "fukusayo_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_ar_discon",
                table: "m34_ar_discon",
                columns: new[] { "yj_cd", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m34_ar_code",
                table: "m34_ar_code",
                column: "fukusayo_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m28_drug_mst",
                table: "m28_drug_mst",
                column: "yj_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m14_cmt_code",
                table: "m14_cmt_code",
                column: "attention_cmt_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m14_age_check",
                table: "m14_age_check",
                columns: new[] { "yj_cd", "attention_cmt_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m12_food_alrgy_kbn",
                table: "m12_food_alrgy_kbn",
                column: "food_kbn");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m12_food_alrgy",
                table: "m12_food_alrgy",
                columns: new[] { "yj_cd", "food_kbn", "tenpu_level" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m10_day_limit",
                table: "m10_day_limit",
                columns: new[] { "yj_cd", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m01_kinki_cmt",
                table: "m01_kinki_cmt",
                column: "cmt_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m01_kinki",
                table: "m01_kinki",
                columns: new[] { "a_cd", "b_cd", "cmt_cd", "sayokijyo_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_m01_kijyo_cmt",
                table: "m01_kijyo_cmt",
                column: "cmt_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_koui_kbn_mst",
                table: "koui_kbn_mst",
                columns: new[] { "hp_id", "koui_kbn_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_kohi_priority",
                table: "kohi_priority",
                columns: new[] { "pref_no", "houbetu", "priority_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_kogaku_limit",
                table: "kogaku_limit",
                columns: new[] { "hp_id", "age_kbn", "kogaku_kbn", "start_date" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_kantoku_mst",
                table: "kantoku_mst",
                columns: new[] { "roudou_cd", "kantoku_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_kacode_rece_yousiki",
                table: "kacode_rece_yousiki",
                columns: new[] { "rece_ka_cd", "yousiki_ka_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_kacode_mst",
                table: "kacode_mst",
                column: "rece_ka_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ipn_name_mst",
                table: "ipn_name_mst",
                columns: new[] { "hp_id", "ipn_name_cd", "start_date", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ipn_min_yakka_mst",
                table: "ipn_min_yakka_mst",
                columns: new[] { "hp_id", "id", "ipn_name_cd", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ipn_kasan_mst",
                table: "ipn_kasan_mst",
                columns: new[] { "hp_id", "start_date", "ipn_name_cd", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ipn_kasan_exclude_item",
                table: "ipn_kasan_exclude_item",
                columns: new[] { "hp_id", "start_date", "item_cd" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ipn_kasan_exclude",
                table: "ipn_kasan_exclude",
                columns: new[] { "hp_id", "start_date", "ipn_name_cd", "seq_no" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_gc_std_mst",
                table: "gc_std_mst",
                columns: new[] { "hp_id", "std_kbn", "sex", "point" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_function_mst",
                table: "function_mst",
                column: "function_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_event_mst",
                table: "event_mst",
                column: "event_cd");

            migrationBuilder.AddPrimaryKey(
                name: "PK_drug_unit_conv",
                table: "drug_unit_conv",
                columns: new[] { "item_cd", "start_date" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_byomei_mst_aftercare",
                table: "byomei_mst_aftercare",
                columns: new[] { "byomei_cd", "byomei", "start_date" });

            migrationBuilder.CreateTable(
                name: "yoyaku_odr_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    yoyakukarteno = table.Column<long>(name: "yoyaku_karte_no", type: "bigint", nullable: false),
                    rpno = table.Column<long>(name: "rp_no", type: "bigint", nullable: false),
                    rpedano = table.Column<long>(name: "rp_eda_no", type: "bigint", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    dayscnt = table.Column<int>(name: "days_cnt", type: "integer", nullable: false),
                    inoutkbn = table.Column<int>(name: "inout_kbn", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    odrkouikbn = table.Column<int>(name: "odr_koui_kbn", type: "integer", nullable: false),
                    rpname = table.Column<string>(name: "rp_name", type: "character varying(120)", maxLength: 120, nullable: true),
                    santeikbn = table.Column<int>(name: "santei_kbn", type: "integer", nullable: false),
                    sikyukbn = table.Column<int>(name: "sikyu_kbn", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    syohosbt = table.Column<int>(name: "syoho_sbt", type: "integer", nullable: false),
                    tosekikbn = table.Column<int>(name: "toseki_kbn", type: "integer", nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    yoyakudate = table.Column<int>(name: "yoyaku_date", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_yoyaku_odr_inf", x => new { x.hpid, x.ptid, x.yoyakukarteno, x.rpno, x.rpedano });
                });

            migrationBuilder.CreateTable(
                name: "yoyaku_odr_inf_detail",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    yoyakukarteno = table.Column<long>(name: "yoyaku_karte_no", type: "bigint", nullable: false),
                    rpno = table.Column<long>(name: "rp_no", type: "bigint", nullable: false),
                    rpedano = table.Column<long>(name: "rp_eda_no", type: "bigint", nullable: false),
                    rowno = table.Column<long>(name: "row_no", type: "bigint", nullable: false),
                    bunkatu = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    cmtname = table.Column<string>(name: "cmt_name", type: "character varying(32)", maxLength: 32, nullable: true),
                    cmtopt = table.Column<string>(name: "cmt_opt", type: "character varying(38)", maxLength: 38, nullable: true),
                    drugkbn = table.Column<int>(name: "drug_kbn", type: "integer", nullable: false),
                    fontcolor = table.Column<int>(name: "font_color", type: "integer", nullable: false),
                    ipncd = table.Column<string>(name: "ipn_cd", type: "character varying(12)", maxLength: 12, nullable: true),
                    ipnname = table.Column<string>(name: "ipn_name", type: "character varying(120)", maxLength: 120, nullable: true),
                    isnodsprece = table.Column<int>(name: "is_nodsp_rece", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    itemname = table.Column<string>(name: "item_name", type: "character varying(120)", maxLength: 120, nullable: true),
                    kohatukbn = table.Column<int>(name: "kohatu_kbn", type: "integer", nullable: false),
                    kokuji1 = table.Column<int>(type: "integer", nullable: false),
                    sinkouikbn = table.Column<int>(name: "sin_koui_kbn", type: "integer", nullable: false),
                    suryo = table.Column<double>(type: "double precision", nullable: false),
                    syohokbn = table.Column<int>(name: "syoho_kbn", type: "integer", nullable: false),
                    syoholimitkbn = table.Column<int>(name: "syoho_limit_kbn", type: "integer", nullable: false),
                    termval = table.Column<double>(name: "term_val", type: "double precision", nullable: false),
                    unitname = table.Column<string>(name: "unit_name", type: "character varying(24)", maxLength: 24, nullable: true),
                    unitsbt = table.Column<int>(name: "unit_sbt", type: "integer", nullable: false),
                    yohokbn = table.Column<int>(name: "yoho_kbn", type: "integer", nullable: false),
                    yoyakudate = table.Column<int>(name: "yoyaku_date", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_yoyaku_odr_inf_detail", x => new { x.hpid, x.ptid, x.yoyakukarteno, x.rpno, x.rpedano, x.rowno });
                });

            migrationBuilder.CreateTable(
                name: "yoyaku_sbt_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    yoyakusbt = table.Column<int>(name: "yoyaku_sbt", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    defaultcmt = table.Column<string>(name: "default_cmt", type: "character varying(120)", maxLength: 120, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    sbtname = table.Column<string>(name: "sbt_name", type: "character varying(120)", maxLength: 120, nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_yoyaku_sbt_mst", x => new { x.hpid, x.yoyakusbt });
                });

            migrationBuilder.CreateIndex(
                name: "ipn_min_yakka_mst_idx01",
                table: "ipn_min_yakka_mst",
                columns: new[] { "hp_id", "ipn_name_cd", "start_date" });

            migrationBuilder.CreateIndex(
                name: "ipn_min_yakka_mst_idx02",
                table: "ipn_min_yakka_mst",
                columns: new[] { "hp_id", "start_date", "end_date", "ipn_name_cd" });

            migrationBuilder.CreateIndex(
                name: "ipn_kasan_exclude_item_idx01",
                table: "ipn_kasan_exclude_item",
                columns: new[] { "hp_id", "start_date", "end_date", "item_cd" });

            migrationBuilder.CreateIndex(
                name: "ipn_kasan_exclude_idx01",
                table: "ipn_kasan_exclude",
                columns: new[] { "hp_id", "start_date", "end_date", "ipn_name_cd" });
        }
    }
}
