CREATE OR REPLACE FUNCTION public.addseqgroupnotrigger_function()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
begin
  if (select count(*) from "kensa_inf_detail" where "irai_cd" = NEW."irai_cd" and "kensa_item_cd" = NEW."kensa_item_cd" and "is_deleted" = 0) > 0 
  --then NEW."SEQ_GROUP_NO" := (select max("SEQ_GROUP_NO") from "KENSA_INF_DETAIL" where "IRAI_CD" = NEW."IRAI_CD" and "KENSA_ITEM_CD" = NEW."KENSA_ITEM_CD" and "IS_DELETED" = 0) + 1;
 then NEW."seq_group_no" := (select count(*) from "kensa_inf_detail" where "irai_cd" = NEW."irai_cd" and "kensa_item_cd" = NEW."kensa_item_cd" and "is_deleted" = 0);
  end if;
return new;
end;
$function$
;

---
CREATE OR REPLACE FUNCTION public."INSERT_HISTORY_TABLE"()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$DECLARE
    HISTORY_TABLE_NAME character varying(63);
    clientAddr varchar(100);
    clientHostName varchar(100);
BEGIN
--
-- Create a row in history table to reflect the operation performed on original one,
-- make use of the special variable TG_OP to work out the operation.
--
HISTORY_TABLE_NAME := 'z_' || TG_TABLE_NAME;
select CLIENT_ADDR, CLIENT_HOSTNAME from pg_stat_activity where pid = pg_backend_pid() into clientAddr, clientHostName;

if (TG_OP = 'INSERT') then
    execute format('INSERT INTO %I '
                   'SELECT $1, $2, $3, $4, $5, $6.* ON CONFLICT ("op_id") DO NOTHING', HISTORY_TABLE_NAME)
                   using nextval('"OP_ID_seq"'::regclass), 'INSERT', current_timestamp, clientAddr, clientHostName, new;
    return new;
elsif (TG_OP = 'UPDATE') then
    execute format('INSERT INTO %I '  
                   'SELECT $1, $2, $3, $4, $5, $6.* ON CONFLICT ("op_id") DO NOTHING', HISTORY_TABLE_NAME)
                    using nextval('"OP_ID_seq"'::regclass), 'UPDATE', current_timestamp, clientAddr, clientHostName, new;
    return new;
elsif (TG_OP = 'DELETE') then
    execute format('INSERT INTO %I '  
                   'SELECT $1, $2, $3, $4, $5, $6.* ON CONFLICT ("op_id") DO NOTHING', HISTORY_TABLE_NAME)
                    using nextval('"OP_ID_seq"'::regclass), 'DELETE', current_timestamp, clientAddr, clientHostName, old;
    return old;
end if;
return null; -- result is ignored since this is an AFTER trigger
end;$function$
;
---
CREATE OR REPLACE FUNCTION public."INSERT_LIMIT_INF_HISTORY_TABLE"()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$DECLARE
    REVISION_NUMBER bigint;
    HISTORY_TABLE_NAME character varying(63);
    clientAddr varchar(100);
    clientHostName varchar(100);
	hokenPid int;
BEGIN
--
-- Create a row in history table to reflect the operation performed on original one,
-- make use of the special variable TG_OP to work out the operation.
--
hokenPid = NEW."hoken_pid";
if (hokenPid != 0) then return null;
end if;

hokenPid = OLD."hoken_pid";
if (hokenPid != 0) then return null;
end if;

HISTORY_TABLE_NAME := 'z_' || TG_TABLE_NAME;
execute format('SELECT COALESCE(MAX("op_id"), 0) FROM %I ', HISTORY_TABLE_NAME) into REVISION_NUMBER;
REVISION_NUMBER := REVISION_NUMBER + 1;
select CLIENT_ADDR, CLIENT_HOSTNAME from pg_stat_activity where pid = pg_backend_pid() into clientAddr, clientHostName;

if (TG_OP = 'INSERT') then
    execute format('INSERT INTO %I '
                   'SELECT $1, $2, $3, $4, $5, $6.*', HISTORY_TABLE_NAME)
                   using REVISION_NUMBER, 'INSERT', current_timestamp, clientAddr, clientHostName, new;
    return new;
