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
