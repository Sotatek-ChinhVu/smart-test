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