elsif (TG_OP = 'UPDATE') then
    execute format('INSERT INTO %I '  
                   'SELECT $1, $2, $3, $4, $5, $6.*', HISTORY_TABLE_NAME)
                    using REVISION_NUMBER, 'UPDATE', current_timestamp, clientAddr, clientHostName, new;
    return new;
elsif (TG_OP = 'DELETE') then
    execute format('INSERT INTO %I '  
                   'SELECT $1, $2, $3, $4, $5, $6.*', HISTORY_TABLE_NAME)
                    using REVISION_NUMBER, 'DELETE', current_timestamp, clientAddr, clientHostName, old;
    return old;
end if;
return null; -- result is ignored since this is an AFTER trigger
end;$function$
;

---
CREATE OR REPLACE FUNCTION public.renkei_req_trg01_func()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
	DECLARE
		iHpCd     "renkei_req"."hp_id"%TYPE;
		iKanId    "renkei_req"."pt_id"%TYPE;
		sUpdFg    "renkei_req"."req_type"%TYPE;
	BEGIN
		iHpCd  = NEW."hp_id";
		iKanId = NEW."pt_id";

		IF (TG_OP = 'INSERT' AND (NEW."is_delete" = 0 OR NEW."is_delete" IS NULL)) OR
		   (TG_OP = 'UPDATE' AND OLD."is_delete" = 1 AND NEW."is_delete" = 0) THEN
			IF NEW."name" IS NOT NULL AND
			   NEW."kana_name" IS NOT NULL AND
			   NEW."sex" IS NOT NULL        AND NEW."birthday" IS NOT NULL THEN
				sUpdFg = 'I';
			END IF;
		ELSIF TG_OP = 'UPDATE' AND (NEW."is_delete" = 0 OR NEW."is_delete" IS NULL) THEN
			IF (OLD."name" <> NEW."name") OR (OLD."name" IS NULL AND NEW."name" IS NOT NULL) OR
			   (OLD."kana_name" <> NEW."kana_name") OR (OLD."kana_name" IS NULL AND NEW."kana_name" IS NOT NULL) OR
			   (OLD."sex"        <> NEW."sex") OR (OLD."sex" IS NULL AND NEW."sex" IS NOT NULL) OR
			   (OLD."birthday"    <> NEW."birthday") OR  (OLD."birthday" IS NULL AND NEW."birthday" IS NOT NULL) THEN
				sUpdFg = 'U';
			END IF;
		ELSIF (TG_OP = 'INSERT' AND NEW."is_delete" = 1) THEN
			sUpdFg = 'D';
		ELSIF TG_OP = 'DELETE' OR (TG_OP = 'UPDATE' AND NEW."is_delete" = 1) THEN
			sUpdFg = 'D';
			iHpCd  = OLD."hp_id";
			iKanId = OLD."pt_id";
		END IF;

		IF sUpdFg IS NOT NULL THEN
			INSERT INTO "renkei_req"
				(
					"hp_id",
					"pt_id",
					"raiin_no",
					"req_sbt",
					"req_type",
					"create_date",
					"create_id",
					"update_date",
					"update_id"
				) VALUES (
					iHpCd,
					iKanId,
					0,
					1,
					sUpdFg,
					NOW(),
					0,
					NOW(),
					0
				);
		END IF;
		 RETURN NULL;
	   END;
$function$
;

---
CREATE OR REPLACE FUNCTION public.renkei_req_trg02_func()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
DECLARE
		sUpdFg    "renkei_req"."req_type"%TYPE;
BEGIN
	IF TG_OP = 'INSERT' AND NEW."status" >= 1 THEN
		  sUpdFg = 'I';
ELSIF TG_OP = 'UPDATE' AND OLD."status" = 0 AND NEW."status" >= 1 THEN
		  sUpdFg = 'U';
END
IF;

		IF sUpdFg IS NOT NULL THEN
INSERT INTO "renkei_req"
	(
	"hp_id",
	"pt_id",
	"raiin_no",
	"req_sbt",
	"req_type",
	"create_date",
	"create_id",
	"update_date",
	"update_id"
	)
