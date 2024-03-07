---
create trigger doc_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."doc_inf" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger filing_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."filing_inf" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger kensa_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."kensa_inf" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger kensa_inf_detail_history_trigger after
insert
    or
delete
    or
update
    on
    public."kensa_inf_detail" for each row execute function "INSERT_HISTORY_TABLE"();
--- 
create trigger limit_cnt_list_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."limit_cnt_list_inf" for each row execute function "INSERT_LIMIT_INF_HISTORY_TABLE"();
---
create trigger limit_list_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."limit_list_inf" for each row execute function "INSERT_LIMIT_INF_HISTORY_TABLE"();
---
create trigger monshin_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."monshin_inf" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_alrgy_drug_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_alrgy_drug" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_alrgy_else_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_alrgy_else" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_alrgy_food_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_alrgy_food" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_cmt_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_cmt_inf" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_family_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_family" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_family_reki_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_family_reki" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_grp_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_grp_inf" for each row execute function "INSERT_HISTORY_TABLE"();
--- 
create trigger pt_hoken_check_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_hoken_check" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_hoken_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_hoken_inf" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_hoken_pattern_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_hoken_pattern" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_hoken_scan_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_hoken_scan" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_inf" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_infection_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_infection" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_jibkar_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_jibkar" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_kio_reki_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_kio_reki" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_kohi_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_kohi" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_kyusei_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_kyusei" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_memo_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_memo" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_otc_drug_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_otc_drug" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_other_drug_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_other_drug" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_pregnancy_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_pregnancy" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_rousai_tenki_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_rousai_tenki" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_santei_conf_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_santei_conf" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_supple_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_supple" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger pt_tag_history_trigger after
insert
    or
delete
    or
update
    on
    public."pt_tag" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger raiin_cmt_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."raiin_cmt_inf" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger raiin_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."raiin_inf" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger raiin_kbn_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."raiin_kbn_inf" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger raiin_list_cmt_history_trigger after
insert
    or
delete
    or
update
    on
    public."raiin_list_cmt" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger raiin_list_tag_history_trigger after
insert
    or
delete
    or
update
    on
    public."raiin_list_tag" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger rece_check_cmt_history_trigger after
insert
    or
delete
    or
update
    on
    public."rece_check_cmt" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger rece_cmt_history_trigger after
insert
    or
delete
    or
update
    on
    public."rece_cmt" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger rece_inf_edit_history_trigger after
insert
    or
delete
    or
update
    on
    public."rece_inf_edit" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger rece_seikyu_history_trigger after
insert
    or
delete
    or
update
    on
    public."rece_seikyu" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger rsv_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."rsv_inf" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger santei_inf_detail_history_trigger after
insert
    or
delete
    or
update
    on
    public."santei_inf_detail" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger seikatureki_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."seikatureki_inf" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger summary_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."summary_inf" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger syobyo_keika_history_trigger after
insert
    or
delete
    or
update
    on
    public."syobyo_keika" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger syouki_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."syouki_inf" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger syuno_nyukin_history_trigger after
insert
    or
delete
    or
update
    on
    public."syuno_nyukin" for each row execute function "INSERT_HISTORY_TABLE"();
---
create trigger todo_inf_history_trigger after
insert
    or
delete
    or
update
    on
    public."todo_inf" for each row execute function "INSERT_HISTORY_TABLE"();
---

