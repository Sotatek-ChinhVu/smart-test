CREATE OR REPLACE FUNCTION public.addseqgroupnotrigger_function()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
begin
  if (select count(*) from "KENSA_INF_DETAIL" where "IRAI_CD" = NEW."IRAI_CD" and "KENSA_ITEM_CD" = NEW."KENSA_ITEM_CD" and "IS_DELETED" = 0) > 0 
  --then NEW."SEQ_GROUP_NO" := (select max("SEQ_GROUP_NO") from "KENSA_INF_DETAIL" where "IRAI_CD" = NEW."IRAI_CD" and "KENSA_ITEM_CD" = NEW."KENSA_ITEM_CD" and "IS_DELETED" = 0) + 1;
 then NEW."SEQ_GROUP_NO" := (select count(*) from "KENSA_INF_DETAIL" where "IRAI_CD" = NEW."IRAI_CD" and "KENSA_ITEM_CD" = NEW."KENSA_ITEM_CD" and "IS_DELETED" = 0);
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
HISTORY_TABLE_NAME := 'Z_' || TG_TABLE_NAME;
select CLIENT_ADDR, CLIENT_HOSTNAME from pg_stat_activity where pid = pg_backend_pid() into clientAddr, clientHostName;

if (TG_OP = 'INSERT') then
    execute format('INSERT INTO %I '
                   'SELECT $1, $2, $3, $4, $5, $6.* ON CONFLICT ("OP_ID") DO NOTHING', HISTORY_TABLE_NAME)
                   using nextval('"OP_ID_seq"'::regclass), 'INSERT', current_timestamp, clientAddr, clientHostName, new;
    return new;
elsif (TG_OP = 'UPDATE') then
    execute format('INSERT INTO %I '  
                   'SELECT $1, $2, $3, $4, $5, $6.* ON CONFLICT ("OP_ID") DO NOTHING', HISTORY_TABLE_NAME)
                    using nextval('"OP_ID_seq"'::regclass), 'UPDATE', current_timestamp, clientAddr, clientHostName, new;
    return new;
elsif (TG_OP = 'DELETE') then
    execute format('INSERT INTO %I '  
                   'SELECT $1, $2, $3, $4, $5, $6.* ON CONFLICT ("OP_ID") DO NOTHING', HISTORY_TABLE_NAME)
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
hokenPid = NEW."HOKEN_PID";
if (hokenPid != 0) then return null;
end if;

hokenPid = OLD."HOKEN_PID";
if (hokenPid != 0) then return null;
end if;

