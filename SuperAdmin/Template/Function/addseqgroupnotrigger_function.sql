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
