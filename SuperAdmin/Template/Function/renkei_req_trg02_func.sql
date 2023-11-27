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