VALUES
	(
		NEW."hp_id",
		NEW."pt_id",
		NEW."raiin_no",
		2,
		sUpdFg,
		NOW(),
		0,
		NOW(),
		0
				);
END
IF;
		 RETURN NULL;
END;
$function$
;

---
CREATE OR REPLACE FUNCTION public.update_pt_last_visit_date_func()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
DECLARE
   _LAST_VISIT_DATE bigint;
   COUNT_LAST_VISIT_DATE bigint;
   begin
	   
   EXECUTE format('SELECT COALESCE(MAX("sin_date"), 0) FROM "raiin_inf" WHERE "status" >= 3 AND "hp_id"=$1 AND "pt_id"=$2 AND "is_deleted" = 0') USING NEW."hp_id", NEW."pt_id" INTO _LAST_VISIT_DATE;
   EXECUTE format('SELECT COUNT(*) FROM "pt_last_visit_date" WHERE "hp_id"=$1 AND "pt_id"=$2') USING NEW."hp_id", NEW."pt_id" INTO COUNT_LAST_VISIT_DATE;
   IF (COUNT_LAST_VISIT_DATE = 0) THEN
        insert into "pt_last_visit_date" 
        values(
        new."hp_id", 
        new."pt_id", 
        _LAST_VISIT_DATE, 
        current_timestamp, 
        new."update_id", 
        new."update_machine", 
        current_timestamp, 
        new."update_id", 
        new."update_machine");
   	RETURN NEW;
   else
   UPDATE "pt_last_visit_date" SET
            "last_visit_date" = _LAST_VISIT_DATE,
            "update_id"= new."update_id",
            "update_date"= current_timestamp,
            "update_machine"= new."update_machine"
       WHERE
            "hp_id" = NEW."hp_id"
        AND "pt_id" = NEW."pt_id";
   RETURN NEW;
    END IF;
     RETURN NULL;
   END;
$function$
;

---
CREATE OR REPLACE FUNCTION public.update_raiin_inf_func()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
begin
	IF (NEW."item_cd" = '@SHIN') THEN
	UPDATE "raiin_inf" SET
            "syosaisin_kbn"         = NEW."suryo"
        WHERE
            "hp_id"         = NEW."hp_id"
		AND "pt_id"           = NEW."pt_id"
		AND "raiin_no" = NEW."raiin_no";
	RETURN NEW;
	ELSIF
	(NEW."item_cd" = '@JIKAN') THEN
	UPDATE "raiin_inf" SET
            "jikan_kbn"         = NEW."suryo"
       WHERE
            "hp_id"         = NEW."hp_id"
		AND "pt_id"           = NEW."pt_id"
		AND "raiin_no" = NEW."raiin_no";
	RETURN NEW;
END
IF;
     RETURN NULL;
END;
$function$
;
---
CREATE OR REPLACE FUNCTION public.updateseqgroupnotrigger_function()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$   
DECLARE 
    sequennocalculate numeric = 0;
    minSeqNo numeric = 0;
    maxSeqNo numeric = 0;
    cnt numeric = 0;
BEGIN    
	IF NEW."is_deleted" = 1 THEN
	    minSeqNo = (select  min("seq_no") from "kensa_inf_detail" where "hp_id" = new."hp_id" and "irai_cd" = new."irai_cd" and "kensa_item_cd" = new."kensa_item_cd" and "is_deleted" = 0 and new."seq_no" != "seq_no");
	    maxSeqNo = (select  max("seq_no") from "kensa_inf_detail" where "hp_id" = new."hp_id" and "irai_cd" = new."irai_cd" and "kensa_item_cd" = new."kensa_item_cd" and "is_deleted" = 0 and new."seq_no" != "seq_no");
	   if minSeqNo is null then return old; end if;
	   if maxSeqNo is null then return old; end if;
	   for cnt in minSeqNo..maxSeqNo loop
	    if (select count(*) from "kensa_inf_detail" where "seq_no" = cnt and "is_deleted" = 0 ) > 0
	    then 
	    	update "kensa_inf_detail" set "seq_group_no" = sequennocalculate where "seq_no" = cnt and "is_deleted" = 0;
	    	sequennocalculate = sequennocalculate + 1;
	    END if;
	    end loop;
	END if;
	RETURN new;
end $function$
;
