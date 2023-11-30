---
create trigger doc_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."DOC_INF" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger filing_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."FILING_INF" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger kensa_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."KENSA_INF" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger kensa_inf_detail_history_trigger after
insert
    or
delete
    or
update
    on
    public."KENSA_INF_DETAIL" for each row execute function "INSERT_HISTORY_TABLE"();
--- 
create trigger limit_cnt_list_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."LIMIT_CNT_LIST_INF" for each row execute function "INSERT_LIMIT_INF_HISTORY_TABLE"();
---
create trigger limit_list_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."LIMIT_LIST_INF" for each row execute function "INSERT_LIMIT_INF_HISTORY_TABLE"();
---
create trigger monshin_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."MONSHIN_INF" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_alrgy_drug_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_ALRGY_DRUG" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_alrgy_else_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_ALRGY_ELSE" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_alrgy_food_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_ALRGY_FOOD" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_cmt_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_CMT_INF" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_family_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_FAMILY" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_family_reki_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_FAMILY_REKI" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_grp_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_GRP_INF" for each row execute function "INSERT_HISTORY_TABLE"();
--- 
create trigger pt_hoken_check_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_HOKEN_CHECK" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_hoken_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_HOKEN_INF" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_hoken_pattern_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_HOKEN_PATTERN" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_hoken_scan_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_HOKEN_SCAN" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_INF" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_infection_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_INFECTION" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_jibkar_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_JIBKAR" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_kio_reki_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_KIO_REKI" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_kohi_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_KOHI" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_kyusei_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_KYUSEI" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_memo_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_MEMO" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_otc_drug_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_OTC_DRUG" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_other_drug_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_OTHER_DRUG" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_pregnancy_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_PREGNANCY" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_rousai_tenki_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_ROUSAI_TENKI" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_santei_conf_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_SANTEI_CONF" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_supple_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_SUPPLE" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_tag_history_trigger after
insert
    or
delete
    or
update
    on
    public."PT_TAG" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger raiin_cmt_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."RAIIN_CMT_INF" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger raiin_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."RAIIN_INF" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger raiin_kbn_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."RAIIN_KBN_INF" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger raiin_list_cmt_history_trigger after
insert
    or
delete
    or
update
    on
    public."RAIIN_LIST_CMT" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger raiin_list_tag_history_trigger after
insert
    or
delete
    or
update
    on
    public."RAIIN_LIST_TAG" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger rece_check_cmt_history_trigger after
insert
    or
delete
    or
update
    on
    public."RECE_CHECK_CMT" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger rece_cmt_history_trigger after
insert
    or
delete
    or
update
    on
    public."RECE_CMT" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger rece_inf_edit_history_trigger after
insert
    or
delete
    or
update
    on
    public."RECE_INF_EDIT" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger rece_seikyu_history_trigger after
insert
    or
delete
    or
update
    on
    public."RECE_SEIKYU" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger rsv_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."RSV_INF" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger santei_inf_detail_history_trigger after
insert
    or
delete
    or
update
    on
    public."SANTEI_INF_DETAIL" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger seikatureki_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."SEIKATUREKI_INF" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger summary_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."SUMMARY_INF" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger syobyo_keika_history_trigger after
insert
    or
delete
    or
update
    on
    public."SYOBYO_KEIKA" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger syouki_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."SYOUKI_INF" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger syuno_nyukin_history_trigger after
insert
    or
delete
    or
update
    on
    public."SYUNO_NYUKIN" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger todo_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."TODO_INF" for each row execute function "INSERT_HISTORY_TABLE"();
---