HISTORY_TABLE_NAME := 'Z_' || TG_TABLE_NAME;
execute format('SELECT COALESCE(MAX("OP_ID"), 0) FROM %I ', HISTORY_TABLE_NAME) into REVISION_NUMBER;
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
		iHpCd     "RENKEI_REQ"."HP_ID"%TYPE;
		iKanId    "RENKEI_REQ"."PT_ID"%TYPE;
		sUpdFg    "RENKEI_REQ"."REQ_TYPE"%TYPE;
	BEGIN
		iHpCd  = NEW."HP_ID";
		iKanId = NEW."PT_ID";

		IF (TG_OP = 'INSERT' AND (NEW."IS_DELETE" = 0 OR NEW."IS_DELETE" IS NULL)) OR
		   (TG_OP = 'UPDATE' AND OLD."IS_DELETE" = 1 AND NEW."IS_DELETE" = 0) THEN
			IF NEW."NAME" IS NOT NULL AND
			   NEW."KANA_NAME" IS NOT NULL AND
			   NEW."SEX" IS NOT NULL        AND NEW."BIRTHDAY" IS NOT NULL THEN
				sUpdFg = 'I';
			END IF;
		ELSIF TG_OP = 'UPDATE' AND (NEW."IS_DELETE" = 0 OR NEW."IS_DELETE" IS NULL) THEN
			IF (OLD."NAME" <> NEW."NAME") OR (OLD."NAME" IS NULL AND NEW."NAME" IS NOT NULL) OR
			   (OLD."KANA_NAME" <> NEW."KANA_NAME") OR (OLD."KANA_NAME" IS NULL AND NEW."KANA_NAME" IS NOT NULL) OR
			   (OLD."SEX"        <> NEW."SEX") OR (OLD."SEX" IS NULL AND NEW."SEX" IS NOT NULL) OR
			   (OLD."BIRTHDAY"    <> NEW."BIRTHDAY") OR  (OLD."BIRTHDAY" IS NULL AND NEW."BIRTHDAY" IS NOT NULL) THEN
				sUpdFg = 'U';
			END IF;
		ELSIF (TG_OP = 'INSERT' AND NEW."IS_DELETE" = 1) THEN
			sUpdFg = 'D';
		ELSIF TG_OP = 'DELETE' OR (TG_OP = 'UPDATE' AND NEW."IS_DELETE" = 1) THEN
			sUpdFg = 'D';
			iHpCd  = OLD."HP_ID";
			iKanId = OLD."PT_ID";
		END IF;

		IF sUpdFg IS NOT NULL THEN
			INSERT INTO "RENKEI_REQ"
				(
					"HP_ID",
					"PT_ID",
					"RAIIN_NO",
					"REQ_SBT",
					"REQ_TYPE",
					"CREATE_DATE",
					"CREATE_ID",
					"UPDATE_DATE",
					"UPDATE_ID"
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
		sUpdFg    "RENKEI_REQ"."REQ_TYPE"%TYPE;
BEGIN
	IF TG_OP = 'INSERT' AND NEW."STATUS" >= 1 THEN
		  sUpdFg = 'I';
ELSIF TG_OP = 'UPDATE' AND OLD."STATUS" = 0 AND NEW."STATUS" >= 1 THEN
		  sUpdFg = 'U';
END
IF;

		IF sUpdFg IS NOT NULL THEN
INSERT INTO "RENKEI_REQ"
	(
	"HP_ID",
	"PT_ID",
	"RAIIN_NO",
	"REQ_SBT",
	"REQ_TYPE",
	"CREATE_DATE",
	"CREATE_ID",
	"UPDATE_DATE",
	"UPDATE_ID"
	)
VALUES
	(
		NEW."HP_ID",
		NEW."PT_ID",
		NEW."RAIIN_NO",
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
   LAST_VISIT_DATE bigint;
   COUNT_LAST_VISIT_DATE bigint;
   begin
	   
   EXECUTE format('SELECT COALESCE(MAX("SIN_DATE"), 0) FROM "RAIIN_INF" WHERE "STATUS" >= 3 AND "HP_ID"=$1 AND "PT_ID"=$2 AND "IS_DELETED" = 0') USING NEW."HP_ID", NEW."PT_ID" INTO LAST_VISIT_DATE;
   EXECUTE format('SELECT COUNT(*) FROM "PT_LAST_VISIT_DATE" WHERE "HP_ID"=$1 AND "PT_ID"=$2') USING NEW."HP_ID", NEW."PT_ID" INTO COUNT_LAST_VISIT_DATE;
   IF (COUNT_LAST_VISIT_DATE = 0) THEN
        insert into "PT_LAST_VISIT_DATE" 
        values(
        new."HP_ID", 
        new."PT_ID", 
        LAST_VISIT_DATE, 
        current_timestamp, 
        new."UPDATE_ID", 
        new."UPDATE_MACHINE", 
        current_timestamp, 
        new."UPDATE_ID", 
        new."UPDATE_MACHINE");
   	RETURN NEW;
   else
   UPDATE "PT_LAST_VISIT_DATE" SET
            "LAST_VISIT_DATE" = LAST_VISIT_DATE,
            "UPDATE_ID"= new."UPDATE_ID",
            "UPDATE_DATE"= current_timestamp,
            "UPDATE_MACHINE"= new."UPDATE_MACHINE"
       WHERE
            "HP_ID" = NEW."HP_ID"
        AND "PT_ID" = NEW."PT_ID";
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
	IF (NEW."ITEM_CD" = '@SHIN') THEN
	UPDATE "RAIIN_INF" SET
            "SYOSAISIN_KBN"         = NEW."SURYO"
        WHERE
            "HP_ID"         = NEW."HP_ID"
		AND "PT_ID"           = NEW."PT_ID"
		AND "RAIIN_NO" = NEW."RAIIN_NO";
	RETURN NEW;
	ELSIF
	(NEW."ITEM_CD" = '@JIKAN') THEN
	UPDATE "RAIIN_INF" SET
            "JIKAN_KBN"         = NEW."SURYO"
       WHERE
            "HP_ID"         = NEW."HP_ID"
		AND "PT_ID"           = NEW."PT_ID"
		AND "RAIIN_NO" = NEW."RAIIN_NO";
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
	IF NEW."IS_DELETED" = 1 THEN
	    minSeqNo = (select  min("SEQ_NO") from "KENSA_INF_DETAIL" where "HP_ID" = new."HP_ID" and "IRAI_CD" = new."IRAI_CD" and "KENSA_ITEM_CD" = new."KENSA_ITEM_CD" and "IS_DELETED" = 0 and new."SEQ_NO" != "SEQ_NO");
	    maxSeqNo = (select  max("SEQ_NO") from "KENSA_INF_DETAIL" where "HP_ID" = new."HP_ID" and "IRAI_CD" = new."IRAI_CD" and "KENSA_ITEM_CD" = new."KENSA_ITEM_CD" and "IS_DELETED" = 0 and new."SEQ_NO" != "SEQ_NO");
	   if minSeqNo is null then return old; end if;
	   if maxSeqNo is null then return old; end if;
	   for cnt in minSeqNo..maxSeqNo loop
	    if (select count(*) from "KENSA_INF_DETAIL" where "SEQ_NO" = cnt and "IS_DELETED" = 0 ) > 0
	    then 
	    	update "KENSA_INF_DETAIL" set "SEQ_GROUP_NO" = sequennocalculate where "SEQ_NO" = cnt and "IS_DELETED" = 0;
	    	sequennocalculate = sequennocalculate + 1;
	    END if;
	    end loop;
	END if;
	RETURN new;
end $function$
;
