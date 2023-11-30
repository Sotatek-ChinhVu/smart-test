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
