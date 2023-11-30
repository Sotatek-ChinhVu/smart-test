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
